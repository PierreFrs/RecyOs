using System.Collections.Generic;
using RecyOs.ORM.Entities.hub;
using System.Threading.Tasks;
using RecyOs.ORM.Entities;
using RecyOs.ORM.Filters.hub;

namespace RecyOs.ORM.Interfaces.hub;

public interface IClientEuropeRepository<TClientEurope> : IEuropeRepository<TClientEurope> 
    where TClientEurope : ClientEurope
{
}