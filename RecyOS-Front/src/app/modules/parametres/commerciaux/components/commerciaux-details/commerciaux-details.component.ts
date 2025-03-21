import {
    ChangeDetectorRef,
    Component,
    Input,
    OnDestroy,
    OnInit,
} from '@angular/core';
import { Commercial } from '../../../../../models/commercial.type';
import { CommerciauxService } from '../../../../../core/services/commerciaux.service';
import { Subject, Subscription, takeUntil } from 'rxjs';
import {
    FormBuilder,
    FormControl,
    FormGroup,
    Validators,
    ReactiveFormsModule,
} from '@angular/forms';
import { forEach } from 'lodash';
import { UserService } from '../../../../../core/services/user.service';
import { MatDialog, MatDialogModule } from '@angular/material/dialog';
import { ConfirmCommercialDeleteComponent } from './confirm-commercial-delete/confirm-commercial-delete.component';
import { Router } from '@angular/router';
import { InputFieldComponent } from 'app/shared/components/form-components/input-field/input-field.component';
import { CommercialPhoneInputFieldComponent } from './commercial-phone-input-field/commercial-phone-input-field.component';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { NgIf } from '@angular/common';
import { SelectFieldComponent } from '../../../../../shared/components/form-components/select-field/select-field.component';
import { MatFormFieldModule } from '@angular/material/form-field';

@Component({
    selector: 'app-commerciaux-details',
    templateUrl: './commerciaux-details.component.html',
    standalone: true,
    imports: [
        InputFieldComponent,
        CommercialPhoneInputFieldComponent,
        MatDialogModule,
        MatButtonModule,
        MatIconModule,
        NgIf,
        SelectFieldComponent,
        ReactiveFormsModule,
        MatFormFieldModule,
    ],
})
export class CommerciauxDetailsComponent implements OnInit, OnDestroy {
    @Input() commercialId: number;
    userCanCreateCommercial: boolean;
    userCanUpdateCommercial: boolean;
    userCanDeleteCommercial: boolean;
    userIsAdmin: boolean;
    @Input() commerciaux: Commercial[];
    selectedCommercial: Commercial;
    hasCommercialData = false;
    selectedCommercialForm: FormGroup;
    flashMessage: 'success' | 'error' | null = null;

    private readonly _subscription: Subscription = new Subscription();
    private readonly _unsubscribeAll: Subject<void> = new Subject<void>();

    constructor(
        private readonly _userService: UserService,
        private readonly _commerciauxService: CommerciauxService,
        private readonly _fb: FormBuilder,
        private readonly _changeDetectorRef: ChangeDetectorRef,
        private readonly _dialog: MatDialog,
        private readonly _router: Router
    ) {}

    ngOnInit(): void {
        this.determineUserPrivileges();
        this.initializeForm();
        this.fetchCommercialData();
    }

    private initializeForm(): void {
        this.selectedCommercialForm = this._fb.group({
            firstname: ['', Validators.required],
            lastname: ['', Validators.required],
            username: ['', Validators.required],
            phone: [
                '',
                [
                    Validators.required,
                    Validators.pattern(/^\+33\s[1-9](?:\s\d{2}){4}$/),
                ],
            ],
            email: ['', [Validators.required, Validators.email]],
            codeMkgt: [
                '',
                [
                    Validators.required,
                    Validators.minLength(2),
                    Validators.maxLength(2),
                ],
            ],
            idHubSpot: [
                '',
                [Validators.minLength(9), Validators.maxLength(10)],
            ],
            userId: [''],
        });
    }

    private determineUserPrivileges(): void {
        this._userService.user$
            .pipe(takeUntil(this._unsubscribeAll))
            .subscribe((user) => {
                if (user?.id) {
                    forEach(user.roles, (role) => {
                        if (role.name === 'admin') {
                            this.userIsAdmin = true;
                        }
                        if (role.name === 'create_commercial') {
                            this.userCanCreateCommercial = true;
                        }
                        if (role.name === 'update_commercial') {
                            this.userCanUpdateCommercial = true;
                        }
                        if (role.name === 'delete_commercial') {
                            this.userCanDeleteCommercial = true;
                        }
                    });
                }
            });
    }

