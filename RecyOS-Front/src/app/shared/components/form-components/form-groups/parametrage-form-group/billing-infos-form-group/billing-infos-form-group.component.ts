import { Component, Input, OnInit } from '@angular/core';
import { FormControl, FormGroup } from '@angular/forms';
import { EntityStrategyService } from '../../../../../../core/strategies/entity-strategy/entity-strategy.service';
import { IStrategyObject } from '../../../../../../core/strategies/entity-strategy/IStrategyObject';
import { EntityDto } from '../../../../../../models/entities-models/entity.type';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule } from '@angular/forms';
import { MatSelectModule } from '@angular/material/select';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { SelectFieldComponent } from '../../../select-field/select-field.component';
import { InputFieldComponent } from '../../../input-field/input-field.component';

@Component({
    selector: 'app-billing-infos-form-group',
    templateUrl: './billing-infos-form-group.component.html',
    standalone: true,
    imports: [
        CommonModule,
        ReactiveFormsModule,
        MatSelectModule,
        MatFormFieldModule,
        MatInputModule,
        SelectFieldComponent,
        InputFieldComponent,
    ],
})
export class BillingInfosFormGroupComponent implements OnInit {
    @Input() billingInfosFormGroup: FormGroup;
    @Input() selectedEntity: EntityDto;
    strategyObject: IStrategyObject;
    tauxTvaOptions: { value: number; label: string }[] = [];

    constructor(
        private readonly _entityStrategyService: EntityStrategyService
    ) {}

    ngOnInit(): void {
        this.strategyObject =
            this._entityStrategyService.determineStrategyFromEntity(
                this.selectedEntity
            );
        this.initializeDefaultValues();
        this.initializeTauxTvaOptions();
    }

    private initializeDefaultValues(): void {
        const conditionReglementControlName =
            this.strategyObject.type === 'supplier'
                ? 'frnConditionReglement'
                : 'conditionReglement';
        const modeReglementControlName =
            this.strategyObject.type === 'supplier'
                ? 'frnModeReglement'
                : 'modeReglement';
        const delaiReglementControlName =
            this.strategyObject.type === 'supplier'
                ? 'frnDelaiReglement'
                : 'delaiReglement';
        const encoursMaxControlName =
            this.strategyObject.type === 'supplier'
                ? 'frnEncoursMax'
                : 'encoursMax';
        const compteComptableControlName =
            this.strategyObject.type === 'supplier'
                ? 'frnCompteComptable'
                : 'compteComptable';
        const tauxTvaControlName =
            this.strategyObject.type === 'supplier' ? 'frnTauxTva' : 'tauxTva';

        this.billingInfosFormGroup
            .get(conditionReglementControlName)
            .valueChanges.subscribe((value) => {
                if (value === 0 || value === 2) {
                    // Both cases are handled the same way
                    this.billingInfosFormGroup
                        .get(modeReglementControlName)
                        ?.setValue(0);
                    this.billingInfosFormGroup
                        .get(delaiReglementControlName)
                        ?.setValue(30);
                    this.billingInfosFormGroup
                        .get(encoursMaxControlName)
                        ?.setValue(1000);
                }
                if (value === 1) {
                    this.billingInfosFormGroup
                        .get(modeReglementControlName)
                        ?.setValue(4);
                    this.billingInfosFormGroup
                        .get(delaiReglementControlName)
                        ?.setValue(0);
                    this.billingInfosFormGroup
                        .get(encoursMaxControlName)
                        ?.setValue(0);
                }
            });

        // Subscribe to tauxTva changes and adjust compteComptable accordingly
        this.billingInfosFormGroup
            .get(tauxTvaControlName)
            .valueChanges.subscribe((value) => {
                if (this.strategyObject.status === 'professional') {
                    this.initializeProfessionnalBillingInfosFormGroup(
                        value,
                        compteComptableControlName
                    );
                } else if (this.strategyObject.status === 'particulier') {
                    this.initializeFrenchAccountingCode(
                        value,
                        compteComptableControlName
                    );
                }
            });
    }

