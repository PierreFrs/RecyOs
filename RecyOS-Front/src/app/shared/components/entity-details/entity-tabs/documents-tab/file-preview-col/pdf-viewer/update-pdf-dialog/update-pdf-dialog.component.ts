import { Component, Inject, Input, OnInit } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { FormBuilder, FormGroup } from '@angular/forms';
import { TypeDocumentPdf } from '../../../../../../../../models/file-type.model';
import { DialogData } from '../../../../../../../../models/france-files-tab.models';
import { IFilesServiceStrategy } from '../../../../../../../../core/strategies/files-strategies/IFilesServiceStrategy';
import { EntityStrategyService } from '../../../../../../../../core/strategies/entity-strategy/entity-strategy.service';

@Component({
    selector: 'app-update-pdf-dialog',
    templateUrl: './update-pdf-dialog.component.html',
})
export class UpdatePdfDialogComponent implements OnInit {
    @Input() documentId: number;

    selectedFile: File = null;
    types: TypeDocumentPdf[] = this.data.pdfTypes;
    selectedEntity = this.data.selectedEntity;
    isChanged: boolean = false;
    updateForm: FormGroup;
    private filesServiceStrategy: IFilesServiceStrategy;

    constructor(
        public dialogRef: MatDialogRef<UpdatePdfDialogComponent>,
        @Inject(MAT_DIALOG_DATA) public data: DialogData,
        private _entityStrategyService: EntityStrategyService,
        private formBuilder: FormBuilder,
    ) {}

    ngOnInit(): void {
        this.filesServiceStrategy =
            this._entityStrategyService.determineFilesStrategyFromEntity(
                this.selectedEntity,
            );
        this.updateForm = this.formBuilder.group({
            selectedFile: [null],
            selectedType: [null],
        });

        this.updateForm.valueChanges.subscribe(() => {
            const selectedType = this.updateForm.get('selectedType').value;
            const selectedFile = this.updateForm.get('selectedFile').value;
            this.isChanged = !!(selectedType || selectedFile);
        });
    }

    onNoClick(): void {
        this.dialogRef.close();
    }

    onFileChange(event: any): void {
        this.selectedFile = <File>event.target.files[0];
        this.isChanged = true;
        console.log(this.isChanged);
    }

    onTypeChange(event: any): void {
        this.updateForm.patchValue({
            selectedType: event.value,
        });
    }

    onSubmit(): void {
        if (!this.isChanged) {
            console.error(
                'At least one field must be changed fot the upload to be valid',
            );
            return;
        }

        const formData = new FormData();
        const selectedType = this.updateForm.get('selectedType').value;

        if (selectedType != null) {
            formData.append('typeDocumentPdfId', selectedType);
        }
        if (this.selectedFile) {
            formData.append('file', this.selectedFile);
        }

        if (this.data.customerId != null) {
            formData.append(
                'etablissementClientId',
                this.data.customerId.toString(),
            );
        }

        this.filesServiceStrategy
            .updatePdfDocument(this.data.documentId, formData)
            .subscribe({
                next: (response) => {
                    this.filesServiceStrategy.pdfUpdated.emit(response.id);
                    this.dialogRef.close(response);
                },
                error: (error) => {
                    console.error('Error while updating PDF:', error);
                },
            });
    }

    retryFile(): void {
        const fileInput = document.getElementById(
            'fileUpload',
        ) as HTMLInputElement;
        fileInput.click();
    }

    cancelFile(): void {
        this.selectedFile = null;
        this.updateForm.reset();
    }
}
