import {ChangeDetectorRef, Injectable} from '@angular/core';
import {catchError, map, Observable, of, throwError} from "rxjs";
import {EntityDto, EntityDTOPagination, EntityFormDto} from "../../../models/entities-models/entity.type";
import {MatDialog, MatDialogRef} from "@angular/material/dialog";
import {IEntityServiceStrategy} from "../../strategies/entity-strategy/IEntityServiceStrategy";
import {BusinessUnitDto, EntityBusinessUnitDto} from "../../../models/business-unit.type";
import {MkgtFcliService} from "../mkgt-fcli.service";
import {switchMap} from "rxjs/operators";
import {MkgtCodeDialogComponent} from "../../../shared/components/dialogs/mkgt-code-dialog/mkgt-code-dialog.component";
import {FormGroup} from "@angular/forms";
import {ErpCreationWaitingDialog} from "../../../modules/customers/OdooCreationWaitDialog/ErpCreationWaitingDialog";
import {EntityStrategyService} from "../../strategies/entity-strategy/entity-strategy.service";

@Injectable({
  providedIn: 'root'
})
export class ErpCodesService {

    private entityServiceStrategy: IEntityServiceStrategy<
        EntityDto,
        EntityDTOPagination,
        EntityFormDto,
        BusinessUnitDto,
        EntityBusinessUnitDto
    >;

  constructor(private readonly _mkgtFcliService: MkgtFcliService,
              private readonly _dialog: MatDialog,
              private readonly _entityStrategyService: EntityStrategyService // <-- Inject the service that can provide the strategy.
  ) { }

    setEntityServiceStrategy(entity: EntityDto): void {
        this.entityServiceStrategy = this._entityStrategyService.determineStrategyFromEntity(entity).strategy;
    }


    /**
     * Helper method to update the entity with MKGT code and return an Observable.
     * @param entity - The entity to update.
     * @param waitingDialogRef - The dialog reference to close on completion.
     * @param erpCodesFormGroup - The form group to update with the entity.
     * @param changeDetectorRef - The change detector to mark for check.
     * @param regenerateMkgt - Whether to regenerate the MKGT code.
     */
    updateEntityWithMkgt(
        entity: EntityDto,
        waitingDialogRef: MatDialogRef<any>,
        erpCodesFormGroup: FormGroup,
        changeDetectorRef: ChangeDetectorRef,
        regenerateMkgt: boolean = true
    ): Observable<EntityDto> {
        entity = this.verifyAndGenerateMkgtCode(entity, regenerateMkgt);
        return this.verifyAndUpdateMkgtCode(entity, waitingDialogRef, erpCodesFormGroup, changeDetectorRef);
    }

    /**
     * Helper method to verify the MKGT code and generate a new one if necessary.
     * @param entity - The entity to verify and generate the MKGT code for.
     * @param regenerateMkgt - Whether to regenerate the MKGT code.
     */
    private verifyAndGenerateMkgtCode(entity: EntityDto, regenerateMkgt: boolean = true): EntityDto {
        if (regenerateMkgt) {
            entity.codeMkgt = this.generateMkgtCode(
                entity.nom,
                entity.villeFacturation,
                entity.codePostalFacturation
            );
        }
        return entity;
    }

    /**
     * Génère un code client MKGT basé sur la raison sociale, la ville et le département.
     *
     * @param {string} raisonSociale - La raison sociale de l'entreprise.
     * @param {string} ville - La ville de l'adresse de facturation de l'entreprise.
     * @param {string} departement - Le département de l'adresse de facturation de l'entreprise.
     * @returns {string} - Le code client MKGT généré.
     *
     * Le code client MKGT est généré en suivant ces règles :
     * 1. Prendre les 7 premiers caractères prononcés de la raison sociale (en supprimant les espaces, les points, les caractères spéciaux et tout ce qui n'est pas prononcé).
     * 2. Prendre les 4 premiers caractères de la ville.
     * 3. Prendre les 2 premiers chiffres du département.
     *
     * Le code client MKGT est limité à 13 caractères et est unique pour chaque fiche client.
     */
    private generateMkgtCode(
        raisonSociale: string,
        ville: string,
        departement: string,
    ): string {
        const cleanedRaisonSociale = this.cleanAndTruncate(raisonSociale, 7).toUpperCase();
        const cleanedVille = this.cleanAndTruncate(ville, 4).toUpperCase();
        const cleanedDepartement = departement.slice(0, 2).toUpperCase();

        return `${cleanedRaisonSociale}${cleanedVille}${cleanedDepartement}`;
    }

