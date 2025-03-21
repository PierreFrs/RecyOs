import {
    AfterViewInit,
    Component,
    OnDestroy,
    OnInit,
    ViewChild,
} from '@angular/core';
import { MatPaginator, MatPaginatorModule } from '@angular/material/paginator';
import { MatSort, MatSortModule } from '@angular/material/sort';
import {
    Commercial,
    CommercialPagination,
} from '../../../../../models/commercial.type';
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
} from 'rxjs';
import { MatDialog } from '@angular/material/dialog';
import { UserService } from '../../../../../core/services/user.service';
import { CommerciauxService } from '../../../../../core/services/commerciaux.service';
import { forEach } from 'lodash';
import {
    FormControl,
    UntypedFormControl,
    ReactiveFormsModule,
} from '@angular/forms';
import { NewCommercialDialogComponent } from '../new-commercial-dialog/new-commercial-dialog.component';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatSelectModule } from '@angular/material/select';
import { MatInputModule } from '@angular/material/input';
import { MatIconModule } from '@angular/material/icon';
import { MatButtonModule } from '@angular/material/button';
import {
    AsyncPipe,
    NgClass,
    NgForOf,
    NgIf,
    NgTemplateOutlet,
} from '@angular/common';
import { FormatFrenchPhonePipe } from 'app/shared/pipes/format-french-phone.pipe';
import { CommerciauxDetailsComponent } from '../commerciaux-details/commerciaux-details.component';
@Component({
    selector: 'app-commerciaux-list',
    templateUrl: './commerciaux-list.component.html',
    styleUrls: ['./commerciaux-list.component.scss'],
    standalone: true,
    imports: [
        MatFormFieldModule,
        MatSelectModule,
        MatInputModule,
        MatIconModule,
        MatButtonModule,
        MatPaginatorModule,
        NgClass,
        MatSortModule,
        ReactiveFormsModule,
        AsyncPipe,
        NgIf,
        NgForOf,
        FormatFrenchPhonePipe,
        NgTemplateOutlet,
        CommerciauxDetailsComponent,
    ],
})
export class CommerciauxListComponent
    implements OnInit, AfterViewInit, OnDestroy
{
    @ViewChild(MatPaginator) private readonly _paginator: MatPaginator;
    @ViewChild(MatSort) private readonly _sort: MatSort;

    isLoading: boolean = false;
    searchInputControl: UntypedFormControl = new UntypedFormControl();
    selectedFilterType = new FormControl(0);
    commerciaux$: Observable<Commercial[]>;
    selectedCommercialId: number;
    pagination: CommercialPagination;

    private readonly _unsubscribeAll: Subject<any> = new Subject<any>();

    userIsAdmin = false;
    userIsCreateCommercial = false;

    constructor(
        private readonly _dialog: MatDialog,
        private readonly _userService: UserService,
        private readonly _commerciauxService: CommerciauxService
    ) {}

    ngOnInit(): void {
        this.determineUserPrivileges();
        this.setEntitiesObservable();
        this.subscribeToRefreshNeeded();
        this.fetchPaginationModel();
        this.setSortingOrder();
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
                        if (role.name === 'create_commercial') {
                            this.userIsCreateCommercial = true;
                        }
                    });
                }
            });
    }
    private setEntitiesObservable(): void {
        this.commerciaux$ = this._commerciauxService.commerciaux$;
    }
    private subscribeToRefreshNeeded(): void {
        this._commerciauxService.refreshNeeded.subscribe(() => {
            this.updateCommerciauxStream();
        });
    }
    private fetchPaginationModel(): void {
        this._commerciauxService.pagination$
            .pipe(takeUntil(this._unsubscribeAll))
            .subscribe((pagination: CommercialPagination) => {
                this.pagination = pagination;
            });
    }
    private setSortingOrder(): void {
        const defaultSortField = 'lastname';
        combineLatest([
            this.searchInputControl.valueChanges.pipe(startWith('')),
            this.selectedFilterType.valueChanges.pipe(startWith(0)),
        ])
            .pipe(
                takeUntil(this._unsubscribeAll),
                debounceTime(300),
                switchMap(([query, filterType]) => {
                    this.closeDetails();
                    this.isLoading = true;
                    return this._commerciauxService.fetchCommerciauxWithPagination(
                        0,
                        10,
                        defaultSortField,
                        'asc',
                        query,
                        filterType
                    );
                }),
                map(() => {
                    this.isLoading = false;
                })
            )
            .subscribe();

        if (this._sort && this._paginator) {
            this._sort.sort({
                id: defaultSortField,
                start: 'asc',
                disableClear: true,
            });
        }
    }

    ngAfterViewInit(): void {
        this.updateCommerciauxStream();
    }
    private updateCommerciauxStream(): void {
        const sortChange$ = this._sort.sortChange.pipe(
            startWith({ active: 'firstname', direction: 'asc' })
        );
        const pageChange$ = this._paginator.page.pipe(
            startWith({ pageIndex: 0, pageSize: 10 })
        );

        merge(sortChange$, pageChange$)
            .pipe(
                takeUntil(this._unsubscribeAll),
                switchMap(() => {
                    const sortDir = this._sort.direction;
                    const sortActive = this._sort.active;
                    const pageIndex = this._paginator.pageIndex;
                    const pageSize = this._paginator.pageSize;
                    const searchQuery = this.searchInputControl.value;
                    const filterType = this.selectedFilterType.value;

                    return this._commerciauxService.fetchCommerciauxWithPagination(
                        pageIndex,
                        pageSize,
                        sortActive,
                        sortDir,
                        searchQuery,
                        filterType
                    );
                }),
                catchError(() => of([]))
            )
            .subscribe();
        this.commerciaux$ = this._commerciauxService.commerciaux$;
    }

    createCommercial(): void {
        const dialogRef = this._dialog.open(NewCommercialDialogComponent, {
            width: '700px',
        });

        dialogRef.afterClosed().subscribe(() => {
            console.log('The dialog was closed');
        });
    }
    trackByFn(index: number, item: any): any {
        return item.id || index;
    }
    closeDetails(): void {
        this.selectedCommercialId = null;
    }
    ngOnDestroy(): void {
        this._unsubscribeAll.next(null);
        this._unsubscribeAll.complete();
    }
    toggleDetails(prmId: number): void {
        if (this.selectedCommercialId === prmId) {
            this.closeDetails();
            return;
        }
        this.selectedCommercialId = prmId;
    }
}
