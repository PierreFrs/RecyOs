import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { BehaviorSubject, Observable, tap } from 'rxjs';
import { BalanceEurope, BalanceEuropeGridResponse, BalanceGridParams } from './balances-europe.type';
import { BackendApiService } from 'app/backend.api.service';
import { BalancePaginator } from '../balances-france/balances-france.types';

@Injectable({
    providedIn: 'root'
})
export class BalancesEuropeService {
    private readonly _balances: BehaviorSubject<BalanceEurope[]> = new BehaviorSubject<BalanceEurope[]>(null);
    private readonly _pagination: BehaviorSubject<BalancePaginator> = new BehaviorSubject<BalancePaginator>(null);
    private readonly _sommeTotal: BehaviorSubject<number> = new BehaviorSubject<number>(0);
    private readonly _baseUrl = this._apiService.getBaseUrl();

    constructor(
        private readonly _httpClient: HttpClient,
        private readonly _apiService: BackendApiService
    ) { }

    get balances$(): Observable<BalanceEurope[]> {
        return this._balances.asObservable();
    }

    get pagination$(): Observable<BalancePaginator> {
        return this._pagination.asObservable();
    }

    get sommeTotal$(): Observable<number> {
        return this._sommeTotal.asObservable();
    }

    getBalances(): Observable<BalanceEurope[]> {
        return this._httpClient.get<BalanceEurope[]>(`${this._baseUrl}/balances-europe`)
            .pipe(
                tap((balances) => {
                    this._balances.next(balances);
                })
            );
    }

    getBalanceById(id: number): Observable<BalanceEurope> {
        return this._httpClient.get<BalanceEurope>(`${this._baseUrl}/balances-europe/${id}`);
    }

    getBalancesByClientId(clientId: number): Observable<BalanceEurope> {
        return this._httpClient.get<BalanceEurope>(`${this._baseUrl}/balances-europe/client/${clientId}`);
    }

    createBalance(balance: BalanceEurope): Observable<BalanceEurope> {
        return this._httpClient.post<BalanceEurope>(`${this._baseUrl}/balances-europe`, balance)
            .pipe(
                tap((newBalance) => {
                    const currentBalances = this._balances.getValue();
                    if (currentBalances) {
                        this._balances.next([...currentBalances, newBalance]);
                    }
                })
            );
    }

    updateBalance(id: number, balance: BalanceEurope): Observable<BalanceEurope> {
        return this._httpClient.put<BalanceEurope>(`${this._baseUrl}/balances-europe/${id}`, balance)
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

    deleteBalance(id: number): Observable<BalanceEurope> {
        return this._httpClient.delete<BalanceEurope>(`${this._baseUrl}/balances-europe/${id}`)
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

    getBalancesGrid(params: BalanceGridParams): Observable<BalanceEuropeGridResponse> {
        let httpParams = new HttpParams();

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

        return this._httpClient.get<BalanceEuropeGridResponse>(`${this._baseUrl}/balances-europe/grid`, { params: httpParams })
            .pipe(
                tap((response) => {
                    this._balances.next(response.items);
                    this._pagination.next(response.paginator);
                    this._sommeTotal.next(response.sommeTotal);
                })
            );
    }
}
