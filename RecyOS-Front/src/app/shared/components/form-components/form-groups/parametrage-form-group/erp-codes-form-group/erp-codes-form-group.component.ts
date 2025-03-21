import {
    ChangeDetectorRef,
    Component,
    Input,
    OnChanges,
    OnDestroy,
    OnInit,
    SimpleChanges,
} from '@angular/core';
import { FormControl, FormGroup } from '@angular/forms';
import {Subject} from 'rxjs';
import { IEntityServiceStrategy } from '../../../../../../core/strategies/entity-strategy/IEntityServiceStrategy';
import {
    BusinessUnitDto,
    EntityBusinessUnitDto,
} from '../../../../../../models/business-unit.type';
import { EntityStrategyService } from '../../../../../../core/strategies/entity-strategy/entity-strategy.service';
import { IStrategyObject } from '../../../../../../core/strategies/entity-strategy/IStrategyObject';
import {
    EntityDto,
    EntityDTOPagination,
    EntityFormDto,
} from '../../../../../../models/entities-models/entity.type';
import { ErpCodesService } from '../../../../../../core/services/helpers/erp-codes.service';
import { RoleAssignmentService } from '../../../../../../core/services/role-assignment.service';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule } from '@angular/forms';
import { MatSelectModule } from '@angular/material/select';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { InputAddFieldComponent } from '../../../input-add-field/input-add-field.component';

