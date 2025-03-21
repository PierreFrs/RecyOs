import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import {BehaviorSubject, Observable, tap} from 'rxjs';

@Injectable({
    providedIn: 'root'
})
export class ConfigService {
    private readonly configSubject = new BehaviorSubject<any>(null);
    private config: any;

    constructor(private readonly http: HttpClient) {}

    loadConfig(): Observable<any> {
        return this.http.get('/assets/config.json').pipe(
            tap(config => {
                this.config = config;
                this.configSubject.next(this.config);
            })
        );
    }

    getConfig(): Observable<any> {
        return this.configSubject.asObservable();
    }

    get backendUrl(): string {
        return this.config ? this.config.backendUrl : '';
    }
}
