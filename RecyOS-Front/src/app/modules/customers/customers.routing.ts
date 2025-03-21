import { FranceParametrageTabResolver } from '../../core/resolvers/france-parametrage-tab.resolver';
import { Route } from '@angular/router';
import { BusinessesListComponent } from './businesses/businesses-list/businesses.list.component';
import { CustomerOperationsComponent } from './operations/customer-operations.component';
import { EuropeListComponent } from './europe/europe-list/europe-list.component';
import { EuropeParametrageTabResolver } from '../../core/resolvers/europe-parametrage-tab.resolver';
import { FileTypeResolver } from '../../core/resolvers/file-type.resolver';
import { BusinessUnitResolver } from '../../core/resolvers/businessUnit.resolver';
import { SocietesResolver } from '../../core/resolvers/societes.resolver';
import { CommerciauxResolver } from '../../core/resolvers/commerciaux.resolver';
import { ParticuliersListComponent } from './particuliers/particuliers-list/particuliers-list.component';
import { GroupResolver } from '../../core/resolvers/group.resolver';

export const customersRoutes: Route[] = [
    {
        path: 'businesses',
        children: [
            {
                path: '',
                component: BusinessesListComponent,
                resolve: {
                    types: FileTypeResolver,
                    categories: FranceParametrageTabResolver,
                    businessUnits: BusinessUnitResolver,
                    societes: SocietesResolver,
                    commerciaux: CommerciauxResolver,
                    groups: GroupResolver,
                },
            },
        ],
    },
    {
        path: 'europe',
        children: [
            {
                path: '',
                component: EuropeListComponent,
                resolve: {
                    types: FileTypeResolver,
                    categories: EuropeParametrageTabResolver,
                    businessUnits: BusinessUnitResolver,
                    societes: SocietesResolver,
                    commerciaux: CommerciauxResolver,
                },
            },
        ],
    },
    {
        path: 'particuliers',
        children: [
            {
                path: '',
                component: ParticuliersListComponent,
                resolve: {
                    commerciaux: CommerciauxResolver,
                },
            },
        ],
    },
    {
        path: 'operations',
        children: [
            {
                path: '',
                component: CustomerOperationsComponent,
            },
        ],
    },
];
