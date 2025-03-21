import { Injectable } from '@angular/core';
import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { BackendApiService } from '../../backend.api.service';
import { BehaviorSubject, map, Observable, tap, of, throwError } from 'rxjs';

import { catchError } from 'rxjs/operators';
import {
    EntrepriseNDCoverDTO,
    EntrepriseNDCoverDTOPagination,
} from '../../models/business-nd-cover.type';

@Injectable({
    providedIn: 'root',
})
export class BusinessNDCoverServices {
    private readonly _ndCover: BehaviorSubject<EntrepriseNDCoverDTO[] | null> =
        new BehaviorSubject(null);
    private readonly _pagination: BehaviorSubject<EntrepriseNDCoverDTOPagination | null> =
        new BehaviorSubject(null);
    /**
     * Constructor
     */
    constructor(
        private readonly _httpClient: HttpClient,
        private readonly apiService: BackendApiService,
    ) {}

    /**
     * Get NDCover Clients
     * @param {number} [page=0] - The current page (default is 0)
     * @param {number} [size=10] - The number of items per page (default is 10)
     * @param {string} [sort='Siret'] - The sorting field (default is 'Siret')
     * @param {'asc' | 'desc' | ''} [order='asc'] - The order of sorting: 'asc' or 'desc' (default is 'asc')
     * @param {string} [search=''] - The search keyword (default is an empty string)
     * @param {number} [filterType=0] - The filter type, based on the user's choice (default is 0):
     *                                  0: all coverages query
     *                                  1: agreement coverages query
     *                                  2: refusal coverages query
     * @returns {Observable<{ paginator: EntrepriseNDCoverDTOPagination; items: EntrepriseNDCoverDTO[] }>}
     *         An Observable containing the pagination data and the list of EntrepriseNDCoverDTO items
     */
    getNDCoverClients(
        page: number = 0,
        size: number = 10,
        sort: string = 'Siret',
        order: 'asc' | 'desc' | '' = 'asc',
        search: string = '',
        filterType: number = 0,
    ): Observable<{
        paginator: EntrepriseNDCoverDTOPagination;
        items: EntrepriseNDCoverDTO[];
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
                paginator: EntrepriseNDCoverDTOPagination;
                items: EntrepriseNDCoverDTO[];
            }>(`${baseUrl}/entreprise-nd-cover`, {
                params,
            })
            .pipe(
                tap((response) => {
                    this._ndCover.next(response.items);
                    this._pagination.next(response.paginator);
                }),
            );
    }

    /**
     * Get Etablissement NDCover by SIRET
     * @param {string} siret
     * @returns {Observable<EntrepriseNDCoverDTO>}
     */
    getEtablissementNDCoverBySiret(
        siret: string,
    ): Observable<EntrepriseNDCoverDTO> {
        const siretWithoutHyphens = siret.replace(/-/g, '');
        // On extrait les neuf premiers chiffres pour obtenir le SIREN
        const siren = siretWithoutHyphens.substring(0, 9);
        const baseUrl = this.apiService.getBaseUrl();
        return this._httpClient
            .get<EntrepriseNDCoverDTO>(
                `${baseUrl}/entreprise-nd-cover/siren/${siren}`,
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
                    return throwError(() => new Error(error.message));
                }),
            );
    }

    /**
     * Get Etablissement NDCover by Id
     * @param {number} id - The unique identifier of the Etablissement NDCover to retrieve
     * @returns {Observable<EntrepriseNDCoverDTO>} An Observable of the EtablissementNDCoverDTO containing the Etablissement NDCover data
     */
    getEtablissementNDCoverById(id: number): Observable<EntrepriseNDCoverDTO> {
        const baseUrl = this.apiService.getBaseUrl();
        return this._httpClient.get<EntrepriseNDCoverDTO>(
            `${baseUrl}/entreprise-nd-cover/${id}`,
        );
    }

    /**
     * Get the NDCover Clients stream
     * @returns An Observable stream of EntrepriseNDCoverDTO[] or null
     */
    get couvertureClients$(): Observable<EntrepriseNDCoverDTO[] | null> {
        return this._ndCover.asObservable();
    }

    /**
     * Get the pagination stream for NDCover Clients
     * @returns An Observable stream of EntrepriseNDCoverDTOPagination or null
     */
    get pagination$(): Observable<EntrepriseNDCoverDTOPagination | null> {
        return this._pagination.asObservable();
    }

    /**
     * Get the NDCover status by SIRET
     *@param {string} siret - The SIRET number
     * @returns {Observable<boolean>} An Observable of a boolean indicating the NDCover status
     */
    getEtablissementNDCoverStatut(siret: string): Observable<boolean> {
        return this.getEtablissementNDCoverBySiret(siret).pipe(
            map((ndCover: EntrepriseNDCoverDTO) => {
                return ndCover.statut === 'Garantie totale';
            }),
            catchError((error: HttpErrorResponse) => {
                if (error.status === 404) {
                    return of(false);
                }
                return of(false);
            }),
        );
    }
}