@Component({
    selector: 'app-erp-codes-form-group',
    templateUrl: './erp-codes-form-group.component.html',
    standalone: true,
    imports: [
        CommonModule,
        ReactiveFormsModule,
        MatSelectModule,
        MatFormFieldModule,
        MatInputModule,
        InputAddFieldComponent,
    ],
})
export class ErpCodesFormGroupComponent
    implements OnInit, OnDestroy, OnChanges
{
    @Input() erpCodesFormGroup: FormGroup;
    @Input() selectedEntity: EntityDto;
    @Input() readonlyConditions: {
        codeMkgt: boolean;
        idOdoo: boolean;
        codeGpi: boolean;
        frnCodeGpi: boolean;
        idDashdoc: boolean;
    };
    @Input() selectedEntityForm: FormGroup;

    strategyObject: IStrategyObject;

    private entityServiceStrategy: IEntityServiceStrategy<
        EntityDto,
        EntityDTOPagination,
        EntityFormDto,
        BusinessUnitDto,
        EntityBusinessUnitDto
    >;
    private readonly unsubscribeAll: Subject<any> = new Subject<any>();

    constructor(
        private readonly _changeDetectorRef: ChangeDetectorRef,
        private readonly _entityStrategyService: EntityStrategyService,
        private readonly _erpCodeService: ErpCodesService,
        private readonly _roleAssignmentService: RoleAssignmentService
    ) {}

    ngOnInit() {
        this.strategyObject =
            this._entityStrategyService.determineStrategyFromEntity(
                this.selectedEntity
            );
    }

    ngOnChanges(changes: SimpleChanges): void {
        if (changes.selectedEntity && this.selectedEntity) {
            const strategyObject =
                this._entityStrategyService.determineStrategyFromEntity(
                    this.selectedEntity
                );
            this.entityServiceStrategy = strategyObject.strategy;
        }
    }

    ngOnDestroy(): void {
        this.unsubscribeAll.next(null);
        this.unsubscribeAll.complete();
    }

    get codeMkgtControl(): FormControl {
        return this.erpCodesFormGroup.get('codeMkgt') as FormControl;
    }

    get idOdooControl(): FormControl {
        return this.erpCodesFormGroup.get('idOdoo') as FormControl;
    }

    get codeGpiControl(): FormControl {
        return this.erpCodesFormGroup.contains('codeGpi')
            ? (this.erpCodesFormGroup.get('codeGpi') as FormControl)
            : null;
    }

    get frnCodeGpiControl(): FormControl {
        return this.erpCodesFormGroup.contains('frnCodeGpi')
            ? (this.erpCodesFormGroup.get('frnCodeGpi') as FormControl)
            : null;
    }

    get idDashdocControl(): FormControl {
        return this.erpCodesFormGroup.get('idDashdoc') as FormControl;
    }

    get canCreateMkgt(): boolean {
        return this._roleAssignmentService.userRoles.canCreateMkgt;
    }
    get canCreateOdoo(): boolean {
        return this._roleAssignmentService.userRoles.canCreateOdoo;
    }
    get canCreateGpi(): boolean {
        return this._roleAssignmentService.userRoles.canCreateGpi;
    }
    get canCreateDashdoc(): boolean {
        return this._roleAssignmentService.userRoles.canCreateDashdoc;
    }

    /**
     * Create an MKGT entity for the given entity asynchronously.
     * @param entity
     */
    createMKGT(entity: EntityDto): void {
        const dialogRef = this._erpCodeService.openWaitingDialog('MKGT');

        this._erpCodeService
            .checkAndFetchEntity(
                entity,
                this.erpCodesFormGroup,
                this._changeDetectorRef
            )
            .subscribe({
                next: (updatedEntity) => {
                    if (updatedEntity.idOdoo) {
                        this._erpCodeService
                            .updateEntityWithMkgt(
                                updatedEntity,
                                dialogRef,
                                this.erpCodesFormGroup,
                                this._changeDetectorRef
                            )
                            .subscribe({
                                next: () => {
                                    console.log(
                                        'Entity successfully updated with MKGT code'
                                    );
                                    this._erpCodeService
                                        .createHubspotEntityIfEligible(
                                            updatedEntity,
                                            dialogRef
                                        )
                                        .subscribe();
                                },
                                error: this._erpCodeService.handleError(
                                    dialogRef,
                                    'Error updating entity with MKGT code'
                                ),
                            });
                    } else {
                        dialogRef.close();
                        alert('Failed to find Odoo entity');
                    }
                },
                error: this._erpCodeService.handleError(
                    dialogRef,
                    'Error fetching updated entity'
                ),
            });
    }

    /**
     * Create an Odoo entity for the given entity.
     * @param entity
     */
    createOdooEntity(entity: EntityDto): void {
        const dialogRef = this._erpCodeService.openWaitingDialog('Odoo');

        this._erpCodeService
            .createAndHandleOdooEntity(
                entity,
                dialogRef,
                this.erpCodesFormGroup,
                this._changeDetectorRef
            )
            .subscribe({
                next: () => {
                    console.log('Odoo entity created successfully');
                },
                error: this._erpCodeService.handleError(
                    dialogRef,
                    'Error creating Odoo entity'
                ),
            });
    }

    /**
     * Create a GPI entity for the given entity.
     * @param entity
     */
    createGpi(entity: EntityDto): void {
        const dialogRef = this._erpCodeService.openWaitingDialog('GPI');

        this._erpCodeService
            .createAndHandleGpiEntity(
                entity,
                dialogRef,
                this.erpCodesFormGroup,
                this._changeDetectorRef
            )
            .subscribe({
                next: () => {
                    console.log('GPI entity created successfully');
                },
                error: this._erpCodeService.handleError(
                    dialogRef,
                    'Error creating GPI entity'
                ),
            });

    }

    /**
     * Create a Dashdoc entity for the given entity.
     * @param entity
     */
    createDashdoc(entity: EntityDto): void {
        const dialogRef = this._erpCodeService.openWaitingDialog('Dashdoc');

        this._erpCodeService
            .handleDashdocCreation(
                entity,
                dialogRef,
                this.erpCodesFormGroup,
                this._changeDetectorRef
            )
            .subscribe({
                next: () => {
                    console.log('Dashdoc entity created successfully');
                },
                error: this._erpCodeService.handleError(
                    dialogRef,
                    'Error during Dashdoc creation'
                ),
            });
    }
}
