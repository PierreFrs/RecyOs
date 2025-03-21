import { Injectable } from '@angular/core';
import {
    Resolve,
    RouterStateSnapshot,
    ActivatedRouteSnapshot,
} from '@angular/router';
import { Observable } from 'rxjs';
import { SocieteService } from '../services/societe.service';
import { SocieteDto } from '../../models/societe.type';

@Injectable({
    providedIn: 'root',
})
export class SocietesResolver implements Resolve<SocieteDto[]> {
    constructor(private readonly _societeService: SocieteService) {}

    resolve(
        route: ActivatedRouteSnapshot,
        state: RouterStateSnapshot
    ): Observable<SocieteDto[]> {
        return this._societeService.getSocietes();
    }
}
