import { Route } from '@angular/router';
import { UsersListComponent } from './list/users.list.component';
import {
    UserResolver,
    UserRolesResolver,
    UsersResolver,
} from './users.resolver';
import { UsersComponent } from './users.component';
import { DetailsUsersComponent } from './details/details.users.component';
import { CanDeactivateUsersDetails } from './users.guards';
import { SocietesResolver } from '../../../core/resolvers/societes.resolver';

export const usersRoutes: Route[] = [
    {
        path: '',
        component: UsersComponent,
        children: [
            {
                path: '',
                component: UsersListComponent,
                resolve: {
                    users: UsersResolver,
                },
                children: [
                    {
                        path: ':id',
                        component: DetailsUsersComponent,
                        resolve: {
                            user: UserResolver,
                            roles: UserRolesResolver,
                            societes: SocietesResolver,
                        },
                        canDeactivate: [CanDeactivateUsersDetails],
                    },
                ],
            },
        ],
    },
];
