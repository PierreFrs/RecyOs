import {Component} from "@angular/core";
import {MkgtCustomerCodeDialogComponent} from "./mkgt-customer-code-dialog/mkgt-customer-code-dialog.component";
import {MatDialog} from "@angular/material/dialog";
import {OperationsServices} from "../operations.services";
import {
    OperationsCardComponent
} from "../../../../shared/cards/referentiels-cards/operations-card/operations-card.component";

@Component({
    selector: 'import-customers',
    templateUrl: './import-customers.component.html',
    standalone: true,
    imports: [
        OperationsCardComponent
    ]
})
export class ImportCustomersComponent {

    constructor(private readonly _matDialog: MatDialog, private readonly _operationsServices: OperationsServices) {

    }

    ImportCustomerMkgtByCode(): void {
        this._matDialog.open(MkgtCustomerCodeDialogComponent, {
            width: '700px',
        });
    }

    ImportAllValidMkgtCustomers(): void {
        this._operationsServices.importAllValidMkgtCustomers().subscribe((response) => {
            console.log(response);
        }
        );
    }
}
