import { AbstractControl, ValidationErrors, AsyncValidatorFn } from '@angular/forms';
import { Observable, of } from 'rxjs';
import { catchError, map } from 'rxjs/operators';
import {ParticulierService} from "../services/entity-services/particulier.service";

export class ClientValidators {
    static clientExists(particulierService: ParticulierService): AsyncValidatorFn {
        return (control: AbstractControl): Observable<ValidationErrors | null> => {
            const nom = control.parent?.get('nom')?.value;
            const prenom = control.parent?.get('prenom')?.value;
            const ville = control.parent?.get('ville')?.value;

            // If either name is missing, we can't validate, so return null
            if (!nom || !prenom || !ville) {
                return of(null);
            }

            // Perform the check for an existing client
            return particulierService.checkNomExists(prenom, nom, ville).pipe(
                map((exists: boolean) => (exists ? {clientExists: true} : null)),
                catchError(() => of(null))  // Gracefully handle errors
            );
        };
    }
}
