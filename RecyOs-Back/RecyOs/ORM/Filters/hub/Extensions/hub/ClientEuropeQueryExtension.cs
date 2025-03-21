using System.Collections.Generic;
using System.Linq;
using RecyOs.ORM.Entities.hub;
using RecyOs.ORM.Filters.hub;

namespace RecyOs.ORM.Filters.Extensions.hub;

public static class ClientEuropeQueryExtension
{
    public static IQueryable<ClientEurope> ApplyFilter<TClientEurope>(
        this IQueryable<TClientEurope> query, ClientEuropeGridFilter filter)
        where TClientEurope : ClientEurope, new()
    {
        // map dictionary for EtablissementClient object
        var mapDictionary = new Dictionary<string, string>
        {
            {"Vat", "Vat"},
            {"CodeKerlog", "CodeKerlog"},
            {"CodeMkgt", "CodeMkgt"},
            {"IdOdoo", "IdOdoo"},
            {"CodeGpi", "CodeGpi"},
            {"IdDashdoc", "IdDashdoc"},
            {"nom", "Nom"},
            {"ville", "VilleFacturation" }
        };

        if (!string.IsNullOrWhiteSpace(filter.FiltredByVat))
        {
            query = query.Where(x => x.Vat.Contains(filter.FiltredByVat));
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
        if (!string.IsNullOrWhiteSpace(filter.SortBy) && !string.IsNullOrWhiteSpace(filter.OrderBy))
        {
            query = query.OrderByDynamic(mapDictionary[filter.SortBy], filter.OrderBy == "desc");
        }
        else
        {
            query = query.OrderByDescending(obj => obj.Vat);
        }
        return query;
    }
}