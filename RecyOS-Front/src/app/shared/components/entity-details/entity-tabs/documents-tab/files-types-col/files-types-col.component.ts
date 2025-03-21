import {
    Component,
    OnInit,
    Input,
    Output,
    EventEmitter,
    ChangeDetectorRef,
    OnDestroy,
} from '@angular/core';
import {
    combineLatest,
    map,
    Observable,
    Subject,
    Subscription,
    takeUntil,
} from 'rxjs';
import { ActivatedRoute } from '@angular/router';
import { TypeDocumentPdf } from '../../../../../../models/file-type.model';
import { IFilesServiceStrategy } from '../../../../../../core/strategies/files-strategies/IFilesServiceStrategy';
import { EntityStrategyService } from '../../../../../../core/strategies/entity-strategy/entity-strategy.service';
import { GlobalDocumentPdfType } from '../../../../../../models/global-document-pdf.type';
import {EntityDto} from "../../../../../../models/entities-models/entity.type";

@Component({
    selector: 'files-types-col',
    templateUrl: './files-types-col.component.html',
})
export class FilesTypesColComponent implements OnInit, OnDestroy {
    @Input() selectedEntity: EntityDto;
    @Output() typeSelected = new EventEmitter<number>();

    pdfFiles: GlobalDocumentPdfType[] = [];
    filesTypes: TypeDocumentPdf[] = [];
    filesTypes$: Observable<TypeDocumentPdf[]>;
    uniqueDocumentTypes: TypeDocumentPdf[] = [];
    activeType: number | null = null;

    private filesServiceStrategy: IFilesServiceStrategy;
    private readonly unsubscribe$ = new Subject<void>();
    private readonly subscription: Subscription = new Subscription();

    constructor(
        private readonly route: ActivatedRoute,
        private readonly _entityStrategyService: EntityStrategyService,
        private readonly _changeDetectorRef: ChangeDetectorRef,
    ) {}

    ngOnInit(): void {
        this.filesServiceStrategy =
            this._entityStrategyService.determineFilesStrategyFromEntity(
                this.selectedEntity,
            );
        this.filesTypes$ = this.route.data.pipe(map((data) => data.types));

        const pdfFiles$ = this.filesServiceStrategy.fetchPdfDocumentsByClientId(
            this.selectedEntity.id,
        );

        this.subscription.add(
            combineLatest([this.filesTypes$, pdfFiles$])
                .pipe(takeUntil(this.unsubscribe$))
                .subscribe({
                    next: ([types, files]) => {
                        this.filesTypes = types;
                        this.pdfFiles = files;
                        this.getUniqueTypes(this.pdfFiles);
                        this._changeDetectorRef.detectChanges();
                    },
                    error: (error) => {
                        console.error('An error occurred: ', error);
                    },
                }),
        );

        this.filesServiceStrategy.pdfCreated
            .pipe(takeUntil(this.unsubscribe$))
            .subscribe((newDocumentId: number) => {
                this.handlePdfCreated(newDocumentId);
            });

        this.filesServiceStrategy.pdfDeleted
            .pipe(takeUntil(this.unsubscribe$))
            .subscribe((deletedDocumentId: number) => {
                this.handlePdfDeleted(deletedDocumentId);
            });

        this.filesServiceStrategy.pdfUpdated
            .pipe(takeUntil(this.unsubscribe$))
            .subscribe(() => {
                this.handlePdfUpdated();
            });
    }

    ngOnDestroy(): void {
        this.unsubscribe$.next();
        this.unsubscribe$.complete();
        this.subscription.unsubscribe();
    }

    selectType(typeId: number | null): void {
        this.activeType = typeId;
        this.typeSelected.emit(typeId);
    }

    getUniqueTypes(docs: GlobalDocumentPdfType[]): void {
        if (!this.filesTypes) return;

        const allTypeIds =
            docs.length > 0
                ? Array.from(new Set(docs.map((doc) => doc.typeDocumentPdfId)))
                : [];
        this.uniqueDocumentTypes = this.filesTypes.filter((type) =>
            allTypeIds.includes(type.id),
        );
    }

    handlePdfCreated(newDocumentId: number) {
        this.filesServiceStrategy
            .fetchPdfDocumentById(newDocumentId)
            .subscribe((newDocument: GlobalDocumentPdfType) => {
                this.pdfFiles.unshift(newDocument);
                this.getUniqueTypes(this.pdfFiles);
                this._changeDetectorRef.detectChanges();
            });
    }

    handlePdfDeleted(deletedDocumentId: number) {
        const index = this.pdfFiles.findIndex(
            (doc) => doc.id === deletedDocumentId,
        );
        if (index !== -1) {
            this.pdfFiles.splice(index, 1);
            this.getUniqueTypes(this.pdfFiles);
            this._changeDetectorRef.detectChanges();
        }
    }

    handlePdfUpdated() {
        this.filesServiceStrategy
            .fetchPdfDocumentsByClientId(this.selectedEntity.id)
            .subscribe((files) => {
                this.pdfFiles = files;
                this.getUniqueTypes(this.pdfFiles);
                this._changeDetectorRef.detectChanges();
            });
    }
}
