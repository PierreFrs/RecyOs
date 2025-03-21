using System.Collections.Generic;
using System.Linq;
using RecyOs.ORM.Entities.hub;
using RecyOs.ORM.Filters.hub;

namespace RecyOs.ORM.Filters.Extensions.hub;

public static class EntrepriseCouvertureQueryExtension
{
    public static IQueryable<EntrepriseCouverture> ApplyFilter<TEntrepriseCouverture>(
        this IQueryable<TEntrepriseCouverture> query, EntrepriseCouvertureGridFilter filter)
        where TEntrepriseCouverture : EntrepriseCouverture, new()
    {
        var mapDictionary = new Dictionary<string, string>
        {
            {"Id","Id"},
            {"CoverId", "CoverId"},
            {"NumeroContratPrimaire", "NumeroContratPrimaire"},
            {"NumeroContratExtension", "NumeroContratExtension"},
            {"TypeGarantie", "TypeGarantie"},
            {"EhId","EhId"},
            {"RaisonSociale", "RaisonSociale"},
            {"ReferenceClient", "ReferenceClient"},
            {"Notation","Notation"},
            {"DateAttributionNotation", "DateAttributionNotation"},
            {"TypeReponse", "TypeReponse"},
            {"DateDecision", "DateDecision"},
            {"DateDemande","DateDemande"},
            {"MontantGarantie", "MontantGarantie"},
            {"DeviseGarantie", "DeviseGarantie"},
            {"Decision", "Decision"},
            {"MotifDecision", "MotifDecision"},
            {"NotreCommentaire", "NotreCommentaire"},
            {"CommentaireArbitre", "CommentaireArbitre"},
            {"QuotiteGarantie", "QuotiteGarantie"},
            {"DelaiPaiementSpecifique", "DelaiPaiementSpecifique"},
            {"DateEffetDiffere", "DateEffetDiffere"},
            {"DateExpirationGarantie", "DateExpirationGarantie"},
            {"MontantTemporaire", "MontantTemporaire"},
            {"DeviseMontantTemporaire", "DeviseMontantTemporaire"},
            {"DateExpirationMontantTemporaire", "DateExpirationMontantTemporaire"},
            {"QuotiteGarantieMontantTemporaire", "QuotiteGarantieMontantTemporaire"},
            {"DelaiPaiementMontantTemporaire", "DelaiPaiementMontantTemporaire"},
            {"MontantDemande","MontantDemande"},
            {"Reponse","Reponse"},
            {"MontantAgrement","MontantAgrement"},
            {"NomDemandeur","NomDemandeur"},
            {"DeviseDemandee","DeviseDemandee"},
            {"ConditionsPaiementDemandees", "ConditionsPaiementDemandees"},
            {"DateExpirationDemandee", "DateExpirationDemandee"},
            {"MontantTemporaireDemande", "MontantTemporaireDemande"},
            {"NumeroDemande", "NumeroDemande"},
            {"IdDemande", "IdDemande"},
            {"HeureReponse", "HeureReponse"},
            {"TempsReponse", "TempsReponse"},
            {"Demandeur", "Demandeur"},
            {"DateDemandeMontantTemporaire", "DateDemandeMontantTemporaire"},
            {"TypeIdentifiant", "TypeIdentifiant"},
            {"Siren", "Siren"},
            {"StatutEntreprise", "StatutEntreprise"},
            {"NomRue", "NomRue"},
            {"CodePostal", "CodePostal"},
            {"Ville", "Ville"},
            {"EtatRegionPays", "EtatRegionPays"},
            {"Pays", "Pays"},
            {"ConditionsSpecifiques", "ConditionsSpecifiques"},
            {"AutresConditions1", "AutresConditions1"},
            {"AutresConditions2", "AutresConditions2"},
            {"AutresConditions3", "AutresConditions3"},
            {"AutresConditions4", "AutresConditions4"},
            {"AutresConditionsTemporaires", "AutresConditionsTemporaires"},
            {"DateExtraction", "DateExtraction"},
            {"RepriseGarantiePossible", "RepriseGarantiePossible"},
            {"DateRepriseGarantiePossible", "DateRepriseGarantiePossible"},
            {"CoverGroupRole", "CoverGroupRole"},
            {"CoverGroupId", "CoverGroupId"},
        };
        
        if (!string.IsNullOrWhiteSpace(filter.FilteredBSiren))
        {
            query = query.Where(x => x.Siren.Contains(filter.FilteredBSiren));
        }
        
        if (!string.IsNullOrWhiteSpace(filter.Refus))
        {
            query = query.Where(x => x.MontantGarantie == 0);
        }
        
        if (!string.IsNullOrWhiteSpace(filter.Agreement))
        {
            query = query.Where(x => x.MontantGarantie > 0);
        }

        if (!string.IsNullOrWhiteSpace(filter.SortBy) && !string.IsNullOrWhiteSpace(filter.OrderBy))
        {
            query = query.OrderByDynamic(mapDictionary[filter.SortBy], filter.OrderBy == "desc");
        }
        else
        {
            query = query.OrderByDynamic("siren", true);
        }
        
        return query;
    }
}