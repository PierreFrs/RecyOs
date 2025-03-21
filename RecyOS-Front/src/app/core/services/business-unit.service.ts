import { Observable } from 'rxjs';
import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { BackendApiService } from '../../backend.api.service';
import { BusinessUnitDto } from '../../models/business-unit.type';

@Injectable({
    providedIn: 'root',
})
export class BusinessUnitServices {
    constructor(
        private _httpClient: HttpClient,
        private apiService: BackendApiService,
    ) {}
    getBusinessUnitList(): Observable<BusinessUnitDto[]> {
        const baseUrl = this.apiService.getBaseUrl();
        return this._httpClient.get<BusinessUnitDto[]>(
            `${baseUrl}/business-unit`,
        );
    }

    getBusinessUnitById(id: number): Observable<BusinessUnitDto> {
        const baseUrl = this.apiService.getBaseUrl();
        return this._httpClient.get<BusinessUnitDto>(
            `${baseUrl}/business-unit/${id}`,
        );
    }
}
