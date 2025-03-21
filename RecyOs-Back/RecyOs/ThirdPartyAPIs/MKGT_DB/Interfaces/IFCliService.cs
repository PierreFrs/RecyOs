using System.Collections.Generic;
using System.Threading.Tasks;
using RecyOs.Engine.Modules.Mkgt;
using RecyOs.ORM.Entities.hub;

namespace RecyOs.MKGT_DB.Interfaces;

public interface IFCliService
{
    public Task<EtablissementMkgtDto> InsertEtablissementClient(EtablissementMkgtDto etablissementClient);
    public Task<EtablissementMkgtDto> UpdateEtablissementClient(EtablissementMkgtDto etablissementMkgtDto);
    public Task<List<EtablissementMkgtDto>> GetValidsClients();
    public Task<EtablissementMkgtDto> GetClient(string code);
}