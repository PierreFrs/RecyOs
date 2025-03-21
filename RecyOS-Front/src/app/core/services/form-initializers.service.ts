import { Injectable } from '@angular/core';
import { UntypedFormBuilder, Validators } from '@angular/forms';
import { ValidatorService } from 'angular-iban';
import { emailListValidator } from '../validators/email-list-validator';

@Injectable({
    providedIn: 'root',
})
export class FormInitializersService {
    constructor(private readonly _formBuilder: UntypedFormBuilder) {}

    initializeFrenchClientForm() {
        return this._formBuilder.group({
            id: [''],
            siret: ['', [Validators.required]],
            siren: [''],
            identificationForm: this._formBuilder.group({
                addressForm: this._formBuilder.group({
                    nom: ['', Validators.required],
                    adresseFacturation1: ['', Validators.required],
                    adresseFacturation2: [''],
                    adresseFacturation3: [''],
                    villeFacturation: ['', Validators.required],
                    codePostalFacturation: ['', Validators.required],
                    paysFacturation: ['', Validators.required],
                }),
                bankInfosForm: this._formBuilder.group({
                    iban: ['', [ValidatorService.validateIban]],
                    bic: [
                        '',
                        [
                            Validators.pattern(
                                /^[A-Za-z]{4}[A-Za-z]{2}\w{2}(\w{3})?$/,
                            ),
                        ],
                    ],
                }),
                contactsForm: this._formBuilder.group({
                    contactFacturation: [''],
                    emailFacturation: [
                        '',
                        [Validators.required, Validators.email],
                    ],
                    telephoneFacturation: ['', [Validators.required]],
                    portableFacturation: [''],
                    contactAlternatif: [''],
                    emailAlternatif: ['', [emailListValidator()]],
                    telephoneAlternatif: [''],
                    portableAlternatif: [''],
                }),
            }),
            parametrageForm: this._formBuilder.group({
                billingInfosForm: this._formBuilder.group({
                    conditionReglement: ['', [Validators.required]],
                    modeReglement: ['', [Validators.required]],
                    delaiReglement: ['', [Validators.required]],
                    tauxTva: ['', [Validators.required]],
                    compteComptable: ['', [Validators.required]],
                    encoursMax: ['', [Validators.required]],
                    frnConditionReglement: [''],
                    frnModeReglement: [''],
                    frnDelaiReglement: [''],
                    frnTauxTva: [''],
                    frnCompteComptable: [''],
                    frnEncoursMax: [''],
                }),
                erpCodesForm: this._formBuilder.group({
                    codeMkgt: [''],
                    idOdoo: [''],
                    codeGpi: [''],
                    frnCodeGpi: [''],
                    idDashdoc: [''],
                }),
                tva: [''],
                clientBloque: [''],
                motifBlocage: [''],
                dateBlocage: [''],
                categorieId: ['', [Validators.required]],
                commercialId: ['', [Validators.required]],
            }),
        });
    }

