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
import { fuseAnimations } from '../../../../../@fuse/animations';
import { ActivatedRoute } from '@angular/router';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { BusinessCouvertureServices } from '../../../../shared/components/entity-details/business-couverture/business-couverture.service';
import { map, merge, Observable, Subject, switchMap, takeUntil } from 'rxjs';
import {
    EntrepriseCouvertureDTO,
    EntrepriseCouvertureDTOPagination,
} from '../../../../shared/components/entity-details/business-couverture/business-couverture.type';
import { BusinessesServices } from '../../../../core/services/entity-services/businesses.services';
import { EntrepriseBaseDTO } from '../../../../models/entities-models/french.type';

@Component({
    selector: 'dashboard-customer-coverage-list',
    templateUrl: './dashboard-customer-coverage-list.component.html',
    styleUrls: ['./dashboard-customer-coverage-list.component.scss'],
    encapsulation: ViewEncapsulation.None,
    changeDetection: ChangeDetectionStrategy.OnPush,
    animations: fuseAnimations,
})
export class DashboardCustomerCoverageListComponent
    implements OnInit, OnDestroy, AfterViewInit
{
    @ViewChild(MatPaginator) private _paginator: MatPaginator;
    @ViewChild(MatSort) private _sort: MatSort;
    isLoading: boolean = false;
    title: string;
    private _unsubscribeAll: Subject<any> = new Subject<any>();
    entreprisesCouvertures: EntrepriseCouvertureDTO[];
    entreprisesCouverture$: Observable<EntrepriseCouvertureDTO[]>;
    selectedCouvertureId: number | null = null;
    pagination: EntrepriseCouvertureDTOPagination;
    modeFiltre: number;
    siren: string;
    entrepriseBaseClient: EntrepriseBaseDTO;
    constructor(
        private _activatedRoute: ActivatedRoute,
        private _changeDetectorRef: ChangeDetectorRef,
        private _businessCouvertureServices: BusinessCouvertureServices,
        private _businessesServices: BusinessesServices,
    ) {}

    ngOnInit(): void {
        this._activatedRoute.queryParams.pipe().subscribe((data) => {
            if (data.mode === 'all') {
                this.title = 'Liste des demandes de couverture';
                this.modeFiltre = 0;
            } else if (data.mode === 'accord') {
                this.title = 'Liste des demandes de couverture accordées';
                this.modeFiltre = 1;
            } else if (data.mode === 'refus') {
                this.title = 'Liste des demandes de couverture refusées';
                this.modeFiltre = 2;
            }
        });

        this._businessCouvertureServices.couvertureClients$
            .pipe(takeUntil(this._unsubscribeAll))
            .subscribe((entreprisesCouvertures: EntrepriseCouvertureDTO[]) => {
                this.entreprisesCouvertures = entreprisesCouvertures;
                this._changeDetectorRef.markForCheck();
            });

        this.entreprisesCouverture$ =
            this._businessCouvertureServices.couvertureClients$;

        this._businessCouvertureServices.pagination$
            .pipe(takeUntil(this._unsubscribeAll))
            .subscribe((pagination) => {
                this.pagination = pagination;
                this._changeDetectorRef.markForCheck();
            });
    }

    ngAfterViewInit(): void {
        if (this._sort && this._paginator) {
            // If the user changes the sort order...
            this._sort.sortChange
                .pipe(takeUntil(this._unsubscribeAll))
                .subscribe(() => {
                    // Reset back to the first page
                    this._paginator.pageIndex = 0;

                    // Close the details
                    // this.closeDetails();
                });
            // Get coverage if sort or page changes
            merge(this._sort.sortChange, this._paginator.page)
                .pipe(
                    switchMap(() => {
                        //  this.closeDetails();
                        this.isLoading = true;
                        return this._businessCouvertureServices.getCouvertureClients(
                            this._paginator.pageIndex,
                            this._paginator.pageSize,
                            this._sort.active,
                            this._sort.direction,
                            'on',
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

    toggleDetails(prmId: number) {
        if (this.selectedCouvertureId === prmId) {
            this.closeDetails();
            return;
        }
        this.selectedCouvertureId = prmId;
        const selectedCouverture = this.entreprisesCouvertures.find(
            (couverture) => couverture.id === prmId,
        );
        if (selectedCouverture) {
            const siren = selectedCouverture.siren;
            this._businessesServices
                .getEntrepriseBaseBySiren(siren)
                .subscribe((entrepriseBase) => {
                    this.entrepriseBaseClient = entrepriseBase;
                    this._changeDetectorRef.markForCheck();
                });
        }
        this._changeDetectorRef.markForCheck();
    }

    closeDetails(): void {
        this.selectedCouvertureId = null;
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
