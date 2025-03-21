import { Component, Inject } from '@angular/core';
import {
    MAT_DIALOG_DATA,
    MatDialogRef,
    MatDialogModule,
} from '@angular/material/dialog';
import { MatButtonModule } from '@angular/material/button';
@Component({
    selector: 'app-confirm-commercial-delete',
    templateUrl: './confirm-commercial-delete.component.html',
    standalone: true,
    imports: [MatDialogModule, MatButtonModule],
})
export class ConfirmCommercialDeleteComponent {
    constructor(
        public dialogRef: MatDialogRef<ConfirmCommercialDeleteComponent>,
        @Inject(MAT_DIALOG_DATA) public data: { commercialName: string }
    ) {}
}
