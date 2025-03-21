import { NgModule } from '@angular/core';
import { DashboardCustomerComponent } from './customer.component';
import { MatButtonToggleModule } from '@angular/material/button-toggle';
import { MatButtonModule } from '@angular/material/button';
import { MatMenuModule } from '@angular/material/menu';
import { MatIconModule } from '@angular/material/icon';
import { DashboardCustomerListComponent } from './lists/dashboard-cutomer-list.component';
import { MatProgressBarModule } from '@angular/material/progress-bar';
import {
    AsyncPipe,
    NgClass,
    NgForOf,
    NgIf,
    NgTemplateOutlet,
} from '@angular/common';
import { MatSortModule } from '@angular/material/sort';
import { MatPaginatorModule } from '@angular/material/paginator';
import { SharedModule } from '../../../shared/shared.module';
import { MatTooltipModule } from '@angular/material/tooltip';
import { DashboardCustomerService } from './customer.service';
import {
    DashboardCustomerListResolver,
    DashboardCustomerResolver,
} from './customer.resolver';
import { EntityDetailsComponent } from '../../../shared/components/entity-details/entity-details.component';
import { DashboardCoverClientCardComponent } from '../../../shared/cards/dashboard-cards/dashboard-cover-client-card/dashboard-cover-client-card.component';
import { DashboardBaseClientCardComponent } from '../../../shared/cards/dashboard-cards/dashboard-base-client-card/dashboard-base-client-card.component';

@NgModule({
    declarations: [DashboardCustomerComponent, DashboardCustomerListComponent],
    exports: [DashboardCustomerComponent, DashboardCustomerListComponent],
    providers: [
        DashboardCustomerService,
        DashboardCustomerListResolver,
        DashboardCustomerResolver,
    ],
    imports: [
        MatButtonModule,
        MatButtonToggleModule,
        MatIconModule,
        MatMenuModule,
        MatProgressBarModule,
        MatTooltipModule,
        NgIf,
        AsyncPipe,
        MatSortModule,
        MatPaginatorModule,
        SharedModule,
        NgClass,
        NgForOf,
        NgTemplateOutlet,
        EntityDetailsComponent,
        DashboardCoverClientCardComponent,
        DashboardBaseClientCardComponent,
    ],
})
export class DashboardCustomerModule {}
