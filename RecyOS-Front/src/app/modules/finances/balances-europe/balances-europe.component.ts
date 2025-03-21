import { ChangeDetectionStrategy, Component, OnInit, ViewChild, ViewEncapsulation } from '@angular/core';
import { BalancesEuropeService } from './balances-europe.service';
import { BalanceEurope, BalanceEuropeGridResponse } from './balances-europe.type';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { Observable, Subject, takeUntil } from 'rxjs';
import { FormBuilder, FormGroup } from '@angular/forms';
import { SocieteService } from 'app/core/services/societe.service';
import { SocieteDto } from 'app/models/societe.type';
import { CommerciauxService } from 'app/core/services/commerciaux.service';
import { Commercial } from 'app/models/commercial.type';
import { map } from 'rxjs/operators';
import { BalancePaginator } from '../balances-france/balances-france.types';

@Component({
    selector       : 'finances-balances-europe',
    templateUrl    : './balances-europe.component.html',
    styleUrls      : ['./balances-europe.component.scss'],
    encapsulation  : ViewEncapsulation.None,
    changeDetection: ChangeDetectionStrategy.OnPush
})
export class BalancesEuropeComponent implements OnInit {
    @ViewChild(MatPaginator) private _paginator: MatPaginator;
    @ViewChild(MatSort) private _sort: MatSort;

    balances$: Observable<BalanceEurope[]>;
    pagination$: Observable<BalancePaginator>;
    societes$: Observable<SocieteDto[]>;
    commerciaux$: Observable<Commercial[]>;
    commerciauxMap: Map<number, Commercial> = new Map();
    sommeTotal$: Observable<number>;
    
    displayedColumns: string[] = [
        'etablissementClient.nom', 
        'societe.nom',
        'commercial',
        'montant',
        'encoursMax',
        'dateRecuperationBalance'
    ];
    filterForm: FormGroup;
    private readonly _unsubscribeAll: Subject<any> = new Subject<any>();

    constructor(
        public balancesService: BalancesEuropeService,
        private readonly _formBuilder: FormBuilder,
        private readonly _societeService: SocieteService,
        private readonly _commerciauxService: CommerciauxService
    ) {
        this.filterForm = this._formBuilder.group({
            clientId: [''],
            societeId: [''],
            commercialId: ['']
        });
    }

    ngOnInit(): void {
        this.balances$ = this.balancesService.balances$;
        this.pagination$ = this.balancesService.pagination$;
        this.sommeTotal$ = this.balancesService.sommeTotal$;
        
        // Récupérer et trier les sociétés par ordre alphabétique
        this.societes$ = this._societeService.getSocietes().pipe(
            map(societes => 
                societes.sort((a, b) => a.nom.localeCompare(b.nom))
            )
        );

        // Récupérer et trier les commerciaux
        this.commerciaux$ = this._commerciauxService.fetchCommerciaux().pipe(
            map(commerciaux => {
                const sortedCommerciaux = commerciaux.sort((a, b) => {
                    const lastNameCompare = a.lastname.localeCompare(b.lastname);
                    if (lastNameCompare !== 0) {
                        return lastNameCompare;
                    }
                    return a.firstname.localeCompare(b.firstname);
                });

                this.commerciauxMap = new Map(
                    sortedCommerciaux.map(commercial => [commercial.id, commercial])
                );
                
                return sortedCommerciaux;
            })
        );

        // Charger les données initiales
        this._getBalancesData();

        // S'abonner aux changements de filtres
        this.filterForm.valueChanges
            .pipe(takeUntil(this._unsubscribeAll))
            .subscribe(() => {
                this._getBalancesData();
            });
    }

    ngAfterViewInit(): void {
        if (this._sort && this._paginator) {
            this._sort.sortChange.subscribe(() => this._getBalancesData());
            this._paginator.page.subscribe(() => this._getBalancesData());
        }
    }

    ngOnDestroy(): void {
        this._unsubscribeAll.next(null);
        this._unsubscribeAll.complete();
    }

    private _getBalancesData(): void {
        const params = {
            FilterClientName: this.filterForm.get('clientId').value,
            FilterBySocieteId: this.filterForm.get('societeId').value,
            FilterByClientCommercialId: this.filterForm.get('commercialId').value,
            PageNumber: this._paginator ? this._paginator.pageIndex + 1 : 1,
            PageSize: this._paginator ? this._paginator.pageSize : 10,
            SortBy: this._sort ? this._sort.active : 'dateRecuperationBalance',
            OrderBy: this._sort ? this._sort.direction : 'desc'
        };

        // Nettoyage des paramètres vides
        Object.keys(params).forEach(key => {
            if (params[key] === '' || params[key] === null || params[key] === undefined) {
                delete params[key];
            }
        });

        this.balancesService.getBalancesGrid(params).subscribe();
    }

    isOverLimit(balance: BalanceEurope): boolean {
        let value = balance.montant * -1;
        if (value <= 0) {
            return false;
        }
        return Math.abs(value) > balance.clientEurope.encoursMax;
    }

    getCommercial(commercialId: number): Commercial | undefined {
        return this.commerciauxMap.get(commercialId);
    }
} 