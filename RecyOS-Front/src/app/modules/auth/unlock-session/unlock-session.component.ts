import { Component, OnInit, ViewChild, ViewEncapsulation } from '@angular/core';
import {
    UntypedFormBuilder,
    UntypedFormGroup,
    NgForm,
    Validators,
} from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { fuseAnimations } from '@fuse/animations';
import { AuthService } from 'app/core/auth/auth.service';
import { UserService } from 'app/core/services/user.service';
import { FuseAlertType } from '@fuse/components/alert';

@Component({
    selector: 'auth-unlock-session',
    templateUrl: './unlock-session.component.html',
    encapsulation: ViewEncapsulation.None,
    animations: fuseAnimations,
})
export class AuthUnlockSessionComponent implements OnInit {
    @ViewChild('unlockSessionNgForm') unlockSessionNgForm: NgForm;

    alert: { type: FuseAlertType; message: string } = {
        type: 'success',
        message: '',
    };
    name: string;
    showAlert: boolean = false;
    unlockSessionForm: UntypedFormGroup;
    private _email: string;

    /**
     * Constructor
     */
    constructor(
        private _activatedRoute: ActivatedRoute,
        private _authService: AuthService,
        private _formBuilder: UntypedFormBuilder,
        private _router: Router,
        private _userService: UserService,
    ) {}

    // -----------------------------------------------------------------------------------------------------
    // @ Lifecycle hooks
    // -----------------------------------------------------------------------------------------------------

    /**
     * On init
     */
    ngOnInit(): void {
        this._userService.user$.subscribe((user) => {
            this.name = user.userName;
            this._email = user.email;
        });
        this.unlockSessionForm = this._formBuilder.group({
            name: [
                {
                    value: this.name,
                    disabled: true,
                },
            ],
            password: ['', Validators.required],
        });
    }

    // -----------------------------------------------------------------------------------------------------
    // @ Public methods
    // -----------------------------------------------------------------------------------------------------

    /**
     * Unlock
     */
    unlock(): void {
        if (this.unlockSessionForm.invalid) {
            return;
        }
        this.unlockSessionForm.disable();
        this.showAlert = false;
        this._authService
            .unlockSession({
                email: this._email ?? '',
                password: this.unlockSessionForm.get('password').value,
            })
            .subscribe(
                () => {
                    const redirectURL =
                        this._activatedRoute.snapshot.queryParamMap.get(
                            'redirectURL',
                        ) || '/signed-in-redirect';
                    this._router.navigateByUrl(redirectURL);
                },
                () => {
                    this.unlockSessionForm.enable();
                    this.unlockSessionNgForm.resetForm({
                        name: {
                            value: this.name,
                            disabled: true,
                        },
                    });
                    this.alert = {
                        type: 'error',
                        message: 'Invalid password',
                    };
                    this.showAlert = true;
                },
            );
    }
}
