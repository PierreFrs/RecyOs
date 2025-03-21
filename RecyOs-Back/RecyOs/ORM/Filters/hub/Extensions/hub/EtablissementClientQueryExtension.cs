using System.Collections.Generic;
using System.Linq;
using RecyOs.ORM.Entities.hub;
using RecyOs.ORM.Filters.hub;

namespace RecyOs.ORM.Filters.Extensions.hub;

public static class EtablissementClientQueryExtension
{
    public static IQueryable<EtablissementClient> ApplyFilter<TEtablissementClient>(
        this IQueryable<TEtablissementClient> query, EtablissementClientGridFilter filter)
        where TEtablissementClient : EtablissementClient, new()
    {
        // map dictionary for EtablissementClient object
        var mapDictionary = new Dictionary<string, string>
        {
            {"Siret", "Siret"},
            {"CodeKerlog", "CodeKerlog"},
            {"CodeMkgt", "CodeMkgt"},
            {"CodeGpi", "CodeGpi"},
            {"IdOdoo", "IdOdoo"},
            {"IdDashdoc", "IdDashdoc"},
            {"Nom", "Nom"},
            {"Ville", "VilleFacturation" },
            {"NDCover", "NDCover"}
        };
        
        if (!string.IsNullOrWhiteSpace(filter.FiltredBySiret))
        {
            query = query.Where(x => x.Siret.Contains(filter.FiltredBySiret));
        }
        if (!string.IsNullOrWhiteSpace(filter.FiltredByCodeKerlog))
        {
            query = query.Where(x => x.CodeKerlog.Contains(filter.FiltredByCodeKerlog));
        }
        if (!string.IsNullOrWhiteSpace(filter.FiltredByCodeMkgt))
        {
            query = query.Where(x => x.CodeMkgt.Contains(filter.FiltredByCodeMkgt));
        }
        if (!string.IsNullOrWhiteSpace(filter.FiltredByIdOdoo))
        {
            query = query.Where(x => x.IdOdoo.Contains(filter.FiltredByIdOdoo));
        }
        if (!string.IsNullOrWhiteSpace(filter.FiltredByNom))
        {
            query = query.Where(x => x.Nom.Contains(filter.FiltredByNom));
        }
        if (!string.IsNullOrWhiteSpace(filter.FiltredByCodeGpi))
        {
            query = query.Where(x => x.CodeGpi.Contains(filter.FiltredByCodeGpi));
        }
        if (!string.IsNullOrWhiteSpace(filter.FilteredByIdDashdoc))
        {
            query = query.Where(x => x.IdDashdoc.ToString().Contains(filter.FilteredByIdDashdoc));
        }
        if (!string.IsNullOrWhiteSpace(filter.BadMail))
        {
            query = query.Where(x => x.EmailFacturation == null);
        }
        if (!string.IsNullOrWhiteSpace(filter.BadTel))
        {
            query = query.Where(x => x.TelephoneFacturation == null);
        }
        if (!string.IsNullOrWhiteSpace(filter.Factor))
        {
            query = query.Where(x => x.ConditionReglement == 2);
        }
        if (!string.IsNullOrWhiteSpace(filter.Radie))
        {
            query = query.Where(x => x.Radie == true);
        }
        if (filter.SortBy == "NDCover")
        {
            if (filter.OrderBy.ToLower() == "asc")
            {
                query = query.OrderBy(x => x.EntrepriseBase.EntrepriseNDCover == null ? 1 : 0)
                    .ThenBy(x => x.EntrepriseBase.EntrepriseNDCover != null && x.EntrepriseBase.EntrepriseNDCover.Statut == "Garantie totale" ? 1 : 0);
            }
            else if (filter.OrderBy.ToLower() == "desc")
            {
                query = query.OrderBy(x => x.EntrepriseBase.EntrepriseNDCover != null && x.EntrepriseBase.EntrepriseNDCover.Statut == "Garantie totale" ? 0 : 1)
                    .ThenBy(x => x.EntrepriseBase.EntrepriseNDCover == null ? 0 : 1);
            }
        }
        else if (!string.IsNullOrWhiteSpace(filter.SortBy) && mapDictionary.ContainsKey(filter.SortBy))
        {
            query = query.OrderByDynamic(mapDictionary[filter.SortBy], filter.OrderBy == "desc");
        }
        else
        {
            query = query.OrderByDescending(obj => obj.Siret);
        }

        return query;
    }
}