import { Injectable } from '@angular/core';
import {
    Resolve,
    RouterStateSnapshot,
    ActivatedRouteSnapshot,
} from '@angular/router';
import { Observable } from 'rxjs';
import { CommerciauxService } from '../services/commerciaux.service';
import { Commercial } from '../../models/commercial.type';

@Injectable({
    providedIn: 'root',
})
export class CommerciauxResolver implements Resolve<any> {
    constructor(private _commerciauxService: CommerciauxService) {}
    resolve(
        route: ActivatedRouteSnapshot,
        state: RouterStateSnapshot,
    ): Observable<Commercial[]> {
        return this._commerciauxService.fetchCommerciaux();
    }
}
