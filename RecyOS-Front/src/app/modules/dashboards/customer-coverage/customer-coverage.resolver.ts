import { Injectable } from '@angular/core';
import {
    ActivatedRouteSnapshot,
    Resolve,
    RouterStateSnapshot,
} from '@angular/router';
import { BusinessCouvertureServices } from '../../../shared/components/entity-details/business-couverture/business-couverture.service';
import { Observable } from 'rxjs';
import {
    EntrepriseCouvertureDTO,
    EntrepriseCouvertureDTOPagination,
} from '../../../shared/components/entity-details/business-couverture/business-couverture.type';

@Injectable({
    providedIn: 'root',
})
export class DashboardCustomerCoverageResolver implements Resolve<any> {
    /**
     * Constructor
     */
    constructor(
        private _customerCoverageDashboardService: BusinessCouvertureServices,
    ) {}

    resolve(
        route: ActivatedRouteSnapshot,
        state: RouterStateSnapshot,
    ): Observable<{
        paginator: EntrepriseCouvertureDTOPagination;
        items: EntrepriseCouvertureDTO[];
    }> {
        const mode = route.queryParams.mode;
        if (mode === 'all') {
            return this._customerCoverageDashboardService.getCouvertureClients(
                0,
                10,
                'Siren',
                'asc',
            );
        } else if (mode === 'accord') {
            return this._customerCoverageDashboardService.getCouvertureClients(
                0,
                10,
                'Siren',
                'asc',
                'on',
                1,
            );
        } else if (mode === 'refus') {
            return this._customerCoverageDashboardService.getCouvertureClients(
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
