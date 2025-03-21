import { EntrepriseNDCoverDTO } from '../business-nd-cover.type';

export interface FrenchDTO {
    id: number;
    nom: string;
    siret?: string;
    siren?: string;
    idOdoo?: string;
    codeKerlog?: string;
    codeMkgt?: string;
    contactFacturation?: string;
    emailFacturation?: string;
    telephoneFacturation?: string;
    portableFacturation?: string;
    contactAlternatif?: string;
    emailAlternatif?: string;
    telephoneAlternatif?: string;
    portableAlternatif?: string;
    adresseFacturation1?: string;
    adresseFacturation2?: string;
    adresseFacturation3?: string;
    codePostalFacturation?: string;
    villeFacturation?: string;
    paysFacturation?: string;
    conditionReglement?: number;
    modeReglement?: number;
    delaiReglement?: number;
    tauxTva?: number;
    encoursMax?: number;
    compteComptable?: string;
    frnConditionReglement?: number;
    frnModeReglement?: number;
    frnDelaiReglement?: number;
    frnTauxTva?: number;
    frnEncoursMax?: number;
    frnCompteComptable?: string;
    iban?: string;
    bic?: string;
    clientBloque?: boolean;
    motifBlocage?: string;
    dateBlocage?: string;
    dateCreMKGT?: string;
    DateCreOdoo?: string;
    categorieClientId?: number;
    codeGpi?: string;
    frnCodeGpi?: string;
    idDashdoc?: number;
    dateCreDashdoc?: string;
    tva?: string;
    categorieId?: number;
    client: boolean;
    fournisseur: boolean;
    isDeleted: boolean;
    entrepriseBase: EntrepriseBaseDTO;
    commercialId?: number;
    groupId?: number;
    idShipperDashdoc?: number;
    noBalance?: boolean;
    clientGroupe?: boolean;
}

export interface EtablissementFormDto {
    id: number;
    siret?: string;
    siren?: string;
    idShipperDashdoc?: number;
    noBalance?: boolean;
    clientGroupe?: boolean;
    client: boolean;
    fournisseur: boolean;
    identificationForm: {
        addressForm: {
            nom: string;
            adresseFacturation1?: string;
            adresseFacturation2?: string;
            adresseFacturation3?: string;
            codePostalFacturation?: string;
            villeFacturation?: string;
            paysFacturation?: string;
        };
        bankInfosForm: {
            iban?: string;
            bic?: string;
        };
        contactsForm: {
            contactFacturation?: string;
            emailFacturation?: string;
            telephoneFacturation?: string;
            portableFacturation?: string;
            contactAlternatif?: string;
            emailAlternatif?: string;
            telephoneAlternatif?: string;
            portableAlternatif?: string;
        };
    };
    parametrageForm: {
        billingInfosForm: {
            conditionReglement?: number;
            modeReglement?: number;
            delaiReglement?: number;
            tauxTva?: number;
            compteComptable?: string;
            encoursMax?: number;
            frnConditionReglement?: number;
            frnModeReglement?: number;
            frnDelaiReglement?: number;
            frnTauxTva?: number;
            frnCompteComptable?: string;
            frnEncoursMax?: number;
        };
        erpCodesForm: {
            codeMkgt?: string;
            idOdoo?: string;
            codeGpi?: string;
            frnCodeGpi?: string;
            idDashdoc?: number;
        };
        tva?: string;
        clientBloque?: boolean;
        motifBlocage?: string;
        dateBlocage?: string;
        commercialId?: number;
        categorieId?: number;
        groupId?: number;
    };
    entrepriseBase?: EntrepriseBaseDTO;
    dateCreGpi?: string;
}

