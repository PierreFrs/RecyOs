import { ChangeDetectionStrategy, Component, OnInit, ViewChild, ViewEncapsulation } from '@angular/core';
import { BalancesParticuliersService } from './balances-particuliers.service';
import { BalanceParticulier } from './balances-particuliers.types';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { Observable, Subject, takeUntil } from 'rxjs';
import { FormBuilder, FormGroup } from '@angular/forms';
import { SocieteService } from 'app/core/services/societe.service';
import { SocieteDto } from 'app/models/societe.type';
import { map } from 'rxjs/operators';
import { BalancePaginator } from '../balances-france/balances-france.types';

@Component({
    selector       : 'finances-balances-particuliers',
    templateUrl    : './balances-particuliers.component.html',
    styleUrls      : ['./balances-particuliers.component.scss'],
    encapsulation  : ViewEncapsulation.None,
    changeDetection: ChangeDetectionStrategy.OnPush
})
export class BalancesParticuliersComponent implements OnInit {
    @ViewChild(MatPaginator) private _paginator: MatPaginator;
    @ViewChild(MatSort) private _sort: MatSort;

    balances$: Observable<BalanceParticulier[]>;
    pagination$: Observable<BalancePaginator>;
    societes$: Observable<SocieteDto[]>;
    sommeTotal$: Observable<number>;
    
    displayedColumns: string[] = [
        'clientParticulier.nom', 
        'societe.nom',
        'montant',
        'dateRecuperationBalance'
    ];
    filterForm: FormGroup;
    private readonly _unsubscribeAll: Subject<any> = new Subject<any>();

    constructor(
        public balancesService: BalancesParticuliersService,
        private readonly _formBuilder: FormBuilder,
        private readonly _societeService: SocieteService
    ) {
        this.filterForm = this._formBuilder.group({
            clientId: [''],
            societeId: ['']
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

    isOverLimit(balance: BalanceParticulier): boolean {
        let value = balance.montant * -1;
        if (value <= 0) {
            return false;
        }
        return Math.abs(value) > balance.clientParticulier.encoursMax;
    }
} 