import { Injectable } from '@angular/core';
import { ActivatedRouteSnapshot, CanDeactivate, RouterStateSnapshot, UrlTree } from '@angular/router';
import { Observable } from 'rxjs';
import {DetailsUsersComponent} from "./details/details.users.component";

@Injectable({
    providedIn: 'root'
})
export class CanDeactivateUsersDetails implements CanDeactivate<DetailsUsersComponent>
{
    canDeactivate(
        component: DetailsUsersComponent,
        currentRoute: ActivatedRouteSnapshot,
        currentState: RouterStateSnapshot,
        nextState: RouterStateSnapshot
    ): Observable<boolean | UrlTree> | Promise<boolean | UrlTree> | boolean | UrlTree
    {
        // Get the next route
        let nextRoute: ActivatedRouteSnapshot = nextState.root;
        while ( nextRoute.firstChild )
        {
            nextRoute = nextRoute.firstChild;
        }

        // If the next state doesn't contain '/contacts'
        // it means we are navigating away from the
        // contacts app
        if ( !nextState.url.includes('/users') )
        {
            // Let it navigate
            return true;
        }

        // If we are navigating to another contact...
        if ( nextRoute.paramMap.get('id') )
        {
            // Just navigate
            return true;
        }
        // Otherwise...
        else
        {
            // Close the drawer first, and then navigate
            return component.closeDrawer().then(() => true);
        }
    }
}
