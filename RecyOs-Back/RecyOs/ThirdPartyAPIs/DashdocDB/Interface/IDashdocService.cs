// IDashdocService.cs -
// ======================================================================0
// Crée par : pfraisse
// Fichier Crée le : 16/09/2024
// Fichier Modifié le : 16/09/2024
// Code développé pour le projet : RecyOs

using System.Threading.Tasks;
using RecyOs.ThirdPartyAPIs.DashdocDB.DTO;

namespace RecyOs.ThirdPartyAPIs.DashdocDB.Interface;

public interface IDashdocService
{
    Task<DashdocCompanyDto> CreateDashdocCompanyAsync(DashdocCompanyDto dashdocCompanyDto);
    
    Task<DashdocCompanyDto> UpdateDashdocCompanyAsync(DashdocCompanyDto dashdocCompanyDto);
}