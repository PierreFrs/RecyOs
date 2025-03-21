export interface EuropeDTO {
    id: number;
    nom: string;
    vat: string;
    idOdoo?: string;
    idShipperDashdoc?: number;
    codeKerlog?: string;
    codeMkgt?: string;
    codeGpi?: string;
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
    commercialId?: number;
    categorieId?: number;
    groupId?: number;
    dateCreGpi?: string;
    idDashdoc?: number;
    dateCreDashdoc?: string;
    client: boolean;
    fournisseur: boolean;
    frnCodeGpi?: string;
    noBalance?: boolean;
}

export interface EuropeFormDto {
    id: number;
    vat?: string;
    idShipperDashdoc?: number;
    noBalance?: boolean;
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
}

export interface FournisseurEuropeFormDto {
    id: number;
    vat?: string;
    client: boolean;
    idShipperDashdoc?: number;
    fournisseur: boolean;
    noBalance?: boolean;
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
            frnConditionReglement?: number;
            frnModeReglement?: number;
            frnDelaiReglement?: number;
            frnTauxTva?: number;
            frnCompteComptable?: string;
            frnEncoursMax?: number;
            conditionReglement?: number;
            modeReglement?: number;
            delaiReglement?: number;
            tauxTva?: number;
            compteComptable?: string;
            encoursMax?: number;
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
}

export interface EuropeDTOPagination {
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

export interface ClientEuropeBusinessUnitDTO {
    clientEuropeId: number;
    businessUnitId: number;
}
