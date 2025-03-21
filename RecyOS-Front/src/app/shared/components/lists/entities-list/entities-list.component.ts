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
    FormControl,
    ReactiveFormsModule,
    UntypedFormControl,
} from '@angular/forms';
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
import { UserService } from '../../../../core/services/user.service';
import { MatDialog } from '@angular/material/dialog';
import { forEach } from 'lodash';
import { EntityStrategyService } from '../../../../core/strategies/entity-strategy/entity-strategy.service';
import { IEntityServiceStrategy } from '../../../../core/strategies/entity-strategy/IEntityServiceStrategy';
import {
    BusinessUnitDto,
    EntityBusinessUnitDto,
} from '../../../../models/business-unit.type';
import { NewEntityDialogComponent } from '../../dialogs/new-entity-dialog-component/new-entity-dialog.component';
import { IStrategyObject } from '../../../../core/strategies/entity-strategy/IStrategyObject';
import { AsyncPipe, CommonModule, NgClass } from '@angular/common';
import { MatProgressBarModule } from '@angular/material/progress-bar';
import { MatInputModule } from '@angular/material/input';
import { MatSelectModule } from '@angular/material/select';
import { MatIconModule } from '@angular/material/icon';
import { MatButtonModule } from '@angular/material/button';
import { NdCoverStatusComponent } from '../../../../widgets/nd-cover-status/nd-cover-status.component';
import { SharedModule } from '../../../shared.module';
import { StreamEvent } from '../../../../models/stream-event.type';
import { EntityDetailsComponent } from '../../entity-details/entity-details.component';
import {
    EntityDto,
    EntityDTOPagination,
    EntityFormDto,
} from '../../../../models/entities-models/entity.type';

@Component({
    selector: 'app-entities-list',
    templateUrl: './entities-list.component.html',
    styleUrls: ['./entities-list.component.scss'],
    standalone: true,
    imports: [
        CommonModule,
        MatSortModule,
        MatPaginatorModule,
        NgClass,
        MatProgressBarModule,
        MatInputModule,
        MatSelectModule,
        ReactiveFormsModule,
        MatIconModule,
        MatButtonModule,
        AsyncPipe,
        NdCoverStatusComponent,
        SharedModule,
        EntityDetailsComponent,
    ],
})
export class EntitiesListComponent implements OnInit, AfterViewInit, OnDestroy {
    @ViewChild(MatPaginator) private readonly _paginator: MatPaginator;
    @ViewChild(MatSort) private readonly _sort: MatSort;

    selectedEntity: EntityDto;
    selectedEntityId: number | null = null;
    entities: EntityDto[] = [];
    entities$: Observable<EntityDto[]>;
    pagination: EntityDTOPagination;
    strategyObject: IStrategyObject;
    searchInputControl: UntypedFormControl = new UntypedFormControl();
    selectedFilterType = new FormControl(0);
    isLoading: boolean = false;

    private readonly _unsubscribeAll: Subject<any> = new Subject<any>();
    private entityServiceStrategy: IEntityServiceStrategy<
        EntityDto,
        EntityDTOPagination,
        EntityFormDto,
        BusinessUnitDto,
        EntityBusinessUnitDto
    >;
    private pageSize = 10;
    private pageIndex = 0;
    private sortField: string;
    private sortDirection: '' | 'asc' | 'desc';
    private searchValue: string;
    private filterType: number;

    userIsAdmin = false;
    userIsReadSupplier = false;
    userIsCreateSupplier = false;
    userIsReadClient = false;
    userIsCreateClient = false;

    constructor(
        private readonly _changeDetectorRef: ChangeDetectorRef,
        private readonly _dialog: MatDialog,
        private readonly _userService: UserService,
        private readonly _entityStrategyService: EntityStrategyService
    ) {}

    ngOnInit(): void {
        this.determineUserPrivileges();
        this.strategyObject = this._entityStrategyService.determineStrategy();
        this.entityServiceStrategy = this.strategyObject.strategy;
        this.fetchInitialEntities();
        this.fetchPaginationModel();
        this.setEntitiesObservable();
    }

    private determineUserPrivileges(): void {
        this._userService.user$
            .pipe(takeUntil(this._unsubscribeAll))
            .subscribe((user) => {
                if (user?.id) {
                    forEach(user.roles, (role) => {
                        if (role.name === 'admin') {
                            this.userIsAdmin = true;
                        }
                        if (role.name === 'read_fournisseur') {
                            this.userIsReadSupplier = true;
                        }
                        if (role.name === 'create_fournisseur') {
                            this.userIsCreateSupplier = true;
                        }
                        if (role.name === 'read_client') {
                            this.userIsReadClient = true;
                        }
                        if (role.name === 'create_client') {
                            this.userIsCreateClient = true;
                        }
                    });
                }
            });
    }

