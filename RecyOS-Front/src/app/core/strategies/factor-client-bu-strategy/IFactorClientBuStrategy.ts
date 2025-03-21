import {Observable} from "rxjs";
import {FactorClientBu} from "../../../models/factor-client-bu-models/factor-client-bu.type";
import {FactorBatchRequest} from "../../../models/factor-client-bu-models/factor-batch-request.type";

export interface IFactorClientBuStrategy {
    fetchFactorClientBuByIdClient(idClient: number): Observable<FactorClientBu[]>;
    fetchFactorClientBuByIdBu(idBu: number): Observable<FactorClientBu[]>;
    fetchAllFactorClientBu(): Observable<FactorClientBu[]>;
    updateFactorClientBu(request: FactorBatchRequest): Observable<FactorClientBu[]>;
}