    initializeEuropeClientForm() {
        return this._formBuilder.group({
            id: [''],
            vat: ['', [Validators.required]],
            identificationForm: this._formBuilder.group({
                addressForm: this._formBuilder.group({
                    nom: ['', Validators.required],
                    adresseFacturation1: ['', Validators.required],
                    adresseFacturation2: [''],
                    adresseFacturation3: [''],
                    villeFacturation: ['', Validators.required],
                    codePostalFacturation: ['', Validators.required],
                    paysFacturation: ['', Validators.required],
                }),
                bankInfosForm: this._formBuilder.group({
                    iban: ['', [ValidatorService.validateIban]],
                    bic: [
                        '',
                        [
                            Validators.pattern(
                                /^[A-Za-z]{4}[A-Za-z]{2}\w{2}(\w{3})?$/,
                            ),
                        ],
                    ],
                }),
                contactsForm: this._formBuilder.group({
                    contactFacturation: [''],
                    emailFacturation: [
                        '',
                        [Validators.required, Validators.email],
                    ],
                    telephoneFacturation: ['', [Validators.required]],
                    portableFacturation: [''],
                    contactAlternatif: [''],
                    emailAlternatif: ['', [emailListValidator()]],
                    telephoneAlternatif: [''],
                    portableAlternatif: [''],
                }),
            }),
            parametrageForm: this._formBuilder.group({
                billingInfosForm: this._formBuilder.group({
                    conditionReglement: ['', [Validators.required]],
                    modeReglement: ['', [Validators.required]],
                    delaiReglement: ['', [Validators.required]],
                    tauxTva: ['', [Validators.required]],
                    compteComptable: ['', [Validators.required]],
                    encoursMax: ['', [Validators.required]],
                    frnConditionReglement: [''],
                    frnModeReglement: [''],
                    frnDelaiReglement: [''],
                    frnTauxTva: [''],
                    frnCompteComptable: [''],
                    frnEncoursMax: [''],
                }),
                erpCodesForm: this._formBuilder.group({
                    codeMkgt: [''],
                    idOdoo: [''],
                    codeGpi: [''],
                    frnCodeGpi: [''],
                    idDashdoc: [''],
                }),
                tva: [''],
                clientBloque: [''],
                motifBlocage: [''],
                dateBlocage: [''],
                categorieId: ['', [Validators.required]],
                commercialId: ['', [Validators.required]],
            }),
        });
    }

    initializeClientParticulierForm() {
        return this._formBuilder.group({
            id: [''],
            identificationForm: this._formBuilder.group({
                addressForm: this._formBuilder.group({
                    titre: [''],
                    nom: [''],
                    prenom: [''],
                    adresseFacturation1: [''],
                    adresseFacturation2: [''],
                    adresseFacturation3: [''],
                    villeFacturation: [''],
                    codePostalFacturation: [''],
                    paysFacturation: [''],
                }),
                contactsForm: this._formBuilder.group({
                    emailFacturation: [''],
                    telephoneFacturation: [''],
                    portableFacturation: [''],
                    contactAlternatif: [''],
                    emailAlternatif: [''],
                    telephoneAlternatif: [''],
                    portableAlternatif: [''],
                }),
            }),
            parametrageForm: this._formBuilder.group({
                billingInfosForm: this._formBuilder.group({
                    conditionReglement: [''],
                    modeReglement: [''],
                    delaiReglement: [''],
                    tauxTva: [''],
                    compteComptable: [''],
                    encoursMax: [''],
                }),
                erpCodesForm: this._formBuilder.group({
                    codeMkgt: [''],
                    idOdoo: [''],
                    idDashdoc: [''],
                }),
                clientBloque: [''],
                motifBlocage: [''],
                dateBlocage: [''],
                commercialId: [''],
            }),
        });
    }

    initializeFrenchSupplierForm() {
        return this._formBuilder.group({
            id: [''],
            siret: ['', [Validators.required]],
            siren: [''],
            identificationForm: this._formBuilder.group({
                addressForm: this._formBuilder.group({
                    nom: ['', Validators.required],
                    adresseFacturation1: ['', Validators.required],
                    adresseFacturation2: [''],
                    adresseFacturation3: [''],
                    villeFacturation: ['', Validators.required],
                    codePostalFacturation: ['', Validators.required],
                    paysFacturation: ['', Validators.required],
                }),
                bankInfosForm: this._formBuilder.group({
                    iban: ['', [ValidatorService.validateIban]],
                    bic: [
                        '',
                        [
                            Validators.pattern(
                                /^[A-Za-z]{4}[A-Za-z]{2}\w{2}(\w{3})?$/,
                            ),
                        ],
                    ],
                }),
                contactsForm: this._formBuilder.group({
                    contactFacturation: [''],
                    emailFacturation: [
                        '',
                        [Validators.required, Validators.email],
                    ],
                    telephoneFacturation: ['', [Validators.required]],
                    portableFacturation: [''],
                    contactAlternatif: [''],
                    emailAlternatif: ['', [emailListValidator()]],
                    telephoneAlternatif: [''],
                    portableAlternatif: [''],
                }),
            }),
            parametrageForm: this._formBuilder.group({
                billingInfosForm: this._formBuilder.group({
                    frnConditionReglement: ['', [Validators.required]],
                    frnModeReglement: ['', [Validators.required]],
                    frnDelaiReglement: ['', [Validators.required]],
                    frnTauxTva: ['', [Validators.required]],
                    frnCompteComptable: ['', [Validators.required]],
                    frnEncoursMax: ['', [Validators.required]],
                    conditionReglement: [''],
                    modeReglement: [''],
                    delaiReglement: [''],
                    tauxTva: [''],
                    compteComptable: [''],
                    encoursMax: [''],
                }),
                erpCodesForm: this._formBuilder.group({
                    codeMkgt: [''],
                    idOdoo: [''],
                    codeGpi: [''],
                    frnCodeGpi: [''],
                    idDashdoc: [''],
                }),
                tva: [''],
                clientBloque: [''],
                motifBlocage: [''],
                dateBlocage: [''],
                categorieId: [''],
                commercialId: [''],
            }),
        });
    }

