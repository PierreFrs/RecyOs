import { Injectable } from '@angular/core';
import {HttpClient} from "@angular/common/http";
import {BackendApiService} from "../../../backend.api.service";
import {FactorClientBu} from "../../../models/factor-client-bu-models/factor-client-bu.type";
import {catchError, Observable, switchMap, throwError} from "rxjs";
import {FactorBatchRequest} from "../../../models/factor-client-bu-models/factor-batch-request.type";

@Injectable({
  providedIn: 'root'
})
export class FactorClientEuropeBuService {
    private readonly baseUrl = this._apiService.getBaseUrl();
  constructor(
      private readonly httpClient: HttpClient,
      private readonly _apiService: BackendApiService,
  ) { }

    public fetchFactorClientBuByIdClient(idClient: number): Observable<FactorClientBu[]> {
        return this.httpClient.get<FactorClientBu[]>(
            `${this.baseUrl}/factor_client_europe_bu/client/${idClient}`
        );
    }

    public fetchFactorClientBuByIdBu(idBu: number): Observable<FactorClientBu[]> {
        return this.httpClient.get<FactorClientBu[]>(
            `${this.baseUrl}/factor_client_europe_bu/bu/${idBu}`
        );
    }

    public fetchAllFactorClientBu(): Observable<FactorClientBu[]> {
        return this.httpClient.get<FactorClientBu[]>(
            `${this.baseUrl}/factor_client_europe_bu`
        );
    }

    public updateFactorClientBu(request: FactorBatchRequest): Observable<FactorClientBu[]> {
        return this.httpClient.put<void>(
            `${this.baseUrl}/factor_client_europe_bu/factor-batch-update`,
            request
        ).pipe(
            switchMap(() => this.fetchFactorClientBuByIdClient(request.clientId)),
            catchError(error => {
                console.error('Update failed:', error);
                return throwError(() => (error));
            })
        );
    }
}