    /**
     * Nettoie et tronque le texte en supprimant les caractères spéciaux, les espaces et en le limitant à une longueur maximale.
     *
     * @param {string} text - Le texte à nettoyer et tronquer.
     * @param {number} maxLength - La longueur maximale du texte après troncature.
     * @returns {string} - Le texte nettoyé et tronqué.
     *
     * Cette méthode effectue les opérations suivantes :
     * 1. Supprime les caractères spéciaux et les espaces du texte.
     * 2. Tronque le texte à la longueur maximale spécifiée.
     */
    private cleanAndTruncate(text: string, maxLength: number): string {
        const cleanedText = text.replace(/[^a-zA-Z0-9]/g, ''); // Supprime les caractères spéciaux et les espaces
        return cleanedText.slice(0, maxLength);
    }

    /**
     * Helper method to verify and update the MKGT code.
     * @param entity - The entity to verify and update.
     * @param waitingDialogRef - The dialog reference to close on completion.
     * @param erpCodesFormGroup - The form group to update with the entity.
     * @param changeDetectorRef - The change detector to mark for check.
     */
    private verifyAndUpdateMkgtCode(entity: EntityDto,
                                    waitingDialogRef: MatDialogRef<any>,
                                    erpCodesFormGroup: FormGroup,
                                    changeDetectorRef: ChangeDetectorRef
    ): Observable<EntityDto> {
        return this._mkgtFcliService.checkMkgtFcliExistence(entity.codeMkgt).pipe(
            switchMap((exists: boolean) => {
                if (exists) {
                    return this.handleExistingMkgtCode(entity, waitingDialogRef, erpCodesFormGroup, changeDetectorRef);
                }
                return this.handleSavingMkgtCode(entity, waitingDialogRef, erpCodesFormGroup, changeDetectorRef);
            }),
            catchError((error): Observable<EntityDto> => {
                console.error('Error checking MKGT code existence:', error);
                waitingDialogRef.close();
                return throwError(() => new Error('Error checking MKGT code existence')) as Observable<EntityDto>;
            })
        );
    }

    /*
    * Handle the case where the MKGT code already exists.
    * @param entity - The entity to update.
    * @param dialogRef - The dialog reference to close on completion.
    */
    private handleExistingMkgtCode(entity: EntityDto, dialogRef: MatDialogRef<any>, erpCodesFormGroup: FormGroup, changeDetectorRef: ChangeDetectorRef): Observable<EntityDto> {
        return this.openMkgtCodeDialog(entity.codeMkgt).pipe(
            switchMap((result: { action: string, codeMkgt: string } | null): Observable<EntityDto> => {
                if (result?.action === 'overwrite') {
                    return this.saveUpdatedEntity(entity, dialogRef, erpCodesFormGroup, changeDetectorRef);
                } else if (result?.action === 'modify') {
                    entity.codeMkgt = result.codeMkgt;
                    return this.updateEntityWithMkgt(entity, dialogRef, erpCodesFormGroup, changeDetectorRef, false);
                } else {
                    return throwError(() => new Error('User canceled the update process')) as Observable<EntityDto>;
                }
            }),
            catchError((error): Observable<EntityDto> => {
                console.error('Error during MKGT code dialog:', error);
                return throwError(() => new Error('Error during MKGT code dialog')) as Observable<EntityDto>;
            })
        );
    }

    /**
     * Utility to open the dialog for changing mkgt code.
     * @param currentCode - The current MKGT code.
     */
    private openMkgtCodeDialog(currentCode: string): Observable<{ action: string; codeMkgt: string } | null> {
        const dialogRef = this._dialog.open(MkgtCodeDialogComponent, {
            disableClose: true,
            data: { codeMkgt: currentCode }
        });
        // Return the afterClosed observable from the dialogRef.
        return dialogRef.afterClosed();
    }

    /**
     * Helper method to save the updated entity with the MKGT code.
     * @param entity - The entity to save.
     * @param dialogRef - The dialog reference to close on completion.
     * @param erpCodesFormGroup - The form group to update with the entity.
     * @param changeDetectorRef - The change detector to mark for check.
     */
    private handleSavingMkgtCode(entity: EntityDto, dialogRef: MatDialogRef<any>, erpCodesFormGroup: FormGroup, changeDetectorRef: ChangeDetectorRef) : Observable<EntityDto> {
        return this.saveUpdatedEntity(entity, dialogRef, erpCodesFormGroup, changeDetectorRef)
    }

