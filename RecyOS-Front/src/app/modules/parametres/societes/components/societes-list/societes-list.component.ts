import { AfterViewInit, ChangeDetectionStrategy, ChangeDetectorRef, Component, OnDestroy, OnInit, ViewChild } from '@angular/core';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { SocieteDto, SocietePaginator } from 'app/models/societe.type';
import { Observable, Subject, merge, takeUntil, map, switchMap, tap, BehaviorSubject } from 'rxjs';
import { MatDialog } from '@angular/material/dialog';
import { UserService } from 'app/core/services/user.service';
import { SocieteService, SocieteGridParams } from 'app/core/services/societe.service';
import { UntypedFormControl } from '@angular/forms';
import { NewSocieteDialogComponent } from '../new-societe-dialog/new-societe-dialog.component';

@Component({
    selector: 'app-societes-list',
    templateUrl: './societes-list.component.html',
    styleUrls: ['./societes-list.component.scss'],
    changeDetection: ChangeDetectionStrategy.OnPush
})
export class SocietesListComponent implements OnInit, AfterViewInit, OnDestroy {
    @ViewChild(MatPaginator) private _paginator: MatPaginator;
    @ViewChild(MatSort) private _sort: MatSort;

    private _societesBehaviorSubject = new BehaviorSubject<SocieteDto[]>([]);
    societes$: Observable<SocieteDto[]> = this._societesBehaviorSubject.asObservable();
    
    pagination: SocietePaginator = {
        length: 0,
        size: 10,
        page: 0,
        lastPage: 0,
        startIndex: 0,
        endIndex: 0,
        cost: 0
    };
    
    isLoading: boolean = false;
    searchInputControl: UntypedFormControl = new UntypedFormControl();
    selectedSocieteId: number | null = null;
    userIsAdmin = false;
    userCanCreateSociete = false;

    private _unsubscribeAll: Subject<any> = new Subject<any>();

    constructor(
        private _dialog: MatDialog,
        private _userService: UserService,
        private _societeService: SocieteService,
        private _changeDetectorRef: ChangeDetectorRef
    ) {}

    ngOnInit(): void {
        this.determineUserPrivileges();
        this.loadSocietes();
        this.initializeSearchStream();
    }

    private loadSocietes(params: SocieteGridParams = {
        PageNumber: 0,
        PageSize: 10,
        SortBy: 'nom',
        OrderBy: 'asc'
    }): void {
        this.isLoading = true;
        this._societeService.getSocietesGrid(params).subscribe({
            next: (response) => {
                this._societesBehaviorSubject.next(response.items);
                
                // Mettre à jour la pagination avec les données du backend
                if (response.paginator) {
                    this.pagination = response.paginator;
                } else {
                    // Fallback si le backend ne renvoie pas de paginator
                    this.pagination = {
                        length: response.totalCount || 0,
                        size: params.PageSize,
                        page: params.PageNumber,
                        lastPage: Math.ceil((response.totalCount || 0) / params.PageSize) - 1,
                        startIndex: params.PageNumber * params.PageSize,
                        endIndex: Math.min((params.PageNumber * params.PageSize) + params.PageSize - 1, (response.totalCount || 0) - 1),
                        cost: 0
                    };
                }
                
                this.isLoading = false;
                this._changeDetectorRef.markForCheck();
            },
            error: (error) => {
                console.error('Erreur lors du chargement des sociétés', error);
                this.isLoading = false;
                this._changeDetectorRef.markForCheck();
            }
        });
    }

    private determineUserPrivileges(): void {
        this._userService.user$
            .pipe(takeUntil(this._unsubscribeAll))
            .subscribe(user => {
                if (user?.roles) {
                    this.userIsAdmin = user.roles.some(role => role.name === 'admin');
                    this.userCanCreateSociete = user.roles.some(role => role.name === 'create_societe');
                }
                this._changeDetectorRef.markForCheck();
            });
    }

