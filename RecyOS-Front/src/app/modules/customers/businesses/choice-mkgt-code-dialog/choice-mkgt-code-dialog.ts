import { Component, Inject, OnInit } from '@angular/core';
import {
    MAT_DIALOG_DATA,
    MatDialogRef,
    MatDialogModule,
} from '@angular/material/dialog';
import {
    FormControl,
    UntypedFormControl,
    UntypedFormGroup,
    ValidationErrors,
    Validators,
    ReactiveFormsModule,
    FormsModule,
} from '@angular/forms';
import { FrenchDTO } from '../../../../models/entities-models/french.type';
import { MatButtonModule } from '@angular/material/button';
import { CommonModule } from '@angular/common';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';

export interface DialogData {
    codeMkgt: string;
    selectedEtablissementForm: UntypedFormGroup;
    selectedEtablissement: FrenchDTO;
    updateEtablissementClient: () => void;
}

@Component({
    selector: 'app-choice-mkgt-code-dialog',
    templateUrl: './choice-mkgt-code-dialog.html',
    standalone: true,
    imports: [
        CommonModule,
        MatDialogModule,
        MatButtonModule,
        MatFormFieldModule,
        MatInputModule,
        ReactiveFormsModule,
        FormsModule,
    ],
})
export class ChoiceMkgtCodeDialogComponent implements OnInit {
    codeMkgtControl: UntypedFormControl;

    constructor(
        public dialogRef: MatDialogRef<ChoiceMkgtCodeDialogComponent>,
        @Inject(MAT_DIALOG_DATA) public data: DialogData
    ) {
        this.codeMkgtControl = new UntypedFormControl(
            this.data.codeMkgt || '',
            [Validators.required, this.codeMkgtValidator.bind(this)]
        );
    }

    ngOnInit(): void {
        this.markFormControlsAsTouched(this.codeMkgtControl);
    }

    onCancel(): void {
        this.data.codeMkgt = null;
        this.data.selectedEtablissement.codeMkgt = null;
        this.data.selectedEtablissementForm.patchValue(
            this.data.selectedEtablissement
        );
        this.dialogRef.close();
    }

    onCreate(): void {
        this.data.codeMkgt = this.codeMkgtControl.value;
        this.data.selectedEtablissement.codeMkgt = this.codeMkgtControl.value;
        this.data.selectedEtablissementForm.patchValue(
            this.data.selectedEtablissement
        );
        this.data.updateEtablissementClient();
        this.dialogRef.close();
    }

    /**
     * Test si le code MKGT est valide
     * Pour être valide, il doit être composé d'un maximum de 13 caractères alphanumériques (A-Z, 0-9)
     * et ne pas avoir la même valeur que le code MKGT passé par le parent
     */
    codeMkgtValidator(control: FormControl): ValidationErrors | null {
        const codeMkgt = control.value;

        if (codeMkgt == this.data.codeMkgt) {
            return { duplicateCodeMkgt: true };
        }

        if (codeMkgt.length > 13) {
            return { invalidLengthCodeMkgt: true };
        }

        if (!/^[A-Z0-9]{0,13}$/.test(codeMkgt)) {
            return { invalidCodeMkgt: true };
        }
        return null;
    }

    markFormControlsAsTouched(formControl: UntypedFormControl): void {
        formControl.markAsTouched();
    }
}