    /**
     * Helper method to save the updated entity and close the dialog
     * @param entity - The entity to save
     * @param dialogRef - The dialog reference to close on completion
     * @param erpCodesFormGroup - The form group to update with the entity
     * @param changeDetectorRef - The change detector to mark for check.
     */
    private saveUpdatedEntity(entity: EntityDto, dialogRef: MatDialogRef<any>, erpCodesFormGroup: FormGroup, changeDetectorRef: ChangeDetectorRef): Observable<EntityDto> {
        erpCodesFormGroup.patchValue(entity);
        changeDetectorRef.markForCheck();

        return this.entityServiceStrategy.updateEntity(entity.id, entity).pipe(
            map(() => {
                dialogRef.close();
                return entity;
            }),
            catchError(this.handleError(dialogRef, 'Error updating entity with MKGT code'))
        );
    }

    /**
     * Helper method to check if Odoo ID exists and fetch the updated entity.
     * @param entity - The entity to check and fetch.
     * @param erpCodesFormGroup - The form group to update with the entity.
     * @param changeDetectorRef - The change detector to mark for check.
     */
    checkAndFetchEntity(entity: EntityDto, erpCodesFormGroup: FormGroup, changeDetectorRef: ChangeDetectorRef): Observable<EntityDto> {
        this.setEntityServiceStrategy(entity);

        return this.checkIfIdOdooExists(entity, erpCodesFormGroup, changeDetectorRef).pipe(
            switchMap(() => this.entityServiceStrategy.getEntityById(entity.id)),
            catchError(this.handleError(null, 'Error fetching updated entity'))
        );
    }

    /**
     * Check if the Odoo ID exists, and create it if it doesn't.
     * @param entity - The entity to check.
     * @param erpCodesFormGroup - The form group to update with the entity.
     * @param changeDetectorRef - The change detector to mark for check.
     * @returns An observable of the entity with the Odoo ID.
     * If the entity already has an Odoo ID, the observable will emit the entity as is.
     * If the entity doesn't have an Odoo ID, the observable will emit the entity with the newly created Odoo ID.
     * If the entity creation fails, the observable will emit an error.
     */

    private checkIfIdOdooExists(entity: EntityDto, erpCodesFormGroup: FormGroup, changeDetectorRef: ChangeDetectorRef): Observable<EntityDto> {
        if (entity.idOdoo) {
            return of(entity);
        }
        return this.createOdooEntityAsync(entity, erpCodesFormGroup, changeDetectorRef).pipe(
            map((updatedEntity) => {
                return updatedEntity;
            }),
            catchError(this.handleError(null, 'Failed to create Odoo entity'))
        );
    }

    /**
     * Create an Odoo entity for the given entity asynchronously.
     * @param entity
     * @param erpCodesFormGroup
     * @param changeDetectorRef
     */
    createOdooEntityAsync(entity: EntityDto, erpCodesFormGroup: FormGroup, changeDetectorRef: ChangeDetectorRef): Observable<EntityDto> {
        const dialogRef = this.openWaitingDialog('Odoo');

        return this.createAndHandleOdooEntity(entity, dialogRef, erpCodesFormGroup, changeDetectorRef);
    }

    /**
     * Helper function to handle Odoo entity creation and update the UI.
     * @param entity - The entity to create the Odoo entity for.
     * @param dialogRef - The dialog reference to close on completion.
     * @param erpCodesFormGroup - The form group to update with the entity.
     * @param changeDetectorRef - The change detector to mark for check.
     */
    createAndHandleOdooEntity(entity: EntityDto, dialogRef: MatDialogRef<any>, erpCodesFormGroup: FormGroup, changeDetectorRef: ChangeDetectorRef): Observable<EntityDto> {
        this.setEntityServiceStrategy(entity);

        return this.entityServiceStrategy.createOdooEntityFromEntity(entity.id).pipe(
            map((client) => {
                entity.idOdoo = client.idOdoo;
                erpCodesFormGroup.patchValue(entity);
                changeDetectorRef.markForCheck();
                dialogRef.close();
                return entity;
            }),
            catchError(this.handleError(dialogRef, 'Error creating Odoo entity'))
        );
    }

    /**
     * Utility to open the dialog for showing progress.
     * @param erpType - The type of ERP being created.
     */
    openWaitingDialog(erpType: string): MatDialogRef<any> {
        return this._dialog.open(ErpCreationWaitingDialog, {
            disableClose: true,
            data: { erpType },
        });
    }

