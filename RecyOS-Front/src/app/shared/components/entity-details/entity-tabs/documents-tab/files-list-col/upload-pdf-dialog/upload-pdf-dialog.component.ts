import { Component, Inject, OnInit } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { TypeDocumentPdf } from '../../../../../../../models/file-type.model';
import { DialogData } from '../../../../../../../models/france-files-tab.models';
import { IFilesServiceStrategy } from '../../../../../../../core/strategies/files-strategies/IFilesServiceStrategy';
import { EntityStrategyService } from '../../../../../../../core/strategies/entity-strategy/entity-strategy.service';

@Component({
    selector: 'app-upload-pdf-dialog',
    templateUrl: './upload-pdf-dialog.component.html',
})
export class UploadPdfDialogComponent implements OnInit {
    selectedFile: File = null;
    types: TypeDocumentPdf[] = this.data.pdfTypes;
    selectedEntity = this.data.selectedEntity;
    uploadForm: FormGroup;
    private filesServiceStrategy: IFilesServiceStrategy;

    constructor(
        public dialogRef: MatDialogRef<UploadPdfDialogComponent>,
        @Inject(MAT_DIALOG_DATA) public data: DialogData,
        private readonly _entityStrategyService: EntityStrategyService,
        private readonly formBuilder: FormBuilder,
    ) {}

    ngOnInit(): void {
        this.filesServiceStrategy =
            this._entityStrategyService.determineFilesStrategyFromEntity(
                this.selectedEntity,
            );
        this.uploadForm = this.formBuilder.group({
            type: ['', Validators.required],
            file: [null, Validators.required],
        });
    }

    onNoClick(): void {
        this.dialogRef.close();
    }

    onFileChange(event: any): void {
        this.selectedFile = <File>event.target.files[0];
        this.uploadForm.patchValue({ file: this.selectedFile });
    }

    onSubmit(): void {
        if (this.uploadForm.invalid) return;

        const formData = new FormData();
        formData.append('typeDocumentPdfId', this.uploadForm.value.type);
        const clientIdKey = this.isEuropeanClient()
            ? 'etablissementClientEuropeId'
            : 'etablissementClientId';
        formData.append(clientIdKey, String(this.data.customerId));
        formData.append('file', this.selectedFile);

        this.filesServiceStrategy.createPdfDocument(formData).subscribe({
            next: (response) => {
                this.filesServiceStrategy.pdfCreated.emit(response.id);
                this.dialogRef.close(response);
            },
            error: (error) => {
                console.error('Error while creating PDF:', error);
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
        this.uploadForm.reset();
    }

    private isEuropeanClient(): boolean {
        return (
            this._entityStrategyService.determineStrategyFromEntity(
                this.selectedEntity,
            ).region === 'europe'
        );
    }
}
