import { Component, Input, OnInit } from '@angular/core';
import { FormControl, FormGroup } from '@angular/forms';
import { EntityDto } from '../../../../../../models/entities-models/entity.type';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule } from '@angular/forms';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { InputFieldComponent } from '../../../input-field/input-field.component';
import { PhoneInputFieldComponent } from '../../../phone-input-field/phone-input-field.component';
@Component({
    selector: 'app-contacts-form-group',
    templateUrl: './contacts-form-group.component.html',
    standalone: true,
    imports: [
        CommonModule,
        ReactiveFormsModule,
        MatFormFieldModule,
        MatInputModule,
        InputFieldComponent,
        PhoneInputFieldComponent,
    ],
})
export class ContactsFormGroupComponent implements OnInit {
    @Input() entityStatus: 'professional' | 'particulier';
    @Input() entityRegion: 'france' | 'europe' | 'unknown';
    @Input() contactsFormGroup: FormGroup;
    @Input() selectedEntity: EntityDto;

    clientContext: string;

    get contactFacturationControl(): FormControl | null {
        return this.contactsFormGroup.contains('contactFacturation')
            ? (this.contactsFormGroup.get('contactFacturation') as FormControl)
            : null;
    }

    get contactAlternatifControl(): FormControl {
        return this.contactsFormGroup.get('contactAlternatif') as FormControl;
    }
    get emailFacturationControl(): FormControl {
        return this.contactsFormGroup.get('emailFacturation') as FormControl;
    }
    get emailAlternatifControl(): FormControl {
        return this.contactsFormGroup.get('emailAlternatif') as FormControl;
    }
    get telephoneFacturationControl(): FormControl {
        return this.contactsFormGroup.get(
            'telephoneFacturation'
        ) as FormControl;
    }
    get telephoneAlternatifControl(): FormControl {
        return this.contactsFormGroup.get('telephoneAlternatif') as FormControl;
    }
    get portableFacturationControl(): FormControl {
        return this.contactsFormGroup.get('portableFacturation') as FormControl;
    }
    get portableAlternatifControl(): FormControl {
        return this.contactsFormGroup.get('portableAlternatif') as FormControl;
    }

    ngOnInit() {
        this.determineContext();
    }
    private determineContext() {
        if (this.entityRegion === 'france') {
            this.clientContext = 'etablissement';
        } else if (this.entityRegion === 'europe') {
            this.clientContext = 'europe';
        }
    }
}
