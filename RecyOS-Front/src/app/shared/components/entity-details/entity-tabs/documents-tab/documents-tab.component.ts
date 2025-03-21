import { Component, Input } from '@angular/core';
import { GlobalDocumentPdfType } from '../../../../../models/global-document-pdf.type';
import {EntityDto} from "../../../../../models/entities-models/entity.type";

@Component({
    selector: 'app-documents-tab',
    templateUrl: './documents-tab.component.html',
})
export class DocumentsTabComponent {
    @Input() selectedEntity: EntityDto;

    selectedType: number;
    selectedDocument: GlobalDocumentPdfType;

    onTypeSelected(typeId: number): void {
        this.selectedType = typeId;
    }

    onDocumentSelected(doc: GlobalDocumentPdfType): void {
        this.selectedDocument = doc;
    }
}
