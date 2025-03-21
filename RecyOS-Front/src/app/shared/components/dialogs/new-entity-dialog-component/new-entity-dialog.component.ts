import { Component, Inject, OnInit, OnDestroy } from '@angular/core';
import {
    FormControl,
    ReactiveFormsModule,
    ValidationErrors,
    Validators,
} from '@angular/forms';
import {
    MatDialogRef,
    MAT_DIALOG_DATA,
    MatDialogModule,
} from '@angular/material/dialog';
import { catchError, Observable, of, Subject, tap } from 'rxjs';
import { debounceTime, takeUntil, switchMap } from 'rxjs/operators';
import { IEntityServiceStrategy } from '../../../../core/strategies/entity-strategy/IEntityServiceStrategy';
import {
    BusinessUnitDto,
    EntityBusinessUnitDto,
} from '../../../../models/business-unit.type';
import { EntityStrategyService } from '../../../../core/strategies/entity-strategy/entity-strategy.service';
import { IStrategyObject } from '../../../../core/strategies/entity-strategy/IStrategyObject';
import { SiretService } from '../../../../core/services/helpers/siret.service';
import { VatService } from '../../../../core/services/helpers/vat.service';
import { ParticulierService } from '../../../../core/services/entity-services/particulier.service';
import {
    EntityDto,
    EntityDTOPagination,
    EntityFormDto,
} from '../../../../models/entities-models/entity.type';
import { SharedModule } from '../../../shared.module';
import { MatInputModule } from '@angular/material/input';
import { NgIf } from '@angular/common';
import { MatButtonModule } from '@angular/material/button';
import { ClientValidators } from '../../../../core/validators/client-exists-validator';
import { SelectFieldComponent } from '../../form-components/select-field/select-field.component';
import { InputFieldComponent } from '../../form-components/input-field/input-field.component';

@Component({
    selector: 'app-new-supplier-dialog',
    templateUrl: './new-entity-dialog.component.html',
    imports: [
        SharedModule,
        MatInputModule,
        ReactiveFormsModule,
        MatDialogModule,
        NgIf,
        MatButtonModule,
        SelectFieldComponent,
        InputFieldComponent,
    ],
    standalone: true,
})
export class NewEntityDialogComponent implements OnInit, OnDestroy {
    isValid: boolean = false;
    clientExistsError: boolean = false;
    strategyObject: IStrategyObject;
    warningState:
        | 'none'
        | 'duplicateWarning'
        | 'notFound'
        | 'confirmationDuplicate'
        | 'confirmationEmpty' = 'none';
    warningMessage: string = '';

    etablissementControl: FormControl;
    // *** NEW FORM CONTROLS FOR "PARTICULIER" ***
    titreControl: FormControl;
    nomControl: FormControl;
    prenomControl: FormControl;
    adresse1Control: FormControl;
    adresse2Control: FormControl;
    adresse3Control: FormControl;
    codePostalControl: FormControl;
    villeControl: FormControl;
    paysControl: FormControl;
    emailFacturationControl: FormControl;
    telephoneFacturationControl: FormControl;
    portableFacturationControl: FormControl;
    contactAlternatifControl: FormControl;
    emailAlternatifControl: FormControl;
    telephoneAlternatifControl: FormControl;
    portableAlternatifControl: FormControl;

    private readonly _unsubscribeAll: Subject<any> = new Subject<any>();
    private entityServiceStrategy: IEntityServiceStrategy<
        EntityDto,
        EntityDTOPagination,
        EntityFormDto,
        BusinessUnitDto,
        EntityBusinessUnitDto
    >;

