import { Injectable } from '@angular/core';
import {
    ActivatedRouteSnapshot,
    Resolve,
    RouterStateSnapshot,
} from '@angular/router';
import { Observable } from 'rxjs';

import { BusinessUnitServices } from '../services/business-unit.service';
import { BusinessUnitDto } from '../../models/business-unit.type';

@Injectable({
    providedIn: 'root',
})
export class BusinessUnitResolver implements Resolve<BusinessUnitDto[]> {
    /**
     * Constructor
     */
    constructor(private _businessUnitServices: BusinessUnitServices) {}
    resolve(
        route: ActivatedRouteSnapshot,
        state: RouterStateSnapshot,
    ): Observable<BusinessUnitDto[]> {
        return this._businessUnitServices.getBusinessUnitList();
    }
}
