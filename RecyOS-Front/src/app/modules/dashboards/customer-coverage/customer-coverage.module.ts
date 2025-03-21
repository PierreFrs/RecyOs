import { NgModule } from '@angular/core';
import { MatProgressBarModule } from '@angular/material/progress-bar';
import { DashboardCustomerCoverageResolver } from './customer-coverage.resolver';
import { MatPaginatorModule } from '@angular/material/paginator';
import { MatSortModule } from '@angular/material/sort';
import { SharedModule } from '../../../shared/shared.module';
import { MatIconModule } from '@angular/material/icon';
import { MatButtonModule } from '@angular/material/button';
import {
    AsyncPipe,
    CurrencyPipe,
    DatePipe,
    NgClass,
    NgForOf,
    NgIf,
    NgTemplateOutlet,
} from '@angular/common';
import { ClientCouvertureDetailComponent } from '../../../shared/components/entity-details/business-couverture/business-couverture-detail/client-couverture-detail.component';
import { TrimZeroPipe } from '../../../shared/pipes/TrimZero.pipe';
import { ColorPipe } from '../../../shared/pipes/color.pipe';
import { DashboardCustomerCoverageListComponent } from './lists/dashboard-customer-coverage-list.component';

@NgModule({
    declarations: [DashboardCustomerCoverageListComponent],
    exports: [DashboardCustomerCoverageListComponent],
    providers: [DashboardCustomerCoverageResolver],
    imports: [
        MatProgressBarModule,
        MatPaginatorModule,
        MatSortModule,
        MatIconModule,
        MatButtonModule,
        NgClass,
        DatePipe,
        CurrencyPipe,
        SharedModule,
        AsyncPipe,
        ClientCouvertureDetailComponent,
        TrimZeroPipe,
        ColorPipe,
        NgIf,
        NgForOf,
        NgTemplateOutlet,
    ],
})
export class DashboardCustomerCoverageModule {}
