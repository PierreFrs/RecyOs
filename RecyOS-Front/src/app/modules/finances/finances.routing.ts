import { Route } from "@angular/router";

export const financesRoutes: Route[] = [
    {
        path: 'balance-clients-france',
        loadChildren: () => import('./balances-france/balances-france.module').then(m => m.BalancesFranceModule)
    },
    {
        path: 'balance-clients-europe',
        loadChildren: () => import('./balances-europe/balances-europe.module').then(m => m.BalancesEuropeModule)
    },
    {
        path: 'balance-clients-particuliers',
        loadChildren: () => import('./balances-particuliers/balances-particuliers.module').then(m => m.BalancesParticuliersModule)
    }
];