    private initializeProfessionnalBillingInfosFormGroup(
        value,
        compteComptableControlName
    ): void {
        if (this.strategyObject.type === 'client') {
            if (value === 20) {
                this.billingInfosFormGroup
                    .get(compteComptableControlName)
                    ?.setValue('411103');
            } else if (value === 10) {
                this.billingInfosFormGroup
                    .get(compteComptableControlName)
                    ?.setValue('411102');
            } else if (value === 5.5) {
                this.billingInfosFormGroup
                    .get(compteComptableControlName)
                    ?.setValue('411101');
            } else if (value === 0) {
                this.billingInfosFormGroup
                    .get(compteComptableControlName)
                    ?.setValue('411104');
            }
        } else if (this.strategyObject.type === 'supplier') {
            if (this.strategyObject.region === 'france') {
                this.initializeFrenchAccountingCode(
                    value,
                    compteComptableControlName
                );
            } else if (this.strategyObject.region === 'europe') {
                if (value === 0) {
                    this.billingInfosFormGroup
                        .get(compteComptableControlName)
                        ?.setValue('401106');
                }
            }
        }
    }

    private initializeFrenchAccountingCode(
        value,
        compteComptableControlName
    ): void {
        if (value === 20) {
            this.billingInfosFormGroup
                .get(compteComptableControlName)
                ?.setValue('401101');
        } else if (value === 0) {
            this.billingInfosFormGroup
                .get(compteComptableControlName)
                ?.setValue('401105');
        }
    }

    private initializeTauxTvaOptions(): void {
        this.tauxTvaOptions = [
            { value: 20, label: '20' },
            { value: 10, label: '10' },
            { value: 5.5, label: '5.5' },
            { value: 0, label: '0' },
        ];

        if (this.strategyObject.status === 'particulier') {
            this.tauxTvaOptions = [
                { value: 20, label: '20' },
                { value: 0, label: '0' },
            ];
        } else if (this.strategyObject.type === 'supplier') {
            if (this.strategyObject.region === 'france') {
                this.tauxTvaOptions = [
                    { value: 20, label: '20' },
                    { value: 0, label: '0' },
                ];
            }
            if (this.strategyObject.region === 'europe') {
                this.tauxTvaOptions = [{ value: 0, label: '0' }];
            }
        }
    }

    get conditionReglementControl(): FormControl {
        const controlName =
            this.strategyObject.type === 'supplier'
                ? 'frnConditionReglement'
                : 'conditionReglement';
        return this.billingInfosFormGroup.get(controlName) as FormControl;
    }
    get modeReglementControl(): FormControl {
        const controlName =
            this.strategyObject.type === 'supplier'
                ? 'frnModeReglement'
                : 'modeReglement';
        return this.billingInfosFormGroup.get(controlName) as FormControl;
    }
    get delaiReglementControl(): FormControl {
        const controlName =
            this.strategyObject.type === 'supplier'
                ? 'frnDelaiReglement'
                : 'delaiReglement';
        return this.billingInfosFormGroup.get(controlName) as FormControl;
    }
    get tauxTvaControl(): FormControl {
        const controlName =
            this.strategyObject.type === 'supplier' ? 'frnTauxTva' : 'tauxTva';
        return this.billingInfosFormGroup.get(controlName) as FormControl;
    }
    get compteComptableControl(): FormControl {
        const controlName =
            this.strategyObject.type === 'supplier'
                ? 'frnCompteComptable'
                : 'compteComptable';
        return this.billingInfosFormGroup.get(controlName) as FormControl;
    }
    get encoursMaxControl(): FormControl {
        const controlName =
            this.strategyObject.type === 'supplier'
                ? 'frnEncoursMax'
                : 'encoursMax';
        return this.billingInfosFormGroup.get(controlName) as FormControl;
    }
}
