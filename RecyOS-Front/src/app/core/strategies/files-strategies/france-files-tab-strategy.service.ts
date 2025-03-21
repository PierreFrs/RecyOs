import { EventEmitter, Injectable } from '@angular/core';
import { IFilesServiceStrategy } from './IFilesServiceStrategy';
import { Observable } from 'rxjs';
import { FranceFilesTabService } from '../../services/files-tab-services/france-files-tab.service';
import { DocumentPdf } from '../../../models/france-files-tab.models';

@Injectable({
    providedIn: 'root',
})
export class FranceFilesTabStrategyService implements IFilesServiceStrategy {
    get pdfCreated(): EventEmitter<number> {
        return this._frenchFilesService.pdfCreated;
    }

    get pdfUpdated(): EventEmitter<number> {
        return this._frenchFilesService.pdfUpdated;
    }

    get pdfDeleted(): EventEmitter<number> {
        return this._frenchFilesService.pdfDeleted;
    }
    constructor(private _frenchFilesService: FranceFilesTabService) {}
    createPdfDocument(formData: FormData): Observable<DocumentPdf> {
        return this._frenchFilesService.createPdfDocument(formData);
    }

    downloadPdfDocument(
        documentId: number,
    ): Observable<{ blob: Blob; filename: string }> {
        return this._frenchFilesService.downloadPdfDocument(documentId);
    }

    fetchPdfDocumentsByClientId(clientId: number): Observable<DocumentPdf[]> {
        return this._frenchFilesService.fetchPdfDocumentsByClientId(clientId);
    }

    fetchPdfDocumentById(documentId: number): Observable<DocumentPdf> {
        return this._frenchFilesService.fetchPdfDocumentById(documentId);
    }

    updatePdfDocument(
        documentId: number,
        formData: FormData,
    ): Observable<DocumentPdf> {
        return this._frenchFilesService.updatePdfDocument(documentId, formData);
    }

    deletePdfDocument(documentId: number): Observable<void> {
        return this._frenchFilesService.deletePdfDocument(documentId);
    }
}
