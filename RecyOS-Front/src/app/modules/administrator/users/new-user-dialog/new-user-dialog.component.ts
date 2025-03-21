import {Component, OnInit} from "@angular/core";
import {MatDialogRef} from "@angular/material/dialog";
import {UntypedFormControl, UntypedFormGroup, Validators} from "@angular/forms";
import {debounceTime, map, Subject, switchMap, takeUntil} from "rxjs";
import {UsersService} from "../users.service";
import { SignUpDto } from "../users.type";

@Component({
    selector: 'app-new-user-dialog',
    templateUrl: './new-user-dialog.component.html',
})
export class NewUserDialogComponent implements OnInit {
    isValid: boolean = false;
    userForm: UntypedFormGroup = new UntypedFormGroup({
        email: new UntypedFormControl('', [Validators.required, Validators.email]),
        firstName: new UntypedFormControl('', [Validators.required]),
        lastName: new UntypedFormControl('', [Validators.required]),
        password: new UntypedFormControl('', [
            Validators.required,
            Validators.minLength(8),
            Validators.pattern(/^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,}$/)
        ]),
        confirmPassword: new UntypedFormControl('', [Validators.required])
    }, { validators: this.passwordMatchValidator });
    
    private _unsubscribeAll: Subject<any> = new Subject<any>();
    hidePassword: boolean = true;

    constructor(
        public dialogRef: MatDialogRef<NewUserDialogComponent>,
        public userService: UsersService
    ) {}

    ngOnInit() {
        this.userForm.get('email').valueChanges
            .pipe(
                takeUntil(this._unsubscribeAll),
                debounceTime(300),
                switchMap((query) => {
                    const isValid = this.checkEmail(query);
                    if (isValid) {
                        this.userService.checkEmail(query).subscribe((exists) => {
                            this.isValid = !exists && this.userForm.valid;
                        });
                    } else {
                        this.isValid = false;
                    }
                    return [];
                })
            )
            .subscribe();

        // Surveiller les changements de tous les champs pour mettre à jour isValid
        this.userForm.valueChanges.subscribe(() => {
            this.isValid = this.userForm.valid;
        });
    }

    onCancel(): void {
        this.dialogRef.close();
    }

    onCreate(): void {
        if (this.userForm.valid) {
            const signUpData: SignUpDto = {
                email: this.userForm.get('email')?.value,
                password: this.userForm.get('password')?.value,
                firstName: this.userForm.get('firstName')?.value,
                lastName: this.userForm.get('lastName')?.value,
                userName: this.userForm.get('firstName')?.value + " "+ this.userForm.get('lastName')?.value,
                confirmPassword: this.userForm.get('confirmPassword')?.value
            };

            this.userService.signUp(signUpData).subscribe(() => {
                this.dialogRef.close();
            });
        }
    }

    checkEmail(email: string): boolean {
        const regex = /^[\w-]+(\.[\w-]+)*@([\w-]+\.)+[a-zA-Z]{2,7}$/;
        return regex.test(email);
    }

    private passwordMatchValidator(form: UntypedFormGroup): {[key: string]: boolean} | null {
        const password = form.get('password');
        const confirmPassword = form.get('confirmPassword');

        if (password && confirmPassword && password.value !== confirmPassword.value) {
            return { 'passwordMismatch': true };
        }
        return null;
    }
}
