import { CommonModule } from '@angular/common';
import {
    AfterViewInit,
    Component,
    OnDestroy,
    OnInit,
    ViewChild,
} from '@angular/core';
import {
    FormControl,
    ReactiveFormsModule,
    UntypedFormControl,
} from '@angular/forms';
import { MatDialog } from '@angular/material/dialog';
import { MatPaginator, MatPaginatorModule } from '@angular/material/paginator';
import { MatSort, MatSortModule } from '@angular/material/sort';
import { MatTableModule } from '@angular/material/table';
import { GroupService } from 'app/core/services/group.service';
import { UserService } from 'app/core/services/user.service';
import { Group, GroupPagination } from 'app/models/group.type';
import { forEach } from 'lodash';
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
import { NewGroupDialogComponent } from '../new-group-dialog/new-group-dialog.component';
import { MatInputModule } from '@angular/material/input';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatIconModule } from '@angular/material/icon';
import { MatButtonModule } from '@angular/material/button';
import { MatSelectModule } from '@angular/material/select';
import { GroupesDetailsComponent } from '../groupes-details/groupes-details.component';
import { StreamEvent } from 'app/models/stream-event.type';
import { Sort } from '@angular/material/sort';
import { PageEvent } from '@angular/material/paginator';
import { BehaviorSubject } from 'rxjs';

@Component({
    selector: 'app-groupes-list',
    templateUrl: './groupes-list.component.html',
    styleUrls: ['./groupes-list.component.scss'],
    standalone: true,
    imports: [
        CommonModule,
        MatTableModule,
        MatPaginatorModule,
        MatSortModule,
        ReactiveFormsModule,
        MatFormFieldModule,
        MatInputModule,
        MatIconModule,
        MatButtonModule,
        MatSelectModule,
        GroupesDetailsComponent,
    ],
})
export class GroupesListComponent implements OnInit, AfterViewInit, OnDestroy {
    @ViewChild(MatPaginator) private readonly _paginator: MatPaginator;
    @ViewChild(MatSort) private readonly _sort: MatSort;

    isLoading: boolean = false;
    searchInputControl: UntypedFormControl = new UntypedFormControl();
    selectedFilterType = new FormControl(0);
    groups$: Observable<Group[]>;
    selectedGroupId: number;
    pagination: GroupPagination;
    groupedCompanyNumber: number;

    private readonly _unsubscribeAll: Subject<any> = new Subject<any>();
    private readonly _groups = new BehaviorSubject<Group[]>(null);

    userIsAdmin = false;
    userIsCreateGroup = false;
    userIsUpdateGroup = false;
    userIsDeleteGroup = false;

    constructor(
        private readonly _dialog: MatDialog,
        private readonly _groupService: GroupService,
        private readonly _userService: UserService
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
                        if (role.name === 'create_group') {
                            this.userIsCreateGroup = true;
                        }
                    });
                }
            });
    }

    private setEntitiesObservable(): void {
        this.groups$ = this._groupService.groups$;
    }

    private subscribeToRefreshNeeded(): void {
        this._groupService.refreshNeeded.subscribe(() => {
            this.setEntitiesObservable();
        });
    }

    private fetchPaginationModel(): void {
        this._groupService.pagination$
            .pipe(takeUntil(this._unsubscribeAll))
            .subscribe((pagination: GroupPagination) => {
                this.pagination = pagination;
            });
    }

    private setSortingOrder(): void {
        const defaultSortField = 'Nom';
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
                    return this._groupService.getPaginatedGroupsWithClients(
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
        // Initialize sort with default values
        this._sort.sort({
            id: 'Nom',
            start: 'asc',
            disableClear: true,
        });

        // Create a stream of search and filter changes
        const filterChanges$ = combineLatest([
            this.searchInputControl.valueChanges.pipe(startWith('')),
            this.selectedFilterType.valueChanges.pipe(startWith(0)),
        ]).pipe(debounceTime(300));

        // Combine all triggers that should cause a data refresh
        merge(this._sort.sortChange, this._paginator.page, filterChanges$)
            .pipe(
                takeUntil(this._unsubscribeAll),
                tap(() => {
                    this.isLoading = true;
                }),
                switchMap(() => {
                    return this._groupService.getPaginatedGroupsWithClients(
                        this._paginator.pageIndex,
                        this._paginator.pageSize,
                        this._sort.active || 'Nom',
                        this._sort.direction || 'asc',
                        this.searchInputControl.value || '',
                        this.selectedFilterType.value
                    );
                })
            )
            .subscribe({
                next: (response) => {
                    this.isLoading = false;
                    this._groups.next(response.items);
                    this.pagination = response.paginator;
                },
                error: () => {
                    this.isLoading = false;
                },
            });
    }

    createGroup(): void {
        const dialogRef = this._dialog.open(NewGroupDialogComponent, {
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
        this.selectedGroupId = null;
    }

    ngOnDestroy(): void {
        this._unsubscribeAll.next(null);
        this._unsubscribeAll.complete();
    }

    toggleDetails(prmId: number): void {
        if (this.selectedGroupId === prmId) {
            this.closeDetails();
            return;
        }
        this.selectedGroupId = prmId;
    }
}
