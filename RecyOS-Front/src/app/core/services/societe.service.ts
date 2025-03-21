import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { BackendApiService } from '../../backend.api.service';
import { BehaviorSubject, Observable, Subject, catchError, tap, throwError } from 'rxjs';
import { SocieteDto, SocieteGridResponse } from '../../models/societe.type';

export interface SocieteGridParams {
    FilterByNom?: string;
    FilterBySocieteId?: string;
    FilterByIdOdoo?: string;
    PageNumber?: number;
    PageSize?: number;
    SortBy?: string;
    OrderBy?: string;
}

@Injectable({
    providedIn: 'root',
})
export class SocieteService {
    private readonly _baseUrl: string;
    private readonly _societes: BehaviorSubject<SocieteDto[]> = new BehaviorSubject(null);
    private readonly _pagination: BehaviorSubject<{totalCount: number}> = new BehaviorSubject(null);
    private readonly refreshNeeded$ = new Subject<void>();

    constructor(
        private readonly _httpClient: HttpClient,
        private readonly _apiService: BackendApiService,
    ) {
        this._baseUrl = this._apiService.getBaseUrl();
    }

    // Récupérer la liste des sociétés
    getSocietes(): Observable<SocieteDto[]> {
        return this._httpClient.get<SocieteDto[]>(`${this._baseUrl}/societes`).pipe(
            tap((societes) => {
                this._societes.next(societes);
            }),
            catchError((error) => {
                return throwError(() => error);
            })
        );
    }

    // Créer une nouvelle société
    createSociete(societe: SocieteDto): Observable<SocieteDto> {
        return this._httpClient.post<SocieteDto>(`${this._baseUrl}/societes`, societe).pipe(
            tap((newSociete) => {
                this.refreshNeeded$.next();
                if (newSociete) {
                    const currentSocietes = this._societes.getValue();
                    if (currentSocietes) {
                        this._societes.next([...currentSocietes, newSociete]);
                    }
                }
            }),
            catchError((error) => {
                return throwError(() => error);
            })
        );
    }

    // Récupérer une société par son ID
    getSocieteById(id: number): Observable<SocieteDto> {
        return this._httpClient.get<SocieteDto>(`${this._baseUrl}/societes/${id}`).pipe(
            catchError((error) => {
                return throwError(() => error);
            })
        );
    }

    // Mettre à jour une société
    updateSociete(id: number, societe: SocieteDto): Observable<SocieteDto> {
        return this._httpClient.put<SocieteDto>(`${this._baseUrl}/societes/${id}`, societe).pipe(
            tap(() => {
                this.refreshNeeded$.next();
            }),
            catchError((error) => {
                return throwError(() => error);
            })
        );
    }

    // Supprimer une société
    deleteSociete(id: number): Observable<void> {
        return this._httpClient.delete<void>(`${this._baseUrl}/societes/${id}`).pipe(
            tap(() => {
                this.refreshNeeded$.next();
                const currentSocietes = this._societes.getValue();
                if (currentSocietes) {
                    this._societes.next(currentSocietes.filter(s => s.id !== id));
                }
            }),
            catchError((error) => {
                return throwError(() => error);
            })
        );
    }

    // Récupérer la grille des sociétés avec pagination et filtres
    getSocietesGrid(params?: SocieteGridParams): Observable<SocieteGridResponse> {
        let httpParams = new HttpParams();

        if (params) {
            if (params.FilterByNom) {
                httpParams = httpParams.set('FilterByNom', params.FilterByNom);
            }
            if (params.FilterBySocieteId) {
                httpParams = httpParams.set('FilterBySocieteId', params.FilterBySocieteId);
            }
            if (params.FilterByIdOdoo) {
                httpParams = httpParams.set('FilterByIdOdoo', params.FilterByIdOdoo);
            }
            if (params.PageNumber !== undefined) {
                httpParams = httpParams.set('PageNumber', params.PageNumber.toString());
            }
            if (params.PageSize !== undefined) {
                httpParams = httpParams.set('PageSize', params.PageSize.toString());
            }
            if (params.SortBy) {
                httpParams = httpParams.set('SortBy', params.SortBy);
            }
            if (params.OrderBy) {
                httpParams = httpParams.set('OrderBy', params.OrderBy);
            }
        }

        return this._httpClient.get<SocieteGridResponse>(`${this._baseUrl}/societes/grid`, { params: httpParams }).pipe(
            tap((response) => {
                this._societes.next(response.items);
                this._pagination.next({ totalCount: response.paginator?.length || response.totalCount || 0 });
            }),
            catchError((error) => {
                return throwError(() => error);
            })
        );
    }

    // Getters pour les observables
    get societes$(): Observable<SocieteDto[]> {
        return this._societes.asObservable();
    }

    get pagination$(): Observable<{totalCount: number}> {
        return this._pagination.asObservable();
    }

    get refreshNeeded() {
        return this.refreshNeeded$.asObservable();
    }
}
