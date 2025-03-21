using System;
using System.Threading.Tasks;

namespace RecyOs.ORM.Interfaces;

public interface IPappersApiService
{
    public Task<Boolean> ObtenirDonneesAsync(string endPoint, string numeroSirenOuSiret);
    public Task<string> GetEtablissement(string siret);

}