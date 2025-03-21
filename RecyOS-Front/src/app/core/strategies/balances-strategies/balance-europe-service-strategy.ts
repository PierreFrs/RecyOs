import { IBalanceServiceStrategy } from './iBalance-service-strategy';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { BalanceService } from '../../services/balance.service';
import { BalanceDto } from '../../../models/balances.type';

@Injectable({
    providedIn: 'root',
})
export class BalanceEuropeServiceStrategy implements IBalanceServiceStrategy {
    constructor(private _balanceService: BalanceService) {}
    getBalancesByClientId(clientId: number): Observable<BalanceDto[]> {
        return this._balanceService.getBalancesEurope(clientId);
    }
}
