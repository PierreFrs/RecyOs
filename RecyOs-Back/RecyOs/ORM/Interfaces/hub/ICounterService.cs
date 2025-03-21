using System.Threading.Tasks;
using RecyOs.ORM.DTO.hub;

namespace RecyOs.ORM.Interfaces.hub;

public interface ICounterService
{
    Task<CounterDto> GetCounterById(int id);
    Task<CounterDto> GetCounterByName(string name);
    Task<CounterDto> IncrementCounterByName(string name);
}