import { EventEmitter, Injectable } from '@angular/core';
import { IFilesServiceStrategy } from './IFilesServiceStrategy';
import { Observable } from 'rxjs';
import { DocumentPdfEurope } from '../../../models/europe-files-tab.models';
import { EuropeFilesTabService } from '../../services/files-tab-services/europe-files-tab.service';

@Injectable({
    providedIn: 'root',
})
export class EuropeFilesTabStrategyService implements IFilesServiceStrategy {
    get pdfCreated(): EventEmitter<number> {
        return this._europeFilesService.pdfCreated;
    }

    get pdfUpdated(): EventEmitter<number> {
        return this._europeFilesService.pdfUpdated;
    }

    get pdfDeleted(): EventEmitter<number> {
        return this._europeFilesService.pdfDeleted;
    }
    constructor(private _europeFilesService: EuropeFilesTabService) {}
    createPdfDocument(formData: FormData): Observable<DocumentPdfEurope> {
        return this._europeFilesService.createPdfDocument(formData);
    }

    downloadPdfDocument(
        documentId: number,
    ): Observable<{ blob: Blob; filename: string }> {
        return this._europeFilesService.downloadPdfDocument(documentId);
    }

    fetchPdfDocumentsByClientId(
        clientId: number,
    ): Observable<DocumentPdfEurope[]> {
        return this._europeFilesService.fetchPdfDocumentsByClientId(clientId);
    }

    fetchPdfDocumentById(documentId: number): Observable<DocumentPdfEurope> {
        return this._europeFilesService.fetchPdfDocumentById(documentId);
    }

    updatePdfDocument(
        documentId: number,
        formData: FormData,
    ): Observable<DocumentPdfEurope> {
        return this._europeFilesService.updatePdfDocument(documentId, formData);
    }

    deletePdfDocument(documentId: number): Observable<void> {
        return this._europeFilesService.deletePdfDocument(documentId);
    }
}
