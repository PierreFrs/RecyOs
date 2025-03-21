import { Route } from '@angular/router';
import { FrenchSuppliersListComponent } from './french-suppliers/french-suppliers-list/french-suppliers-list.component';
import { EuropeSuppliersListComponent } from './europe-suppliers/europe-suppliers-list/europe-suppliers-list.component';
import { FileTypeResolver } from '../../core/resolvers/file-type.resolver';
import { BusinessUnitResolver } from '../../core/resolvers/businessUnit.resolver';
import { SocietesResolver } from '../../core/resolvers/societes.resolver';

export const supplierRoutes: Route[] = [
    {
        path: 'businesses',
        children: [
            {
                path: '',
                component: FrenchSuppliersListComponent,
                resolve: {
                    types: FileTypeResolver,
                    businessUnits: BusinessUnitResolver,
                    societes: SocietesResolver,
                },
            },
        ],
    },
    {
        path: 'europe',
        children: [
            {
                path: '',
                component: EuropeSuppliersListComponent,
                resolve: {
                    types: FileTypeResolver,
                    businessUnits: BusinessUnitResolver,
                    societes: SocietesResolver,
                },
            },
        ],
    },
];
