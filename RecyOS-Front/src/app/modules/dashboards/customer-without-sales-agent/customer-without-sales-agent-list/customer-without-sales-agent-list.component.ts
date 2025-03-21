import {
    AfterViewInit,
    ChangeDetectorRef,
    Component,
    OnDestroy,
    OnInit,
    ViewChild,
} from '@angular/core';
import {
    MatPaginator,
    MatPaginatorModule,
    PageEvent,
} from '@angular/material/paginator';
import { MatSort, MatSortModule, Sort } from '@angular/material/sort';
import {
    catchError,
    combineLatest,
    debounceTime,
    map,
    merge,
    Observable,
    of,
    startWith,
    Subject,
    switchMap,
    takeUntil,
    tap,
} from 'rxjs';
import { CommerciauxService } from '../../../../core/services/commerciaux.service';
import {
    FormControl,
    ReactiveFormsModule,
    UntypedFormControl,
} from '@angular/forms';
import { StreamEvent } from '../../../../models/stream-event.type';
import { MatProgressBarModule } from '@angular/material/progress-bar';
import { MatInputModule } from '@angular/material/input';
import { MatSelectModule } from '@angular/material/select';
import { MatIconModule } from '@angular/material/icon';
import {
    AsyncPipe,
    NgClass,
    NgForOf,
    NgIf,
    NgTemplateOutlet,
} from '@angular/common';
import { SharedModule } from '../../../../shared/shared.module';
import { EntityDetailsComponent } from '../../../../shared/components/entity-details/entity-details.component';
import { MatButtonModule } from '@angular/material/button';
import {ActivatedRoute} from "@angular/router";
import {Commercial} from "../../../../models/commercial.type";
import {EntityDto, EntityDTOPagination} from "../../../../models/entities-models/entity.type";

