import {Injectable} from '@angular/core';
import {
    ActivatedRouteSnapshot,
    Resolve,
    RouterStateSnapshot,
} from '@angular/router';
import { CommerciauxService } from '../../../core/services/commerciaux.service';
import {catchError, map, Observable, of, switchMap} from 'rxjs';
import {
    ClientDTO,
    ClientDTOPagination,
} from '../../../models/entities-models/client.type';

@Injectable({
    providedIn: 'root',
})
export class CustomerWithoutSalesAgentResolver implements Resolve<any> {
    constructor(private _commerciauxService: CommerciauxService) {}
    resolve(
        route: ActivatedRouteSnapshot,
        state: RouterStateSnapshot,
    ): Observable<{
        paginator: ClientDTOPagination;
        items: ClientDTO[];
    }> {
        return this._commerciauxService.fetchCommerciaux().pipe(
            map(commerciauxList => {
                    const sansCommercial = commerciauxList.find(
                        (commercial) => commercial.email === 'null@null.null'
                    );
                    if (!sansCommercial)
                    {
                        throw new Error('Commercial sans email non trouvé');
                    }
                    return sansCommercial.id;
                }),
            switchMap(sansCommercialId =>
    this._commerciauxService.fetchClientsByCommercialId(
        sansCommercialId,
            0,
            10,
            'Siret',
            'asc',
        )
            ),
            catchError(error => {
                console.error('Error fetching commercial ID or clients', error);
                const paginator: ClientDTOPagination = {
                    length: 0,
                    size: 10,
                    page: 0,
                    lastPage: 0,
                    startIndex: 0,
                    cost: 0,
                };
                return of({ paginator, items: [] });
            })
        );
    }
}
