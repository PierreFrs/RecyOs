import {Component} from "@angular/core";
import {MatDialog, MatDialogModule} from "@angular/material/dialog";
import {RefreshSirenDialogComponent} from "./refresh-siren-dialog/refresh-siren-dialog.component";
import {
    OperationsCardComponent
} from "../../../../shared/cards/referentiels-cards/operations-card/operations-card.component";

@Component({
    selector: 'refresh-siren',
    templateUrl: './refresh-siren.component.html',
    standalone: true,
    imports: [
        OperationsCardComponent,
        MatDialogModule
    ]
})
export class RefreshSirenComponent {
    constructor(private readonly _matDialog: MatDialog) {

    }

    RefreshSirenBySiret(): void {
        this._matDialog.open(RefreshSirenDialogComponent, {
            width: '700px',
        });
    }

}
