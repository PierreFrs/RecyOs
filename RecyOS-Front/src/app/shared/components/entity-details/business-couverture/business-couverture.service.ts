import { Injectable } from '@angular/core';
import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { BackendApiService } from '../../../../backend.api.service';
import { BehaviorSubject, map, Observable, tap } from 'rxjs';

import { of } from 'rxjs';
import { catchError } from 'rxjs/operators';
import {
    EntrepriseCouvertureDTO,
    EntrepriseCouvertureDTOPagination,
} from './business-couverture.type';

@Injectable({
    providedIn: 'root',
})
export class BusinessCouvertureServices {
    private _couvertureClients: BehaviorSubject<
        EntrepriseCouvertureDTO[] | null
    > = new BehaviorSubject(null);
    private _pagination: BehaviorSubject<EntrepriseCouvertureDTOPagination | null> =
        new BehaviorSubject(null);
    /**
     * Constructor
     */
    constructor(
        private _httpClient: HttpClient,
        private apiService: BackendApiService,
    ) {}

    /**
     * Get Couverture Clients
     * @param {number} [page=0] - The current page (default is 0)
     * @param {number} [size=10] - The number of items per page (default is 10)
     * @param {string} [sort='Siret'] - The sorting field (default is 'Siret')
     * @param {'asc' | 'desc' | ''} [order='asc'] - The order of sorting: 'asc' or 'desc' (default is 'asc')
     * @param {string} [search=''] - The search keyword (default is an empty string)
     * @param {number} [filterType=0] - The filter type, based on the user's choice (default is 0):
     *                                  0: all coverages query
     *                                  1: agreement coverages query
     *                                  2: refusal coverages query
     * @returns {Observable<{ paginator: EntrepriseCouvertureDTOPagination; items: EntrepriseCouvertureDTO[] }>}
     *         An Observable containing the pagination data and the list of EntrepriseCouvertureDTO items
     */
    getCouvertureClients(
        page: number = 0,
        size: number = 10,
        sort: string = 'Siret',
        order: 'asc' | 'desc' | '' = 'asc',
        search: string = '',
        filterType: number = 0,
    ): Observable<{
        paginator: EntrepriseCouvertureDTOPagination;
        items: EntrepriseCouvertureDTO[];
    }> {
        const baseUrl = this.apiService.getBaseUrl();
        let filterParam = '';

        switch (filterType) {
            case 0:
                filterParam = 'FiltredByNom';
                break;
            case 1:
                filterParam = 'Agreement';
                break;
            case 2:
                filterParam = 'Refus';
                break;
        }

        const params: { [key: string]: string } = {
            PageNumber: '' + page,
            PageSize: '' + size,
            SortBy: '' + sort,
            OrderBy: '' + order,
        };

        if (search) {
            params[filterParam] = search;
        }

        return this._httpClient
            .get<{
                paginator: EntrepriseCouvertureDTOPagination;
                items: EntrepriseCouvertureDTO[];
            }>(`${baseUrl}/entreprise_couverture`, {
                params,
            })
            .pipe(
                tap((response) => {
                    this._couvertureClients.next(response.items);
                    this._pagination.next(response.paginator);
                }),
            );
    }

    /**
     * Get Etablissement couverture by SIRET
     * @param {string} siret
     * @returns {Observable<EntrepriseCouvertureDTO>}
     */
    getEtablissementCouvertureBySiret(
        siret: string,
    ): Observable<EntrepriseCouvertureDTO> {
        const siretWithoutHyphens = siret.replace(/-/g, '');
        // On extrait les neuf premiers chiffres pour obtenir le SIREN
        const siren = siretWithoutHyphens.substring(0, 9);
        const baseUrl = this.apiService.getBaseUrl();
        return this._httpClient
            .get<EntrepriseCouvertureDTO>(
                `${baseUrl}/entreprise_couverture/siren/${siren}`,
                {
                    observe: 'response',
                },
            )
            .pipe(
                map((response) => response.body),
                catchError((error: HttpErrorResponse) => {
                    if (error.status === 404) {
                        // If it's a 404 error, return null (or an empty object as needed)
                        return of(null);
                    }
                    // For other errors, rethrow them to be handled elsewhere
                    throw error;
                }),
            );
    }

    /**
     * Get Etablissement couverture by Id
     * @param {number} id - The unique identifier of the Etablissement Couverture to retrieve
     * @returns {Observable<EntrepriseCouvertureDTO>} An Observable of the EtablissementCouvertureDTO containing the Etablissement Couverture data
     */
    getEtablissementCouvertureById(
        id: number,
    ): Observable<EntrepriseCouvertureDTO> {
        const baseUrl = this.apiService.getBaseUrl();
        return this._httpClient.get<EntrepriseCouvertureDTO>(
            `${baseUrl}/entreprise_couverture/${id}`,
        );
    }

    /**
     * Get the Couverture Clients stream
     * @returns An Observable stream of EntrepriseCouvertureDTO[] or null
     */
    get couvertureClients$(): Observable<EntrepriseCouvertureDTO[] | null> {
        return this._couvertureClients.asObservable();
    }

    /**
     * Get the pagination stream for Couverture Clients
     * @returns An Observable stream of EntrepriseCouvertureDTOPagination or null
     */
    get pagination$(): Observable<EntrepriseCouvertureDTOPagination | null> {
        return this._pagination.asObservable();
    }
}
