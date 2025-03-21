import { CommonModule, NgIf, NgFor } from '@angular/common';
import { ChangeDetectorRef, Component, Input } from '@angular/core';
import {
    FormBuilder,
    FormControl,
    FormGroup,
    Validators,
} from '@angular/forms';
import { MatButtonModule } from '@angular/material/button';
import { MatDialog } from '@angular/material/dialog';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatIconModule } from '@angular/material/icon';
import { MatInputModule } from '@angular/material/input';
import { MatListModule } from '@angular/material/list';
import { Router } from '@angular/router';
import { GroupService } from 'app/core/services/group.service';
import { UserService } from 'app/core/services/user.service';
import { Group } from 'app/models/group.type';
import { InputFieldComponent } from 'app/shared/components/form-components/input-field/input-field.component';
import { forEach } from 'lodash';
import { Subject, Subscription, takeUntil } from 'rxjs';
import { ConfirmGroupeDeleteComponent } from './confirm-groupe-delete/confirm-groupe-delete.component';

@Component({
    selector: 'app-groupes-details',
    templateUrl: './groupes-details.component.html',
    standalone: true,
    imports: [
        CommonModule,
        MatFormFieldModule,
        MatInputModule,
        MatIconModule,
        InputFieldComponent,
        NgIf,
        NgFor,
        MatListModule,
        MatButtonModule,
    ],
})
export class GroupesDetailsComponent {
    @Input() groupId: number;

    userCanUpdateGroup: boolean;
    userCanDeleteGroup: boolean;
    userIsAdmin: boolean;
    selectedGroup: Group;
    hasGroupData = false;
    selectedGroupForm: FormGroup;
    flashMessage: 'success' | 'error' | null = null;

    private readonly _subscription: Subscription = new Subscription();
    private readonly _unsubscribeAll: Subject<void> = new Subject<void>();

    constructor(
        private readonly _userService: UserService,
        private readonly _groupService: GroupService,
        private readonly _fb: FormBuilder,
        private readonly _changeDetectorRef: ChangeDetectorRef,
        private readonly _dialog: MatDialog,
        private readonly _router: Router
    ) {}

    ngOnInit(): void {
        this.determineUserPrivileges();
        this.initializeForm();
        this.fetchGroupData();
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
                        if (role.name === 'update_group') {
                            this.userCanUpdateGroup = true;
                        }
                        if (role.name === 'delete_group') {
                            this.userCanDeleteGroup = true;
                        }
                    });
                }
            });
    }

    private initializeForm(): void {
        this.selectedGroupForm = this._fb.group({
            name: ['', Validators.required],
        });
    }

    private fetchGroupData(): void {
        if (this.groupId) {
            this._groupService
                .getGroupById(this.groupId)
                .pipe(takeUntil(this._unsubscribeAll))
                .subscribe({
                    next: (group) => {
                        this.selectedGroup = group;
                        this.hasGroupData = true;
                        this.selectedGroupForm.patchValue(group);
                    },
                    error: (error) => {
                        console.error('Failed to load group details', error);
                    },
                });
        }
    }

    updateGroup(): void {
        if (!this.canProceedWithUpdate()) return;
        const formData = new FormData();
        const formValue = this.selectedGroupForm.value;
        Object.keys(formValue).forEach((key) => {
            formData.append(key, formValue[key]);

            this._groupService
                .updateGroup(this.groupId, formData)
                .pipe(takeUntil(this._unsubscribeAll))
                .subscribe({
                    next: (updatedGroup) =>
                        this.handleSuccessfulUpdate(updatedGroup),
                    error: (error) => this.handleErrorDuringUpdate(error),
                });
        });
    }

    private canProceedWithUpdate(): boolean {
        if (this.selectedGroupForm.invalid) {
            console.error('Form is invalid.');
            return false;
        }

        if (!this.userCanUpdateGroup) {
            console.error('User lacks admin privileges.');
            return false;
        }

        return true;
    }

    private handleSuccessfulUpdate(updatedGroup: Group): void {
        this.selectedGroup = updatedGroup;
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

    deleteGroup(): void {
        const dialogRef = this._dialog.open(ConfirmGroupeDeleteComponent, {
            width: '400px',
            data: { groupName: this.groupName },
        });

        dialogRef.afterClosed().subscribe((result) => {
            if (result) {
                this._groupService
                    .deleteGroup(this.groupId)
                    .pipe(takeUntil(this._unsubscribeAll))
                    .subscribe({
                        next: () => this.handleSuccessfulDeletion(),
                        error: (error) => this.handleErrorDuringDeletion(error),
                    });
            }
        });
    }

    private handleSuccessfulDeletion(): void {
        console.log('Group successfully deleted');
        this._router.navigate(['/groupes']);
    }

    private handleErrorDuringDeletion(error: any): void {
        console.error('Error during deletion:', error);
    }

    ngOnDestroy(): void {
        this._subscription.unsubscribe();
        this._unsubscribeAll.next(null);
        this._unsubscribeAll.complete();
    }

    get groupName(): string {
        if (!this.selectedGroup) {
            return '';
        }
        return this.selectedGroup.name;
    }

    get nameControl() {
        return this.selectedGroupForm.get('name') as FormControl;
    }
}
