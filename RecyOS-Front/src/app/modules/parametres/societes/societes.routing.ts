import { Route } from '@angular/router';
import { SocietesListComponent } from './components/societes-list/societes-list.component';

export const societesRoutes: Route[] = [
    {
        path: '',
        component: SocietesListComponent,
    },
]; 