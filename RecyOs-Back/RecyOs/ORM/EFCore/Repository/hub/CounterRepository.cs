using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using RecyOs.Helpers;
using RecyOs.ORM.Entities;
using RecyOs.ORM.Entities.hub;
using RecyOs.ORM.Interfaces.hub;

namespace RecyOs.ORM.EFCore.Repository.hub;

/// <summary>
/// Represents a repository for managing counter entities.
/// </summary>
public class CounterRepository: BaseRepository<Counter, DataContext>, ICounterRepository<Counter>
{
    
    public CounterRepository(DataContext context) : base(context)
    {
    }

    /// <summary>
    /// Retrieves a counter entity with the given ID from the database.
    /// If the counter does not exist, null is returned.
    /// </summary>
    /// <param name="id">The ID of the counter to retrieve.</param>
    /// <param name="session">The session context.</param>
    /// <returns>The counter entity with the given ID or null if not found.</returns>
    public async Task<Counter> GetCounterById(int id, ContextSession session)
    {
        return await Get(id, session);
    }

    /// <summary>
    /// Retrieves a counter entity with the given name from the database.
    /// If the counter does not exist, null is returned.
    /// </summary>
    /// <param name="name">The name of the counter to retrieve.</param>
    /// <param name="session">The session context.</param>
    /// <returns>The counter entity with the given name or null if not found.</returns>
    public async Task<Counter> GetCounterByName(string name, ContextSession session)
    {
        return await GetEntities(session).Where( i => i.Name == name).AsNoTracking().FirstOrDefaultAsync();
    }

    /// <summary>
    /// Increments the value of a counter with the given name. If the counter does not exist, a new counter is created with the initial value of 1.
    /// </summary>
    /// <param name="name">The name of the counter.</param>
    /// <param name="session">The session context.</param>
    /// <returns>The updated counter.</returns>
    public async Task<Counter> IncrementCounterByName(string name, ContextSession session)
    {
        var counter = await GetCounterByName(name, session);
        if (counter == null)
        {
            counter = new Counter
            {
                Name = name,
                Value = 1,
                Description = "Counter for " + name
            };
            await Edit(counter, session);
        }
        else
        {
            
            counter.Value++;
            await Edit(counter, session);
        }
        return counter;
    }
    
}