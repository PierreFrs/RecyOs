import {
    ChangeDetectionStrategy,
    ChangeDetectorRef,
    Component,
    ElementRef,
    OnDestroy,
    OnInit,
    Renderer2,
    TemplateRef,
    ViewChild,
    ViewContainerRef,
    ViewEncapsulation,
} from '@angular/core';
import { UsersListComponent } from '../list/users.list.component';
import { Subject, takeUntil, combineLatest, BehaviorSubject } from 'rxjs';
import { Overlay, OverlayRef } from '@angular/cdk/overlay';
import { TemplatePortal } from '@angular/cdk/portal';
import { MatDrawerToggleResult } from '@angular/material/sidenav';
import { RoleDto, UserDto } from '../users.type';
import { UsersService } from '../users.service';
import { UntypedFormBuilder, UntypedFormGroup } from '@angular/forms';
import { ActivatedRoute } from '@angular/router';
import { SocieteDto } from '../../../../models/societe.type';
import { SocieteService } from '../../../../core/services/societe.service';

@Component({
    selector: 'details-users',
    templateUrl: './details.users.component.html',
    encapsulation: ViewEncapsulation.None,
    changeDetection: ChangeDetectionStrategy.OnPush,
})
export class DetailsUsersComponent implements OnInit, OnDestroy {
    @ViewChild('tagsPanel') private readonly _tagsPanel: TemplateRef<any>;
    @ViewChild('tagsPanelOrigin') private readonly _tagsPanelOrigin: ElementRef;

    private _tagsPanelOverlayRef: OverlayRef;
    private readonly _unsubscribeAll: Subject<any> = new Subject<any>();
    protected user: UserDto;
    private users: UserDto[];
    editMode: boolean = false;
    userForm: UntypedFormGroup;
    rolesEditMode: boolean = false;
    filteredRoles: RoleDto[];
    roles: RoleDto[];
    societes: SocieteDto[];
    private userSociete$: BehaviorSubject<SocieteDto | undefined> =
        new BehaviorSubject<SocieteDto | undefined>(undefined);

    /**
     * Constructor
     */
    constructor(
        private readonly _usersListComponent: UsersListComponent,
        private readonly _formBuilder: UntypedFormBuilder,
        private readonly _usersService: UsersService,
        private readonly _overlay: Overlay,
        private readonly _renderer2: Renderer2,
        private readonly _changeDetectorRef: ChangeDetectorRef,
        private readonly _viewContainerRef: ViewContainerRef,
        private readonly _route: ActivatedRoute,
        private readonly _societeService: SocieteService
    ) {}

    ngOnInit(): void {
        this.userForm = this._formBuilder.group({
            id: [''],
            userName: [''],
            firstName: [''],
            lastName: [''],
            email: [''],
            roles: [[]],
            societeId: [''],
        });

        // Combine both data streams
        combineLatest([this._usersService.user$, this._route.data])
            .pipe(takeUntil(this._unsubscribeAll))
            .subscribe(([user, data]) => {
                this.user = user;
                this.societes = data['societes'];

                if (user) {
                    this.userForm.patchValue(user);
                    if (user.societeId && this.societes) {
                        const societe = this.societes.find(
                            (s) => s.id === user.societeId
                        );
                        this.userSociete$.next(societe);
                    } else {
                        this.userSociete$.next(undefined);
                    }
                }

                // Force change detection
                this._changeDetectorRef.markForCheck();
            });

        this._usersListComponent.matDrawer.open();

        // Get the contacts
        this._usersService.users$
            .pipe(takeUntil(this._unsubscribeAll))
            .subscribe((contacts: UserDto[]) => {
                this.users = contacts;

                // Mark for check
                this._changeDetectorRef.markForCheck();
            });

        // Get the tags
        this._usersService.roles$
            .pipe(takeUntil(this._unsubscribeAll))
            .subscribe((roles: RoleDto[]) => {
                this.roles = roles;
                this.filteredRoles = roles;

                // Mark for check
                this._changeDetectorRef.markForCheck();
            });
    }

    ngOnDestroy(): void {
        // Unsubscribe from all subscriptions
        this._unsubscribeAll.next(null);
        this._unsubscribeAll.complete();

        // Dispose the overlays if they are still on the DOM
        if (this._tagsPanelOverlayRef) {
            this._tagsPanelOverlayRef.dispose();
        }
    }

    /**
     * Close the drawer
     */
    closeDrawer(): Promise<MatDrawerToggleResult> {
        return this._usersListComponent.matDrawer.close();
    }

