import { Component, Input, OnInit } from '@angular/core';
import { FormGroup } from '@angular/forms';
import { IStrategyObject } from '../../../../../core/strategies/entity-strategy/IStrategyObject';
import { EntityStrategyService } from '../../../../../core/strategies/entity-strategy/entity-strategy.service';
import { EntityDto } from '../../../../../models/entities-models/entity.type';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule } from '@angular/forms';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { AddressFormGroupComponent } from 'app/shared/components/form-components/form-groups/identification-form-group/address-form-group/address-form-group.component';
import { ContactsFormGroupComponent } from 'app/shared/components/form-components/form-groups/identification-form-group/contacts-form-group/contacts-form-group.component';

@Component({
    selector: 'app-identification-tab',
    templateUrl: './identification-tab.component.html',
    standalone: true,
    imports: [
        CommonModule,
        ReactiveFormsModule,
        MatFormFieldModule,
        MatInputModule,
        AddressFormGroupComponent,
        ContactsFormGroupComponent,
    ],
})
export class IdentificationTabComponent implements OnInit {
    @Input() entityStatus: 'professional' | 'particulier';
    @Input() entityType: 'client' | 'supplier';
    @Input() entityRegion: 'france' | 'europe' | 'unknown';
    @Input() selectedEntity: EntityDto;
    @Input() identificationFormGroup: FormGroup;
    strategyObject: IStrategyObject;

    constructor(private readonly _strategyService: EntityStrategyService) {}

    ngOnInit() {
        this.strategyObject = this._strategyService.determineStrategyFromEntity(
            this.selectedEntity
        );
    }

    get addressFormGroup(): FormGroup | null {
        return (
            (this.identificationFormGroup?.get('addressForm') as FormGroup) ||
            null
        );
    }

    get bankInfosFormGroup(): FormGroup | null {
        return this.identificationFormGroup?.contains('bankInfosForm')
            ? (this.identificationFormGroup.get('bankInfosForm') as FormGroup)
            : null;
    }

    get contactsFormGroup(): FormGroup | null {
        return (
            (this.identificationFormGroup?.get('contactsForm') as FormGroup) ||
            null
        );
    }
}
