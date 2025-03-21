import {Component, Inject} from '@angular/core';
import {MAT_DIALOG_DATA, MatDialogModule, MatDialogRef} from "@angular/material/dialog";
import {FormBuilder, FormControl, FormGroup, ReactiveFormsModule} from "@angular/forms";
import {MatInputModule} from "@angular/material/input";
import {MatButtonModule} from "@angular/material/button";
import {NgIf} from "@angular/common";

@Component({
    selector: 'app-mkgt-code-dialog',
    templateUrl: './mkgt-code-dialog.component.html',
    imports: [
        MatInputModule,
        ReactiveFormsModule,
        MatDialogModule,
        MatButtonModule,
        NgIf
    ],
    standalone: true
})
export class MkgtCodeDialogComponent {
    form: FormGroup;
    showModifyInput = false;

    constructor(
        public dialogRef: MatDialogRef<MkgtCodeDialogComponent>,
        @Inject(MAT_DIALOG_DATA) public data: { codeMkgt: string },
        private readonly fb : FormBuilder
    ) {
        this.form = this.fb.group({
            codeMkgt: new FormControl(data.codeMkgt)
        });
    }

    onOverwrite(): void {
        this.dialogRef.close({action: 'overwrite'});
    }

    onModify(): void {
        this.showModifyInput = !this.showModifyInput;
    }

    onValidate(): void {
        const modifiedCode = (this.form.get('codeMkgt') as FormControl)?.value;
        console.log('Validating with modified code:', modifiedCode);
        this.dialogRef.close({ action: 'modify', codeMkgt: modifiedCode });
    }

    // Getter to cast form control properly
    get codeMkgtControl(): FormControl {
        return this.form.get('codeMkgt') as FormControl;
    }
}
