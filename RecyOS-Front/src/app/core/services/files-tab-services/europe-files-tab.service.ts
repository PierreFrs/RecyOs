import { EventEmitter, Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable, forkJoin, map, tap } from 'rxjs';
import { DocumentPdfEurope } from '../../../models/europe-files-tab.models';
import { BackendApiService } from '../../../backend.api.service';

@Injectable({
    providedIn: 'root',
})
export class EuropeFilesTabService {
    constructor(
        private httpClient: HttpClient,
        private _apiService: BackendApiService,
    ) {}

    createPdfDocument(combinedData: FormData): Observable<any> {
        const baseUrl = this._apiService.getBaseUrl();
        const httpOptions = {
            headers: new HttpHeaders(),
        };
        return this.httpClient.post<any>(
            `${baseUrl}/documents-pdf-europe`,
            combinedData,
            httpOptions,
        );
    }

    public pdfCreated = new EventEmitter<number>();

    downloadPdfDocument(
        documentId: number,
    ): Observable<{ blob: Blob; filename: string }> {
        const baseUrl = this._apiService.getBaseUrl();
        const blob$ = this.httpClient.get(
            `${baseUrl}/documents-pdf-europe/download/${documentId}`,
            { responseType: 'blob' },
        );
        const metadata$ = this.fetchPdfDocumentById(documentId);

        return forkJoin({
            blob: blob$,
            metadata: metadata$,
        }).pipe(
            map(({ blob, metadata }) => {
                return {
                    blob: blob,
                    filename: metadata.fileName,
                };
            }),
        );
    }

    fetchAllPdfDocuments(): Observable<DocumentPdfEurope[]> {
        const baseUrl = this._apiService.getBaseUrl();
        return this.httpClient.get<DocumentPdfEurope[]>(
            `${baseUrl}/documents-pdf-europe`,
        );
    }

    fetchPdfDocumentById(documentId: number): Observable<DocumentPdfEurope> {
        const baseUrl = this._apiService.getBaseUrl();
        return this.httpClient.get<DocumentPdfEurope>(
            `${baseUrl}/documents-pdf-europe/${documentId}`,
        );
    }

    fetchPdfDocumentsByClientId(
        clientId: number,
    ): Observable<DocumentPdfEurope[]> {
        const baseUrl = this._apiService.getBaseUrl();
        return this.httpClient.get<DocumentPdfEurope[]>(
            `${baseUrl}/documents-pdf-europe/client/${clientId}`,
        );
    }

    updatePdfDocument(
        documentId: number,
        combinedData: FormData,
    ): Observable<DocumentPdfEurope> {
        const baseUrl = this._apiService.getBaseUrl();
        const httpOptions = {
            headers: new HttpHeaders(),
        };
        return this.httpClient.put<any>(
            `${baseUrl}/documents-pdf-europe/${documentId}`,
            combinedData,
            httpOptions,
        );
    }

    public pdfUpdated = new EventEmitter<number>();

    deletePdfDocument(documentId: number): Observable<void> {
        const baseUrl = this._apiService.getBaseUrl();
        return this.httpClient
            .delete<void>(`${baseUrl}/documents-pdf-europe/${documentId}`)
            .pipe(
                tap({
                    complete: () => {
                        this.pdfDeleted.emit(documentId);
                    },
                }),
            );
    }

    // Notifies deletion of doc
    public pdfDeleted = new EventEmitter<number>();
}
