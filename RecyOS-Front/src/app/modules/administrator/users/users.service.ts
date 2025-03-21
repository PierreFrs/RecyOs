import { Injectable } from '@angular/core';
import {
    BehaviorSubject,
    catchError,
    filter,
    map,
    mapTo,
    Observable,
    of,
    switchMap,
    take,
    tap,
    throwError,
} from 'rxjs';
import { RoleDto, UserDto, UserDtoPaginator, SignUpDto } from './users.type';
import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { BackendApiService } from '../../../backend.api.service';

@Injectable({
    providedIn: 'root',
})
export class UsersService {
    private readonly _user: BehaviorSubject<UserDto | null> =
        new BehaviorSubject<UserDto | null>(null);
    private readonly _users: BehaviorSubject<UserDto[] | null> =
        new BehaviorSubject<UserDto[] | null>(null);
    private readonly _roles: BehaviorSubject<RoleDto[] | null> =
        new BehaviorSubject<RoleDto[] | null>(null);
    private readonly _pagination: BehaviorSubject<UserDtoPaginator | null> =
        new BehaviorSubject<UserDtoPaginator | null>(null);
    private readonly _baseUrl = this.apiService.getBaseUrl();
    constructor(
        private readonly _httpClient: HttpClient,
        private readonly apiService: BackendApiService
    ) {}

    /**
     * Get Users
     * @param {number} [page=0] - The current page (default is 0)
     * @param {number} [size=10] - The number of items per page (default is 10)
     * @param {string} [sort='Siret'] - The sorting field (default is 'Siret')
     * @param {'asc' | 'desc' | ''} [order='asc'] - The order of sorting: 'asc' or 'desc' (default is 'asc')
     * @param {string} [search=''] - The search keyword (default is an empty string)
     * @returns {Observable<{ paginator: UserDtoPaginator; items: UserDto[] }>}
     *          An Observable containing the pagination data and the list of UserDto items
     */
    getUsers(
        page: number = 0,
        size: number = 10,
        sort: string = 'Username',
        order: 'asc' | 'desc' | '' = 'asc',
        search: string = ''
    ): Observable<{
        paginator: UserDtoPaginator;
        items: UserDto[];
    }> {
        const params: { [key: string]: string } = {
            PageNumber: '' + page,
            PageSize: '' + size,
            SortBy: '' + sort,
            OrderBy: '' + order,
            FilterByLogin: '' + search,
        };

        return this._httpClient
            .get<{
                paginator: UserDtoPaginator;
                items: UserDto[];
            }>(`${this._baseUrl}/users`, {
                params: params,
            })
            .pipe(
                tap(
                    (response: {
                        paginator: UserDtoPaginator;
                        items: UserDto[];
                    }) => {
                        console.log('Response:', response);
                        this._users.next(response.items);
                        this._pagination.next(response.paginator);
                    }
                )
            );
    }

    get users$(): Observable<UserDto[] | null> {
        return this._users.asObservable();
    }

    get pagination$(): Observable<UserDtoPaginator | null> {
        return this._pagination.asObservable();
    }

    get user$(): Observable<UserDto | null> {
        return this._user.asObservable();
    }

    /**
     * Get User by Id
     */
    getUserById(id: number): Observable<UserDto> {
        return this._httpClient
            .get<UserDto>(`${this._baseUrl}/users/${id}`)
            .pipe(
                tap((user: UserDto) => {
                    this._user.next(user);
                })
            );
    }

    /**
     * Get Roles
     */
    getRoles(): Observable<RoleDto[]> {
        return this._httpClient.get<RoleDto[]>(`${this._baseUrl}/roles`).pipe(
            tap((roles: RoleDto[]) => {
                this._roles.next(roles);
            })
        );
    }

    get roles$(): Observable<RoleDto[] | null> {
        return this._roles.asObservable();
    }

