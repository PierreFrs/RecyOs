using System.Threading.Tasks;
using RecyOs.ORM.Entities;
using RecyOs.ORM.Entities.hub;

namespace RecyOs.ORM.Interfaces.hub;

public interface ICounterRepository<TCounter> where TCounter : Counter
{
    Task<TCounter> GetCounterById(int id, ContextSession session);
    Task<TCounter> GetCounterByName(string name, ContextSession session);
    Task<TCounter> IncrementCounterByName(string name, ContextSession session);
}