    private initializeSearchStream(): void {
        this.searchInputControl.valueChanges
            .pipe(
                takeUntil(this._unsubscribeAll),
                tap(() => {
                    this.closeDetails();
                    this.isLoading = true;
                    
                    // Réinitialiser la pagination lors d'une recherche
                    if (this._paginator) {
                        this._paginator.pageIndex = 0;
                    }
                }),
                switchMap(query => {
                    const params: SocieteGridParams = {
                        PageNumber: 0,
                        PageSize: this.pagination.size,
                        FilterByNom: query,
                        SortBy: this._sort?.active || 'nom',
                        OrderBy: this._sort?.direction || 'asc'
                    };
                    return this._societeService.getSocietesGrid(params);
                })
            )
            .subscribe({
                next: (response) => {
                    this._societesBehaviorSubject.next(response.items);
                    
                    // Mettre à jour la pagination avec les données du backend
                    if (response.paginator) {
                        this.pagination = response.paginator;
                    } else {
                        // Fallback si le backend ne renvoie pas de paginator
                        this.pagination = {
                            ...this.pagination,
                            length: response.totalCount || 0,
                            page: 0,
                            lastPage: Math.ceil((response.totalCount || 0) / this.pagination.size) - 1,
                            startIndex: 0,
                            endIndex: Math.min(this.pagination.size - 1, (response.totalCount || 0) - 1),
                            cost: 0
                        };
                    }
                    
                    this.isLoading = false;
                    this._changeDetectorRef.markForCheck();
                },
                error: (error) => {
                    console.error('Erreur lors de la recherche des sociétés', error);
                    this.isLoading = false;
                    this._changeDetectorRef.markForCheck();
                }
            });
    }

    ngAfterViewInit(): void {
        if (this._sort && this._paginator) {
            // S'abonner aux changements de tri et de pagination
            merge(this._sort.sortChange, this._paginator.page)
                .pipe(
                    takeUntil(this._unsubscribeAll),
                    tap(() => {
                        this.closeDetails();
                        this.isLoading = true;
                    }),
                    switchMap(() => {
                        return this._societeService.getSocietesGrid({
                            PageNumber: this._paginator.pageIndex,
                            PageSize: this._paginator.pageSize,
                            SortBy: this._sort.active,
                            OrderBy: this._sort.direction,
                            FilterByNom: this.searchInputControl.value || undefined
                        });
                    })
                )
                .subscribe({
                    next: (response) => {
                        this._societesBehaviorSubject.next(response.items);
                        
                        // Mettre à jour la pagination avec les données du backend
                        if (response.paginator) {
                            this.pagination = response.paginator;
                        } else {
                            // Fallback si le backend ne renvoie pas de paginator
                            this.pagination = {
                                ...this.pagination,
                                length: response.totalCount || 0,
                                size: this._paginator.pageSize,
                                page: this._paginator.pageIndex,
                                lastPage: Math.ceil((response.totalCount || 0) / this._paginator.pageSize) - 1,
                                startIndex: this._paginator.pageIndex * this._paginator.pageSize,
                                endIndex: Math.min((this._paginator.pageIndex * this._paginator.pageSize) + this._paginator.pageSize - 1, (response.totalCount || 0) - 1),
                                cost: 0
                            };
                        }
                        
                        this.isLoading = false;
                        this._changeDetectorRef.markForCheck();
                    },
                    error: (error) => {
                        console.error('Erreur lors du changement de page ou de tri', error);
                        this.isLoading = false;
                        this._changeDetectorRef.markForCheck();
                    }
                });
        }
    }

    createSociete(): void {
        const dialogRef = this._dialog.open(NewSocieteDialogComponent, {
            width: '600px'
        });

        dialogRef.afterClosed().subscribe(result => {
            if (result) {
                // Rafraîchir la liste
                this.loadSocietes({
                    PageNumber: this._paginator?.pageIndex || 0,
                    PageSize: this._paginator?.pageSize || 10,
                    SortBy: this._sort?.active || 'nom',
                    OrderBy: this._sort?.direction || 'asc',
                    FilterByNom: this.searchInputControl.value || undefined
                });
            }
        });
    }

    toggleDetails(societeId: number): void {
        this.selectedSocieteId = this.selectedSocieteId === societeId ? null : societeId;
        this._changeDetectorRef.markForCheck();
    }

    closeDetails(): void {
        this.selectedSocieteId = null;
        this._changeDetectorRef.markForCheck();
    }

    trackByFn(index: number, item: any): any {
        return item.id || index;
    }

    ngOnDestroy(): void {
        this._unsubscribeAll.next(null);
        this._unsubscribeAll.complete();
    }
} 