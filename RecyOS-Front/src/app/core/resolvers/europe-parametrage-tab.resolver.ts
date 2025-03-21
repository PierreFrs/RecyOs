import { Injectable } from '@angular/core';
import {
    ActivatedRouteSnapshot,
    Resolve,
    RouterStateSnapshot,
} from '@angular/router';
import { Observable } from 'rxjs';
import { EuropeServices } from '../services/entity-services/europe.services';
import { CategorieClientDto } from '../../models/entities-models/europe.type';

@Injectable({
    providedIn: 'root',
})
export class EuropeParametrageTabResolver implements Resolve<any> {
    /**
     * Constructor
     */
    constructor(private _europeServices: EuropeServices) {}
    resolve(
        route: ActivatedRouteSnapshot,
        state: RouterStateSnapshot,
    ): Observable<CategorieClientDto[]> {
        return this._europeServices.getCategorieClientList();
    }
}
