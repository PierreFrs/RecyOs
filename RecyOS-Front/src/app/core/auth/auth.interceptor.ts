import { Injectable } from '@angular/core';
import {
    HttpEvent,
    HttpHandler,
    HttpInterceptor,
    HttpRequest,
    HttpErrorResponse,
} from '@angular/common/http';
import { Observable, throwError, Subject, BehaviorSubject } from 'rxjs';
import { catchError, switchMap, filter, take, finalize } from 'rxjs/operators';
import { AuthService } from './auth.service';

@Injectable()
export class AuthInterceptor implements HttpInterceptor {
    private readonly excludeUrls = [
        '/auth/login',
        '/auth/refresh-token',
        'auth/user-id',
        '/sign-in',
        '/sign-out',
    ];

    private isRefreshing = false;
    private refreshTokenSubject: BehaviorSubject<any> =
        new BehaviorSubject<any>(null);

    constructor(private readonly authService: AuthService) {}

    intercept(
        req: HttpRequest<any>,
        next: HttpHandler
    ): Observable<HttpEvent<any>> {
        const isExcluded = this.shouldExcludeRequest(req);

        const modifiedRequest = isExcluded
            ? req
            : req.clone({ withCredentials: true });

        return next.handle(modifiedRequest).pipe(
            catchError((error: HttpErrorResponse) => {
                if (
                    error.status === 401 &&
                    !isExcluded &&
                    !req.url.includes('refresh-token')
                ) {
                    return this.handle401Error(modifiedRequest, next);
                }
                return throwError(() => error);
            })
        );
    }

    private shouldExcludeRequest(req: HttpRequest<any>): boolean {
        return this.excludeUrls.some((url) => req.url.includes(url));
    }

    private handle401Error(
        request: HttpRequest<any>,
        next: HttpHandler
    ): Observable<HttpEvent<any>> {
        if (!this.isRefreshing) {
            this.isRefreshing = true;
            this.refreshTokenSubject.next(null);

            return this.authService.refreshAccessToken().pipe(
                switchMap(() => {
                    this.isRefreshing = false;
                    this.refreshTokenSubject.next(true);
                    return next.handle(request);
                }),
                catchError((error) => {
                    this.isRefreshing = false;
                    this.authService.signOut();
                    return throwError(() => error);
                }),
                finalize(() => {
                    this.isRefreshing = false;
                })
            );
        }

        return this.refreshTokenSubject.pipe(
            filter((token) => token !== null),
            take(1),
            switchMap(() => next.handle(request))
        );
    }
}
