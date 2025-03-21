import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { map, Observable, ReplaySubject, tap } from 'rxjs';
import { BackendApiService } from '../../backend.api.service';
import { UserDto } from 'app/modules/administrator/users/users.type';

@Injectable({
    providedIn: 'root',
})
export class UserService {
    private readonly _user: ReplaySubject<UserDto> = new ReplaySubject<UserDto>(1);

    /**
     * Constructor
     */
    constructor(
        private readonly _httpClient: HttpClient,
        private readonly apiService: BackendApiService
    ) {}

    // -----------------------------------------------------------------------------------------------------
    // @ Accessors
    // -----------------------------------------------------------------------------------------------------

    /**
     * Setter & getter for user
     *
     * @param value
     */
    set user(value: UserDto) {
        // Store the value
        this._user.next(value);
    }

    get user$(): Observable<UserDto> {
        return this._user.asObservable();
    }

    // -----------------------------------------------------------------------------------------------------
    // @ Public methods
    // -----------------------------------------------------------------------------------------------------

    /**
     * Get the current logged in user data
     */
    get(): Observable<UserDto> {
        const baseUrl = this.apiService.getBaseUrl();
        return this._httpClient.get<UserDto>(`${baseUrl}/users/current`).pipe(
            tap((user) => {
                this._user.next(user);
            })
        );
    }

    /**
     * Update the user
     *
     * @param user
     */
    update(user: UserDto): Observable<any> {
        const baseUrl = this.apiService.getBaseUrl();
        return this._httpClient
            .patch<UserDto>(`${baseUrl}/users/current`, { user })
            .pipe(
                map((response) => {
                    this._user.next(response);
                })
            );
    }
}
