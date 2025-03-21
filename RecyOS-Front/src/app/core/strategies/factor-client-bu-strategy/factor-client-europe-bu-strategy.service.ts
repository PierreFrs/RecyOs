import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { IFactorClientBuStrategy } from "./IFactorClientBuStrategy";
import { FactorClientEuropeBuService } from "../../services/factor-client-bu-services/factor-client-europe-bu.service";
import { FactorClientBu } from "../../../models/factor-client-bu-models/factor-client-bu.type";
import {FactorBatchRequest} from "../../../models/factor-client-bu-models/factor-batch-request.type";

@Injectable({
    providedIn: 'root'
})
export class FactorClientEuropeBuStrategyService implements IFactorClientBuStrategy {

    constructor(private _factorClientEuropeBuService: FactorClientEuropeBuService) { }

    fetchFactorClientBuByIdClient(idClient: number): Observable<FactorClientBu[]> {
        return this._factorClientEuropeBuService.fetchFactorClientBuByIdClient(idClient).pipe(
            catchError(this.handleError)
        );
    }

    fetchFactorClientBuByIdBu(idBu: number): Observable<FactorClientBu[]> {
        return this._factorClientEuropeBuService.fetchFactorClientBuByIdBu(idBu).pipe(
            catchError(this.handleError)
        );
    }

    fetchAllFactorClientBu(): Observable<FactorClientBu[]> {
        return this._factorClientEuropeBuService.fetchAllFactorClientBu().pipe(
            catchError(this.handleError)
        );
    }

    updateFactorClientBu(request: FactorBatchRequest): Observable<FactorClientBu[]> {
        return this._factorClientEuropeBuService.updateFactorClientBu(request).pipe(
            catchError(this.handleError)
        );
    }

    private handleError(error: any): Observable<never> {
        console.error('An error occurred', error);
        throw error;
    }
}
