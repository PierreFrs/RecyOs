import { Route } from '@angular/router';
import { CommerciauxListComponent } from './components/commerciaux-list/commerciaux-list.component';
import { SocietesResolver } from 'app/core/resolvers/societes.resolver';

export const commerciauxRoutes: Route[] = [
    {
        path: '',
        children: [
            {
                path: '',
                component: CommerciauxListComponent,
            },
        ],
    },
];
