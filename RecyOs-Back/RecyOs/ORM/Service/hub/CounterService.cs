using System.Threading.Tasks;
using RecyOs.ORM.Entities.hub;
using RecyOs.ORM.Interfaces.hub;
using AutoMapper;
using RecyOs.ORM.DTO.hub;
using RecyOs.ORM.Interfaces;

namespace RecyOs.ORM.Service.hub;

/// <summary>
/// Represents a counter service.
/// </summary>
/// <typeparam name="TCounter">The type of counter entity.</typeparam>
public class CounterService<TCounter> : BaseService, ICounterService where TCounter : Counter, new()
{
    private readonly ICounterRepository<TCounter> _repository;
    private readonly IMapper _mapper;

    /// <summary>
    /// Represents a counter service.
    /// </summary>
    /// <param name="contextProvider"></param>
    /// <param name="counterRepository"></param>
    /// <param name="mapper"></param>
    public CounterService(ICurrentContextProvider contextProvider, 
        ICounterRepository<TCounter> counterRepository, IMapper mapper) : base(contextProvider)
    {
        _repository = counterRepository;
        _mapper = mapper;
    }

    /// <summary>
    /// Retrieves a counter by its ID.
    /// </summary>
    /// <param name="id">The ID of the counter.</param>
    /// <returns>The counter object with the specified ID.</returns>
    public async Task<CounterDto> GetCounterById(int id)
    {
        var counter = await _repository.GetCounterById(id, Session);
        return _mapper.Map<CounterDto>(counter);
    }

    /// <summary>
    /// Retrieves a counter by its name.
    /// </summary>
    /// <param name="name">The name of the counter.</param>
    /// <returns>The counter object with the specified name.</returns>
    public async Task<CounterDto> GetCounterByName(string name)
    {
        var counter = await _repository.GetCounterByName(name, Session);
        return _mapper.Map<CounterDto>(counter);
    }

    /// <summary>
    /// Increments the counter value by name.
    /// </summary>
    /// <param name="name">The name of the counter.</param>
    /// <returns>The updated counter value.</returns>
    public async Task<CounterDto> IncrementCounterByName(string name)
    {
        var counter = await _repository.IncrementCounterByName(name, Session);
        return _mapper.Map<CounterDto>(counter);
    }
}