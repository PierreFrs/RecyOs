import {
    ChangeDetectionStrategy,
    Component,
    OnDestroy,
    OnInit,
    ViewChild,
    ViewEncapsulation,
} from '@angular/core';
import { MatDrawer, MatSidenavModule } from '@angular/material/sidenav';
import { Subject } from 'rxjs';
import { CommonModule } from '@angular/common';
import { MatIconModule } from '@angular/material/icon';
import {ExportFactorComponent} from "./export-factor/export-factor.component";
import {ImportCustomersComponent} from "./import-customers/import-customers.component";
import {MatButtonModule} from "@angular/material/button";
import {RefreshSirenComponent} from "./refresh-siren/refresh-siren.component";
import {ImportInsuranceComponent} from "./import-insurance/import-insurance.component";

@Component({
    selector: 'app-customer-operations',
    templateUrl: './customer-operations.component.html',
    standalone: true,
    imports: [
        CommonModule,
        MatSidenavModule,
        MatIconModule,
        ExportFactorComponent,
        ImportCustomersComponent,
        MatButtonModule,
        RefreshSirenComponent,
        ImportInsuranceComponent,

    ],
    styles: [
        `
            app-customer-operations fuse-card {
                margin: 16px;
            }
        `,
    ],
    encapsulation: ViewEncapsulation.None,
    changeDetection: ChangeDetectionStrategy.OnPush,
})
export class CustomerOperationsComponent implements OnInit, OnDestroy {
    @ViewChild('drawer') drawer: MatDrawer;
    drawerMode: 'over' | 'side' = 'side';
    drawerOpened: boolean = true;
    panels: any[] = [];
    selectedPanel: string = 'import';
    private readonly _unsubscribeAll: Subject<any> = new Subject<any>();
    constructor() {}

    ngOnInit(): void {
        // Setup available panels
        this.panels = [
            {
                id: 'import',
                icon: 'heroicons_outline:cloud-upload',
                title: 'Import direct',
                description:
                    'Permet de récupérer les données directement depuis un autre logiciel',
            },
            {
                id: 'sycnhronisation',
                icon: 'heroicons_outline:refresh',
                title: 'Actualiser données SIREN',
                description:
                    "Permet de mettre à jour les données SIREN depuis la base l'INSEE",
            },
            {
                id: 'couverture',
                icon: 'feather:umbrella',
                title: 'Assurances clients',
                description:
                    'Permet de mettre à jour les informations concernant le couverture clients',
            },
            {
                id: 'export-factor',
                icon: 'heroicons_outline:cloud-download',
                title: 'Export Affacturage',
                description:
                    "Permet d'exporter les données vers un autre logiciel",
            },
        ];
    }

    /**
     * On destroy
     */
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
    goToPanel(panel: string): void {
        this.selectedPanel = panel;

        // Close the drawer on 'over' mode
        if (this.drawerMode === 'over') {
            this.drawer.close();
        }
    }

    /**
     * Get the details of the panel
     *
     * @param id
     */
    getPanelInfo(id: string): any {
        return this.panels.find((panel) => panel.id === id);
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
