import { EntrepriseBaseDTO } from '../../../../models/entities-models/french.type';

export interface EntrepriseCouvertureDTO {
    id: number;
    coverId: string;
    numeroContratPrimaire: string;
    numeroContratExtension?: string;
    typeGarantie: string;
    ehId: string;
    raisonSociale: string;
    referenceClient: string;
    notation?: string;
    dateAttributionNotation?: Date;
    typeReponse: string;
    dateDecision: Date;
    dateDemande: Date;
    montantGarantie: number;
    deviseGarantie: string;
    decision: string;
    motifDecision?: string;
    notreCommentaire: string;
    commentaireArbitre?: string;
    quotiteGarantie?: string;
    delaiPaiementSpecifique?: number;
    dateEffetDiffere?: Date;
    dateExpirationGarantie?: Date;
    montantTemporaire?: number;
    deviseMontantTemporaire?: string;
    dateExpirationMontantTemporaire?: Date;
    quotiteGarantieMontantTemporaire?: string;
    delaiPaiementMontantTemporaire?: number;
    montantDemande?: number;
    deviseDemandee?: string;
    conditionsPaiementDemandees?: string;
    dateExpirationDemandee?: Date;
    montantTemporaireDemande?: number;
    numeroDemande?: string;
    idDemande?: number;
    heureReponse?: string;
    tempsReponse?: string;
    demandeur?: number;
    dateDemandeMontantTemporaire?: Date;
    typeIdentifiant: string;
    siren: string;
    statutEntreprise: string;
    nomRue: string;
    codePostal: number;
    ville: string;
    etatRegionPays?: string;
    pays: string;
    conditionsSpecifiques?: string;
    autresConditions1?: string;
    autresConditions2?: string;
    autresConditions3?: string;
    autresConditions4?: string;
    autresConditionsTemporaires?: string;
    dateExtraction: Date;
    repriseGarantiePossible?: Date;
    coverGroupRole?: string;
    coverGroupId?: string;
    entrepriseBase?: EntrepriseBaseDTO;
}

export interface EntrepriseCouvertureDTOPagination {
    length: number;
    size: number;
    page: number;
    lastPage: number;
    startIndex: number;
    cost: number;
}
