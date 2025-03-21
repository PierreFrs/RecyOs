import { Injectable } from '@angular/core';
import {HttpClient} from "@angular/common/http";
import {BackendApiService} from "../../backend.api.service";
import {catchError, map, Observable, of} from "rxjs";

@Injectable({
  providedIn: 'root'
})
export class MkgtFcliService {
    private readonly baseUrl = this._apiService.getBaseUrl();

  constructor(
      private readonly _httpClient: HttpClient,
      private readonly _apiService: BackendApiService,
  ) { }

    checkMkgtFcliExistence(mkgtCode: string): Observable<boolean> {
        return this._httpClient.get(`${this.baseUrl}/mkgt-fcli/${mkgtCode}`, { observe: 'response' }).pipe(
            map(response => response.status === 200),
            catchError(() => of(false)) // If there's an error, assume the entity doesn't exist
        );
    }
}
