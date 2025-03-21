import { Component } from '@angular/core';
import {OperationsServices} from "../operations.services";
import {
    OperationsCardComponent
} from "../../../../shared/cards/referentiels-cards/operations-card/operations-card.component";

@Component({
  selector: 'app-export-factor',
  templateUrl: './export-factor.component.html',
  standalone: true,
  imports: [OperationsCardComponent]

})
export class ExportFactorComponent {

    constructor(private readonly _operationsService: OperationsServices) {}
    handleFactorFolder() {
        this._operationsService.getFactorExportFolder().subscribe({
            next: (blob: Blob) => {
                const blobUrl = URL.createObjectURL(blob);
                const a = document.createElement('a');
                a.href = blobUrl;
                a.download = 'Factor_Export_Folder';
                document.body.appendChild(a);
                a.click();
                document.body.removeChild(a);
                URL.revokeObjectURL(blobUrl);
            },
            error: (error) => {
                console.error('Error downloading the Excel file:', error);
            },
        });
    }
}
