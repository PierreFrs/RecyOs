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
} from '../../models/entities-models/europe.type';
import { EuropeServices } from '../services/entity-services/europe.services';

@Injectable({
    providedIn: 'root',
})
export class EuropeResolver implements Resolve<any> {
    /**
     * Constructor
     */
    constructor(private _europeServices: EuropeServices) {}

    resolve(
        route: ActivatedRouteSnapshot,
        state: RouterStateSnapshot,
    ): Observable<{
        paginator: EuropeDTOPagination;
        items: EuropeDTO[];
    }> {
        return this._europeServices.getClientsEurope(0, 10);
    }
}
