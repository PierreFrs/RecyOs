export interface EntrepriseNDCoverDTO {
    id: number;
    coverId: string;
    numeroContratPrimaire: string;
    numeroContratExtension?: string;
    nomPolice: string;
    ehId: string;
    raisonSociale: string;
    formeJuridiqueCode: string;
    secteurActivite: string;
    typeIdentifiant: string;
    siren: string;
    statutEntreprise: string;
    statut: string;
    tempsReponse: string;
    dateChangementPosition: Date;
    periodeRenouvellementOuverte: string;
    nomRue: string;
    codePostal: number;
    ville: string;
    pays: string;
    dateExtraction: Date;
    referenceClient?: string;
    dateSuppression?: Date;
    seraRenouvele?: string;
    dateRenouvellementPrevue?: Date;
    dateExpiration?: Date;
}

export interface EntrepriseNDCoverDTOPagination {
    length: number;
    size: number;
    page: number;
    lastPage: number;
    startIndex: number;
    cost: number;
}