    private fetchInitialEntities(): void {
        this.entityServiceStrategy.entities$
            .pipe(takeUntil(this._unsubscribeAll))
            .subscribe((entities) => {
                this.entities = entities || [];
                this._changeDetectorRef.markForCheck();
            });
    }

    private fetchPaginationModel(): void {
        this.entityServiceStrategy.pagination$
            .pipe(takeUntil(this._unsubscribeAll))
            .subscribe((pagination: EntityDTOPagination) => {
                this.pagination = pagination;
                this._changeDetectorRef.markForCheck();
            });
    }

    private setEntitiesObservable(): void {
        this.entities$ = this.entityServiceStrategy.entities$;
    }

    private getDefaultSortField(): string {
        return this.strategyObject.region === 'france' ? 'Siret' : 'Vat';
    }

    ngAfterViewInit(): void {
        const initialTrigger$: Observable<StreamEvent> = of({
            type: 'initial',
        });

        const sortChanges$: Observable<StreamEvent> =
            this._sort.sortChange.pipe(
                map((sort) => ({ type: 'sort', value: sort }))
            );

        const pageChanges$: Observable<StreamEvent> = this._paginator.page.pipe(
            map((page) => ({ type: 'page', value: page }))
        );

        const filterChanges$: Observable<StreamEvent> = combineLatest([
            this.searchInputControl.valueChanges.pipe(startWith('')),
            this.selectedFilterType.valueChanges.pipe(startWith(0)),
        ]).pipe(
            debounceTime(300),
            map((filter) => ({ type: 'filter', value: filter }))
        );

        merge(initialTrigger$, sortChanges$, pageChanges$, filterChanges$)
            .pipe(
                switchMap((event) => {
                    if (event.type === 'initial') {
                        this.isLoading = false;
                        return of([]);
                    }

                    // Update state based on the event type and immediately fetch data
                    switch (event.type) {
                        case 'sort':
                            this.handleSortChange(event.value);
                            this.resetPagination(); // Reset pagination for sort changes
                            break;
                        case 'page':
                            this.handlePageChange(event.value);
                            break;
                        case 'filter':
                            this.handleFilterChange(
                                event.value as [any, number]
                            );
                            this.resetPagination(); // Reset pagination for filter changes
                            break;
                    }

                    // Directly fetch entities after state update and pagination reset
                    return this.fetchEntities();
                }),
                takeUntil(this._unsubscribeAll)
            )
            .subscribe((items) => {
                this.entities = items;
                this._changeDetectorRef.markForCheck();
            });
    }

    private fetchEntities(): Observable<EntityDto[]> {
        this.isLoading = true;

        return this.entityServiceStrategy
            .getEntities(
                this._paginator.pageIndex,
                this.pageSize,
                this.sortField,
                this.sortDirection,
                this.searchValue,
                this.filterType
            )
            .pipe(
                map((response) => response.items),
                tap(() => (this.isLoading = false)),
                catchError(() => {
                    this.isLoading = false;
                    return of([]);
                })
            );
    }

    private resetPagination(): void {
        this._paginator.pageIndex = 0;
    }

    private handleSortChange(sort: Sort): void {
        this.sortField = sort.active || this.getDefaultSortField();
        this.sortDirection = sort.direction as '' | 'asc' | 'desc';
    }

    private handlePageChange(page: PageEvent): void {
        this.pageIndex = page.pageIndex;
        this.pageSize = page.pageSize;
    }

    private handleFilterChange(filter: [any, number]): void {
        [this.searchValue, this.filterType] = filter;
    }

    createEntity(): void {
        const dialogRef = this._dialog.open(NewEntityDialogComponent, {
            width: '700px',
        });

        dialogRef.afterClosed().subscribe(() => {});
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

    ngOnDestroy(): void {
        this._unsubscribeAll.next(null);
        this._unsubscribeAll.complete();
    }

    toggleDetails(entity: EntityDto): void {
        if (this.selectedEntityId === entity.id) {
            this.closeDetails();
            return;
        }
        this.selectedEntityId = entity.id;
        this.selectedEntity = entity;
        this._changeDetectorRef.markForCheck();
    }

    closeDetails(): void {
        this.selectedEntityId = null;
    }

    get entityDisplayTitle(): string {
        let status = '';
        let entityType = '';
        let region = '';

        if (this.strategyObject.status === 'professional') {
            status = 'professionnal';
        } else if (this.strategyObject.status === 'particulier') {
            status = 'particulier';
        }

        if (this.strategyObject.type === 'client') {
            entityType = 'Clients';
        } else if (this.strategyObject.type === 'supplier') {
            entityType = 'Fournisseurs';
        }

        if (this.strategyObject.region === 'france') {
            region = 'français';
        } else if (this.strategyObject.region === 'europe') {
            region = 'européens';
        }

        if (this.strategyObject.status === 'professional') {
            return `${entityType} ${region}`;
        } else if (this.strategyObject.status === 'particulier') {
            status = 'particuliers';
            return `${entityType} ${status}`;
        }

        return '';
    }
}
