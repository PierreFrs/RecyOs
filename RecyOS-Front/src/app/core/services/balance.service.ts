import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { BackendApiService } from '../../backend.api.service';
import { Observable } from 'rxjs';
import { BalanceDto } from '../../models/balances.type';

@Injectable({
    providedIn: 'root',
})
export class BalanceService {
    constructor(
        private readonly _httpClient: HttpClient,
        private readonly _apiService: BackendApiService,
    ) {}

    getBalancesFrance(customerId: number): Observable<BalanceDto[]> {
        const baseUrl = this._apiService.getBaseUrl();
        return this._httpClient.get<BalanceDto[]>(
            `${baseUrl}/balances-france/client/${customerId}`,
        );
    }

    getBalancesEurope(customerId: number): Observable<BalanceDto[]> {
        const baseUrl = this._apiService.getBaseUrl();
        return this._httpClient.get<BalanceDto[]>(
            `${baseUrl}/balances-europe/client/${customerId}`,
        );
    }
}
