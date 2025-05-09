import { Component, Inject } from '@angular/core';
import {
    MAT_DIALOG_DATA,
    MatDialogModule,
    MatDialogRef,
} from '@angular/material/dialog';
import { MatButtonModule } from '@angular/material/button';

@Component({
    selector: 'app-confirm-groupe-delete',
    templateUrl: './confirm-groupe-delete.component.html',
    standalone: true,
    imports: [MatDialogModule, MatButtonModule],
})
export class ConfirmGroupeDeleteComponent {
    constructor(
        public dialogRef: MatDialogRef<ConfirmGroupeDeleteComponent>,
        @Inject(MAT_DIALOG_DATA) public data: { groupName: string }
    ) {}
}
