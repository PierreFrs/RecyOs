import { Injectable } from '@angular/core';
import {
    ActivatedRouteSnapshot,
    Resolve,
    RouterStateSnapshot,
} from '@angular/router';
import { Observable } from 'rxjs';

import {
    FrenchDTO,
    EtablissementDTOPagination,
} from '../../../models/entities-models/french.type';
import { FrenchSuppliersService } from '../../../core/services/entity-services/french-suppliers.service';

@Injectable({
    providedIn: 'root',
})
export class FrenchSuppliersResolver implements Resolve<any> {
    constructor(private _frenchSuppliers: FrenchSuppliersService) {}
    resolve(
        route: ActivatedRouteSnapshot,
        state: RouterStateSnapshot,
    ): Observable<{
        paginator: EtablissementDTOPagination;
        items: FrenchDTO[];
    }> {
        return this._frenchSuppliers.getFrenchSuppliers(0, 10);
    }
}
