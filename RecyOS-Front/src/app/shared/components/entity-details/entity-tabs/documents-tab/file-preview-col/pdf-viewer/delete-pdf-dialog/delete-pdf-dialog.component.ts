import { Component, Inject, OnInit } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { DialogData } from '../../../../../../../../models/france-files-tab.models';
import { IFilesServiceStrategy } from '../../../../../../../../core/strategies/files-strategies/IFilesServiceStrategy';
import { EntityStrategyService } from '../../../../../../../../core/strategies/entity-strategy/entity-strategy.service';

@Component({
    selector: 'app-delete-pdf-dialog',
    templateUrl: './delete-pdf-dialog.component.html',
})
export class DeletePdfDialogComponent implements OnInit {
    selectedEntity = this.data.selectedEntity;
    private filesServiceStrategy: IFilesServiceStrategy;

    constructor(
        private _entityStrategyService: EntityStrategyService,
        public dialogRef: MatDialogRef<DeletePdfDialogComponent>,
        @Inject(MAT_DIALOG_DATA) public data: DialogData,
    ) {}

    ngOnInit(): void {
        this.filesServiceStrategy =
            this._entityStrategyService.determineFilesStrategyFromEntity(
                this.selectedEntity,
            );
    }

    deletePDF() {
        this.filesServiceStrategy
            .deletePdfDocument(this.data.documentId)
            .subscribe({
                next: () => {
                    this.dialogRef.close(true);
                },
                error: (error) => {
                    console.error('An error occurred:', error);
                },
                complete: () => {
                    console.log(
                        'Delete operation completed for ID:',
                        this.data.documentId,
                    );
                },
            });
    }

    onNoClick(): void {
        this.dialogRef.close();
    }
}
