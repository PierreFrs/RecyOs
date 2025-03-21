import { DashboardCustomerComponent } from './customer/customer.component';
import { Route } from '@angular/router';
import {
    DashboardCustomerListCategoryResolver,
    DashboardCustomerListResolver,
    DashboardCustomerListTypesResolver,
    DashboardCustomerResolver,
} from './customer/customer.resolver';
import { DashboardCustomerListComponent } from './customer/lists/dashboard-cutomer-list.component';
import { DashboardCustomerCoverageListComponent } from './customer-coverage/lists/dashboard-customer-coverage-list.component';
import { DashboardCustomerCoverageResolver } from './customer-coverage/customer-coverage.resolver';
import { DashboardCustomerNDCoverListComponent } from './customer-nd-cover/lists/dashboard-customer-nd-cover-list.component';
import { DashboardCustomerNDCoverResolver } from './customer-nd-cover/customer-nd-cover.resolver';
import { SocietesResolver } from '../../core/resolvers/societes.resolver';
import { CustomerWithoutSalesAgentListComponent } from './customer-without-sales-agent/customer-without-sales-agent-list/customer-without-sales-agent-list.component';
import { CustomerWithoutSalesAgentResolver } from './customer-without-sales-agent/customer-without-sales-agent.resolver';
import { CommerciauxResolver } from '../../core/resolvers/commerciaux.resolver';
import {EuropeParametrageTabResolver} from "../../core/resolvers/europe-parametrage-tab.resolver";
import {FileTypeResolver} from "../../core/resolvers/file-type.resolver";
import {BusinessUnitResolver} from "../../core/resolvers/businessUnit.resolver";

export const dashboardsRoutes: Route[] = [
    {
        path: 'customer',
        children: [
            {
                path: '',
                component: DashboardCustomerComponent,
                resolve: {
                    data: DashboardCustomerResolver,
                },
            },
            {
                path: 'lists',
                component: DashboardCustomerListComponent,
                resolve: {
                    data: DashboardCustomerListResolver,
                    categories: DashboardCustomerListCategoryResolver,
                    types: DashboardCustomerListTypesResolver,
                    societes: SocietesResolver,
                    businessUnits: BusinessUnitResolver,
                    commerciaux: CommerciauxResolver,
                },
            },
        ],
    },
    {
        path: 'customer-coverage',
        children: [
            {
                path: 'lists',
                component: DashboardCustomerCoverageListComponent,
                resolve: {
                    data: DashboardCustomerCoverageResolver,
                },
            },
        ],
    },
    {
        path: 'customer-nd-cover',
        children: [
            {
                path: 'lists',
                component: DashboardCustomerNDCoverListComponent,
                resolve: {
                    data: DashboardCustomerNDCoverResolver,
                },
            },
        ],
    },
    {
        path: 'customer-without-sales-agent',
        children: [
            {
                path: 'lists',
                component: CustomerWithoutSalesAgentListComponent,
                resolve: {
                    data: CustomerWithoutSalesAgentResolver,
                    types: FileTypeResolver,
                    categories: EuropeParametrageTabResolver,
                    businessUnits: BusinessUnitResolver,
                    societes: SocietesResolver,
                    commerciaux: CommerciauxResolver,
                },
            },
        ],
    },
];
