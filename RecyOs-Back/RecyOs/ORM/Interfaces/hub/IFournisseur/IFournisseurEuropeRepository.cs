using RecyOs.ORM.Entities.hub;

namespace RecyOs.ORM.Interfaces.hub;

public interface IFournisseurEuropeRepository<TFournisseurEurope> : IEuropeRepository<TFournisseurEurope> 
    where TFournisseurEurope : ClientEurope
{
}