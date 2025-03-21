// Created by : Pierre FRAISSE
// RecyOs => RecyOs => EntrepriseNDCoverQueryExtension.cs
// Created : 2023/12/19 - 14:19
// Updated : 2023/12/19 - 14:19

using System.Collections.Generic;
using System.Linq;
using RecyOs.ORM.Entities.hub;
using RecyOs.ORM.Filters.hub;

namespace RecyOs.ORM.Filters.Extensions.hub;

public static class EntrepriseNDCoverQueryExtension
{
    public static IQueryable<EntrepriseNDCover> ApplyFilter<TEntrepriseNDCover>(
        this IQueryable<TEntrepriseNDCover> query, EntrepriseNDCoverGridFilter filter)
        where TEntrepriseNDCover : EntrepriseNDCover, new()
    {
        var mapDictionary = new Dictionary<string, string>
        {
            {"Id","Id"},
            {"CoverId", "CoverId"},
            {"NumeroContratPrimaire", "NumeroContratPrimaire"},
            {"NomPolice", "NomPolice"},
            {"EhId","EhId"},
            {"RaisonSociale", "RaisonSociale"},
            {"FormeJuridiqueCode", "FormeJuridiqueCode"},
            {"SecteurActivite", "SecteurActivite"},
            {"TypeIdentifiant", "TypeIdentifiant"},
            {"Siren", "Siren"},
            {"StatutEntreprise", "StatutEntreprise"},
            {"ReferenceClient", "ReferenceClient"},
            {"Statut", "Statut"},
            {"TempsReponse", "TempsReponse"},
            {"DateChangementPosition", "DateChangementPosition"},
            {"DateSuppression", "DateSuppression"},
            {"DateDemande","DateDemande"},
            {"PeriodeRenouvellementOuverte","PeriodeRenouvellementOuverte"},
            {"DateRenouvellementPrevue", "DateRenouvellementPrevue"},
            {"SeraRenouvele", "SeraRenouvele"},
            {"DateExpiration", "DateExpiration"},
            {"NomRue", "NomRue"},
            {"CodePostal", "CodePostal"},
            {"Ville", "Ville"},
            {"Pays", "Pays"},
            {"DateExtraction", "DateExtraction"},
        };
        
        if (!string.IsNullOrWhiteSpace(filter.FilteredBSiren))
        {
            query = query.Where(x => x.Siren.Contains(filter.FilteredBSiren));
        }
        
        if (!string.IsNullOrWhiteSpace(filter.Refus))
        {
            query = query.Where(x => x.Statut == "Pas de garantie");
        }
        
        if (!string.IsNullOrWhiteSpace(filter.Agreement))
        {
            query = query.Where(x => x.Statut == "Garantie totale");
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