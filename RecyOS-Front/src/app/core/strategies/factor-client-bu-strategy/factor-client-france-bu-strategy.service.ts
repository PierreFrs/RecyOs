import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { IFactorClientBuStrategy } from "./IFactorClientBuStrategy";
import { FactorClientFranceBuService } from "../../services/factor-client-bu-services/factor-client-france-bu.service";
import { FactorClientBu } from "../../../models/factor-client-bu-models/factor-client-bu.type";
import {FactorBatchRequest} from "../../../models/factor-client-bu-models/factor-batch-request.type";

@Injectable({
    providedIn: 'root'
})
export class FactorClientFranceBuStrategyService implements IFactorClientBuStrategy {

    constructor(private _factorClientFranceBuService: FactorClientFranceBuService) { }

    fetchFactorClientBuByIdClient(idClient: number): Observable<FactorClientBu[]> {
        return this._factorClientFranceBuService.fetchFactorClientBuByIdClient(idClient).pipe(
            catchError(this.handleError)
        );
    }

    fetchFactorClientBuByIdBu(idBu: number): Observable<FactorClientBu[]> {
        return this._factorClientFranceBuService.fetchFactorClientBuByIdBu(idBu).pipe(
            catchError(this.handleError)
        );
    }

    fetchAllFactorClientBu(): Observable<FactorClientBu[]> {
        return this._factorClientFranceBuService.fetchAllFactorClientBu().pipe(
            catchError(this.handleError)
        );
    }

    updateFactorClientBu(request: FactorBatchRequest): Observable<FactorClientBu[]> {
        return this._factorClientFranceBuService.updateFactorClientBu(request).pipe(
            catchError(this.handleError)
        );
    }

    private handleError(error: any): Observable<never> {
        console.error('An error occurred', error);
        throw error;
    }
}
