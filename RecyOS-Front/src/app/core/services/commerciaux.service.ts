import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { BackendApiService } from '../../backend.api.service';
import { Commercial, CommercialPagination } from '../../models/commercial.type';
import {
    BehaviorSubject,
    catchError,
    Observable,
    Subject,
    tap,
    throwError,
} from 'rxjs';
import {
    ClientDTO,
    ClientDTOPagination,
} from '../../models/entities-models/client.type';

@Injectable({
    providedIn: 'root',
})
export class CommerciauxService {
    private readonly _commerciaux: BehaviorSubject<Commercial[] | null> =
        new BehaviorSubject(null);
    private readonly _pagination: BehaviorSubject<CommercialPagination> =
        new BehaviorSubject(null);
    private readonly clientsByCommercialId: BehaviorSubject<ClientDTO[]> =
        new BehaviorSubject(null);
    private readonly refreshNeeded$ = new Subject<void>();

    constructor(
        private readonly httpClient: HttpClient,
        private readonly _apiService: BackendApiService
    ) {}

    fetchCommerciaux() {
        const baseUrl = this._apiService.getBaseUrl();
        return this.httpClient.get<Commercial[]>(`${baseUrl}/commerciaux`).pipe(
            tap((commerciaux) => {
                this._commerciaux.next(commerciaux);
            })
        );
    }

    fetchCommerciauxWithPagination(
        page: number = 0,
        size: number = 10,
        sort: string = 'lastname',
        order: 'asc' | 'desc' | '' = 'asc',
        search: string = '',
        filterType: number = 0
    ): Observable<{
        paginator: CommercialPagination;
        items: Commercial[];
    }> {
        const baseUrl = this._apiService.getBaseUrl();
        let filterParam = this.getFilterParam(filterType);

        const params: { [key: string]: string } = {
            PageNumber: '' + page,
            PageSize: '' + size,
            SortBy: '' + sort,
            OrderBy: '' + order,
            Search: '' + search,
            FilterType: '' + filterType,
        };
        if (search) {
            params[filterParam] = search;
        }

        return this.httpClient
            .get<{
                paginator: CommercialPagination;
                items: Commercial[];
            }>(`${baseUrl}/commerciaux/filtered`, {
                params: params,
            })
            .pipe(
                tap((response) => {
                    this._pagination.next(response.paginator);
                    this._commerciaux.next(response.items);
                })
            );
    }

    fetchClientsByCommercialId(
        commercialId: number,
        page: number = 0,
        size: number = 10,
        sort: string = 'Nom',
        order: 'asc' | 'desc' | '' = 'asc',
        search: string = '',
        searchFilterType: number = 0,
        unitFilterType: number = 0
    ): Observable<{
        paginator: ClientDTOPagination;
        items: ClientDTO[];
    }> {
        const baseUrl = this._apiService.getBaseUrl();
        let searchFilterParam =
            this.getSearchFilterParamClientsByCommercialId(searchFilterType);
        let unitFilterParam =
            this.getUnitFilterParamClientsByCommercialId(unitFilterType);

        const params: { [key: string]: string } = {
            PageNumber: '' + page,
            PageSize: '' + size,
            SortBy: '' + sort,
            OrderBy: '' + order,
        };

        if (search) {
            params[searchFilterParam] = search;
        }

        if (unitFilterParam) {
            params[unitFilterParam] = 'true';
        }

        return this.httpClient
            .get<{
                paginator: ClientDTOPagination;
                items: ClientDTO[];
            }>(`${baseUrl}/commerciaux/${commercialId}/clients`, {
                params: params,
            })
            .pipe(
                tap((response) => {
                    this.clientsByCommercialId.next(response.items);
                    this._pagination.next(response.paginator);
                })
            );
    }

    getSearchFilterParamClientsByCommercialId(filterType: number = 0): string {
        if (filterType === 0) {
            return 'SearchByNom';
        } else if (filterType === 1) {
            return 'SearchByIdentifiant';
        } else if (filterType === 2) {
            return 'SearchByCodeMkgt';
        } else if (filterType === 3) {
            return 'SearchByIdOdoo';
        } else if (filterType === 4) {
            return 'SearchByCodeGpi';
        } else {
            return '';
        }
    }

    getUnitFilterParamClientsByCommercialId(filterType: number = 0): string {
        if (filterType === 0) {
            return '';
        } else if (filterType === 1) {
            return 'FilterByMkgt';
        } else if (filterType === 2) {
            return 'FilterByGpi';
        }
    }

    get commerciaux$(): Observable<Commercial[] | null> {
        return this._commerciaux.asObservable();
    }

    get clientsByCommercialId$(): Observable<ClientDTO[]> {
        return this.clientsByCommercialId.asObservable();
    }

    get pagination$(): Observable<CommercialPagination> {
        return this._pagination.asObservable();
    }

    getFilterParam(filterType: number = 0): string {
        if (filterType === 0) {
            return 'FilteredByNom';
        } else if (filterType === 1) {
            return 'FilteredByPrenom';
        } else {
            return '';
        }
    }

    fetchCommercialById(id: number): Observable<Commercial> {
        const baseUrl = this._apiService.getBaseUrl();
        return this.httpClient.get<Commercial>(`${baseUrl}/commerciaux/${id}`);
    }

    createCommercial(commercial: Commercial): Observable<Commercial> {
        const baseUrl = this._apiService.getBaseUrl();
        const formData = new FormData();

        Object.keys(commercial).forEach((key) => {
            if (commercial[key] !== null) {
                formData.append(key, commercial[key]);
            }
        });

        return this.httpClient
            .post<Commercial>(`${baseUrl}/commerciaux`, formData)
            .pipe(
                tap(() => {
                    this.refreshNeeded$.next();
                }),
                catchError((error) => {
                    return throwError(() => error);
                })
            );
    }

    updateCommercial(id: number, form: FormData): Observable<Commercial> {
        const baseUrl = this._apiService.getBaseUrl();
        return this.httpClient.put<Commercial>(
            `${baseUrl}/commerciaux/${id}`,
            form
        );
    }

    deleteCommercial(id: number): Observable<void> {
        const baseUrl = this._apiService.getBaseUrl();
        return this.httpClient
            .delete<void>(`${baseUrl}/commerciaux/${id}`)
            .pipe(
                tap({
                    complete: () => {
                        this.refreshNeeded$.next();
                    },
                })
            );
    }

    get refreshNeeded() {
        return this.refreshNeeded$.asObservable();
    }
}