    /**
     * Reusable error handling method
     */
    handleError(dialogRef: MatDialogRef<any>, message: string): (error: any) => Observable<never> {
        return (error: any) => {
            console.error(`${message}:`, error);
            dialogRef.close();
            return throwError(() => new Error(message));
        };
    }

    /**
     * Shared logic to handle Dashdoc creation for both synchronous and asynchronous methods.
     */
    handleDashdocCreation(entity: EntityDto, dialogRef: MatDialogRef<any>, erpCodesFormGroup: FormGroup, changeDetectorRef: ChangeDetectorRef): Observable<EntityDto> {
        this.setEntityServiceStrategy(entity);

        return this.checkIfIdOdooExists(entity, erpCodesFormGroup, changeDetectorRef).pipe(
            switchMap(() => {
                return this.entityServiceStrategy.createDashdocEntityFromEntity(entity.id);
            }),
            map((client) => {

                if ('idDashdoc' in entity) {
                    entity.idDashdoc = client.idDashdoc;
                }
                erpCodesFormGroup.patchValue(entity);
                changeDetectorRef.markForCheck();
                dialogRef.close();
                return entity;
            }),
            catchError(this.handleError(dialogRef, 'Failed to create DashDoc Entity'))
        );
    }

    /**
     * Creates a Hubspot entity from the given entity if it is eligible.
     * @param entity - The entity to create the Hubspot entity for.
     * @param dialogRef - The dialog reference to close on completion.
     */
    createHubspotEntityIfEligible(entity: EntityDto, dialogRef: MatDialogRef<any>): Observable<EntityDto> {
        if (this.isFrenchOrEuropeanMkgtCode(entity)) {
            return this.createHubspotEntity(entity, dialogRef);
        } else {
            console.log('Entity is not eligible for Hubspot creation.');
            return of(entity); // Return the entity unchanged.
        }
    }

    /**
     * Helper method to check if an entity is a French or European MKGT code.
     * @param entity - The entity to check.
     * @returns True if the entity is French/European MKGT, false otherwise.
     */
    private isFrenchOrEuropeanMkgtCode(entity: EntityDto): boolean {
        // Replace this logic with the actual condition to identify French/European MKGT codes.
        return "client" in entity && entity.client && !("prenom" in entity);
    }

    /**
     * Helper method to create a Hubspot entity from the given entity.
     * @param entity - The entity to create the Hubspot entity for.
     * @param dialogRef - The dialog reference to close on completion.
     */
    private createHubspotEntity(entity: EntityDto, dialogRef: MatDialogRef<any>): Observable<EntityDto> {
        return this.entityServiceStrategy.createHubspotEntityFromEntity(entity.id).pipe(
            map(() => {
                console.log('Hubspot entity created successfully.');
                dialogRef.close();
                return entity;
            }),
            catchError((error) => {
                if (error.message === 'Not Implemented') {
                    console.warn('Hubspot entity creation not implemented for this type.');
                    return of(entity); // Return the entity unchanged.
                } else {
                    return this.handleError(dialogRef, 'Error creating Hubspot entity')(error);
                }
            })
        );
    }

    createAndHandleGpiEntity(
        entity: EntityDto,
        dialogRef: MatDialogRef<any>,
        erpCodesFormGroup: FormGroup,
        changeDetectorRef: ChangeDetectorRef
    ): Observable<EntityDto> {
        this.setEntityServiceStrategy(entity);

        return this.checkIfIdOdooExists(entity, erpCodesFormGroup, changeDetectorRef).pipe(
            switchMap(() => {
                // Proceed to create GPI entity only after checking Odoo ID
                return this.entityServiceStrategy.createGpiEntityFromEntity(entity.id);
            }),
            map((client) => {
                if ('codeGpi' in entity) {
                    entity.codeGpi = client.codeGpi;
                }
                if ('frnCodeGpi' in entity) {
                    entity.frnCodeGpi = client.frnCodeGpi;
                }
                erpCodesFormGroup.patchValue(entity); // Update the form group with the new GPI codes
                changeDetectorRef.markForCheck(); // Trigger Angular change detection
                dialogRef.close(); // Close the dialog
                return entity; // Return the updated entity
            }),
            catchError((error) => {
                dialogRef.close(); // Ensure the dialog is closed on error
                console.error('Error creating GPI entity:', error);
                return throwError(() => new Error('Failed to create GPI entity'));
            })
        );
    }
}
