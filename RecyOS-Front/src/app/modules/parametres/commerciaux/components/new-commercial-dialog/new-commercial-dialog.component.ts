import { Component, OnInit } from '@angular/core';
import {
    FormBuilder,
    FormControl,
    FormGroup,
    ReactiveFormsModule,
    Validators,
} from '@angular/forms';
import { CommerciauxService } from '../../../../../core/services/commerciaux.service';
import { MatDialogRef, MatDialogModule } from '@angular/material/dialog';
import { MatButtonModule } from '@angular/material/button';
import { InputFieldComponent } from 'app/shared/components/form-components/input-field/input-field.component';
import { CommercialPhoneInputFieldComponent } from '../commerciaux-details/commercial-phone-input-field/commercial-phone-input-field.component';
import { NgIf } from '@angular/common';
import { SelectFieldComponent } from '../../../../../shared/components/form-components/select-field/select-field.component';
import { UsersService } from 'app/modules/administrator/users/users.service';
import { UserDto } from 'app/modules/administrator/users/users.type';
import { Commercial } from 'app/models/commercial.type';
@Component({
    selector: 'app-new-commercial-dialog',
    templateUrl: './new-commercial-dialog.component.html',
    standalone: true,
    imports: [
        MatDialogModule,
        MatButtonModule,
        InputFieldComponent,
        CommercialPhoneInputFieldComponent,
        NgIf,
        ReactiveFormsModule,
        SelectFieldComponent,
    ],
})
export class NewCommercialDialogComponent implements OnInit {
    users: UserDto[];
    commerciaux: Commercial[];
    commercialForm: FormGroup;
    isCommercialCreated = false;
    userOptions: { value: number; label: string }[] = [];

    constructor(
        private readonly _fb: FormBuilder,
        private readonly _commerciauxService: CommerciauxService,
        private readonly _usersService: UsersService,
        private readonly dialogRef: MatDialogRef<NewCommercialDialogComponent>
    ) {}

    ngOnInit(): void {
        this.initializeCommercialCreationForm();
        this.fetchUsers();
        this.fetchCommerciaux();

        // Listen to user selection changes
        this.userIdControl.valueChanges.subscribe((userId) => {
            const selectedUser = this.users.find((user) => user.id === userId);
            if (selectedUser) {
                this.commercialForm.patchValue({
                    firstname: selectedUser.firstName,
                    lastname: selectedUser.lastName,
                    email: selectedUser.email,
                });
            }
        });
    }

    private initializeCommercialCreationForm(): void {
        this.commercialForm = this._fb.group({
            userId: [''],
            firstname: [''],
            lastname: [''],
            username: ['', Validators.required],
            phone: [
                '',
                [
                    Validators.required,
                    Validators.pattern(/^\+33\s[1-9](?:\s\d{2}){4}$/),
                ],
            ],
            email: [''],
            codeMkgt: [
                '',
                [
                    Validators.required,
                    Validators.minLength(2),
                    Validators.maxLength(2),
                ],
            ],
            idHubSpot: ['', [Validators.minLength(9), Validators.maxLength(9)]],
        });
    }

    private fetchUsers(): void {
        // First get all commerciaux to know which userIds are taken
        this._commerciauxService.fetchCommerciaux().subscribe((commerciaux) => {
            const takenUserIds = commerciaux.map(
                (commercial) => commercial.userId
            );

            // Then get all users and filter out the ones already assigned
            this._usersService.getUsers(0, 100).subscribe({
                next: (response) => {
                    this.users = response.items.filter(
                        (user) => !takenUserIds.includes(user.id)
                    );
                    this.userOptions = this.users.map((user) => ({
                        value: Number(user.id),
                        label: user.userName,
                    }));
                },
                error: (error) => console.error('Error fetching users:', error),
            });
        });
    }

    private fetchCommerciaux(): void {
        this._commerciauxService.fetchCommerciaux().subscribe({
            next: (response: any) => {
                this.commerciaux = response.items || [];
            },
        });
    }

    createCommercial(): void {
        if (this.commercialForm.valid) {
            this._commerciauxService
                .createCommercial(this.commercialForm.value)
                .subscribe({
                    next: () => {
                        this.isCommercialCreated = true;
                    },
                    error: (error) =>
                        console.error('Error creating commercial:', error),
                });
        }
    }

    get userIdControl() {
        return this.commercialForm.get('userId') as FormControl;
    }

    get firstnameControl() {
        return this.commercialForm.get('firstname') as FormControl;
    }

    get lastnameControl() {
        return this.commercialForm.get('lastname') as FormControl;
    }

    get usernameControl() {
        return this.commercialForm.get('username') as FormControl;
    }

    get phoneControl() {
        return this.commercialForm.get('phone') as FormControl;
    }

    get emailControl() {
        return this.commercialForm.get('email') as FormControl;
    }

    get codeMkgtControl() {
        return this.commercialForm.get('codeMkgt') as FormControl;
    }

    get idHubSpotControl() {
        return this.commercialForm.get('idHubSpot') as FormControl;
    }

    closeDialog(): void {
        this.dialogRef.close();
    }
}
