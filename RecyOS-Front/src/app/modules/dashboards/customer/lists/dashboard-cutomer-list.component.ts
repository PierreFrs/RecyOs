import {
    AfterViewInit,
    ChangeDetectionStrategy,
    ChangeDetectorRef,
    Component,
    OnDestroy,
    OnInit,
    ViewChild,
    ViewEncapsulation,
} from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { map, merge, Observable, Subject, switchMap, takeUntil } from 'rxjs';
import {
    FrenchDTO,
    EtablissementDTOPagination,
} from '../../../../models/entities-models/french.type';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { fuseAnimations } from '../../../../../@fuse/animations';
import { BusinessesServices } from '../../../../core/services/entity-services/businesses.services';
import {EntityDto} from "../../../../models/entities-models/entity.type";

@Component({
    selector: 'dashboard-customer-list',
    templateUrl: './dashboard-customer-list.component.html',
    styleUrls: ['./dashboard-customer-list.component.scss'],
    encapsulation: ViewEncapsulation.None,
    changeDetection: ChangeDetectionStrategy.OnPush,
    animations: fuseAnimations,
})
export class DashboardCustomerListComponent
    implements OnInit, OnDestroy, AfterViewInit
{
    @ViewChild(MatPaginator) private readonly _paginator: MatPaginator;
    @ViewChild(MatSort) private readonly _sort: MatSort;
    isLoading: boolean = false;
    title: string;
    private readonly _unsubscribeAll: Subject<any> = new Subject<any>();
    etablissementsClients: FrenchDTO[];
    etablissementsClient$: Observable<FrenchDTO[]>;
    selectedEtablissementId: number | null = null;
    selectedEntity: EntityDto;
    pagination: EtablissementDTOPagination;
    modeFiltre: number;
    constructor(
        private readonly _activatedRoute: ActivatedRoute,
        private readonly _businessesServices: BusinessesServices,
        private readonly _changeDetectorRef: ChangeDetectorRef,
    ) {}

    ngOnInit(): void {
        this._activatedRoute.queryParams
            .pipe(takeUntil(this._unsubscribeAll))
            .subscribe((data) => {
                // definir un titre de page en fonction du parametre mode
                if (data.mode === 'radie') {
                    this.title = 'Liste des établissements radiés';
                    this.modeFiltre = 7;
                } else if (data.mode === 'badmail') {
                    this.title =
                        'Liste des établissements avec adresse mail invalide';
                    this.modeFiltre = 4;
                } else if (data.mode === 'badtel') {
                    this.title =
                        'Liste des établissements avec numéro de téléphone invalide';
                    this.modeFiltre = 5;
                } else if (data.mode === 'factor') {
                    this.title = 'Liste des établissements en affacturage';
                    this.modeFiltre = 6;
                }
            });

        this._businessesServices.etablissementsClients$
            .pipe(takeUntil(this._unsubscribeAll))
            .subscribe((etablissementsClient: FrenchDTO[]) => {
                this.etablissementsClients = etablissementsClient;
                this._changeDetectorRef.markForCheck();
            });

        this.etablissementsClient$ =
            this._businessesServices.etablissementsClients$;

        this._businessesServices.pagination$
            .pipe(takeUntil(this._unsubscribeAll))
            .subscribe((pagination: EtablissementDTOPagination) => {
                this.pagination = pagination;
                this._changeDetectorRef.markForCheck();
            });
    }

    ngAfterViewInit(): void {
        if (this._sort && this._paginator) {
            // Mark for check
            this._changeDetectorRef.markForCheck();

            // If the user changes the sort order...
            this._sort.sortChange
                .pipe(takeUntil(this._unsubscribeAll))
                .subscribe(() => {
                    // Reset back to the first page
                    this._paginator.pageIndex = 0;

                    // Close the details
                    // this.closeDetails();
                });

            // Get products if sort or page changes
            merge(this._sort.sortChange, this._paginator.page)
                .pipe(
                    switchMap(() => {
                        this.isLoading = true;
                        return this._businessesServices.getEtablissementsClients(
                            this._paginator.pageIndex,
                            this._paginator.pageSize,
                            this._sort.active,
                            this._sort.direction,
                            'radiationDate',
                            this.modeFiltre,
                        );
                    }),
                    map(() => {
                        this.isLoading = false;
                        this._changeDetectorRef.markForCheck();
                    }),
                )
                .subscribe();
        }
    }

    ngOnDestroy(): void {
        this._unsubscribeAll.next(null);
        this._unsubscribeAll.complete();
    }

    toggleDetails(entity: EntityDto) {
        if (this.selectedEtablissementId === entity.id) {
            this.closeDetails();
            return;
        }
        this.selectedEtablissementId = entity.id;
        this.selectedEntity = entity;
        this._changeDetectorRef.markForCheck();
    }

    closeDetails(): void {
        this.selectedEtablissementId = null;
    }

    /**
     * Track by function for ngFor loops
     *
     * @param index
     * @param item
     */
    trackByFn(index: number, item: any): any {
        return item.id || index;
    }
}
