using System.Threading.Tasks;
using RecyOs.ORM.DTO.hub;

namespace RecyOs.ORM.Interfaces;

public interface IPappersUtilitiesService
{
    bool CheckSiret(string siret);
    Task<EtablissementClientDto> CreateEtablissementClientBySiret(string siret, bool estClient, bool estFournisseur, bool disableTracking = false);
    #nullable enable
    Task<bool?> UpdateEtablissementClientBySiret(string siret);
    Task<EtablissementFicheDto?> CreateEntrepriseBySiret(string siret);
    #nullable disable
    
}