using System.Collections.Generic;
using System.Threading.Tasks;
using RecyOs.MKGT_DB.Entities;

namespace RecyOs.MKGT_DB.Interfaces;

public interface IFCliRepository<TFCli> where TFCli : Fcli, new()
{
    Task<TFCli> CreFac(TFCli prmObj);
    Task<TFCli> UpFac(TFCli prmObj);
    Task<IList<Fcli>> GetValidsFcli();
    Task<Fcli> GetByCode(string prmCode, bool includeDeleted = false);
}