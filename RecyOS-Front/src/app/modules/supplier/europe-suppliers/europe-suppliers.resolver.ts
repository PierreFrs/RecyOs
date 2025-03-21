import { Injectable } from '@angular/core';
import {
    ActivatedRouteSnapshot,
    Resolve,
    RouterStateSnapshot,
} from '@angular/router';
import { Observable } from 'rxjs';
import {
    EuropeDTO,
    EuropeDTOPagination,
} from '../../../models/entities-models/europe.type';
import { EuropeSuppliersService } from '../../../core/services/entity-services/europe-suppliers.service';

@Injectable({
    providedIn: 'root',
})
export class EuropeSuppliersResolver implements Resolve<any> {
    constructor(private _europeSuppliers: EuropeSuppliersService) {}
    resolve(
        route: ActivatedRouteSnapshot,
        state: RouterStateSnapshot,
    ): Observable<{
        paginator: EuropeDTOPagination;
        items: EuropeDTO[];
    }> {
        return this._europeSuppliers.getSuppliersEurope(0, 10);
    }
}