    /**
     * Toggle edit mode
     *
     * @param editMode
     */
    toggleEditMode(editMode: boolean | null = null): void {
        if (editMode === null) {
            this.editMode = !this.editMode;
        } else {
            this.editMode = editMode;
        }

        // Mark for check
        this._changeDetectorRef.markForCheck();
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

    openTagsPanel(): void {
        // Create the overlay
        this._tagsPanelOverlayRef = this._overlay.create({
            backdropClass: '',
            hasBackdrop: true,
            scrollStrategy: this._overlay.scrollStrategies.block(),
            positionStrategy: this._overlay
                .position()
                .flexibleConnectedTo(this._tagsPanelOrigin.nativeElement)
                .withFlexibleDimensions(true)
                .withViewportMargin(64)
                .withLockedPosition(true)
                .withPositions([
                    {
                        originX: 'start',
                        originY: 'bottom',
                        overlayX: 'start',
                        overlayY: 'top',
                    },
                ]),
        });

        // Subscribe to the attachments observable
        this._tagsPanelOverlayRef.attachments().subscribe(() => {
            // Add a class to the origin
            this._renderer2.addClass(
                this._tagsPanelOrigin.nativeElement,
                'panel-opened'
            );

            // Focus to the search input once the overlay has been attached
            this._tagsPanelOverlayRef.overlayElement
                .querySelector('input')
                .focus();
        });

        // Create a portal from the template
        const templatePortal = new TemplatePortal(
            this._tagsPanel,
            this._viewContainerRef
        );

        // Attach the portal to the overlay
        this._tagsPanelOverlayRef.attach(templatePortal);

        // Subscribe to the backdrop click
        this._tagsPanelOverlayRef.backdropClick().subscribe(() => {
            // Remove the class from the origin
            this._renderer2.removeClass(
                this._tagsPanelOrigin.nativeElement,
                'panel-opened'
            );

            // If overlay exists and attached...
            if (
                this._tagsPanelOverlayRef &&
                this._tagsPanelOverlayRef.hasAttached()
            ) {
                // Detach it
                this._tagsPanelOverlayRef.detach();

                // Reset the tag filter
                this.filteredRoles = this.roles;

                // Toggle the edit mode off
                this.rolesEditMode = false;
            }

            // If template portal exists and attached...
            if (templatePortal && templatePortal.isAttached) {
                // Detach it
                templatePortal.detach();
            }
        });
    }

    /**
     * Toggle contact tag
     *
     * @param tag
     */
    toggleContactRole(role: RoleDto): void {
        if (this.user.roles?.map((r) => r.id).includes(role.id)) {
            this.removeTagFromContact(role);
            this._usersService.deleteUserRole(this.user.id, role).subscribe();
        } else {
            this.addTagToContact(role);
            this._usersService.addUserRole(this.user.id, role).subscribe();
        }
    }

    /**
     * Should the create tag button be visible
     *
     * @param inputValue
     */
    shouldShowCreateTagButton(inputValue: string): boolean {
        return !!!(
            inputValue === '' ||
            this.roles.findIndex(
                (tag) => tag.name.toLowerCase() === inputValue.toLowerCase()
            ) > -1
        );
    }

    /**
     * Filter tags
     *
     * @param event
     */
    filterRole(event): void {
        // Get the value
        const value = event.target.value.toLowerCase();

        // Filter the tags
        this.filteredRoles = this.roles.filter((tag) =>
            tag.name.toLowerCase().includes(value)
        );
    }
    /**
     * Remove tag from the contact
     *
     * @param tag
     */
    removeTagFromContact(tag: RoleDto): void {
        console.log('removeTagFromContact : ' + tag);
        // Remove the tag
        this.user.roles.splice(
            this.user.roles.findIndex((item) => item.id == tag.id),
            1
        );

        // Update the contact form
        this.userForm.get('roles').patchValue(this.user.roles);

        // Mark for check
        this._changeDetectorRef.markForCheck();
    }

    /**
     * Add tag to the contact
     *
     * @param tag
     */
    addTagToContact(tag: RoleDto): void {
        // Add the tag
        this.user.roles?.unshift(tag);

        // Update the contact form
        this.userForm.get('roles').patchValue(this.user.roles);

        // Mark for check
        this._changeDetectorRef.markForCheck();
    }

    /**
     * Toggle the tags edit mode
     */
    toggleRolesEditMode(): void {
        this.rolesEditMode = !this.rolesEditMode;
    }

    /**
     * Filter tags input key down event
     *
     * @param event
     */
    filterRolesInputKeyDown(event): void {
        // Return if the pressed key is not 'Enter'
        if (event.key !== 'Enter') {
            return;
        }

        // If there is no tag available...
        if (this.filteredRoles.length === 0) {
            // Create the role
            this.createRole(event.target.value);

            // Clear the input
            event.target.value = '';

            // Return
            return;
        }

        // If there is a tag...
        const role = this.filteredRoles[0];
        const isTagApplied = this.user.roles.find((id) => id === role);

        // If the found tag is already applied to the contact...
        if (isTagApplied) {
            // Remove the tag from the contact
            //this.removeRoleFromContact(role);
        } else {
            // Otherwise add the tag to the contact
            //this.addToleToContact(role);
        }
    }

    /**
     * Create a new tag
     *
     * @param title
     */
    createRole(title: string): void {
        const tag = {
            title,
        };
        /*
        // Create tag on the server
        this._contactsService.createTag(tag)
            .subscribe((response) => {

                // Add the tag to the contact
                this.addTagToContact(response);
            });*/
    }

    /**
     * Role is checked?
     */
    roleIsChecked(role: RoleDto): boolean {
        return this.user.roles?.map((r) => r.id).includes(role.id);
    }

    /**
     * update user
     */
    updateUser(): void {
        const data = this.userForm.getRawValue();
        data.id = this.user.id;
        this._usersService.updateUser(data).subscribe((response) => {
            // Toggle the edit mode off
            this.toggleEditMode(false);
        });
    }

    /**
     * Delete the user
     */
    deleteUser(): void {
        // Afficher une confirmation avant la suppression
        if (confirm('Êtes-vous sûr de vouloir supprimer cet utilisateur ?')) {
            this._usersService.deleteUser(this.user.id).subscribe(() => {
                // Fermer le drawer et retourner à la liste
                this.closeDrawer();
            });
        }
    }

    get userSociete(): SocieteDto | undefined {
        return this.userSociete$.value;
    }
}
