import { Route } from '@angular/router';
import { SocietesResolver } from '../../core/resolvers/societes.resolver';

export const administratorRoutes: Route[] = [
    {
        path: 'users',
        loadChildren: () =>
            import('./users/users.module').then((m) => m.UsersModule),
    },
    {
        path: 'settings',
        loadChildren: () =>
            import('./settings/settings.module').then((m) => m.SettingsModule),
    },
];
