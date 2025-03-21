import { EventEmitter, Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable, forkJoin, map, tap } from 'rxjs';
import { DocumentPdf } from '../../../models/france-files-tab.models';
import { BackendApiService } from '../../../backend.api.service';

@Injectable({
    providedIn: 'root',
})
export class FranceFilesTabService {
    constructor(
        private httpClient: HttpClient,
        private _apiService: BackendApiService,
    ) {}

    createPdfDocument(combinedData: FormData): Observable<DocumentPdf> {
        const baseUrl = this._apiService.getBaseUrl();
        const httpOptions = {
            headers: new HttpHeaders(),
        };
        return this.httpClient.post<any>(
            `${baseUrl}/documents-pdf`,
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
            `${baseUrl}/documents-pdf/download/${documentId}`,
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

    fetchAllPdfDocuments(): Observable<DocumentPdf[]> {
        const baseUrl = this._apiService.getBaseUrl();
        return this.httpClient.get<DocumentPdf[]>(`${baseUrl}/documents-pdf`);
    }

    fetchPdfDocumentById(documentId: number): Observable<DocumentPdf> {
        const baseUrl = this._apiService.getBaseUrl();
        return this.httpClient.get<DocumentPdf>(
            `${baseUrl}/documents-pdf/${documentId}`,
        );
    }

    fetchPdfDocumentsByClientId(clientId: number): Observable<DocumentPdf[]> {
        const baseUrl = this._apiService.getBaseUrl();
        return this.httpClient.get<DocumentPdf[]>(
            `${baseUrl}/documents-pdf/client/${clientId}`,
        );
    }

    updatePdfDocument(
        documentId: number,
        combinedData: FormData,
    ): Observable<DocumentPdf> {
        const baseUrl = this._apiService.getBaseUrl();
        const httpOptions = {
            headers: new HttpHeaders(),
        };
        return this.httpClient.put<any>(
            `${baseUrl}/documents-pdf/${documentId}`,
            combinedData,
            httpOptions,
        );
    }

    public pdfUpdated = new EventEmitter<number>();

    // Delete PDF File by ID
    deletePdfDocument(documentId: number): Observable<void> {
        const baseUrl = this._apiService.getBaseUrl();
        return this.httpClient
            .delete<void>(`${baseUrl}/documents-pdf/${documentId}`)
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
