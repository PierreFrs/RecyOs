import {Component, Inject, OnInit} from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { NewEntityDialogComponent } from '../../../shared/components/dialogs/new-entity-dialog-component/new-entity-dialog.component';

@Component({
    selector: 'odoo-creation-waiting-dialog',
    templateUrl: './ErpCreationWaitingDialog.html',
})
export class ErpCreationWaitingDialog implements OnInit {
    erpType = this.data.erpType;

    constructor(
        public dialogRef: MatDialogRef<NewEntityDialogComponent>,
        @Inject(MAT_DIALOG_DATA) public data: { erpType: string },
    ) {}

    ngOnInit(): void {
        this.erpType = this.data.erpType;
        console.log(this.erpType);
    }
}
