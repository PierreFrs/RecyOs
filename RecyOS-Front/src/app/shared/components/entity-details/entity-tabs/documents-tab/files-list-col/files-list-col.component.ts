import {
    ChangeDetectorRef,
    Component,
    EventEmitter,
    Input,
    OnChanges,
    OnInit,
    Output,
    SimpleChanges,
    LOCALE_ID,
    OnDestroy,
} from '@angular/core';
import { map, Observable, Subject, Subscription, takeUntil } from 'rxjs';
import { ActivatedRoute } from '@angular/router';
import { MatDialog } from '@angular/material/dialog';
import { UploadPdfDialogComponent } from './upload-pdf-dialog/upload-pdf-dialog.component';
import { forEach } from 'lodash';
import { UserService } from '../../../../../../core/services/user.service';
import { TypeDocumentPdf } from 'app/models/file-type.model';
import { IFilesServiceStrategy } from '../../../../../../core/strategies/files-strategies/IFilesServiceStrategy';
import { EntityStrategyService } from '../../../../../../core/strategies/entity-strategy/entity-strategy.service';
import { GlobalDocumentPdfType } from '../../../../../../models/global-document-pdf.type';
import {EntityDto} from "../../../../../../models/entities-models/entity.type";

@Component({
    selector: 'files-list-col',
    templateUrl: './files-list-col.component.html',
    providers: [{ provide: LOCALE_ID, useValue: 'fr-FR' }],
})
export class FilesListColComponent implements OnInit, OnChanges, OnDestroy {
    @Input() selectedType: number = null;
    @Input() selectedEntity: EntityDto;
    @Output() documentSelected = new EventEmitter<GlobalDocumentPdfType>();

    pdfFiles: GlobalDocumentPdfType[];
    filteredDocuments: GlobalDocumentPdfType[] = [];
    filesTypes: TypeDocumentPdf[] = [];
    filesTypes$: Observable<TypeDocumentPdf[]>;
    activeFile: GlobalDocumentPdfType | null = null;

    private readonly unsubscribe$ = new Subject<void>();
    private readonly subscription: Subscription = new Subscription();

    userCanCreatePdf = false;

    private filesServiceStrategy: IFilesServiceStrategy;

    constructor(
        private readonly route: ActivatedRoute,
        private readonly _entityStrategyService: EntityStrategyService,
        private readonly _changeDetectorRef: ChangeDetectorRef,
        public _dialog: MatDialog,
        private readonly _userService: UserService,
    ) {}

    ngOnInit(): void {
        this.filesServiceStrategy =
            this._entityStrategyService.determineFilesStrategyFromEntity(
                this.selectedEntity,
            );
        this._userService.user$
            .pipe(takeUntil(this.unsubscribe$))
            .subscribe((user) => {
                if (user?.id) {
                    forEach(user.roles, (role) => {
                        if (role.name === 'add_PDF') {
                            this.userCanCreatePdf = true;
                        }
                    });
                }
            });

        this.filesServiceStrategy
            .fetchPdfDocumentsByClientId(this.selectedEntity.id)
            .pipe(takeUntil(this.unsubscribe$))
            .subscribe({
                next: (files) => {
                    this.pdfFiles = files;
                    this.filterDocumentsByType();
                },
                error: (error) => {
                    console.error('An error occurred: ', error);
                },
            });

        this.filesServiceStrategy.pdfCreated
            .pipe(takeUntil(this.unsubscribe$))
            .subscribe({
                next: (newDocumentId: number) => {
                    this.handlePdfCreated(newDocumentId);
                },
                error: (error) => {
                    console.error('An error occurred: ', error);
                },
            });

        this.filesServiceStrategy.pdfDeleted
            .pipe(takeUntil(this.unsubscribe$))
            .subscribe({
                next: (deletedDocumentId: number) => {
                    this.handlePdfDeleted(deletedDocumentId);
                },
                error: (error) => {
                    console.error('An error occurred: ', error);
                },
            });

        this.filesServiceStrategy.pdfUpdated
            .pipe(takeUntil(this.unsubscribe$))
            .subscribe({
                next: (updatedDocumentId: number) => {
                    this.handlePdfUpdated(updatedDocumentId);
                },
                error: (error) => {
                    console.error('An error occurred', error);
                },
            });

        this.filesTypes$ = this.route.data.pipe(map((data) => data.types));

        this.subscription.add(
            this.filesTypes$.pipe(takeUntil(this.unsubscribe$)).subscribe({
                next: (types) => {
                    this.filesTypes = types;
                    this._changeDetectorRef.detectChanges();
                },
                error: (error) => {
                    console.error('An error occurred: ', error);
                },
            }),
        );
    }

    ngOnChanges(changes: SimpleChanges): void {
        if (
            changes.selectedType &&
            changes.selectedType.currentValue !==
                changes.selectedType.previousValue
        ) {
            this.filterDocumentsByType();
        }
    }

    ngOnDestroy(): void {
        this.unsubscribe$.next();
        this.unsubscribe$.complete();
        this.subscription.unsubscribe();
    }

    filterDocumentsByType(): void {
        if (!this.pdfFiles) {
            this.filteredDocuments = [];
            return;
        }
        if (this.selectedType === null || this.selectedType === undefined) {
            this.filteredDocuments = this.pdfFiles;
        } else {
            this.filteredDocuments = this.pdfFiles.filter(
                (doc) => doc.typeDocumentPdfId === this.selectedType,
            );
        }
    }

    removeGuidFromFilename(filename: string): string {
        const guidPattern =
            /[0-9a-f]{8}-[0-9a-f]{4}-[0-9a-f]{4}-[0-9a-f]{4}-[0-9a-f]{12}_/i;
        return filename.replace(guidPattern, '');
    }

    selectDocument(doc: GlobalDocumentPdfType): void {
        this.activeFile = doc;
        this.documentSelected.emit(doc);
    }

    getSelectedLabel(): string {
        if (this.selectedType === null || this.selectedType === undefined) {
            return 'Tous';
        }
        const fileType = this.filesTypes.find(
            (type) => type.id === this.selectedType,
        );
        return fileType ? fileType.label : 'Unknown';
    }

    openUploadDialog(): void {
        this._dialog.open(UploadPdfDialogComponent, {
            data: {
                customerId: this.selectedEntity.id,
                pdfTypes: this.filesTypes,
                selectedEntity: this.selectedEntity,
            },
        });
    }

    handlePdfCreated(newDocumentId: number) {
        this.filesServiceStrategy
            .fetchPdfDocumentById(newDocumentId)
            .subscribe((newDocument: GlobalDocumentPdfType) => {
                this.pdfFiles.unshift(newDocument);
                this.filterDocumentsByType();
                this._changeDetectorRef.detectChanges();
            });
    }

    handlePdfUpdated(updatedDocumentId: number) {
        this.filesServiceStrategy
            .fetchPdfDocumentById(updatedDocumentId)
            .subscribe((updatedDocument: GlobalDocumentPdfType) => {
                const index = this.pdfFiles.findIndex(
                    (doc) => doc.id === updatedDocumentId,
                );
                if (index !== -1) {
                    this.pdfFiles[index] = updatedDocument;
                    this.filterDocumentsByType();
                    this._changeDetectorRef.detectChanges();
                }
            });
    }

    handlePdfDeleted(deletedDocumentId: number) {
        const index = this.filteredDocuments.findIndex(
            (doc) => doc.id === deletedDocumentId,
        );
        if (index !== -1) {
            this.filteredDocuments.splice(index, 1);
            this.activeFile = null;
            this._changeDetectorRef.detectChanges();
        }
    }
}
