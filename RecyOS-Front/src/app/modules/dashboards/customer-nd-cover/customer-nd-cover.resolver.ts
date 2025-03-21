import { Injectable } from '@angular/core';
import {
    ActivatedRouteSnapshot,
    Resolve,
    RouterStateSnapshot,
} from '@angular/router';
import { BusinessNDCoverServices } from '../../../core/services/business-nd-cover.service';
import { Observable } from 'rxjs';
import {
    EntrepriseNDCoverDTO,
    EntrepriseNDCoverDTOPagination,
} from '../../../models/business-nd-cover.type';

@Injectable({
    providedIn: 'root',
})
export class DashboardCustomerNDCoverResolver implements Resolve<any> {
    /**
     * Constructor
     */
    constructor(
        private _customerNDCoverDashboardService: BusinessNDCoverServices,
    ) {}

    resolve(
        route: ActivatedRouteSnapshot,
        state: RouterStateSnapshot,
    ): Observable<{
        paginator: EntrepriseNDCoverDTOPagination;
        items: EntrepriseNDCoverDTO[];
    }> {
        const mode = route.queryParams.mode;
        if (mode === 'all') {
            return this._customerNDCoverDashboardService.getNDCoverClients(
                0,
                10,
                'Siren',
                'asc',
            );
        } else if (mode === 'accord') {
            return this._customerNDCoverDashboardService.getNDCoverClients(
                0,
                10,
                'Siren',
                'asc',
                'on',
                1,
            );
        } else if (mode === 'refus') {
            return this._customerNDCoverDashboardService.getNDCoverClients(
                0,
                10,
                'Siren',
                'asc',
                'on',
                2,
            );
        }
        return null;
    }
}
