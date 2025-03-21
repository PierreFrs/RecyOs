import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, BehaviorSubject } from 'rxjs';
import { TypeDocumentPdf } from '../../models/file-type.model';
import { BackendApiService } from '../../backend.api.service';

@Injectable({
    providedIn: 'root',
})
export class FileTypeService {
    private _filesTypes: BehaviorSubject<TypeDocumentPdf[] | null> =
        new BehaviorSubject(null);

    constructor(
        private httpClient: HttpClient,
        private _apiService: BackendApiService,
    ) {}

    // Create a new PDF File Type
    createPdfType(newType: TypeDocumentPdf): Observable<TypeDocumentPdf> {
        const baseUrl = this._apiService.getBaseUrl();
        return this.httpClient.post<TypeDocumentPdf>(
            `${baseUrl}/documents-types`,
            newType,
        );
    }

    // Fetch All PDF File Types
    fetchAllPdfTypes(): Observable<TypeDocumentPdf[]> {
        const baseUrl = this._apiService.getBaseUrl();
        return this.httpClient.get<TypeDocumentPdf[]>(
            `${baseUrl}/documents-types`,
        );
    }

    get filesTypes$(): Observable<TypeDocumentPdf[] | null> {
        return this._filesTypes.asObservable();
    }

    // Fetch PDF File Type by ID
    fetchPdfTypeById(typeId: number): Observable<TypeDocumentPdf> {
        const baseUrl = this._apiService.getBaseUrl();
        return this.httpClient.get<TypeDocumentPdf>(
            `${baseUrl}/documents-types/${typeId}`,
        );
    }

    // Fetch PDF File Type by Label
    fetchPdfTypeByLabel(label: string): Observable<TypeDocumentPdf> {
        const baseUrl = this._apiService.getBaseUrl();
        return this.httpClient.get<TypeDocumentPdf>(
            `${baseUrl}/documents-types/${label}`,
        );
    }

    // Update PDF File Type by ID
    updatePdfType(
        typeId: number,
        updatedType: TypeDocumentPdf,
    ): Observable<TypeDocumentPdf> {
        const baseUrl = this._apiService.getBaseUrl();
        return this.httpClient.put<TypeDocumentPdf>(
            `${baseUrl}/documents-types/${typeId}`,
            updatedType,
        );
    }

    // Delete PDF File Type by ID
    deletePdfType(typeId: number): Observable<void> {
        const baseUrl = this._apiService.getBaseUrl();
        return this.httpClient.delete<void>(
            `${baseUrl}/documents-types/${typeId}`,
        );
    }
}
