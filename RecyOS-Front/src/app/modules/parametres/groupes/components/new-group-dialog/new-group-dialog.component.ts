import { Component } from '@angular/core';
import {
    FormBuilder,
    FormControl,
    FormGroup,
    Validators,
    ReactiveFormsModule,
    FormsModule,
} from '@angular/forms';
import { MatButtonModule } from '@angular/material/button';
import { MatDialogModule, MatDialogRef } from '@angular/material/dialog';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatSelectModule } from '@angular/material/select';
import { MatOptionModule } from '@angular/material/core';
import { GroupService } from 'app/core/services/group.service';
import { InputFieldComponent } from 'app/shared/components/form-components/input-field/input-field.component';
import { MatIconModule } from '@angular/material/icon';
import { NgIf } from '@angular/common';
import {
    catchError,
    debounceTime,
    map,
    Observable,
    of,
    switchMap,
    tap,
} from 'rxjs';
@Component({
    selector: 'app-new-group-dialog',
    templateUrl: './new-group-dialog.component.html',
    standalone: true,
    imports: [
        MatDialogModule,
        MatButtonModule,
        InputFieldComponent,
        MatFormFieldModule,
        MatInputModule,
        ReactiveFormsModule,
        FormsModule,
        MatSelectModule,
        MatOptionModule,
        MatIconModule,
        MatInputModule,
        NgIf,
    ],
})
export class NewGroupDialogComponent {
    groupForm: FormGroup;
    isGroupCreated = false;

    constructor(
        private readonly _fb: FormBuilder,
        private readonly _groupService: GroupService,
        private readonly dialogRef: MatDialogRef<NewGroupDialogComponent>
    ) {}

    ngOnInit(): void {
        this.initializeGroupForm();
        this.setupValueChangesSubscription();
    }

    private initializeGroupForm(): void {
        this.groupForm = this._fb.group({
            name: ['', Validators.required],
        });
    }

    private setupValueChangesSubscription(): void {
        this.nameControl.valueChanges
            .pipe(
                debounceTime(300),
                switchMap((query) => {
                    if (!query) return of(false);
                    return this._groupService.checkGroupNameExists(query);
                }),
                tap((exists: boolean) => {
                    if (exists) {
                        this.nameControl.setErrors({
                            groupExists: 'Ce nom de groupe existe déjà',
                        });
                    }
                })
            )
            .subscribe();
    }

    createGroup(): void {
        if (this.groupForm.valid) {
            this._groupService.createGroup(this.groupForm.value).subscribe({
                next: () => {
                    this.isGroupCreated = true;
                },
                error: (error) => {
                    console.error('Error creating group:', error);
                },
            });
        }
    }

    get nameControl() {
        return this.groupForm.get('name') as FormControl;
    }

    closeDialog(): void {
        this.dialogRef.close();
    }
}