@Component({
    selector: 'app-customer-without-sales-agent-list',
    templateUrl: './customer-without-sales-agent-list.component.html',
    styleUrls: ['./customer-without-sales-agent-list.component.scss'],
    standalone: true,
    imports: [
        MatProgressBarModule,
        MatInputModule,
        MatSelectModule,
        ReactiveFormsModule,
        MatIconModule,
        AsyncPipe,
        MatSortModule,
        MatPaginatorModule,
        NgClass,
        SharedModule,
        NgIf,
        NgForOf,
        NgTemplateOutlet,
        EntityDetailsComponent,
        MatButtonModule,
    ],
})
export class CustomerWithoutSalesAgentListComponent
    implements OnInit, OnDestroy, AfterViewInit
{
    @ViewChild(MatPaginator) private readonly _paginator: MatPaginator;
    @ViewChild(MatSort) private readonly _sort: MatSort;

    selectedClientSansCommercialId: number | null = null;
    selectedClientSansCommercial: EntityDto;
    clientsSansCommerciaux: EntityDto[];
    clientsSansCommerciaux$: Observable<EntityDto[]>;
    pagination: EntityDTOPagination;
    searchInputControl: UntypedFormControl = new UntypedFormControl();
    sortTypeControl = new FormControl(0);
    searchTypeControl = new FormControl(0);
    isLoading: boolean = false;
    commerciauxList: Commercial[];
    sansCommercialId: number;

    private readonly _unsubscribeAll: Subject<any> = new Subject<any>();
    private pageSize = 10;
    private pageIndex = 0;
    private sortField: string;
    private sortDirection: '' | 'asc' | 'desc';
    private searchValue: string;
    private searchFilterType: number;
    private unitFilterType: number;

    constructor(
        private readonly _commerciauxService: CommerciauxService,
        private readonly _changeDetectorRef: ChangeDetectorRef,
        private readonly route: ActivatedRoute,
        private readonly cdr : ChangeDetectorRef
    ) {}

    ngOnInit(): void {
        this.fetchInitialEntities();
        this.clientsSansCommerciaux$ =
            this._commerciauxService.clientsByCommercialId$;
        this.fetchPaginationModel();
        this.fetchCommercialList();
        this.getSansCommercialId();

    }
    private fetchInitialEntities(): void {
        this._commerciauxService.clientsByCommercialId$
            .pipe(takeUntil(this._unsubscribeAll))
            .subscribe((clientsSansCommerciaux: EntityDto[]) => {
                this.clientsSansCommerciaux = clientsSansCommerciaux;
                this._changeDetectorRef.markForCheck();
            });
    }
    private fetchPaginationModel(): void {
        this._commerciauxService.pagination$
            .pipe(takeUntil(this._unsubscribeAll))
            .subscribe((pagination: EntityDTOPagination) => {
                this.pagination = pagination;
                this._changeDetectorRef.markForCheck();
            });
    }

    private fetchCommercialList(): void {
        this.route.data.subscribe((data) => {
            this.commerciauxList = data.commerciaux;
        });
    }

    private getSansCommercialId(): void {
        this.sansCommercialId = this.commerciauxList.find(
            (commercial) => commercial.email === 'null@null.null',
        ).id;
    }

    ngAfterViewInit(): void {
        const initialTrigger$: Observable<StreamEvent> = of({
            type: 'initial',
        });

        const sortChanges$: Observable<StreamEvent> =
            this._sort.sortChange.pipe(
                map((sort) => ({ type: 'sort', value: sort })),
            );

        const pageChanges$: Observable<StreamEvent> = this._paginator.page.pipe(
            map((page) => ({ type: 'page', value: page })),
        );

        const filterChanges$: Observable<StreamEvent> = combineLatest([
            this.searchInputControl.valueChanges.pipe(startWith('')),
            this.searchTypeControl.valueChanges.pipe(startWith(0)),
            this.sortTypeControl.valueChanges.pipe(startWith(0)),
        ]).pipe(
            debounceTime(300),
            map((filter) => ({ type: 'filter', value: filter })),
        );

        merge(initialTrigger$, sortChanges$, pageChanges$, filterChanges$)
            .pipe(
                switchMap((event) => {
                    if (event.type === 'initial') {
                        this.isLoading = false;
                        return of([]);
                    }

                    switch (event.type) {
                        case 'sort':
                            this.handleSortChange(event.value);
                            this.resetPagination();
                            break;
                        case 'page':
                            this.handlePageChange(event.value);
                            break;
                        case 'filter':
                            this.handleFilterChange(
                                event.value as [any, number, number],
                            );
                            this.resetPagination();
                            break;
                    }

                    return this.fetchEntities();
                }),
                takeUntil(this._unsubscribeAll),
            )
            .subscribe((items) => {
                this.clientsSansCommerciaux = items;
                this._changeDetectorRef.markForCheck();
            });
    }

    private fetchEntities(): Observable<EntityDto[]> {
        this.isLoading = true;

        return this._commerciauxService
            .fetchClientsByCommercialId(
                this.sansCommercialId,
                this._paginator.pageIndex,
                this.pageSize,
                this.sortField,
                this.sortDirection,
                this.searchValue,
                this.searchFilterType,
                this.unitFilterType,
            )
            .pipe(
                map((response) => response.items),
                tap(() => (this.isLoading = false)),
                catchError(() => {
                    this.isLoading = false;
                    return of([]);
                }),
            );
    }

    private resetPagination(): void {
        this._paginator.pageIndex = 0;
    }

    private handleSortChange(sort: Sort): void {
        this.sortField = sort.active || 'name';
        this.sortDirection = sort.direction || 'asc';
    }

    private handlePageChange(page: PageEvent): void {
        this.pageIndex = page.pageIndex;
        this.pageSize = page.pageSize;
    }

    private handleFilterChange(filter: [any, number, number]): void {
        [this.searchValue, this.searchFilterType, this.unitFilterType] = filter;
    }

    ngOnDestroy(): void {
        this._unsubscribeAll.next(null);
        this._unsubscribeAll.complete();
    }

    toggleDetails(client: EntityDto) {
        if (this.selectedClientSansCommercialId === client.id) {
            this.closeDetails();
            return;
        }
        this.selectedClientSansCommercialId = client.id;
        this.selectedClientSansCommercial = client;
        // Next line get rid of the error "ExpressionChangedAfterItHasBeenCheckedError"
        this.cdr.detectChanges();
    }

    closeDetails(): void {
        this.selectedClientSansCommercialId = null;
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