    private fetchCommercialData(): void {
        if (this.commercialId) {
            this._commerciauxService
                .fetchCommercialById(this.commercialId)
                .pipe(takeUntil(this._unsubscribeAll))
                .subscribe({
                    next: (commercial: Commercial) => {
                        this.selectedCommercial = commercial;
                        this.hasCommercialData = true;
                        this.selectedCommercialForm.patchValue(commercial);
                    },
                    error: (error) =>
                        console.error(
                            'Failed to load commercial details',
                            error
                        ),
                });
        }
    }

    updateCommercial(): void {
        if (!this.canProceedWithUpdate()) return;
        const formData = new FormData();
        const formValue = this.selectedCommercialForm.value;
        Object.keys(formValue).forEach((key) => {
            formData.append(key, formValue[key]);
        });

        this._commerciauxService
            .updateCommercial(this.commercialId, formData)
            .pipe(takeUntil(this._unsubscribeAll))
            .subscribe({
                next: (updatedCommercial) =>
                    this.handleSuccessfulUpdate(updatedCommercial),
                error: (error) => this.handleErrorDuringUpdate(error),
            });
    }

    private canProceedWithUpdate(): boolean {
        if (this.selectedCommercialForm.invalid) {
            console.log('Form validation errors:', {
                firstname: this.firstnameControl.errors,
                lastname: this.lastnameControl.errors,
                username: this.usernameControl.errors,
                phone: this.phoneControl.errors,
                email: this.emailControl.errors,
                codeMkgt: this.codeMkgtControl.errors,
                idHubSpot: this.idHubSpotControl.errors,
                formValue: this.selectedCommercialForm.value,
                formStatus: this.selectedCommercialForm.status,
            });
            return false;
        }

        if (!this.userCanUpdateCommercial) {
            console.error('User lacks update privileges.');
            return false;
        }

        return true;
    }
    private handleSuccessfulUpdate(updatedEntity: Commercial): void {
        const index = this.commerciaux?.findIndex(
            (entity) => entity.id === updatedEntity.id
        );
        if (this.commerciaux && index !== -1) {
            this.commerciaux[index] = updatedEntity;
            this.selectedCommercial = updatedEntity;
            this._changeDetectorRef.markForCheck();
            this.showFlashMessage('success');
        }
    }
    private handleErrorDuringUpdate(error: any): void {
        this.showFlashMessage('error');
        console.error('Update failed:', error);
    }
    private showFlashMessage(type: 'success' | 'error'): void {
        this.flashMessage = type;
        this._changeDetectorRef.markForCheck();
        setTimeout(() => {
            this.flashMessage = null;
            this._changeDetectorRef.markForCheck();
        }, 5000);
    }

    deleteCommercial(): void {
        const dialogRef = this._dialog.open(ConfirmCommercialDeleteComponent, {
            width: '400px',
            data: { commercialName: this.commercialName },
        });

        dialogRef.afterClosed().subscribe((result) => {
            if (result) {
                this._commerciauxService
                    .deleteCommercial(this.commercialId)
                    .pipe(takeUntil(this._unsubscribeAll))
                    .subscribe({
                        next: () => this.handleSuccessfulDeletion(),
                        error: (error) => this.handleErrorDuringDeletion(error),
                    });
            }
        });
    }

    private handleSuccessfulDeletion(): void {
        console.log('Commercial successfully deleted');
        this._router.navigate(['/commerciaux']);
    }

    private handleErrorDuringDeletion(error: any): void {
        console.error('Error during deletion:', error);
    }

    ngOnDestroy(): void {
        this._subscription.unsubscribe();
    }

    get commercialName(): string {
        if (!this.selectedCommercial) {
            return '';
        }

        const capitalize = (str: string) =>
            str.charAt(0).toUpperCase() + str.slice(1).toLowerCase();

        const firstName = capitalize(this.selectedCommercial.firstname);
        const lastName = capitalize(this.selectedCommercial.lastname);

        return `${firstName} ${lastName}`;
    }

    get firstnameControl() {
        return this.selectedCommercialForm.get('firstname') as FormControl;
    }

    get lastnameControl() {
        return this.selectedCommercialForm.get('lastname') as FormControl;
    }

    get usernameControl() {
        return this.selectedCommercialForm.get('username') as FormControl;
    }

    get phoneControl() {
        return this.selectedCommercialForm.get('phone') as FormControl;
    }

    get emailControl() {
        return this.selectedCommercialForm.get('email') as FormControl;
    }

    get codeMkgtControl() {
        return this.selectedCommercialForm.get('codeMkgt') as FormControl;
    }

    get idHubSpotControl() {
        return this.selectedCommercialForm.get('idHubSpot') as FormControl;
    }
}
