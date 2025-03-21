using RecyOs.ORM.Interfaces.hub.BalanceInterfaces;

namespace RecyOs.OdooImporter.Interfaces;

public interface IBalanceRepository<TBalance> where TBalance : IClientBalance, new()
{
     Task<TBalance> SaveBalance(int clientId, int societeId, decimal balance); 
}
