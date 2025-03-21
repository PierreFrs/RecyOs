import { Injectable } from '@angular/core';
import { IBalanceServiceStrategy } from './iBalance-service-strategy';
import { BalanceService } from '../../services/balance.service';
import { Observable } from 'rxjs';
import { BalanceDto } from '../../../models/balances.type';

@Injectable({
    providedIn: 'root',
})
export class BalanceFranceServiceStrategy implements IBalanceServiceStrategy {
    constructor(private _balanceService: BalanceService) {}
    getBalancesByClientId(clientId: number): Observable<BalanceDto[]> {
        return this._balanceService.getBalancesFrance(clientId);
    }
}
