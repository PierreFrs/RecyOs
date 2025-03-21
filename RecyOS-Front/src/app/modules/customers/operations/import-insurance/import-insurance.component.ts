import { Component } from '@angular/core';
import { OperationsServices } from '../operations.services';
import {
    OperationsCardComponent
} from "../../../../shared/cards/referentiels-cards/operations-card/operations-card.component";

@Component({
    selector: 'import-insurance',
    templateUrl: './import-insurance.component.html',
    standalone: true,
    imports: [ OperationsCardComponent ],
})
export class ImportInsuranceComponent {
    constructor(private readonly _operationsServices: OperationsServices) {}

    handleAlianzTradeFileInput(file: File) {
        if (file) {
            this._operationsServices
                .importAlianz(file)
                .subscribe((response) => {
                    console.log(response);
                });
        }
    }

    handleNDCoverFileInput(file: File) {
        if (file) {
            this._operationsServices
                .importNDCover(file)
                .subscribe((response) => {
                    console.log(response);
                });
        }
    }

    handleNDCoverFileExportFrance() {
        this._operationsServices
            .getNDCoverSoumissionExportFileFrance()
            .subscribe({
                next: (blob: Blob) => {
                    const blobUrl = URL.createObjectURL(blob);
                    const a = document.createElement('a');
                    a.href = blobUrl;
                    a.download = 'NDCover_France_Soumission_Export'; // Set a default filename here
                    document.body.appendChild(a); // Append to the body
                    a.click();
                    document.body.removeChild(a); // Remove the element after clicking
                    URL.revokeObjectURL(blobUrl);
                },
                error: (error) => {
                    console.error('Error downloading the Excel file:', error);
                },
            });
    }

    handleNDCoverErrorFileImport(file: File) {
        if (file) {
            this._operationsServices
                .importNDCoverErrorFile(file)
                .subscribe((response) => {
                    console.log(response);
                });
        }
    }
}
