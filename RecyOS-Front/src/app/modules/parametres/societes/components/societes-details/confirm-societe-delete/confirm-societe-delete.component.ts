import { Component, Inject } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';

@Component({
    selector: 'app-confirm-societe-delete',
    templateUrl: './confirm-societe-delete.component.html'
})
export class ConfirmSocieteDeleteComponent {
    constructor(
        public dialogRef: MatDialogRef<ConfirmSocieteDeleteComponent>,
        @Inject(MAT_DIALOG_DATA) public data: { societeName: string }
    ) {}
} 