    initializeEuropeSupplierForm() {
        return this._formBuilder.group({
            id: [''],
            vat: ['', [Validators.required]],
            identificationForm: this._formBuilder.group({
                addressForm: this._formBuilder.group({
                    nom: ['', Validators.required],
                    adresseFacturation1: ['', Validators.required],
                    adresseFacturation2: [''],
                    adresseFacturation3: [''],
                    villeFacturation: ['', Validators.required],
                    codePostalFacturation: ['', Validators.required],
                    paysFacturation: ['', Validators.required],
                }),
                bankInfosForm: this._formBuilder.group({
                    iban: ['', [ValidatorService.validateIban]],
                    bic: [
                        '',
                        [
                            Validators.pattern(
                                /^[A-Za-z]{4}[A-Za-z]{2}\w{2}(\w{3})?$/,
                            ),
                        ],
                    ],
                }),
                contactsForm: this._formBuilder.group({
                    contactFacturation: [''],
                    emailFacturation: [
                        '',
                        [Validators.required, Validators.email],
                    ],
                    telephoneFacturation: ['', [Validators.required]],
                    portableFacturation: [''],
                    contactAlternatif: [''],
                    emailAlternatif: ['', [emailListValidator()]],
                    telephoneAlternatif: [''],
                    portableAlternatif: [''],
                }),
            }),
            parametrageForm: this._formBuilder.group({
                billingInfosForm: this._formBuilder.group({
                    frnConditionReglement: ['', [Validators.required]],
                    frnModeReglement: ['', [Validators.required]],
                    frnDelaiReglement: ['', [Validators.required]],
                    frnTauxTva: ['', [Validators.required]],
                    frnCompteComptable: ['', [Validators.required]],
                    frnEncoursMax: ['', [Validators.required]],
                    conditionReglement: [''],
                    modeReglement: [''],
                    delaiReglement: [''],
                    tauxTva: [''],
                    compteComptable: [''],
                    encoursMax: [''],
                }),
                erpCodesForm: this._formBuilder.group({
                    codeMkgt: [''],
                    idOdoo: [''],
                    codeGpi: [''],
                    frnCodeGpi: [''],
                    idDashdoc: [''],
                }),
                tva: [''],
                clientBloque: [''],
                motifBlocage: [''],
                dateBlocage: [''],
                categorieId: [''],
                commercialId: [''],
            }),
        });
    }

    initializeForm(
        entityStatus: "professional" | "particulier",
        entityType: "client" | "supplier",
        entityRegion: "france" | "europe" | "unknown"
    ) {
        if (entityStatus === 'professional') {
            if (entityRegion === 'france') {
                if (entityType === 'client') {
                    return this.initializeFrenchClientForm();
                } else if (entityType === 'supplier') {
                    return this.initializeFrenchSupplierForm();
                }
            } else if (entityRegion === 'europe') {
                if (entityType === 'client') {
                    return this.initializeEuropeClientForm();
                } else if (entityType === 'supplier') {
                    return this.initializeEuropeSupplierForm();
                }
            }
        } else if (entityStatus === 'particulier') {
            return this.initializeClientParticulierForm();
        }
        throw new Error('Invalid entity configuration');
    }

}
