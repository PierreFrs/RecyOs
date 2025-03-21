import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { MatDialogRef } from '@angular/material/dialog';
import { SocieteService } from 'app/core/services/societe.service';

@Component({
    selector: 'app-new-societe-dialog',
    templateUrl: './new-societe-dialog.component.html'
})
export class NewSocieteDialogComponent implements OnInit {
    societeForm: FormGroup;
    isSocieteCreated = false;

    constructor(
        private _formBuilder: FormBuilder,
        private _societeService: SocieteService,
        private _dialogRef: MatDialogRef<NewSocieteDialogComponent>
    ) {}

    ngOnInit(): void {
        this.initializeSocieteForm();
    }

    private initializeSocieteForm(): void {
        this.societeForm = this._formBuilder.group({
            nom: ['', Validators.required],
            idOdoo: ['', Validators.required]
        });
    }

    createSociete(): void {
        if (this.societeForm.valid) {
            this._societeService.createSociete(this.societeForm.value)
                .subscribe({
                    next: () => {
                        this.isSocieteCreated = true;
                    },
                    error: (error) => console.error('Error creating societe:', error)
                });
        }
    }

    closeDialog(): void {
        this._dialogRef.close(this.isSocieteCreated);
    }

    get nomControl(): FormControl {
        return this.societeForm.get('nom') as FormControl;
    }

    get idOdooControl(): FormControl {
        return this.societeForm.get('idOdoo') as FormControl;
    }
} 