import { Injectable } from '@angular/core';
import { DashboardCustomerService } from './customer.service';
import {
    ActivatedRouteSnapshot,
    Resolve,
    RouterStateSnapshot,
} from '@angular/router';
import { BusinessesServices } from '../../../core/services/entity-services/businesses.services';
import { Observable } from 'rxjs';
import {
    FrenchDTO,
    EtablissementDTOPagination,
} from '../../../models/entities-models/french.type';
import { EuropeServices } from '../../../core/services/entity-services/europe.services';
import { CategorieClientDto } from '../../../models/entities-models/europe.type';
import { FileTypeService } from '../../../core/services/file-type.service';
import { TypeDocumentPdf } from '../../../models/file-type.model';
import { CommerciauxService } from '../../../core/services/commerciaux.service';
import {
    ClientDTO,
    ClientDTOPagination,
} from '../../../models/entities-models/client.type';

@Injectable({
    providedIn: 'root',
})
export class DashboardCustomerResolver implements Resolve<any> {
    /**
     * Constructor
     */
    constructor(private _customerDashboardService: DashboardCustomerService) {}

    resolve() {
        return this._customerDashboardService.getDashboartCustomer();
    }
}

@Injectable({
    providedIn: 'root',
})
export class DashboardCustomerListResolver implements Resolve<any> {
    /**
     * Constructor
     */
    constructor(
        private _businessesServices: BusinessesServices,
        private _commerciauxService: CommerciauxService,
    ) {}
    resolve(
        route: ActivatedRouteSnapshot,
        state: RouterStateSnapshot,
    ): Observable<{
        paginator: EtablissementDTOPagination | ClientDTOPagination;
        items: FrenchDTO[] | ClientDTO[];
    }> {
        const mode = route.queryParams.mode;
        if (mode === 'badmail') {
            return this._businessesServices.getEtablissementsClients(
                0,
                10,
                'Siret',
                'asc',
                'radiationDate',
                4,
            );
        } else if (mode === 'badtel') {
            return this._businessesServices.getEtablissementsClients(
                0,
                10,
                'Siret',
                'asc',
                'radiationDate',
                5,
            );
        } else if (mode === 'factor') {
            return this._businessesServices.getEtablissementsClients(
                0,
                10,
                'Siret',
                'asc',
                'radiationDate',
                6,
            );
        } else if (mode === 'radie') {
            return this._businessesServices.getEtablissementsClients(
                0,
                10,
                'Siret',
                'asc',
                'radiationDate',
                7,
            );
        }
        return this._businessesServices.getEtablissementsClients(0, 1);
    }
}

@Injectable({
    providedIn: 'root',
})
export class DashboardCustomerListCategoryResolver implements Resolve<any> {
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

@Injectable({
    providedIn: 'root',
})
export class DashboardCustomerListTypesResolver implements Resolve<any> {
    /**
     * Constructor
     */
    constructor(private _fileTypeService: FileTypeService) {}
    resolve(): Observable<TypeDocumentPdf[]> {
        return this._fileTypeService.fetchAllPdfTypes();
    }
}