    /**
     * Update User
     */
    updateUser(user: UserDto): Observable<UserDto> {
        return this.users$.pipe(
            take(1),
            switchMap((users) =>
                this._httpClient
                    .put<UserDto>(`${this._baseUrl}/users/${user.id}`, user)
                    .pipe(
                        map((updatedUser: UserDto) => {
                            // Find the index of the updated contact
                            const index = users.findIndex(
                                (item) => item.id === user.id
                            );
                            // Update the contacts
                            users[index] = updatedUser;
                            this._users.next(users);
                            return updatedUser;
                        }),
                        switchMap(() =>
                            this.user$.pipe(
                                take(1),
                                filter((item) => item && item.id === user.id),
                                tap((updatedUser: UserDto) => {
                                    this._user.next(user);
                                    return updatedUser;
                                })
                            )
                        )
                    )
            )
        );
    }

    /**
     * Add User role
     */
    addUserRole(userId: number, role: RoleDto): Observable<UserDto> {
        return this.users$.pipe(
            take(1),
            switchMap((users) =>
                this._httpClient
                    .post<UserDto>(`${this._baseUrl}/users/${userId}/roles`, {
                        id: role.id,
                        name: role.name,
                    })
                    .pipe(
                        map((updatedUser: UserDto) => {
                            return updatedUser;
                        }),
                        switchMap(() =>
                            this.user$.pipe(
                                take(1),
                                filter((item) => item && item.id == userId),
                                tap((updatedUser: UserDto) => {
                                    return updatedUser;
                                })
                            )
                        )
                    )
            )
        );
    }

    /**
     * Delete User role
     */
    deleteUserRole(userId: number, role: RoleDto): Observable<UserDto> {
        return this.users$.pipe(
            take(1),
            switchMap((users) =>
                this._httpClient
                    .delete<UserDto>(`${this._baseUrl}/users/${userId}/roles`, {
                        body: {
                            id: role.id,
                            name: role.name,
                        },
                    })
                    .pipe(
                        map((updatedUser: UserDto) => {
                            return updatedUser;
                        }),
                        switchMap(() =>
                            this.user$.pipe(
                                take(1),
                                filter((item) => item && item.id == userId),
                                tap((updatedUser: UserDto) => {
                                    return updatedUser;
                                })
                            )
                        )
                    )
            )
        );
    }

    /**
     * check if email is already used
     */
    checkEmail(email: string): Observable<boolean> {
        return this._httpClient
            .get<RoleDto>(`${this._baseUrl}/users/email/${email}`)
            .pipe(
                mapTo(true),
                catchError((error: HttpErrorResponse) => {
                    if (error.status === 404) {
                        return of(false);
                    }
                    return throwError(error);
                })
            );
    }

    /**
     * Create User by eMail
     */
    createUserByEmail(email: string): Observable<UserDto> {
        return this._httpClient
            .post<UserDto>(`${this._baseUrl}/users/email/${email}`, null)
            .pipe(
                tap((user: UserDto) => {
                    const users = this._users.getValue();
                    if (users) {
                        const updatedUsers = [...users, user];
                        this._users.next(updatedUsers);
                    }
                })
            );
    }

    /**
     * Sign up a new user
     * @param signUpData - The sign-up data
     * @returns {Observable<UserDto>} - An observable containing the created user
     */
    signUp(signUpData: SignUpDto): Observable<UserDto> {
        return this._httpClient
            .post<UserDto>(`${this._baseUrl}/auth/sign-up`, signUpData)
            .pipe(
                tap((user: UserDto) => {
                    const users = this._users.getValue();
                    if (users) {
                        const updatedUsers = [...users, user];
                        this._users.next(updatedUsers);
                    }
                })
            );
    }

    /**
     * Delete User
     * @param {number} id - The ID of the user to delete
     * @returns {Observable<void>} - An observable indicating the completion of the deletion
     */
    deleteUser(id: number): Observable<void> {
        return this._httpClient
            .delete<void>(`${this._baseUrl}/users/${id}`)
            .pipe(
                tap(() => {
                    // Optionnel : Vous pouvez mettre à jour la liste des utilisateurs ici si nécessaire
                    const users = this._users.getValue();
                    if (users) {
                        const updatedUsers = users.filter(
                            (user) => user.id !== id
                        );
                        this._users.next(updatedUsers);
                    }
                })
            );
    }
}
