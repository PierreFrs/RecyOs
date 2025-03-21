import {
    ChangeDetectorRef,
    Component,
    EventEmitter,
    Input,
    OnDestroy,
    OnInit,
    Output,
} from '@angular/core';
import { FormGroup, UntypedFormGroup } from '@angular/forms';
import { catchError, Observable, Subject } from 'rxjs';
import { MatDialog } from '@angular/material/dialog';
import { RoleAssignmentService } from '../../../core/services/role-assignment.service';
import { EntityStrategyService } from '../../../core/strategies/entity-strategy/entity-strategy.service';
import { FormInitializersService } from '../../../core/services/form-initializers.service';
import { ChoiceMkgtCodeDialogComponent } from '../../../modules/customers/businesses/choice-mkgt-code-dialog/choice-mkgt-code-dialog';
import { IEntityServiceStrategy } from '../../../core/strategies/entity-strategy/IEntityServiceStrategy';
import {
    BusinessUnitDto,
    EntityBusinessUnitDto,
} from '../../../models/business-unit.type';
import { IStrategyObject } from '../../../core/strategies/entity-strategy/IStrategyObject';
import { MatTabsModule } from '@angular/material/tabs';
import { SharedModule } from '../../shared.module';
import { NgIf } from '@angular/common';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { UpdateSiretDialogComponent } from '../dialogs/update-siret-dialog/update-siret-dialog.component';
import {
    EntityDto,
    EntityDTOPagination,
    EntityFormDto,
} from '../../../models/entities-models/entity.type';
import { ParametrageTabComponent } from './entity-tabs/parametrage-tab/parametrage-tab.component';
import { IdentificationTabComponent } from './entity-tabs/identification-tab/identification-tab.component';

@Component({
    selector: 'app-entity-details',
    templateUrl: './entity-details.component.html',
    standalone: true,
    imports: [
        MatTabsModule,
        SharedModule,
        NgIf,
        MatButtonModule,
        MatIconModule,
        IdentificationTabComponent,
        ParametrageTabComponent,
    ],
})
export class EntityDetailsComponent implements OnInit, OnDestroy {
    @Input() entities: EntityDto[] = [];
    @Input() selectedEntity: EntityDto | null = null;
    @Output() updateFactorSignal = new EventEmitter<void>();

    entityType: 'client' | 'supplier';
    entityRegion: 'france' | 'europe' | 'unknown';
    entityStatus: 'professional' | 'particulier';
    selectedEntityForm: UntypedFormGroup;
    selectedEntityFormOptimized: EntityFormDto;
    flashMessage: 'success' | 'error' | null = null;
    siretFlashMessage: 'success' | null = null;
    strategyObject: IStrategyObject;
    readonlyObject: {
        codeMkgt: boolean;
        idOdoo: boolean;
        codeGpi?: boolean;
        frnCodeGpi?: boolean;
        idDashdoc?: boolean;
    } = {
        codeMkgt: false,
        idOdoo: false,
        codeGpi: false,
        frnCodeGpi: false,
        idDashdoc: false,
    };

    private readonly _unsubscribeAll: Subject<any> = new Subject<any>();
    private entityServiceStrategy: IEntityServiceStrategy<
        EntityDto,
        EntityDTOPagination,
        EntityFormDto,
        BusinessUnitDto,
        EntityBusinessUnitDto
    >;

    constructor(
        private readonly _entityStrategyService: EntityStrategyService,
        private readonly _changeDetectorRef: ChangeDetectorRef,
        private readonly _dialog: MatDialog,
        private readonly _roleAssignmentService: RoleAssignmentService,
        private readonly _formInitializerService: FormInitializersService
    ) {}

    ngOnInit(): void {
        this.strategyObject =
            this._entityStrategyService.determineStrategyFromEntity(
                this.selectedEntity
            );
        this.entityServiceStrategy = this.strategyObject.strategy;
        this.entityType = this.strategyObject.type;
        this.entityRegion = this.strategyObject.region;
        this.entityStatus = this.strategyObject.status;
        this.selectedEntityForm = this._formInitializerService.initializeForm(
            this.entityStatus,
            this.entityType,
            this.entityRegion
        );
        this.loadData();
    }

