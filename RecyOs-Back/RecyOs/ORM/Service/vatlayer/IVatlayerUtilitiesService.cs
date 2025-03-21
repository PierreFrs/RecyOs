using System.Threading.Tasks;
using RecyOs.ORM.DTO.hub;

namespace RecyOs.ORM.Service.vatlayer;

public interface IVatlayerUtilitiesService
{
    bool CheckVatNumber(string vat);
    Task<ClientEuropeDto> CreateClientEurope(string vat, bool client, bool prospect, bool checkVat);
} 