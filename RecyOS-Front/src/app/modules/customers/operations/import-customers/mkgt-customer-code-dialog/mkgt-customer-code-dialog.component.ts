import {Component, OnDestroy, OnInit} from "@angular/core";
import {MatDialogModule, MatDialogRef} from "@angular/material/dialog";
import {ReactiveFormsModule, UntypedFormControl, Validators} from "@angular/forms";
import {OperationsServices} from "../../operations.services";
import {catchError, debounceTime, map, of, Subject, switchMap, takeUntil, tap} from "rxjs";
import {BusinessesServices} from "../../../../../core/services/entity-services/businesses.services";
import {MatFormFieldModule} from "@angular/material/form-field";
import {MatInputModule} from "@angular/material/input";
import {NgIf} from "@angular/common";
import {MatButtonModule} from "@angular/material/button";

@Component({
    selector: 'mkgt-customer-code-dialog',
    templateUrl: './mkgt-customer-code-dialog.component.html',
    standalone: true,
    imports: [
        MatDialogModule,
        MatFormFieldModule,
        ReactiveFormsModule,
        MatInputModule,
        NgIf,
        MatButtonModule
    ],
    providers: [OperationsServices, BusinessesServices, MatDialogModule, ReactiveFormsModule]
})
export class MkgtCustomerCodeDialogComponent implements OnInit, OnDestroy{
    importCliControl: UntypedFormControl = new UntypedFormControl('', [Validators.required]);
    isValid: boolean = false;
    private readonly _unsubscribeAll: Subject<any> = new Subject<any>();
    constructor(public dialogRef: MatDialogRef<MkgtCustomerCodeDialogComponent>,
                private readonly _operationService: OperationsServices, private readonly _businessesServices: BusinessesServices ) {

    }

    ngOnInit(): void
    {
        this.importCliControl.valueChanges
            .pipe(
                takeUntil(this._unsubscribeAll),
                debounceTime(300),
                switchMap((query) => {
                    return this._operationService.getMkgtCustomersByCode(query) // verifie si le code MKGT existe
                        .pipe(
                            map(customer => ({ customer, query })),
                            catchError((error) => {
                                if (error.status === 404) {
                                    // Erreur 404: Le client n'a pas été trouvé.
                                    this.isValid = false;
                                    this.importCliControl.setErrors({ customerNotFound: true });
                                    return of({ customer: null, query });
                                } else {
                                    this.importCliControl.setErrors({ otherError: true });
                                    throw error;
                                }
                            })
                        )
                }),
                tap(({ customer, query }) => {
                    if (customer && customer.code === query) {
                        // Le code MKGT est valide.
                        this.isValid = true;
                        this.importCliControl.setErrors(null)
                    } else {
                        this.importCliControl.setErrors({ customerNotFound: true });
                        //Le code MKGT est invalide.
                        this.isValid = false;
                    }
                })
            )
            .subscribe();

    }

    ngOnDestroy(): void
    {
        this._unsubscribeAll.next(null);
        this._unsubscribeAll.complete();
    }

    onCancel(): void {
        this.dialogRef.close();
    }

    onCreate(): void {
        this._businessesServices.getEtablissementClientByCodeMkgt(this.importCliControl.value)
            .pipe(
                catchError((error) => {
                    if (error.status === 404) {
                        // Erreur 404: Le client n'existre pas.
                        this._operationService.importMkgtCustomersByCode(this.importCliControl.value).pipe(
                            catchError((error) => {
                                if (error.status === 404) {
                                    this.importCliControl.setErrors({ importError: true });
                                } else if(error.status === 401) {
                                    console.log("Erreur 401: Vous n'êtes pas autorisé à importer ce client.");
                                    this.importCliControl.setErrors({ notAuthorised: true });
                                }
                                return of(null);
                            }),
                            tap((response => {
                                // Cas sans erreur
                                console.log('Importation réussie.');
                                this.importCliControl.setErrors(null);  // Réinitialisation des erreurs
                                this.dialogRef.close(response);
                            }))
                        ).subscribe();

                        return of(null);
                    }
                }
            ))
            .subscribe((etablissementClient) => {
                if (etablissementClient) {
                    this.importCliControl.setErrors({ codeMkgtAlreadyExists: true });
                }
            });



    }
}
