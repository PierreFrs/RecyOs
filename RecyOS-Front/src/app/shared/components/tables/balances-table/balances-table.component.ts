import {
    AfterViewInit,
    Component,
    Input,
    OnInit,
    ViewChild,
} from '@angular/core';
import { SocieteDto } from '../../../../models/societe.type';
import { ActivatedRoute } from '@angular/router';
import { BalanceFranceServiceStrategy } from '../../../../core/strategies/balances-strategies/balance-france-service-strategy';
import { BalanceEuropeServiceStrategy } from '../../../../core/strategies/balances-strategies/balance-europe-service-strategy';
import { IBalanceServiceStrategy } from '../../../../core/strategies/balances-strategies/iBalance-service-strategy';
import { BalanceDto } from '../../../../models/balances.type';
import { SocieteBalanceDisplay } from '../../../../models/societe-balance-display.type';
import { MatTableDataSource } from '@angular/material/table';
import { LiveAnnouncer } from '@angular/cdk/a11y';
import { MatSort, Sort } from '@angular/material/sort';
import { ClientDTO } from '../../../../models/entities-models/client.type';

@Component({
    selector: 'app-balances-table',
    templateUrl: './balances-table.component.html',
})
export class BalancesTableComponent implements OnInit, AfterViewInit {
    @ViewChild(MatSort) sort: MatSort;

    @Input() selectedClient: ClientDTO;

    societes: SocieteDto[] = [];
    associatedBalances: BalanceDto[] = [];
    displayedColumns: string[] = [
        'extractionDate',
        'balanceOwed',
        'societeName',
    ];
    dataSource = new MatTableDataSource<SocieteBalanceDisplay>([]);

    private balanceServiceStrategy: IBalanceServiceStrategy;

    constructor(
        private _route: ActivatedRoute,
        private _balanceFranceServiceStrategy: BalanceFranceServiceStrategy,
        private _balanceEuropeServiceStrategy: BalanceEuropeServiceStrategy,
        private _liveAnnouncer: LiveAnnouncer,
    ) {}

    ngOnInit() {
        this.importResolversData();
        this.balanceServiceStrategy = this.determineStrategy(
            this.selectedClient,
        );

        this.fetchBalances();
    }

    ngAfterViewInit(): void {
        this.dataSource.sort = this.sort;
    }

    private importResolversData(): void {
        this._route.data.subscribe((data: { societes: SocieteDto[] }) => {
            this.societes = data.societes;
        });
    }

    private determineStrategy(client: ClientDTO): IBalanceServiceStrategy {
        if ('siret' in client) {
            return this._balanceFranceServiceStrategy;
        } else if ('vat' in client) {
            return this._balanceEuropeServiceStrategy;
        }
    }

    private fetchBalances() {
        this.balanceServiceStrategy
            .getBalancesByClientId(this.selectedClient.id)
            .subscribe({
                next: (balances) => {
                    this.associatedBalances = balances;
                    this.dataSource.data = this.mergeSocietesAndBalances();
                },
                error: (error) => {
                    console.error('Error getting associated balances:', error);
                },
            });
    }

    private mergeSocietesAndBalances(): SocieteBalanceDisplay[] {
        return this.societes
            .map((societe) => {
                const balance = this.associatedBalances.find(
                    (balance) => balance.societeId === societe.id,
                );
                return {
                    societeName: societe.nom,
                    balanceOwed: balance ? balance.montant : 0,
                    extractionDate: balance
                        ? balance.dateRecuperationBalance
                        : '',
                };
            })
            .filter((item) => item.balanceOwed > 0);
    }

    getTotal(): number {
        return this.dataSource.data.reduce(
            (acc, curr) => acc + curr.balanceOwed,
            0,
        );
    }

    announceSortChange(sortState: Sort): void {
        if (sortState.direction) {
            this._liveAnnouncer.announce(
                `Sorted ${
                    sortState.direction === 'asc' ? 'ascending' : 'descending'
                }`,
            );
        } else {
            this._liveAnnouncer.announce('Sorting cleared');
        }
    }
}
