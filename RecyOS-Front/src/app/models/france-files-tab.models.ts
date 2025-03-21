import { TypeDocumentPdf } from './file-type.model';
import {EntityDto} from "./entities-models/entity.type";

export interface DocumentPdf {
    id?: number;
    fileSize?: number;
    fileName?: string;
    fileLocation?: string;
    typeDocumentPdfId: number;
    etablissementClientId: number;
    createDate?: string;
    updatedAt?: string;
    createdBy?: string;
    updatedBy?: string;
    isDeleted?: boolean;
}

export interface DialogData {
    customerId: number;
    pdfTypes: TypeDocumentPdf[];
    documentId: number;
    selectedEntity: EntityDto;
}
