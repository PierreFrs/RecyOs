import {Injectable} from "@angular/core";
import {ActivatedRouteSnapshot, Resolve, RouterStateSnapshot} from "@angular/router";
import {UsersService} from "./users.service";

@Injectable({
    providedIn: 'root'
})
export class UsersResolver implements Resolve<any> {
    /**
     * Constructor
     * @param route
     * @param state
     */
    constructor(private _usersServices: UsersService) {

    }

    resolve(route: ActivatedRouteSnapshot, state: RouterStateSnapshot) {
        return this._usersServices.getUsers(0, 100);
    }
}

@Injectable({
    providedIn: 'root'
})
export class UserResolver implements Resolve<any> {
    /**
     * Constructor
     * @param route
     * @param state
     */
    constructor(private _usersServices: UsersService) {

    }

    resolve(route: ActivatedRouteSnapshot, state: RouterStateSnapshot) {
        return this._usersServices.getUserById(route.params.id);
    }
}

@Injectable({
    providedIn: 'root'
})
export class UserRolesResolver implements Resolve<any> {
    /**
     * Constructor
     * @param route
     * @param state
     */
    constructor(private _usersServices: UsersService) {

    }

    resolve(route: ActivatedRouteSnapshot, state: RouterStateSnapshot) {
        return this._usersServices.getRoles();
    }
}
