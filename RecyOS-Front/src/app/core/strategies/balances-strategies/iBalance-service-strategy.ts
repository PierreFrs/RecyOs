import { BalanceDto } from '../../../models/balances.type';
import { Observable } from 'rxjs';

export interface IBalanceServiceStrategy {
    getBalancesByClientId(clientId: number): Observable<BalanceDto[]>;
}
