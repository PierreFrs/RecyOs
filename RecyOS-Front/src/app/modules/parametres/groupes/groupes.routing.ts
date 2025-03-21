import { Route } from '@angular/router';
import { GroupesListComponent } from './components/group-list/groupes-list.component';

export const groupesRoutes: Route[] = [
    {
        path: '',
        component: GroupesListComponent,
    },
];