    constructor(
        public dialogRef: MatDialogRef<NewEntityDialogComponent>,
        @Inject(MAT_DIALOG_DATA) public data: { entityRegion: string },
        private readonly _entityStrategyService: EntityStrategyService,
        private readonly _siretService: SiretService,
        private readonly _vatService: VatService,
        private readonly _particulierService: ParticulierService
    ) {
        // *** Initialize "Strtegy Object" ***
        this.strategyObject = this._entityStrategyService.determineStrategy();

        // *** Initialize "Supplier" Control ***
        this.etablissementControl = new FormControl('', [
            Validators.required,
            this.dynamicValidator(),
        ]);

        // *** Initialize "Particulier" Controls if applicable ***
        if (this.strategyObject.status === 'particulier') {
            this.titreControl = new FormControl('', Validators.required);
            this.nomControl = new FormControl('', {
                validators: Validators.required,
                asyncValidators: ClientValidators.clientExists(
                    this._particulierService
                ),
                updateOn: 'blur',
            });
            this.prenomControl = new FormControl('', {
                validators: Validators.required,
                asyncValidators: ClientValidators.clientExists(
                    this._particulierService
                ),
                updateOn: 'blur',
            });
            this.adresse1Control = new FormControl('', Validators.required);
            this.adresse2Control = new FormControl('');
            this.adresse3Control = new FormControl('');
            this.codePostalControl = new FormControl('', Validators.required);
            this.villeControl = new FormControl('', Validators.required);
            this.paysControl = new FormControl('', Validators.required);
            this.emailFacturationControl = new FormControl('', [
                Validators.required,
                Validators.email,
            ]);
            this.telephoneFacturationControl = new FormControl(
                '',
                Validators.required
            );
            this.portableFacturationControl = new FormControl('');
            this.contactAlternatifControl = new FormControl('');
            this.emailAlternatifControl = new FormControl('', Validators.email);
            this.telephoneAlternatifControl = new FormControl('');
            this.portableAlternatifControl = new FormControl('');
        }
    }

    ngOnInit(): void {
        this.entityServiceStrategy = this.strategyObject.strategy;
        this.setupValueChangesSubscription();
    }

    ngOnDestroy(): void {
        this._unsubscribeAll.next(null);
        this._unsubscribeAll.complete();
    }

    private setupValueChangesSubscription(): void {
        // *** Only subscribe to supplierControl changes for non-particuliers ***
        if (this.strategyObject.status === 'professional') {
            this.etablissementControl.valueChanges
                .pipe(
                    takeUntil(this._unsubscribeAll),
                    debounceTime(300),
                    switchMap((query) => this.processQuery(query))
                )
                .subscribe();
        } else {
            // *** Validate "Particulier" Form Fields ***
            // Debounce the "nom" and "prenom" fields and check if the client exists
            this.nomControl.valueChanges
                .pipe(
                    debounceTime(300),
                    takeUntil(this._unsubscribeAll),
                    switchMap(() => this.verifyClientExists())
                )
                .subscribe();

            this.prenomControl.valueChanges
                .pipe(
                    debounceTime(300),
                    takeUntil(this._unsubscribeAll),
                    switchMap(() => this.verifyClientExists())
                )
                .subscribe();

            this.titreControl.valueChanges.subscribe(() =>
                this.checkIfFormValid()
            );
            this.adresse1Control.valueChanges.subscribe(() =>
                this.checkIfFormValid()
            );
            this.codePostalControl.valueChanges.subscribe(() =>
                this.checkIfFormValid()
            );
            this.villeControl.valueChanges.subscribe(() =>
                this.checkIfFormValid()
            );
            this.paysControl.valueChanges.subscribe(() =>
                this.checkIfFormValid()
            );
            this.emailFacturationControl.valueChanges.subscribe(() =>
                this.checkIfFormValid()
            );
            this.telephoneFacturationControl.valueChanges.subscribe(() =>
                this.checkIfFormValid()
            );
        }
    }

    private processQuery(query: string): Observable<boolean> {
        const isValid = this.validateAdministrativeIdentifier(query);

        if (!isValid) {
            console.log('Invalid identifier format.');
            this.isValid = false;
            return of(null);
        }

        return this.verifyIdentifierExistence(query);
    }

