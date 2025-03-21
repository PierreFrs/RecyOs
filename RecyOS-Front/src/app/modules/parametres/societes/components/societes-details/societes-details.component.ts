import { ChangeDetectionStrategy, ChangeDetectorRef, Component, Input, OnDestroy, OnInit } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { Subject, takeUntil } from 'rxjs';
import { SocieteDto } from 'app/models/societe.type';
import { SocieteService } from 'app/core/services/societe.service';
import { UserService } from 'app/core/services/user.service';
import { MatDialog } from '@angular/material/dialog';
import { ConfirmSocieteDeleteComponent } from './confirm-societe-delete/confirm-societe-delete.component';

@Component({
    selector: 'app-societes-details',
    templateUrl: './societes-details.component.html',
    changeDetection: ChangeDetectionStrategy.OnPush,

})
export class SocietesDetailsComponent implements OnInit, OnDestroy {
    @Input() societeId: number;
    @Input() societes: SocieteDto[];

    selectedSociete: SocieteDto;
    selectedSocieteForm: FormGroup;
    flashMessage: 'success' | 'error' | null = null;
    userIsAdmin = false;
    userCanUpdateSociete = false;
    userCanDeleteSociete = false;

    private readonly _unsubscribeAll: Subject<any> = new Subject<any>();

    constructor(
        private readonly _formBuilder: FormBuilder,
        private readonly _societeService: SocieteService,
        private readonly _userService: UserService,
        private readonly _changeDetectorRef: ChangeDetectorRef,
        private readonly _dialog: MatDialog
    ) {}

    ngOnInit(): void {
        this.initializeForm();
        this.determineUserPrivileges();
        this.loadSocieteDetails();
    }

    private initializeForm(): void {
        this.selectedSocieteForm = this._formBuilder.group({
            nom: ['', Validators.required],
            idOdoo: ['', Validators.required]
        });
    }

    private determineUserPrivileges(): void {
        this._userService.user$
            .pipe(takeUntil(this._unsubscribeAll))
            .subscribe(user => {
                if (user?.roles) {
                    this.userIsAdmin = user.roles.some(role => role.name === 'admin');
                    this.userCanUpdateSociete = user.roles.some(role => role.name === 'update_societe');
                    this.userCanDeleteSociete = user.roles.some(role => role.name === 'delete_societe');
                }
            });
    }

    private loadSocieteDetails(): void {
        if (this.societeId) {
            this._societeService.getSocieteById(this.societeId)
                .pipe(takeUntil(this._unsubscribeAll))
                .subscribe({
                    next: (societe) => {
                        this.selectedSociete = societe;
                        this.selectedSocieteForm.patchValue(societe);
                        this._changeDetectorRef.markForCheck();
                    },
                    error: (error) => console.error('Failed to load societe details', error)
                });
        }
    }

    updateSociete(): void {
        if (this.selectedSocieteForm.invalid || (!this.userCanUpdateSociete && !this.userIsAdmin)) {
            return;
        }

        this._societeService.updateSociete(this.societeId, this.selectedSocieteForm.value)
            .pipe(takeUntil(this._unsubscribeAll))
            .subscribe({
                next: (updatedSociete) => this.handleSuccessfulUpdate(updatedSociete),
                error: (error) => this.handleErrorDuringUpdate(error)
            });
    }

    deleteSociete(): void {
        const dialogRef = this._dialog.open(ConfirmSocieteDeleteComponent, {
            width: '400px',
            data: { societeName: this.selectedSociete.nom }
        });

        dialogRef.afterClosed().subscribe(result => {
            if (result) {
                this._societeService.deleteSociete(this.societeId)
                    .pipe(takeUntil(this._unsubscribeAll))
                    .subscribe({
                        next: () => this.handleSuccessfulDeletion(),
                        error: (error) => this.handleErrorDuringDeletion(error)
                    });
            }
        });
    }

    private handleSuccessfulUpdate(updatedSociete: SocieteDto): void {
        const index = this.societes?.findIndex(s => s.id === updatedSociete.id);
        if (this.societes && index !== -1) {
            this.societes[index] = updatedSociete;
            this.selectedSociete = updatedSociete;
            this.showFlashMessage('success');
        }
    }

    private handleErrorDuringUpdate(error: any): void {
        console.error('Update failed:', error);
        this.showFlashMessage('error');
    }

    private handleSuccessfulDeletion(): void {
        const index = this.societes?.findIndex(s => s.id === this.societeId);
        if (index !== -1) {
            this.societes.splice(index, 1);
        }
        this._changeDetectorRef.markForCheck();
    }

    private handleErrorDuringDeletion(error: any): void {
        console.error('Deletion failed:', error);
    }

    private showFlashMessage(type: 'success' | 'error'): void {
        this.flashMessage = type;
        this._changeDetectorRef.markForCheck();

        setTimeout(() => {
            this.flashMessage = null;
            this._changeDetectorRef.markForCheck();
        }, 3000);
    }

    get nomControl(): FormControl {
        return this.selectedSocieteForm.get('nom') as FormControl;
    }

    get idOdooControl(): FormControl {
        return this.selectedSocieteForm.get('idOdoo') as FormControl;
    }

    ngOnDestroy(): void {
        this._unsubscribeAll.next(null);
        this._unsubscribeAll.complete();
    }
}
