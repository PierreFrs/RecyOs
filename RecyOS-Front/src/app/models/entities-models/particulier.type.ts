export type ClientParticulierDto = {
    id: number;
    nom: string;
    prenom: string;
    titre: string;
    adresseFacturation1?: string;
    adresseFacturation2?: string;
    adresseFacturation3?: string;
    codePostalFacturation?: string;
    villeFacturation?: string;
    paysFacturation?: string;
    emailFacturation?: string;
    telephoneFacturation: string;
    portableFacturation?: string;
    contactAlternatif?: string;
    emailAlternatif?: string;
    telephoneAlternatif?: string;
    portableAlternatif?: string;
    conditionReglement?: number;
    modeReglement?: number;
    delaiReglement?: number;
    tauxTva?: number;
    encoursMax?: number;
    compteComptable?: string;
    clientBloque?: boolean;
    dateBlocage?: string;
    motifBlocage?: string;
    codeMkgt?: string;
    dateCreMkgt?: string;
    idOdoo?: string;
    dateCreOdoo?: string;
    createDate?: string;
    updateDate?: string;
    createdBy?: string;
    updatedBy?: string;
    idShipperDashdoc?: number;
    noBalance?: boolean;
}

export interface ClientParticulierFormDto {
    id: number;
    idShipperDashdoc?: number;
    noBalance?: boolean;
    identificationForm: {
        addressForm: {
            titre: string;
            nom: string;
            prenom: string;
            adresseFacturation1: string;
            adresseFacturation2?: string;
            adresseFacturation3?: string;
            codePostalFacturation: string;
            villeFacturation: string;
            paysFacturation: string;
        };
        contactsForm: {
            emailFacturation: string;
            telephoneFacturation: string;
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
        };
        erpCodesForm: {
            codeMkgt?: string;
            idOdoo?: string;
        };
        tva?: string;
        clientBloque?: boolean;
        motifBlocage?: string;
        dateBlocage?: string;
    };
}

export interface ClientParticulierDtoPagination {
    length: number;
    size: number;
    page: number;
    lastPage: number;
    startIndex: number;
    cost: number;
}