export interface FournisseurFranceFormDto {
    id: number;
    siret?: string;
    siren?: string;
    client: boolean;
    idShipperDashdoc?: number;
    noBalance?: boolean;
    clientGroupe?: boolean;
    fournisseur: boolean;
    identificationForm: {
        addressForm: {
            nom: string;
            adresseFacturation1?: string;
            adresseFacturation2?: string;
            adresseFacturation3?: string;
            codePostalFacturation?: string;
            villeFacturation?: string;
            paysFacturation?: string;
        };
        bankInfosForm: {
            iban?: string;
            bic?: string;
        };
        contactsForm: {
            contactFacturation?: string;
            emailFacturation?: string;
            telephoneFacturation?: string;
            portableFacturation?: string;
            contactAlternatif?: string;
            emailAlternatif?: string;
            telephoneAlternatif?: string;
            portableAlternatif?: string;
        };
    };
    parametrageForm: {
        billingInfosForm: {
            conditionReglement?: number;
            modeReglement?: number;
            delaiReglement?: number;
            tauxTva?: number;
            compteComptable?: string;
            encoursMax?: number;
            frnConditionReglement?: number;
            frnModeReglement?: number;
            frnDelaiReglement?: number;
            frnTauxTva?: number;
            frnCompteComptable?: string;
            frnEncoursMax?: number;
        };
        erpCodesForm: {
            codeMkgt?: string;
            idOdoo?: string;
            codeGpi?: string;
            frnCodeGpi?: string;
            idDashdoc?: number;
        };
        tva?: string;
        clientBloque?: boolean;
        motifBlocage?: string;
        dateBlocage?: string;
        commercialId?: number;
        categorieId?: number;
        groupId?: number;
    };
    entrepriseBase?: EntrepriseBaseDTO;
    dateCreGpi?: string;
}

export interface EntrepriseBaseDTO {
    id: number;
    siren?: string;
    sirenFormate?: string;
    nomEntreprise?: string;
    personneMorale?: boolean;
    denomination?: string;
    nom?: string;
    prenom?: string;
    sexe?: string;
    codeNaf?: string;
    libelleCodeNaf?: string;
    domaineActivite?: string;
    dateCreation?: string;
    dateCreationFormate?: string;
    entrepriseCessee?: boolean;
    dateCessation?: string;
    entrepriseEmployeuse?: boolean;
    categorieJuridique?: string;
    formeJuridique?: string;
    effectif?: string;
    effectifMin?: number;
    effectifMax?: number;
    trancheEffectif?: string;
    anneeEffectif?: number;
    capital?: number;
    statutRcs?: string;
    greffe?: string;
    codeGreffe?: string;
    numeroRcs?: string;
    dateImmatriculationRcs?: string;
    numeroTvaIntracommunautaire?: string;
    dateRadiationRcs?: string;
    dateDebutActivite?: string;
    createDate?: string;
    updatedAt?: string;
    createdBy?: string;
    updatedBy?: string;
    objetSocial?: string;
    entrepriseNDCover?: EntrepriseNDCoverDTO;
}

export interface EtablissementFicheDTO {
    id: number;
    siret?: string;
    siretFormate?: string;
    nic?: string;
    codePostal?: string;
    ville?: string;
    pays?: string;
    codePays?: string;
    latitude?: number;
    longitude?: number;
    etablissementCesse?: boolean;
    siege?: boolean;
    etablissementEmployeur?: boolean;
    effectif?: string;
    effectifMin?: number;
    effectifMax?: number;
    trancheEffectif?: string;
    anneeEffectif?: number;
    codeNaf?: string;
    libelleCodeNaf?: string;
    dateDeCreation?: string;
    numeroVoie?: number;
    indiceRepetition?: string;
    typeVoie?: string;
    libelleVoie?: string;
    complementAdresse?: string;
    adresseLigne1?: string;
    adresseLigne2?: string;
    dateCessation?: string;
    enseigne?: string;
    nomCommercial?: string;
    nomDomiciliation?: string;
    sirenDomiciliation?: string;
    predecesseurs?: string;
    successeurs?: string;
    createDate?: string;
    updatedAt?: string;
}

export interface EtablissementDTOPagination {
    length: number;
    size: number;
    page: number;
    lastPage: number;
    startIndex: number;
    cost: number;
}

export interface CategorieClientDto {
    id: number;
    categorieLabel: string;
}