    private verifyClientExists(): Observable<boolean> {
        const prenom = this.prenomControl.value;
        const nom = this.nomControl.value;
        const ville = this.villeControl.value;

        // If either field is empty, do not make the request
        if (!prenom || !nom || !ville) {
            this.nomControl.setErrors(null);
            this.prenomControl.setErrors(null);
            this.villeControl.setErrors(null);
            this.clientExistsError = false;
            this.checkIfFormValid(); // Recheck form validity
            return of(false);
        }

        return this._particulierService.checkNomExists(prenom, nom, ville).pipe(
            tap((exists: boolean) => {
                if (exists) {
                    // Set clientExists error if the client exists
                    this.nomControl.setErrors({ clientExists: true });
                    this.prenomControl.setErrors({ clientExists: true });
                    this.villeControl.setErrors({ clientExists: true });
                    this.clientExistsError = true;
                } else {
                    // Clear the error if no client exists
                    this.nomControl.setErrors(null);
                    this.prenomControl.setErrors(null);
                    this.villeControl.setErrors(null);
                    this.clientExistsError = false;
                }
                this.checkIfFormValid(); // Recheck form validity to update "Create" button
            }),
            catchError(() => {
                // Handle potential errors gracefully (e.g., network issues)
                this.nomControl.setErrors(null);
                this.prenomControl.setErrors(null);
                this.villeControl.setErrors(null);
                this.clientExistsError = false;
                this.checkIfFormValid();
                return of(false);
            })
        );
    }

    private verifyIdentifierExistence(query: string): Observable<boolean> {
        return this.entityServiceStrategy.checkIfEntityExists(query).pipe(
            tap((exists) => {
                this.isValid = !exists;
                if (exists) {
                    console.log(
                        'Identifier is already used by another establishment.'
                    );
                } else {
                    console.log('Identifier is valid and not used.');
                }
            })
        );
    }

    private validateAdministrativeIdentifier(identifier: string): boolean {
        return this.strategyObject.region === 'france'
            ? this._siretService.isValidSiret(identifier)
            : this._vatService.isValidVat(identifier);
    }

    private dynamicValidator(): any {
        return (control1: FormControl) => {
            if (this.strategyObject?.region === 'france') {
                return this.validateFrenchSupplier(control1.value);
            } else if (this.strategyObject?.region === 'europe') {
                return this.validateEuropeanSupplier(control1.value);
            }
            return null;
        };
    }

    private validateFrenchSupplier(siret: string): ValidationErrors | null {
        return this._siretService.isValidSiret(siret)
            ? null
            : { invalidSiret: true };
    }

    private validateEuropeanSupplier(vat: string): ValidationErrors | null {
        if (vat?.startsWith('FR')) {
            return { FRVat: true };
        }
        return this._vatService.isValidVat(vat)
            ? null
            : { InvalidVatFormat: true };
    }

    // *** Check Form Validity for "Particulier" Fields ***
    private checkIfFormValid(): void {
        this.isValid =
            this.titreControl.valid &&
            this.nomControl.valid &&
            this.prenomControl.valid &&
            this.adresse1Control.valid &&
            this.codePostalControl.valid &&
            this.villeControl.valid &&
            this.paysControl.valid &&
            this.emailFacturationControl.valid &&
            this.telephoneFacturationControl.valid;
    }

    onCancel(): void {
        this.dialogRef.close();
    }

    onCreate(): void {
        // Handle "Particulier" Submission
        if (this.strategyObject.status === 'particulier') {
            const prenom = this.prenomControl.value;
            const nom = this.nomControl.value;
            const ville = this.villeControl.value;

            // Check if the ClientParticulier already exists
            this._particulierService
                .checkNomExists(prenom, nom, ville)
                .subscribe({
                    next: (exists: boolean) => {
                        if (exists) {
                            console.error(
                                'ClientParticulier with this name already exists.'
                            );
                            this.clientExistsError = true;

                            // Show warning to user within the same dialog
                            this.warningMessage = `Un client avec le prénom "${prenom}", le nom "${nom}", et vivant à "${ville}" existe déjà. Voulez-vous continuer ou annuler ?`;
                            this.warningState = 'duplicateWarning';
                        } else {
                            this.finalizeClientParticulierCreation();
                        }
                    },
                    error: (err) => {
                        console.error('Error checking client existence:', err);
                    },
                });
        } else {
            // Handle Non-Particulier Submission
            this.entityServiceStrategy
                .createEntityFromAdministrativeIdentifier(
                    this.etablissementControl.value
                )
                .subscribe({
                    next: (response) => {
                        if (response === null) {
                            this.warningState = 'notFound';
                            this.warningMessage =
                                "L'entitée n'existe pas encore dans la base de données Pappers. Souhaitez-vous le créer à la main ?";
                        } else {
                            console.log('Entity created successfully.');
                            this.dialogRef.close();
                        }
                    },
                    error: (err) => {
                        console.error('Error while creating entity:', err);
                    },
                });
        }
    }

