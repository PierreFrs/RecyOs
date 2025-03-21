import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { Etablissement } from '../../models/pappers-type';
@Injectable({
    providedIn: 'root',
})
export class PappersService {
    private readonly API_URL = 'https://api.pappers.fr/v2/entreprise';

    constructor(private http: HttpClient) {}

    getEtablissement(query: string): Observable<Etablissement> {
        const params = new HttpParams()
            .set('siret', query)
            .set(
                'api_token',
                '3385492b9bada8f36bf1947cd21c349be96e19cd3024422b',
            );

        return this.http.get<Etablissement>(this.API_URL, { params });
    }
}
