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
import { BusinessNDCoverServices } from '../../../../core/services/business-nd-cover.service';
import { map, merge, Observable, Subject, switchMap, takeUntil } from 'rxjs';
import {
    EntrepriseNDCoverDTO,
    EntrepriseNDCoverDTOPagination,
} from '../../../../models/business-nd-cover.type';

@Component({
    selector: 'dashboard-customer-nd-cover-list',
    templateUrl: './dashboard-customer-nd-cover-list.component.html',
    styleUrls: ['./dashboard-customer-nd-cover-list.component.scss'],
    encapsulation: ViewEncapsulation.None,
    changeDetection: ChangeDetectionStrategy.OnPush,
    animations: fuseAnimations,
})
export class DashboardCustomerNDCoverListComponent
    implements OnInit, OnDestroy, AfterViewInit
{
    @ViewChild(MatPaginator) private _paginator: MatPaginator;
    @ViewChild(MatSort) private _sort: MatSort;
    isLoading: boolean = false;
    title: string;
    private _unsubscribeAll: Subject<any> = new Subject<any>();
    entreprisesNDCovers: EntrepriseNDCoverDTO[];
    entreprisesNDCover$: Observable<EntrepriseNDCoverDTO[]>;
    selectedNDCoverId: number | null = null;
    pagination: EntrepriseNDCoverDTOPagination;
    modeFiltre: number;
    constructor(
        private _activatedRoute: ActivatedRoute,
        private _changeDetectorRef: ChangeDetectorRef,
        private _businessNDCoverServices: BusinessNDCoverServices,
    ) {}

    ngOnInit(): void {
        this._activatedRoute.queryParams.pipe().subscribe((data) => {
            if (data.mode === 'all') {
                this.title = 'Liste des demandes de ND Cover';
                this.modeFiltre = 0;
            } else if (data.mode === 'accord') {
                this.title = 'Liste des demandes de ND Cover accordées';
                this.modeFiltre = 1;
            } else if (data.mode === 'refus') {
                this.title = 'Liste des demandes de ND Cover refusées';
                this.modeFiltre = 2;
            }
        });

        this._businessNDCoverServices.couvertureClients$
            .pipe(takeUntil(this._unsubscribeAll))
            .subscribe((entreprisesNDCovers: EntrepriseNDCoverDTO[]) => {
                this.entreprisesNDCovers = entreprisesNDCovers;
                this._changeDetectorRef.markForCheck();
            });

        this.entreprisesNDCover$ =
            this._businessNDCoverServices.couvertureClients$;

        this._businessNDCoverServices.pagination$
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
                        return this._businessNDCoverServices.getNDCoverClients(
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
        if (this.selectedNDCoverId === prmId) {
            this.closeDetails();
            return;
        }
        this.selectedNDCoverId = prmId;
        this._changeDetectorRef.markForCheck();
    }

    closeDetails(): void {
        this.selectedNDCoverId = null;
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
