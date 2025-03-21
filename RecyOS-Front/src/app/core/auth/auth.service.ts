import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import {
    catchError,
    map,
    Observable,
    of,
    switchMap,
    tap,
    throwError,
    BehaviorSubject,
} from 'rxjs';
import { UserService } from 'app/core/services/user.service';
import { BackendApiService } from '../../backend.api.service';
import { Router } from '@angular/router';
import { UserDto } from 'app/modules/administrator/users/users.type';
import { shareReplay } from 'rxjs/operators';

@Injectable()
export class AuthService {
    private _authenticated: boolean = false;
    private _authenticationChecked = false;
    private _authenticationStatus$ = new BehaviorSubject<boolean>(false);
    refreshFailed: boolean = false;

    /**
     * Constructor
     */
    constructor(
        private readonly _httpClient: HttpClient,
        private readonly _userService: UserService,
        private readonly apiService: BackendApiService,
        private readonly router: Router
    ) {
        // Initialize authentication status once
        this.checkAuthenticationStatus();
    }

    private checkAuthenticationStatus(): void {
        if (!this._authenticationChecked) {
            this._httpClient
                .get<{ isAuthenticated: boolean }>(
                    `${this.baseUrl}/auth/is-authenticated`
                )
                .pipe(
                    tap((response) => {
                        this._authenticated = response.isAuthenticated;
                        this._authenticationStatus$.next(
                            response.isAuthenticated
                        );
                        this._authenticationChecked = true;
                    })
                )
                .subscribe();
        }
    }

    private clearAuthenticatedState(): void {
        this._authenticated = false;
        this._authenticationStatus$.next(false);
        this._authenticationChecked = false;
        this._userService.user = null;
    }

    private get baseUrl(): string {
        return this.apiService.getBaseUrl();
    }

    // -----------------------------------------------------------------------------------------------------
    // @ Public methods
    // -----------------------------------------------------------------------------------------------------

    /**
     * Forgot password
     *
     * @param email - user email
     */
    forgotPassword(email: string): Observable<any> {
        return this._httpClient.post(
            `${this.baseUrl}/auth/forgot-password`,
            email
        );
    }

    /**
     * Reset password
     *
     * @param credentials - email, password and password confirmation
     */
    resetPassword(credentials: {
        email: string;
        password: string;
        confirPassword: string;
    }): Observable<any> {
        return this._httpClient.post(
            `${this.baseUrl}/auth/reset-pass`,
            credentials
        );
    }

    /**
     * Sign in
     *
     * @param credentials
     */
    signIn(credentials: { email: string; password: string }): Observable<any> {
        this.refreshFailed = false;

        return this._httpClient
            .post<{ user: UserDto }>(
                `${this.baseUrl}/auth/login`,
                credentials,
                {
                    withCredentials: true,
                }
            )
            .pipe(
                tap((response) => {
                    this._authenticated = true;
                    this._authenticationStatus$.next(true);
                    this._userService.user = response.user;
                }),
                switchMap((response) => {
                    const user = response.user;

                    if (!user) {
                        throw new Error(
                            'Login successful but no user information received.'
                        );
                    }

                    // Return the user information
                    return of(user);
                }),
                catchError((error) => {
                    this._authenticated = false;
                    this._authenticationStatus$.next(false);
                    return throwError(() => error);
                })
            );
    }

    /**
     * Get authenticated user ID
     */
    getUserAuthenticationStatus(): Observable<boolean> {
        // Return cached value if available
        if (this._authenticationChecked) {
            return this._authenticationStatus$.asObservable();
        }

        // Otherwise check authentication status
        return this._httpClient
            .get<{ isAuthenticated: boolean }>(
                `${this.baseUrl}/auth/is-authenticated`
            )
            .pipe(
                map((response) => response.isAuthenticated),
                tap((isAuthenticated) => {
                    this._authenticated = isAuthenticated;
                    this._authenticationStatus$.next(isAuthenticated);
                    this._authenticationChecked = true;
                }),
                shareReplay(1)
            );
    }

    /*
     *  Refresh Token
     */
    refreshAccessToken(): Observable<any> {
        return this._httpClient
            .post(
                `${this.baseUrl}/auth/refresh-token`,
                {},
                { withCredentials: true }
            )
            .pipe(
                tap(() => {
                    this._authenticationChecked = true;
                    this._authenticated = true;
                    this._authenticationStatus$.next(true);
                }),
                catchError((error) => {
                    this._authenticated = false;
                    this._authenticationStatus$.next(false);
                    this._authenticationChecked = false;
                    this.signOut(true); // Skip API call since we're already handling an error
                    return throwError(() => error);
                })
            );
    }

    /**
     * Sign out
     */
    signOut(skipApiCall: boolean = false): Observable<void> {
        const clearAuth = () => {
            this._authenticated = false;
            this._authenticationStatus$.next(false);
            this._authenticationChecked = false;
            this._userService.user = null;
            this.router.navigate(['/sign-in']);
        };

        if (!skipApiCall) {
            return this._httpClient
                .post<void>(`${this.baseUrl}/auth/logout`, {})
                .pipe(
                    tap(() => clearAuth()),
                    catchError((error) => {
                        console.error(
                            'Logout failed, clearing session locally:',
                            error
                        );
                        clearAuth();
                        return of(undefined);
                    })
                );
        }

        clearAuth();
        return of(undefined);
    }

    /**
     * Refresh token failed - clear session and redirect
     */
    handleRefreshTokenFailure(): void {
        console.error('AuthService: Handling refresh token failure.');
        this.clearAuthenticatedState();
        this.router.navigate(['/']);
    }

    /**
     * Sign up
     *
     * @param user
     */
    signUp(user: {
        name: string;
        email: string;
        password: string;
        company: string;
    }): Observable<any> {
        return this._httpClient.post(`${this.baseUrl}/auth/sign-up`, user);
    }

    /**
     * Unlock session
     *
     * @param credentials
     */
    unlockSession(credentials: {
        email: string;
        password: string;
    }): Observable<any> {
        return this._httpClient.post(
            `${this.baseUrl}/auth/unlock-session`,
            credentials
        );
    }

    initializeUser(): Observable<UserDto | null> {
        if (!this._authenticated) {
            // Check authentication status using cookies
            return this.getUserAuthenticationStatus().pipe(
                switchMap((isAuthenticated) => {
                    if (isAuthenticated) {
                        // Fetch user info
                        return this.getUserInfo();
                    } else {
                        return of(null); // User not authenticated
                    }
                }),
                tap((user) => {
                    if (user) {
                        this._authenticated = true;
                        this._userService.user = user;
                    }
                }),
                catchError((error) => {
                    console.error('Failed to initialize user:', error);
                    return of(null); // Handle errors gracefully
                })
            );
        }

        // User already authenticated
        return of(this._userService.user);
    }

    private getUserInfo(): Observable<UserDto> {
        return this._httpClient
            .get<UserDto>(`${this.baseUrl}/users/current`)
            .pipe(
                tap((user: UserDto) => {
                    this._userService.user = user;
                })
            );
    }

    /**
     * Request password
     *
     * @param requestPasswordDTO
     */
    requestPassword(requestPasswordDTO: any): Observable<any> {
        return this._httpClient.post(
            `${this.baseUrl}/auth/request-pass`,
            requestPasswordDTO
        );
    }

    /**
     * Restore password
     *
     * @param restorePasswordDTO
     */
    restorePassword(restorePasswordDTO: any): Observable<any> {
        return this._httpClient.post(
            `${this.baseUrl}/auth/restore-pass`,
            restorePasswordDTO
        );
    }
}