    get canUpdate(): boolean {
        return this._roleAssignmentService.getCanUpdate(
            this.entityType,
            this.entityStatus
        );
    }
    get isAdmin(): boolean {
        return this._roleAssignmentService.isAdmin;
    }
    get canUpdateSiret(): boolean {
        return this._roleAssignmentService.userRoles.canUpdateSiret;
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
    get identificationFormGroup(): FormGroup {
        return this.selectedEntityForm?.get('identificationForm') as FormGroup;
    }
    get parametrageFormGroup(): FormGroup {
        return this.selectedEntityForm?.get('parametrageForm') as FormGroup;
    }
    private updateReadonlyObject() {
        this.readonlyObject = {
            codeMkgt: !!this.selectedEntity?.codeMkgt || !this.canCreateMkgt,
            idOdoo: !!this.selectedEntity?.idOdoo || !this.canCreateOdoo,
            ...('codeGpi' in this.selectedEntity
                ? {
                      codeGpi:
                          !!this.selectedEntity.codeGpi || !this.canCreateGpi,
                  }
                : {}),
            ...('frnCodeGpi' in this.selectedEntity
                ? {
                      frnCodeGpi:
                          !!this.selectedEntity.frnCodeGpi ||
                          !this.canCreateGpi,
                  }
                : {}),
            ...('idDashdoc' in this.selectedEntity
                ? {
                      idDashdoc:
                          !!this.selectedEntity.idDashdoc ||
                          !this.canCreateDashdoc,
                  }
                : {}),
        };
    }

    ngOnDestroy() {
        this._unsubscribeAll.next(null);
        this._unsubscribeAll.complete();
    }
    private loadData(): void {
        this.entityServiceStrategy
            .getEntityById(this.selectedEntity?.id)
            .subscribe({
                next: (entity: EntityDto) => {
                    this.processEntityData(entity);
                },
                error: (error) => {
                    console.error('Failed to load data', error);
                },
            });
    }
    private processEntityData(entity: EntityDto): void {
        this.selectedEntity = entity;
        this.selectedEntityFormOptimized =
            this.entityServiceStrategy.convertEntityDtoToEntityFormDto(entity);
        this.selectedEntityForm?.patchValue(this.selectedEntityFormOptimized);
        this.markFormFieldsAsTouched(this.selectedEntityForm);
        this._changeDetectorRef.detectChanges();
        this.updateReadonlyObject();
    }

    updateEntity(): void {
        if (this.canProceedWithUpdate()) {
            const updatedEntity = this.prepareUpdatedEntity();
            this.performEntityUpdate(updatedEntity).subscribe({
                next: (updatedClient) =>
                    this.handleSuccessfulUpdate(updatedClient),
                error: (error) => this.handleErrorDuringUpdate(error),
            });
            this.updateFactorSignal.emit();
        }
    }

    private canProceedWithUpdate(): boolean {
        if (this.selectedEntityForm?.invalid && !this.isAdmin) {
            console.error('Form is invalid.');
            return false;
        }

        if (!this.canUpdate) {
            console.error('User lacks admin privileges.');
            return false;
        }

        return true;
    }

    private prepareUpdatedEntity(): EntityDto {
        const formDto: EntityFormDto = this.selectedEntityForm?.getRawValue();
        return this.entityServiceStrategy.convertEntityFormDtoToEntityDto(
            formDto,
            this.selectedEntity
        );
    }

    private performEntityUpdate(entity: EntityDto): Observable<EntityDto> {
        return this.entityServiceStrategy.updateEntity(entity.id, entity).pipe(
            catchError((error) => {
                console.error('Error updating client:', error);
                if (error.status === 480) this.choiceMkgtCode();
                throw error;
            })
        );
    }

    private handleSuccessfulUpdate(updatedEntity: EntityDto): void {
        const index = this.entities?.findIndex(
            (entity) => entity.id === updatedEntity.id
        );
        if (this.entities && index !== -1) {
            this.entities[index] = updatedEntity;
            this.selectedEntity = updatedEntity;
            this._changeDetectorRef.markForCheck();
            this.showFlashMessage('success');
            this.updateReadonlyObject();
        }
    }

    private handleErrorDuringUpdate(error: any): void {
        this.showFlashMessage('error');
        console.error('Update failed:', error);
    }

    choiceMkgtCode(): void {
        const dialogRef = this._dialog.open(ChoiceMkgtCodeDialogComponent, {
            width: '700px',
            data: {
                codeMkgt: this.selectedEntity.codeMkgt,
                selectedEntityForm: this.selectedEntityForm,
                selectedEntity: this.selectedEntity,
                updateEntity: this.updateEntity.bind(this),
            },
        });

        dialogRef.afterClosed().subscribe(() => {
            console.log('The dialog was closed');
        });
    }

    private showFlashMessage(type: 'success' | 'error'): void {
        this.flashMessage = type;
        this._changeDetectorRef.markForCheck();
        setTimeout(() => {
            this.flashMessage = null;
            this._changeDetectorRef.markForCheck();
        }, 5000);
    }

    private markFormFieldsAsTouched(formGroup: FormGroup) {
        Object.values(formGroup.controls).forEach((control) => {
            control.markAsTouched();
        });
    }

    openSiretUpdateDialog(): void {
        const dialogRef = this._dialog.open(UpdateSiretDialogComponent, {
            width: '600px',
            data: { selectedEntity: this.selectedEntity },
        });

        dialogRef.afterClosed().subscribe((newSiret) => {
            if (newSiret) {
                this.loadData();
                this.showSiretFlashMessage('success');
            }
        });
    }

    private showSiretFlashMessage(type: 'success'): void {
        this.siretFlashMessage = type;
        this._changeDetectorRef.markForCheck();
        setTimeout(() => {
            this.siretFlashMessage = null;
            this._changeDetectorRef.markForCheck();
        }, 5000);
    }
}
