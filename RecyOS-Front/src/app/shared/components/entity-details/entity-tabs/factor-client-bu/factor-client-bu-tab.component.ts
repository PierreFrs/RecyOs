import {Component, Input} from '@angular/core';
import {ClientDTO} from "../../../../../models/entities-models/client.type";
import {SharedModule} from "../../../../shared.module";
import {
    FactorClientBuDualListboxComponent
} from "./factor-client-bu-dual-listbox/factor-client-bu-dual-listbox.component";
import {Observable} from "rxjs";

@Component({
    selector: 'app-factor-client-bu-tab',
    templateUrl: './factor-client-bu-tab.component.html',
    standalone: true,
    imports: [
        SharedModule,
        FactorClientBuDualListboxComponent
    ]
})
export class FactorClientBuTabComponent {
    @Input() selectedClient: ClientDTO;
    @Input() updateFactorSignal: Observable<void>;
}
