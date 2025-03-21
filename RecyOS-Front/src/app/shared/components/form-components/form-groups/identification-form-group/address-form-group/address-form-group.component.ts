import { Component, Input } from '@angular/core';
import { FormControl, FormGroup } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule } from '@angular/forms';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { InputFieldComponent } from '../../../input-field/input-field.component';
@Component({
    selector: 'app-address-form-group',
    templateUrl: './address-form-group.component.html',
    standalone: true,
    imports: [
        CommonModule,
        ReactiveFormsModule,
        MatFormFieldModule,
        MatInputModule,
        InputFieldComponent,
    ],
})
export class AddressFormGroupComponent {
    @Input() entityStatus: 'professional' | 'particulier';
    @Input() addressFormGroup: FormGroup;

    get nomControl(): FormControl {
        return this.addressFormGroup.get('nom') as FormControl;
    }
    get prenomControl(): FormControl {
        return this.addressFormGroup.contains('prenom')
            ? (this.addressFormGroup.get('prenom') as FormControl)
            : null;
    }
    get titreControl(): FormControl {
        return this.addressFormGroup.contains('titre')
            ? (this.addressFormGroup.get('titre') as FormControl)
            : null;
    }
    get adresseFacturation1Control(): FormControl {
        return this.addressFormGroup.get('adresseFacturation1') as FormControl;
    }
    get adresseFacturation2Control(): FormControl {
        return this.addressFormGroup.get('adresseFacturation2') as FormControl;
    }
    get adresseFacturation3Control(): FormControl {
        return this.addressFormGroup.get('adresseFacturation3') as FormControl;
    }
    get codePostalFacturationControl(): FormControl {
        return this.addressFormGroup.get(
            'codePostalFacturation'
        ) as FormControl;
    }
    get villeFacturationControl(): FormControl {
        return this.addressFormGroup.get('villeFacturation') as FormControl;
    }
    get paysFacturationControl(): FormControl {
        return this.addressFormGroup.get('paysFacturation') as FormControl;
    }
}
