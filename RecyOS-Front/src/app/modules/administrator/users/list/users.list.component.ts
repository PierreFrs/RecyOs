import {
    ChangeDetectionStrategy,
    ChangeDetectorRef,
    Component,
    OnDestroy,
    OnInit,
    ViewChild,
    ViewEncapsulation
} from "@angular/core";
import {fuseAnimations} from "../../../../../@fuse/animations";
import {MatDrawer} from "@angular/material/sidenav";
import {debounceTime, map, Observable, Subject, switchMap, takeUntil} from "rxjs";
import {UntypedFormControl} from "@angular/forms";
import {UserDto} from "../users.type";
import {UsersService} from "../users.service";
import {FuseMediaWatcherService} from "../../../../../@fuse/services/media-watcher";
import {ActivatedRoute, Router} from "@angular/router";
import {MatDialog} from "@angular/material/dialog";
import {NewUserDialogComponent} from "../new-user-dialog/new-user-dialog.component";

@Component({
    selector: 'users-list',
    templateUrl: './users.list.component.html',
    encapsulation  : ViewEncapsulation.None,
    changeDetection: ChangeDetectionStrategy.OnPush,
    animations     : fuseAnimations
})
export class UsersListComponent implements OnInit, OnDestroy {
    drawerMode: 'side' | 'over';
    @ViewChild('matDrawer', {static: true}) matDrawer: MatDrawer;
    users: UserDto[];
    users$: Observable<UserDto[]>;
    private _unsubscribeAll: Subject<any> = new Subject<any>();
    contactsCount: number = 0;
    searchInputControl: UntypedFormControl = new UntypedFormControl();
    selectedUser: UserDto;

    /**
     * Constructor
     */
    constructor(private _usersService: UsersService,
                private _activatedRoute: ActivatedRoute,
                private _router: Router,
                private _changeDetectorRef: ChangeDetectorRef,
                private _fuseMediaWatcherService: FuseMediaWatcherService,
                private _dialog: MatDialog){
    }

    ngOnInit(): void
    {
        this.users$ = this._usersService.users$;
        // Get the users
        this._usersService.users$
            .pipe(takeUntil(this._unsubscribeAll))
            .subscribe((users: UserDto[]) => {
                this.users = users;
                this.contactsCount = users.length;
                this._changeDetectorRef.markForCheck();
            });

        // Get the contact
        this._usersService.user$
            .pipe(takeUntil(this._unsubscribeAll))
            .subscribe((contact: UserDto) => {

                // Update the selected contact
                this.selectedUser = contact;

                // Mark for check
                this._changeDetectorRef.markForCheck();
            });



        // Subscribe to MatDrawer opened change
        this.matDrawer.openedChange.subscribe((opened) => {
            if ( !opened )
            {
                // Remove the selected contact when drawer closed
                this.selectedUser = null;

                // Mark for check
                this._changeDetectorRef.markForCheck();
            }
        });

        // Subscribe to media changes
        this._fuseMediaWatcherService.onMediaChange$
            .pipe(takeUntil(this._unsubscribeAll))
            .subscribe(({matchingAliases}) => {

                // Set the drawerMode if the given breakpoint is active
                if ( matchingAliases.includes('lg') )
                {
                    this.drawerMode = 'side';
                }
                else
                {
                    this.drawerMode = 'over';
                }

                // Mark for check
                this._changeDetectorRef.markForCheck();
            });

        // Subscribe to search input field value changes
        this.searchInputControl.valueChanges.pipe(
            takeUntil(this._unsubscribeAll),
            debounceTime(300),
            switchMap((searchText: string) => {
                    // Search
                    return this._usersService.getUsers(0, 100, 'Username', 'asc', searchText);

            }),
            map((response) => {

            })
        )
    .subscribe();
    }

    ngOnDestroy(): void
    {
        // Unsubscribe from all subscriptions
        this._unsubscribeAll.next(null);
        this._unsubscribeAll.complete();
    }

    onBackdropClicked(): void
    {
        // Go back to the list
        this._router.navigate(['./'], {relativeTo: this._activatedRoute});

        // Mark for check
        this._changeDetectorRef.markForCheck();
    }

    /**
     * methode createContact()
     */
    createContact(): void
    {
        const dialogRef = this._dialog.open(NewUserDialogComponent, {
            width: '700px',
        });
        dialogRef.afterClosed().subscribe((result) => {
            console.log('The dialog was closed');
        });
    }

    /**
     * Track by function for ngFor loops
     *
     * @param index
     * @param item
     */
    trackByFn(index: number, item: any): any
    {
        return item.id || index;
    }



}
