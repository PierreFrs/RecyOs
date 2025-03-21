import { Injectable } from '@angular/core';
import {
    ActivatedRouteSnapshot,
    Resolve,
    RouterStateSnapshot,
} from '@angular/router';
import { Observable } from 'rxjs';
import { CategorieClientDto } from '../../models/entities-models/french.type';
import { BusinessesServices } from '../services/entity-services/businesses.services';

@Injectable({
    providedIn: 'root',
})
export class FranceParametrageTabResolver implements Resolve<any> {
    /**
     * Constructor
     */
    constructor(private _businessesServices: BusinessesServices) {}
    resolve(
        route: ActivatedRouteSnapshot,
        state: RouterStateSnapshot,
    ): Observable<CategorieClientDto[]> {
        return this._businessesServices.getCategorieClientList();
    }
}
