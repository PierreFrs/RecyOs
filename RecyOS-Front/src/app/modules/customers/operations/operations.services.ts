import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { BackendApiService } from '../../../backend.api.service';
import { Observable } from 'rxjs';
import { EtablissementMkgtDto } from './operations.type';

@Injectable({
    providedIn: 'root',
})
export class OperationsServices {
    constructor(
        private readonly _httpClient: HttpClient,
        private readonly apiService: BackendApiService,
    ) {}

    /**
     * import Mkgt Customers by code
     * @param {string} code - The code
     */
    importMkgtCustomersByCode(code: string) {
        const baseUrl = this.apiService.getBaseUrl();
        return this._httpClient.post(
            `${baseUrl}/installator/import-mkgt-client?code=${code}`,
            {},
        );
    }

    /**
     * import all valid Mkgt Customers
     */
    importAllValidMkgtCustomers() {
        const baseUrl = this.apiService.getBaseUrl();
        return this._httpClient.get(`${baseUrl}/installator/import-fcli`, {});
    }

    /**
     * get Mkgt Customers by code
     * @param {string} code - The code
     */
    getMkgtCustomersByCode(code: string): Observable<EtablissementMkgtDto> {
        const baseUrl = this.apiService.getBaseUrl();
        return this._httpClient.get<EtablissementMkgtDto>(
            `${baseUrl}/mkgt-fcli/${code}`,
        );
    }

    /**
     * get SIREN info by SIRET
     */
    getSirenInfoBySiret(siret: string): Observable<any> {
        const baseUrl = this.apiService.getBaseUrl();
        return this._httpClient.get<any>(`${baseUrl}/pappers/update/${siret}`);
    }

    importAlianz(file: File): Observable<any> {
        const baseUrl = this.apiService.getBaseUrl();
        const formData: FormData = new FormData();
        formData.append('file', file, file.name);

        return this._httpClient.post(
            `${baseUrl}/installator/import-couverture`,
            formData,
        );
    }

    importNDCover(file: File): Observable<any> {
        const baseUrl = this.apiService.getBaseUrl();
        const formData: FormData = new FormData();
        formData.append('file', file, file.name);

        return this._httpClient.post(
            `${baseUrl}/installator/import-NDCover`,
            formData,
        );
    }

    /**
     * Export an Excel file with the list of companies without NDCover
     * @returns {Observable<Blob>}
     */
    getNDCoverSoumissionExportFileFrance(): Observable<Blob> {
        const baseUrl = this.apiService.getBaseUrl();
        return this._httpClient.get(
            `${baseUrl}/installator/export-soumission-ndcover-france`,
            {
                responseType: 'blob',
            },
        );
    }

    /**
     * Export an Excel file with the list of factor companies
     * @returns {Observable<Blob>}
     */
    getFactorExportFolder(): Observable<Blob> {
        const baseUrl = this.apiService.getBaseUrl();
        return this._httpClient.get(
            `${baseUrl}/factor/export-factor-file`,
            {
                responseType: 'blob',
            },
        );
    }

    /**
     * Import an Excel file with a list of errors
     * */
    importNDCoverErrorFile(file: File): Observable<any> {
        const baseUrl = this.apiService.getBaseUrl();
        const formData: FormData = new FormData();
        formData.append('file', file, file.name);

        return this._httpClient.post(
            `${baseUrl}/installator/import-ndcover-error`,
            formData,
        );
    }
}
