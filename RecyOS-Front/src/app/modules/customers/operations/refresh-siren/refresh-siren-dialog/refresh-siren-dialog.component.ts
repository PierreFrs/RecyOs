import {Component, OnDestroy, OnInit} from "@angular/core";
import {MatDialogModule, MatDialogRef} from "@angular/material/dialog";
import {ReactiveFormsModule, UntypedFormControl, Validators} from "@angular/forms";
import {OperationsServices} from "../../operations.services";
import {catchError, debounceTime, map, of, Subject, switchMap, takeUntil, tap} from "rxjs";
import {BusinessesServices} from "../../../../../core/services/entity-services/businesses.services";
import {MatInputModule} from "@angular/material/input";
import {MatButtonModule} from "@angular/material/button";
import {NgIf} from "@angular/common";

@Component({
    selector: 'refresh-siren-dialog',
    templateUrl: './refresh-siren-dialog.component.html',
    standalone: true,
    imports: [
        MatDialogModule,
        MatInputModule,
        ReactiveFormsModule,
        MatButtonModule,
        NgIf
    ]
})
export class RefreshSirenDialogComponent implements OnInit, OnDestroy {
    updateEtablissementControl: UntypedFormControl = new UntypedFormControl('', [Validators.required]);
    isValid: boolean = false;
    private readonly _unsubscribeAll: Subject<any> = new Subject<any>();
    constructor(public dialogRef: MatDialogRef<RefreshSirenDialogComponent>, private readonly _operationsServices: OperationsServices,
                private readonly _businessesServices: BusinessesServices) {

    }

    ngOnInit(): void {
        this.updateEtablissementControl.valueChanges
            .pipe(
                takeUntil(this._unsubscribeAll),
                debounceTime(300),
                switchMap((query) => {
                    return this._businessesServices.getEtablissementClientBySiret(query) // verifie si le code MKGT existe
                        .pipe(
                            map(customer => ({ customer, query })),
                            catchError((error) => {
                                if (error.status === 404) {
                                    // Erreur 404: Le client n'a pas été trouvé.
                                    this.isValid = false;
                                    this.updateEtablissementControl.setErrors({ customerNotFound: true });
                                    return of({ customer: null, query });
                                } else {
                                    this.updateEtablissementControl.setErrors({ otherError: true });
                                    throw error;
                                }
                            })
                        )
                }),
                tap(({ customer, query }) => {
                    if (customer && customer.siret === query) {
                        // Le code SIRET est valide.
                        this.isValid = true;
                        this.updateEtablissementControl.setErrors(null)
                    } else {
                        this.updateEtablissementControl.setErrors({ customerNotFound: true });
                        //Le code MKGT est invalide.
                        this.isValid = false;
                    }
                })
            )
            .subscribe();
    }

    ngOnDestroy(): void {
        this._unsubscribeAll.complete();
    }

    onCancel(): void {
        this.dialogRef.close();
    }

    onRefresh(): void {
        this._operationsServices.getSirenInfoBySiret(this.updateEtablissementControl.value).subscribe();
        this.dialogRef.close();
    }
}
