import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { BehaviorSubject, catchError, map, Observable, Subject, tap, throwError, of} from 'rxjs';
import { Group, GroupPagination } from 'app/models/group.type';
import { BackendApiService } from 'app/backend.api.service';

@Injectable({
    providedIn: 'root',
})

export class GroupService {
    private readonly _groups: BehaviorSubject<Group[]> = new BehaviorSubject(null);
    private readonly _pagination: BehaviorSubject<GroupPagination> =new BehaviorSubject(null);

    private readonly refreshNeeded$ = new Subject<void>();

    constructor(
        private readonly _httpClient: HttpClient,
        private readonly _apiService: BackendApiService
    ) {}

    private readonly baseUrl = this._apiService.getBaseUrl();

    getGroups(): Observable<Group[]> {
        return this._httpClient.get<Group[]>(`${this.baseUrl}/group`);
    }

    getGroupById(id: number): Observable<Group> {
        return this._httpClient.get<Group>(`${this.baseUrl}/group/${id}`).pipe(
            catchError((error) => {
                return throwError(() => error);
            })
        );
    }

    getGroupByName(name: string): Observable<Group> {
        return this._httpClient.get<Group>(
            `${this.baseUrl}/group/name/${name}`
        );
    }

    checkGroupNameExists(name: string): Observable<boolean> {
        if (!name) return of(false);
        return this.getGroupByName(name).pipe(
            map((group) => !!group),
            catchError(() => of(false))
        );
    }

    getPaginatedGroupsWithClients(
        page: number = 0,
        size: number = 10,
        sortField: string = 'Nom',
        sortDirection: 'asc' | 'desc' | '' = 'asc',
        search: string = '',
        filterType: number = 0
    ): Observable<{
        paginator: GroupPagination;
        items: Group[];
    }> {
        let filterParam = this.getFilterParam(filterType);

        // Map the Material sort field names to backend sort fields
        const sortFieldMap: { [key: string]: string } = {
            nom: 'Nom',
            ficheCount: 'FicheCount',
        };

        const params: { [key: string]: string } = {
            PageNumber: page.toString(),
            PageSize: size.toString(),
            SortBy: sortFieldMap[sortField.toLowerCase()] || sortField,
            OrderBy: sortDirection.toLowerCase(),
            Search: search,
        };

        if (search) {
            params[filterParam] = search;
        }

        return this._httpClient
            .get<{
                paginator: GroupPagination;
                items: Group[];
            }>(`${this.baseUrl}/group/filtered`, {
                params: params,
            })
            .pipe(
                tap((response) => {
                    this._pagination.next(response.paginator);
                    this._groups.next(response.items);
                })
            );
    }

    getFilterParam(filterType: number = 0): string {
        if (filterType === 0) {
            return 'FilteredByNom';
        } else {
            return '';
        }
    }

    createGroup(group: Group): Observable<Group> {
        const formData = new FormData();

        Object.keys(group).forEach((key) => {
            if (group[key] !== null) {
                formData.append(key, group[key]);
            }
        });

        return this._httpClient
            .post<Group>(`${this.baseUrl}/group`, formData)
            .pipe(
                tap((newGroup) => {
                    this.refreshNeeded$.next();
                    if (newGroup) {
                        newGroup.ficheCount = 0;
                        const currentGroups = this._groups.getValue();
                        if (currentGroups) {
                            this._groups.next([...currentGroups, newGroup]);
                        }
                    }
                }),
                catchError((error) => {
                    return throwError(() => error);
                })
            );
    }

    updateGroup(id: number, form: FormData): Observable<Group> {
        return this._httpClient.put<Group>(`${this.baseUrl}/group/${id}`, form);
    }

    deleteGroup(id: number): Observable<void> {
        return this._httpClient
            .delete<void>(`${this.baseUrl}/group/${id}`)
            .pipe(
                tap({
                    complete: () => {
                        this.refreshNeeded$.next();
                    },
                })
            );
    }

    get groups$(): Observable<Group[]> {
        return this._groups.asObservable();
    }

    get pagination$(): Observable<GroupPagination> {
        return this._pagination.asObservable();
    }

    get refreshNeeded() {
        return this.refreshNeeded$.asObservable();
    }
}
