import { Component, Inject, OnInit, OnDestroy } from '@angular/core';
import {FormControl, Validators} from '@angular/forms';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import {Subject, of, Observable} from 'rxjs';
import { debounceTime, switchMap, takeUntil, tap } from 'rxjs/operators';
import { IEntityServiceStrategy } from '../../../../core/strategies/entity-strategy/IEntityServiceStrategy';
import {
    BusinessUnitDto,
    EntityBusinessUnitDto,
} from '../../../../models/business-unit.type';
import { EntityStrategyService } from '../../../../core/strategies/entity-strategy/entity-strategy.service';
import { IStrategyObject } from '../../../../core/strategies/entity-strategy/IStrategyObject';
import {ServiceResult} from "../../../../models/service-result.type";
import {SiretService} from "../../../../core/services/helpers/siret.service";
import {EntityDto, EntityDTOPagination, EntityFormDto} from "../../../../models/entities-models/entity.type";

@Component({
    selector: 'app-update-siret-dialog',
    templateUrl: './update-siret-dialog.component.html',
})
export class UpdateSiretDialogComponent implements OnInit, OnDestroy {
    siretControl: FormControl;
    errorMessage: string = '';
    isValid: boolean = false;
    isSubmitting: boolean = false;
    strategyObject: IStrategyObject;
    private readonly _unsubscribeAll: Subject<any> = new Subject<any>();
    private entityServiceStrategy: IEntityServiceStrategy<
        EntityDto,
        EntityDTOPagination,
        EntityFormDto,
        BusinessUnitDto,
        EntityBusinessUnitDto
    >;

    constructor(
        public dialogRef: MatDialogRef<UpdateSiretDialogComponent>,
        @Inject(MAT_DIALOG_DATA)
        public data: { selectedEntity: EntityDto },
        private readonly _entityStrategyService: EntityStrategyService,
        private readonly _siretService: SiretService
    ) {
        this.siretControl = new FormControl('', [
            Validators.required,
            this.siretValidator(),
        ]);
    }

    ngOnInit(): void {
        this.strategyObject = this._entityStrategyService.determineStrategyFromEntity(this.data.selectedEntity);
        this.entityServiceStrategy = this.strategyObject.strategy;
        this.setupValueChangesSubscription();
    }

    ngOnDestroy(): void {
        this._unsubscribeAll.next(null);
        this._unsubscribeAll.complete();
    }

    private setupValueChangesSubscription(): void {
        this.siretControl.valueChanges
            .pipe(
                takeUntil(this._unsubscribeAll),
                debounceTime(300),
                switchMap((query) => this.verifyIdentifierExistence(query)),
            )
            .subscribe();
    }

    private verifyIdentifierExistence(query: string): Observable<boolean> {
        const isValid = this._siretService.isValidSiret(query);

        if (!isValid) {
            this.isValid = false;
            this.siretControl.setErrors({ invalidSiret: true });
            return of(false);
        }

        return this.entityServiceStrategy
            .checkIfEntityExists(query)
            .pipe(
                tap((exists) => {
                    this.isValid = !exists;
                    if (exists) {
                        this.siretControl.setErrors({ identifierUsed: true });
                    } else {
                        this.siretControl.setErrors(null);
                    }
                }),
            );
    }

    private siretValidator(): any {
        return (control: FormControl) => {
            return this._siretService.isValidSiret(control.value) ? null : { invalidSiret: true };
        };
    }

    onCancel(): void {
        this.dialogRef.close();
    }

    onUpdate(): void {
        if (this.siretControl.valid && this.data.selectedEntity && !this.isSubmitting) {
            this.isSubmitting = true;
            this.entityServiceStrategy.updateAdministrativeIdentifier(this.siretControl.value, this.data.selectedEntity.id).subscribe({
                next: (result: ServiceResult) => {
                    this.isSubmitting = false;
                    if (result.success) {
                        this.dialogRef.close(this.siretControl.value);
                        window.location.reload();
                    } else {
                        this.errorMessage = result.message;
                        this.siretControl.setErrors({ updateFailed: true });
                    }
                },
                error: (error) => {
                    this.isSubmitting = false;
                    console.error('Failed to update SIRET', error);
                    this.errorMessage = error.error?.message || 'An error occurred while updating the SIRET.';
                    this.siretControl.setErrors({ updateFailed: true });
                }
            });
        }
    }

}
