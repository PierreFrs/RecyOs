import {
    ChangeDetectorRef,
    Component,
    Input,
    OnDestroy,
    OnInit,
} from '@angular/core';
import { Subject, takeUntil } from 'rxjs';
import { TypeDocumentPdf } from '../../../../../../models/file-type.model';
import { FileTypeService } from '../../../../../../core/services/file-type.service';
import { IFilesServiceStrategy } from '../../../../../../core/strategies/files-strategies/IFilesServiceStrategy';
import { EntityStrategyService } from '../../../../../../core/strategies/entity-strategy/entity-strategy.service';
import { GlobalDocumentPdfType } from '../../../../../../models/global-document-pdf.type';
import {EntityDto} from "../../../../../../models/entities-models/entity.type";

@Component({
    selector: 'file-preview-col',
    templateUrl: './file-preview-col.component.html',
})
export class FilePreviewColComponent implements OnInit, OnDestroy {
    @Input() selectedDocument: GlobalDocumentPdfType;
    @Input() selectedEntity: EntityDto;
    filesTypes: TypeDocumentPdf[] = [];
    selectedType: number;

    private readonly unsubscribe$ = new Subject<void>();
    private filesServiceStrategy: IFilesServiceStrategy;

    constructor(
        private readonly _entityStrategyService: EntityStrategyService,
        private readonly _fileTypeService: FileTypeService,
        private readonly _changeDetectorRef: ChangeDetectorRef,
    ) {}

    removeGuidFromFilename(filename: string): string {
        const guidPattern =
            /[0-9a-f]{8}-[0-9a-f]{4}-[0-9a-f]{4}-[0-9a-f]{4}-[0-9a-f]{12}_/i;
        return filename.replace(guidPattern, '');
    }

    getSelectedLabel(): string {
        const fileType = this.filesTypes.find(
            (type) => type.id === this.selectedDocument.typeDocumentPdfId,
        );
        return fileType ? fileType.label : 'Unknown';
    }

    fetchDocumentById(documentId: number): void {
        // Assuming you have a method in your service to fetch a document by its ID
        this.filesServiceStrategy
            .fetchPdfDocumentById(documentId)
            .subscribe((document: GlobalDocumentPdfType) => {
                this.selectedDocument = document;
                this._changeDetectorRef.detectChanges();
            });
    }

    ngOnInit(): void {
        this.filesServiceStrategy =
            this._entityStrategyService.determineFilesStrategyFromEntity(
                this.selectedEntity,
            );
        this.filesServiceStrategy.pdfDeleted
            .pipe(takeUntil(this.unsubscribe$))
            .subscribe((deletedDocumentId) => {
                if (
                    this.selectedDocument &&
                    this.selectedDocument.id === deletedDocumentId
                ) {
                    this.selectedDocument = null;
                    this._changeDetectorRef.detectChanges();
                }
            });

        this._fileTypeService
            .fetchAllPdfTypes()
            .pipe(takeUntil(this.unsubscribe$))
            .subscribe((types) => {
                this.filesTypes = types;
            });

        this.filesServiceStrategy.pdfUpdated
            .pipe(takeUntil(this.unsubscribe$))
            .subscribe((updatedDocumentId: number) => {
                this.fetchDocumentById(updatedDocumentId);
            });

        this.selectedType = this.selectedDocument
            ? this.selectedDocument.typeDocumentPdfId
            : null;
    }

    ngOnDestroy(): void {
        this.unsubscribe$.next();
        this.unsubscribe$.complete();
    }
}
