using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using RecyOs.Engine.Interfaces;
using RecyOs.ORM.Entities.hub;
using RecyOs.ORM.Hub.DTO;
using RecyOs.ORM.Interfaces;
using RecyOs.ORM.Interfaces.gpiSync;

namespace RecyOs.ORM.Service.gpiSync;

public class GpiSyncService : BaseService, IGpiSyncService
{
    private readonly IGpiSyncClientEuropeRepository<ClientEurope> _gpiSyncClientEuropeRepository;
    private readonly IGpiSyncEtablissementClientRepository<EtablissementClient> _gpiSyncEtablissementClientsRepository;
   
    
    public GpiSyncService(ICurrentContextProvider contextProvider, 
        IGpiSyncClientEuropeRepository<ClientEurope> gpiSyncClientEuropeRepository, 
        IGpiSyncEtablissementClientRepository<EtablissementClient> gpiSyncEtablissementClientsRepository)
            :base(contextProvider)
    {
        _gpiSyncClientEuropeRepository = gpiSyncClientEuropeRepository;
        _gpiSyncEtablissementClientsRepository = gpiSyncEtablissementClientsRepository;
    }
    
    public async Task<IList<ClientGpiDto>> GetChangedCustomers()
    {
        IList<ClientGpiDto> clientGpi = new List<ClientGpiDto>();
        var createdClientEurope = await _gpiSyncClientEuropeRepository.GetCreatedClientEurope(Session);
        var updatedClientEurope = await _gpiSyncClientEuropeRepository.GetUpdatedClientEurope(Session);
        var createdEtablissementClient = await _gpiSyncEtablissementClientsRepository.GetCreatedEtablissementClient(Session);
        var updatedEtablissementClient = await _gpiSyncEtablissementClientsRepository.GetUpdatedEtablissementClient(Session);

        createdClientEurope = createdClientEurope.OrderBy(p => p.CodeGpi).ToList();
        updatedClientEurope = updatedClientEurope.OrderBy(p => p.CodeGpi).ToList();
        createdEtablissementClient = createdEtablissementClient.OrderBy(p => p.CodeGpi).ToList();
        updatedEtablissementClient = updatedEtablissementClient.OrderBy(p => p.CodeGpi).ToList();
        
        foreach (var client in createdClientEurope)
        {
            if(client.CodeGpi != null && client.CodeGpi != "") clientGpi.Add(new ClientGpiDto(client,"CRE", "Client"));
            if (client.FrnCodeGpi != null && client.FrnCodeGpi != "") clientGpi.Add(new ClientGpiDto(client,"CRE", "Fournisseur"));
        }
        
        foreach (var etab in createdEtablissementClient)
        {
            if(etab.CodeGpi != null && etab.CodeGpi != "")clientGpi.Add(new ClientGpiDto(etab,"CRE", "Client"));
            if (etab.FrnCodeGpi != null && etab.FrnCodeGpi != "") clientGpi.Add(new ClientGpiDto(etab,"CRE", "Fournisseur"));
        }
        
        clientGpi = clientGpi.OrderBy(p => p.Code).ToList();
        
        foreach (var client in updatedClientEurope)
        {
            if(client.CodeGpi != null && client.CodeGpi != "") clientGpi.Add(new ClientGpiDto(client,"UPD", "Client"));
            if (client.FrnCodeGpi != null && client.FrnCodeGpi != "") clientGpi.Add(new ClientGpiDto(client,"UPD", "Fournisseur"));
        }
        
        foreach (var etab in updatedEtablissementClient)
        {
            if(etab.CodeGpi != null && etab.CodeGpi != "")clientGpi.Add(new ClientGpiDto(etab,"UPD", "Client"));
            if (etab.FrnCodeGpi != null && etab.FrnCodeGpi != "") clientGpi.Add(new ClientGpiDto(etab,"UPD", "Fournisseur"));
        }
        return clientGpi;
    }
}