import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { BehaviorSubject, Observable, tap } from 'rxjs';
import { BalanceParticulier, BalanceParticulierGridResponse } from './balances-particuliers.types';
import { BackendApiService } from 'app/backend.api.service';
import { BalancePaginator } from '../balances-france/balances-france.types';

@Injectable({
    providedIn: 'root'
})
export class BalancesParticuliersService {
    private readonly _balances: BehaviorSubject<BalanceParticulier[]> = new BehaviorSubject<BalanceParticulier[]>(null);
    private readonly _pagination: BehaviorSubject<BalancePaginator> = new BehaviorSubject<BalancePaginator>(null);
    private readonly _sommeTotal: BehaviorSubject<number> = new BehaviorSubject<number>(0);
    private readonly _baseUrl = this._apiService.getBaseUrl();

    constructor(
        private readonly _httpClient: HttpClient,
        private readonly _apiService: BackendApiService
    ) { }

    get balances$(): Observable<BalanceParticulier[]> {
        return this._balances.asObservable();
    }

    get pagination$(): Observable<BalancePaginator> {
        return this._pagination.asObservable();
    }

    get sommeTotal$(): Observable<number> {
        return this._sommeTotal.asObservable();
    }

    getBalancesGrid(params: BalanceGridParams): Observable<BalanceParticulierGridResponse> {
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

        return this._httpClient.get<BalanceParticulierGridResponse>(`${this._baseUrl}/balances-particuliers/grid`, { params: httpParams })
            .pipe(
                tap((response) => {
                    this._balances.next(response.items);
                    this._pagination.next(response.paginator);
                    this._sommeTotal.next(response.sommeTotal);
                })
            );
    }
}

export interface BalanceGridParams {
    FilterClientName?: string;
    FilterBySocieteId?: string;
    FilterByClientCommercialId?: string;
    PageNumber?: number;
    PageSize?: number;
    SortBy?: string;
    OrderBy?: string;
} 