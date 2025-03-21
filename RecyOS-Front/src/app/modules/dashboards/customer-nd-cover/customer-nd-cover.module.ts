import { NgModule } from '@angular/core';
import { DashboardCustomerNDCoverListComponent } from './lists/dashboard-customer-nd-cover-list.component';
import { MatProgressBarModule } from '@angular/material/progress-bar';
import {
    AsyncPipe,
    DatePipe,
    NgClass,
    NgForOf,
    NgIf,
    NgTemplateOutlet,
} from '@angular/common';
import { MatPaginatorModule } from '@angular/material/paginator';
import { MatSortModule } from '@angular/material/sort';
import { MatIconModule } from '@angular/material/icon';
import { MatButtonModule } from '@angular/material/button';
import { DashboardCustomerNDCoverResolver } from './customer-nd-cover.resolver';
import { BusinessNDCoverModule } from '../../../shared/components/entity-details/business-nd-cover/business-nd-cover.module';
import { NdCoverStatusComponent } from '../../../widgets/nd-cover-status/nd-cover-status.component';

@NgModule({
    declarations: [DashboardCustomerNDCoverListComponent],
    exports: [DashboardCustomerNDCoverListComponent],
    providers: [DashboardCustomerNDCoverResolver],
    imports: [
        BusinessNDCoverModule,
        MatProgressBarModule,
        NgIf,
        MatPaginatorModule,
        MatSortModule,
        MatIconModule,
        MatButtonModule,
        AsyncPipe,
        DatePipe,
        NgClass,
        NgForOf,
        NdCoverStatusComponent,
        NgTemplateOutlet,
    ],
})
export class DashboardCustomerNDCoverModule {}
