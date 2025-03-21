import { Injectable } from '@angular/core';
import { Resolve } from '@angular/router';
import { FileTypeService } from '../services/file-type.service';
import { Observable } from 'rxjs';
import { TypeDocumentPdf } from '../../models/file-type.model';

@Injectable({
    providedIn: 'root',
})
export class FileTypeResolver implements Resolve<any> {
    /**
     * Constructor
     */
    constructor(private _fileTypeService: FileTypeService) {}
    resolve(): Observable<TypeDocumentPdf[]> {
        return this._fileTypeService.fetchAllPdfTypes();
    }
}
