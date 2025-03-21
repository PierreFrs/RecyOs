import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { BehaviorSubject, Observable, tap } from 'rxjs';
import { BalanceFrance, BalanceFranceGridResponse, BalanceGridParams, BalancePaginator } from './balances-france.types';
import { BackendApiService } from 'app/backend.api.service';

@Injectable({
    providedIn: 'root'
})
export class BalancesFranceService {
    private readonly _balances: BehaviorSubject<BalanceFrance[]> = new BehaviorSubject<BalanceFrance[]>(null);
    private readonly _pagination: BehaviorSubject<BalancePaginator> = new BehaviorSubject<BalancePaginator>(null);
    private readonly _sommeTotal: BehaviorSubject<number> = new BehaviorSubject<number>(0);
    private readonly _baseUrl = this._apiService.getBaseUrl();

    constructor(
        private readonly _httpClient: HttpClient,
        private readonly _apiService: BackendApiService
    ) { }

    // Getters pour les observables
    get balances$(): Observable<BalanceFrance[]> {
        return this._balances.asObservable();
    }

    get pagination$(): Observable<BalancePaginator> {
        return this._pagination.asObservable();
    }

    get sommeTotal$(): Observable<number> {
        return this._sommeTotal.asObservable();
    }

    // Obtenir toutes les balances
    getBalances(): Observable<BalanceFrance[]> {
        return this._httpClient.get<BalanceFrance[]>(`${this._baseUrl}/balances-france`)
            .pipe(
                tap((balances) => {
                    this._balances.next(balances);
                })
            );
    }

    // Obtenir une balance par ID
    getBalanceById(id: number): Observable<BalanceFrance> {
        return this._httpClient.get<BalanceFrance>(`${this._baseUrl}/balances-france/${id}`);
    }

    // Obtenir les balances d'un client
    getBalancesByClientId(clientId: number): Observable<BalanceFrance> {
        return this._httpClient.get<BalanceFrance>(`${this._baseUrl}/balances-france/client/${clientId}`);
    }

    // Créer une nouvelle balance
    createBalance(balance: BalanceFrance): Observable<BalanceFrance> {
        return this._httpClient.post<BalanceFrance>(`${this._baseUrl}/balances-france`, balance)
            .pipe(
                tap((newBalance) => {
                    const currentBalances = this._balances.getValue();
                    if (currentBalances) {
                        this._balances.next([...currentBalances, newBalance]);
                    }
                })
            );
    }

    // Mettre à jour une balance
    updateBalance(id: number, balance: BalanceFrance): Observable<BalanceFrance> {
        return this._httpClient.put<BalanceFrance>(`${this._baseUrl}/balances-france/${id}`, balance)
            .pipe(
                tap((updatedBalance) => {
                    const currentBalances = this._balances.getValue();
                    if (currentBalances) {
                        const index = currentBalances.findIndex(b => b.id === id);
                        if (index !== -1) {
                            currentBalances[index] = updatedBalance;
                            this._balances.next([...currentBalances]);
                        }
                    }
                })
            );
    }

    // Supprimer une balance
    deleteBalance(id: number): Observable<BalanceFrance> {
        return this._httpClient.delete<BalanceFrance>(`${this._baseUrl}/balances-france/${id}`)
            .pipe(
                tap(() => {
                    const currentBalances = this._balances.getValue();
                    if (currentBalances) {
                        const filteredBalances = currentBalances.filter(b => b.id !== id);
                        this._balances.next(filteredBalances);
                    }
                })
            );
    }

    // Obtenir les balances avec pagination et filtres
    getBalancesGrid(params: BalanceGridParams): Observable<BalanceFranceGridResponse> {
        let httpParams = new HttpParams();

        // Mise à jour pour utiliser FilterClientName au lieu de FilterByClientId
        if (params.FilterClientName) {
            httpParams = httpParams.set('FilterClientName', params.FilterClientName);
        }
        if (params.FilterBySocieteId) {
            httpParams = httpParams.set('FilterBySocieteId', params.FilterBySocieteId);
        }
        if (params.FilterByClientCommercialId) {
            httpParams = httpParams.set('FilterByClientCommercialId', params.FilterByClientCommercialId);
        }
        if (params.PageNumber !== undefined) {
            httpParams = httpParams.set('PageNumber', params.PageNumber.toString());
        }
        if (params.PageSize !== undefined) {
            httpParams = httpParams.set('PageSize', params.PageSize.toString());
        }
        if (params.SortBy) {
            httpParams = httpParams.set('SortBy', params.SortBy);
        }
        if (params.OrderBy) {
            httpParams = httpParams.set('OrderBy', params.OrderBy);
        }

        return this._httpClient.get<BalanceFranceGridResponse>(`${this._baseUrl}/balances-france/grid`, { params: httpParams })
            .pipe(
                tap((response) => {
                    this._balances.next(response.items);
                    this._pagination.next(response.paginator);
                    this._sommeTotal.next(response.sommeTotal);
                })
            );
    }
}