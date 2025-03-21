import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { ConfigService } from './config.service';
import { switchMap } from 'rxjs/operators';
import { Observable } from 'rxjs';

@Injectable({
    providedIn: 'root'
})
export class BackendApiService {
    private baseUrl: string;

    constructor(private readonly http: HttpClient, private readonly configService: ConfigService) {
        this.configService.getConfig().subscribe(config => {
            this.baseUrl = config ? config.backendUrl : '';
        });
    }

    public getBaseUrl(): string {
        return this.baseUrl;
    }

    // Exemple de méthode utilisant l'URL du backend de manière réactive
    public fetchData(): Observable<any> {
        return this.configService.getConfig().pipe(
            switchMap(config => this.http.get(`${config.backendUrl}/api/data`))
        );
    }
}
