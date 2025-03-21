import { Component, Input, OnInit } from '@angular/core';
import { FormControl, FormGroup } from '@angular/forms';
import { RoleAssignmentService } from '../../../../../../core/services/role-assignment.service';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule } from '@angular/forms';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { InputFieldComponent } from '../../../input-field/input-field.component';

@Component({
    selector: 'app-bank-infos-form-group',
    templateUrl: './bank-infos-form-group.component.html',
    standalone: true,
    imports: [
        CommonModule,
        ReactiveFormsModule,
        MatFormFieldModule,
        MatInputModule,
        InputFieldComponent,
    ],
})
export class BankInfosFormGroupComponent implements OnInit {
    @Input() bankInfosFormGroup: FormGroup;
    canWriteBankInfos: boolean = false;
    isAdmin: boolean = false;

    constructor(private _roleAssignmentService: RoleAssignmentService) {}

    ngOnInit() {
        this.determineUserPrivileges();
    }

    private determineUserPrivileges() {
        this.canWriteBankInfos = this._roleAssignmentService.canWriteBankInfos;
        this.isAdmin = this._roleAssignmentService.isAdmin;
    }
    get ibanControl() {
        return this.bankInfosFormGroup.get('iban') as FormControl;
    }
    get bicControl() {
        return this.bankInfosFormGroup.get('bic') as FormControl;
    }
}
