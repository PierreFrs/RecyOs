import { Observable } from 'rxjs';
import { EventEmitter } from '@angular/core';
import { GlobalDocumentPdfType } from '../../../models/global-document-pdf.type';

export interface IFilesServiceStrategy {
    createPdfDocument(formData: FormData): Observable<GlobalDocumentPdfType>;
    downloadPdfDocument(
        documentId: number,
    ): Observable<{ blob: Blob; filename: string }>;
    fetchPdfDocumentsByClientId(
        clientId: number,
    ): Observable<GlobalDocumentPdfType[]>;
    fetchPdfDocumentById(documentId: number): Observable<GlobalDocumentPdfType>;
    updatePdfDocument(
        documentId: number,
        formData: FormData,
    ): Observable<GlobalDocumentPdfType>;
    deletePdfDocument(documentId: number): Observable<void>;
    pdfCreated: EventEmitter<number>;
    pdfUpdated: EventEmitter<number>;
    pdfDeleted: EventEmitter<number>;
}
