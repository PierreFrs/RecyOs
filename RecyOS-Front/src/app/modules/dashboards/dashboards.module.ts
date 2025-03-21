import { NgModule } from '@angular/core';
import { RouterModule } from '@angular/router';
import { dashboardsRoutes } from './dashboards.routing';
import { DashboardCustomerNDCoverModule } from './customer-nd-cover/customer-nd-cover.module';
import { CommonModule } from '@angular/common';
import { DashboardCustomerModule } from './customer/customer.module';
import { DashboardCustomerCoverageModule } from './customer-coverage/customer-coverage.module';

@NgModule({
    imports: [
        CommonModule,
        RouterModule.forChild(dashboardsRoutes),
        DashboardCustomerCoverageModule,
        DashboardCustomerNDCoverModule,
    ],
    declarations: [],
})
export class DashboardsModule {}
