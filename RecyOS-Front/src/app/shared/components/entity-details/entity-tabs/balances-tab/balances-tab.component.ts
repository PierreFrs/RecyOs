import { Component, Input, OnInit } from '@angular/core';
import { BalanceService } from '../../../../../core/services/balance.service';
import { ClientDTO } from '../../../../../models/entities-models/client.type';
import { Observable } from 'rxjs';

@Component({
    selector: 'app-balances-tab',
    templateUrl: './balances-tab.component.html',
})
export class BalancesTabComponent implements OnInit {
    @Input() selectedClient: ClientDTO;
    emptyData: boolean = false;
    loading: boolean = true;

    constructor(private balancesService: BalanceService) {}

    ngOnInit(): void {
        this.checkForBalances();
    }

    private checkForBalances(): void {
        const serviceMethod = this.determineClientRegion();

        if (!serviceMethod) {
            console.error('Unsupported client type');
            this.loading = false;
            return;
        }

        serviceMethod(this.selectedClient.id).subscribe({
            next: (balances) => {
                this.loading = false;
                if (balances.length === 0) {
                    this.emptyData = true;
                }
            },
            error: (error) => {
                console.error('Error getting associated balances:', error);
                this.loading = false;
            },
        });
    }

    private determineClientRegion():
        | ((clientId: number) => Observable<any>)
        | null {
        if ('siret' in this.selectedClient) {
            return this.balancesService.getBalancesFrance.bind(
                this.balancesService,
            );
        } else if ('vat' in this.selectedClient) {
            return this.balancesService.getBalancesEurope.bind(
                this.balancesService,
            );
        }

        console.error('Unsupported client type');
        return null;
    }
}