    continueWithWarning(): void {
        if (this.warningState === 'duplicateWarning') {
            this.warningState = 'confirmationDuplicate';
            this.warningMessage =
                'Vous êtes potentiellement sur le point de créer une entitée en double. Êtes-vous sûr de vouloir continuer ?';
        } else if (this.warningState === 'confirmationDuplicate') {
            this.finalizeClientParticulierCreation();
        }
    }

    cancelCreation(): void {
        this.dialogRef.close();
    }

    confirmManualCreation(): void {
        this.warningState = 'confirmationEmpty';
        this.warningMessage =
            'Êtes-vous sûr de vouloir créer cette entitée à la main ?';
    }

    finalizeManualCreation(): void {
        this.entityServiceStrategy
            .createEmptyEntity(this.etablissementControl.value)
            .subscribe({
                next: (response) => {
                    console.log('Client manually created:', response);
                    this.warningState = 'none';
                    this.dialogRef.close(response);
                },
                error: (err) => {
                    console.error('Error during manual creation:', err);
                },
            });
    }

    private finalizeClientParticulierCreation(): void {
        const clientParticulierFormData = new FormData();
        clientParticulierFormData.append('titre', this.titreControl.value);
        clientParticulierFormData.append('nom', this.nomControl.value);
        clientParticulierFormData.append('prenom', this.prenomControl.value);
        clientParticulierFormData.append(
            'adresseFacturation1',
            this.adresse1Control.value
        );
        clientParticulierFormData.append(
            'adresseFacturation2',
            this.adresse2Control.value || ''
        ); // Optional field
        clientParticulierFormData.append(
            'adresseFacturation3',
            this.adresse3Control.value || ''
        ); // Optional field
        clientParticulierFormData.append(
            'codePostalFacturation',
            this.codePostalControl.value
        );
        clientParticulierFormData.append(
            'villeFacturation',
            this.villeControl.value
        );
        clientParticulierFormData.append(
            'paysFacturation',
            this.paysControl.value
        );
        clientParticulierFormData.append(
            'emailFacturation',
            this.emailFacturationControl.value
        );
        clientParticulierFormData.append(
            'telephoneFacturation',
            this.telephoneFacturationControl.value
        );
        clientParticulierFormData.append(
            'portableFacturation',
            this.portableFacturationControl.value || ''
        ); // Optional field
        clientParticulierFormData.append(
            'contactAlternatif',
            this.contactAlternatifControl.value || ''
        ); // Optional field
        clientParticulierFormData.append(
            'emailAlternatif',
            this.emailAlternatifControl.value || ''
        ); // Optional field
        clientParticulierFormData.append(
            'telephoneAlternatif',
            this.telephoneAlternatifControl.value || ''
        ); // Optional field
        clientParticulierFormData.append(
            'portableAlternatif',
            this.portableAlternatifControl.value || ''
        ); // Optional field

        this._particulierService
            .createClientParticulierFromForm(clientParticulierFormData)
            .subscribe({
                next: () => {
                    console.log('Client Particulier created successfully.');
                    this.dialogRef.close();
                },
                error: (err) => {
                    console.error(
                        'Error while creating client particulier:',
                        err
                    );
                },
            });
    }
}
