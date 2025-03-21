import {
    ChangeDetectorRef,
    Component,
    EventEmitter,
    Input,
    OnInit,
    Output,
    SimpleChanges,
} from '@angular/core';
import { UpdatePdfDialogComponent } from './update-pdf-dialog/update-pdf-dialog.component';
import { MatDialog } from '@angular/material/dialog';
import { map, Observable, Subject, takeUntil } from 'rxjs';
import { ActivatedRoute } from '@angular/router';
import { DeletePdfDialogComponent } from './delete-pdf-dialog/delete-pdf-dialog.component';
import { UserService } from '../../../../../../../core/services/user.service';
import { forEach } from 'lodash';
import { TypeDocumentPdf } from '../../../../../../../models/file-type.model';
import { IFilesServiceStrategy } from '../../../../../../../core/strategies/files-strategies/IFilesServiceStrategy';
import { EntityStrategyService } from '../../../../../../../core/strategies/entity-strategy/entity-strategy.service';
import {EntityDto} from "../../../../../../../models/entities-models/entity.type";

@Component({
    selector: 'pdf-viewer',
    templateUrl: './pdf-viewer.component.html',
})
export class PdfViewerComponent implements OnInit {
    @Input() documentId: number;
    @Input() customerId: number;
    @Input() filesTypes: TypeDocumentPdf[] = [];
    @Input() selectedEntity: EntityDto;
    @Output() pdfDeleted = new EventEmitter<void>();

    filesTypes$: Observable<TypeDocumentPdf[]>;
    public pdfSrc: string | ArrayBuffer | null = null;

    private readonly unsubscribe$ = new Subject<void>();
    private filesServiceStrategy: IFilesServiceStrategy;

    userCanDownloadPdf = false;
    userCanUpdatePdf = false;
    userCanDeletePdf = false;

    constructor(
        private readonly route: ActivatedRoute,
        private readonly _entityStrategyService: EntityStrategyService,
        private readonly _changeDetectorRef: ChangeDetectorRef,
        public dialog: MatDialog,
        private readonly _userService: UserService,
    ) {}

    ngOnInit() {
        this.filesServiceStrategy =
            this._entityStrategyService.determineFilesStrategyFromEntity(
                this.selectedEntity,
            );
        this._userService.user$
            .pipe(takeUntil(this.unsubscribe$))
            .subscribe((user) => {
                if (user?.id) {
                    forEach(user.roles, (role) => {
                        if (role.name === 'download_PDF') {
                            this.userCanDownloadPdf = true;
                        }
                    });
                }
                if (user?.id) {
                    forEach(user.roles, (role) => {
                        if (role.name === 'update_PDF') {
                            this.userCanUpdatePdf = true;
                        }
                    });
                }
                if (user?.id) {
                    forEach(user.roles, (role) => {
                        if (role.name === 'delete_PDF') {
                            this.userCanDeletePdf = true;
                        }
                    });
                }
            });

        this.filesTypes$ = this.route.data.pipe(map((data) => data.types));

        this.filesTypes$
            .pipe(takeUntil(this.unsubscribe$))
            .subscribe((types) => {
                this.filesTypes = types;
                this._changeDetectorRef.detectChanges();
            });

        this.filesServiceStrategy.pdfUpdated.subscribe(
            (updatedDocumentId: number) => {
                if (this.documentId === updatedDocumentId) {
                    this.loadPDF(this.documentId);
                }
            },
        );
        this.loadPDF(this.documentId);
    }

    ngOnChanges(changes: SimpleChanges) {
        if (changes.documentId && !changes.documentId.isFirstChange()) {
            this.loadPDF(this.documentId);
        }
    }

    ngOnDestroy() {
        this.unsubscribe$.next();
        this.unsubscribe$.complete();
    }

    loadPDF(documentId: number) {
        this.filesServiceStrategy.downloadPdfDocument(documentId).subscribe({
            next: (response) => {
                if (response.blob instanceof Blob) {
                    this.pdfSrc = URL.createObjectURL(response.blob);
                    this._changeDetectorRef.detectChanges();
                } else {
                    console.error(
                        'The fetched data is not a Blob:',
                        response.blob,
                    );
                    this.pdfSrc = null;
                }
            },
            error: (error) => {
                console.error('Error fetching the PDF:', error);
                this.pdfSrc = null;
            },
        });
    }

    downloadPDF() {
        this.filesServiceStrategy
            .downloadPdfDocument(this.documentId)
            .subscribe({
                next: ({ blob, filename }) => {
                    const sanitizedFilename =
                        this.removeGuidFromFilename(filename);
                    const blobUrl = URL.createObjectURL(blob);
                    const a = document.createElement('a');
                    a.href = blobUrl;
                    a.download = sanitizedFilename;
                    a.click();
                    URL.revokeObjectURL(blobUrl);
                },
                error: (error) => {
                    console.error('Error downloading the PDF:', error);
                },
            });
    }

    removeGuidFromFilename(filename: string): string {
        // GUID pattern
        const guidPattern =
            /[0-9a-f]{8}-[0-9a-f]{4}-[0-9a-f]{4}-[0-9a-f]{4}-[0-9a-f]{12}_/i;
        return filename.replace(guidPattern, '');
    }

    openUpdateDialog(): void {
        const dialogRef = this.dialog.open(UpdatePdfDialogComponent, {
            data: {
                customerId: this.customerId,
                pdfTypes: this.filesTypes,
                documentId: this.documentId,
                selectedEntity: this.selectedEntity,
            },
        });
        dialogRef.afterClosed().subscribe((result) => {
            if (result) {
                // Check if result exists
                const updatedDocumentId = result.id; // Assuming the result contains the new document ID
                this.handlePdfUpdated(updatedDocumentId);
            }
        });
    }

    handlePdfUpdated(updatedDocumentId: number) {
        this.filesServiceStrategy.pdfUpdated.emit(updatedDocumentId);
    }

    openDeleteDialog(): void {
        const dialogRef = this.dialog.open(DeletePdfDialogComponent, {
            data: {
                customerId: this.customerId,
                pdfTypes: this.filesTypes,
                documentId: this.documentId,
                selectedEntity: this.selectedEntity,
            },
        });
        dialogRef.afterClosed().subscribe((result) => {
            if (result) {
                const deletedDocumentId = result.id;
                this.handlePdfDeleted(deletedDocumentId);
            }
        });
    }

    handlePdfDeleted(deletedDocumentId: number) {
        this.filesServiceStrategy.pdfDeleted.emit(deletedDocumentId);
    }
}
