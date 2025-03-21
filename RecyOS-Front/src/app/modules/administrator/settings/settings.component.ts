import { ChangeDetectionStrategy, ChangeDetectorRef, Component, OnDestroy, OnInit, ViewChild, ViewEncapsulation } from "@angular/core";
import { fuseAnimations } from "@fuse/animations";
import { MatDrawer } from "@angular/material/sidenav";
import { Observable, Subject, takeUntil } from "rxjs";
import { ParameterDto } from "./settings.types";
import { SettingsService } from "./settings.service";
import { FuseMediaWatcherService } from "@fuse/services/media-watcher";

@Component({
    selector: 'settings',
    templateUrl: './settings.component.html',
    encapsulation: ViewEncapsulation.None,
    changeDetection: ChangeDetectionStrategy.OnPush,
    animations: fuseAnimations
})
export class SettingsComponent implements OnInit, OnDestroy{
    @ViewChild('drawer') drawer: MatDrawer;
    drawerMode: 'over' | 'side' = 'side';
    drawerOpened: boolean = true;
    panels: any[] = [];
    selectedPanel: string = 'general';
    private readonly _unsubscribeAll: Subject<any> = new Subject<any>();

        /**
     * Constructor
     */
        constructor(
            private readonly _changeDetectorRef: ChangeDetectorRef,
            private readonly _fuseMediaWatcherService: FuseMediaWatcherService
        )
        {
        }

    ngOnInit(): void {
        // Setup available panels
        this.panels = [
            {
                id         : 'general',
                icon       : 'heroicons_outline:cog',
                title      : 'Général',
                description: 'Paramètres généraux de l\'application RecyOs'
            },
            {
                id         : 'engine',
                icon       : 'heroicons_outline:sparkles',
                title      : 'Moteur',
                description: 'Gestion du moteur de l\'application RecyOs'
            }
        ];
        // Subscribe to media changes
        this._fuseMediaWatcherService.onMediaChange$
            .pipe(takeUntil(this._unsubscribeAll))
            .subscribe(({matchingAliases}) => {

                // Set the drawerMode and drawerOpened
                if ( matchingAliases.includes('lg') )
                {
                    this.drawerMode = 'side';
                    this.drawerOpened = true;
                }
                else
                {
                    this.drawerMode = 'over';
                    this.drawerOpened = false;
                }

                // Mark for check
                this._changeDetectorRef.markForCheck();
            });
    }

    ngOnDestroy(): void {
        // Unsubscribe from all subscriptions
        this._unsubscribeAll.next(null);
        this._unsubscribeAll.complete();
    }

    /**
     * Navigate to the panel
     *
     * @param panel
     */
    goToPanel(panel: string): void
    {
        this.selectedPanel = panel;

        // Close the drawer on 'over' mode
        if ( this.drawerMode === 'over' )
        {
            this.drawer.close();
        }
    }

    

    /**
     * Get the details of the panel
     *
     * @param id
     */
    getPanelInfo(id: string): any
    {
        return this.panels.find(panel => panel.id === id);
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