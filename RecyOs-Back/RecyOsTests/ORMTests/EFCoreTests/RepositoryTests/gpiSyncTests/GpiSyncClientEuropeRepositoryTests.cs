using RecyOs.Helpers;
using RecyOs.ORM.EFCore.Repository.gpiSync;
using RecyOs.ORM.Entities;
using RecyOsTests.Interfaces;

namespace RecyOsTests.ORMTests.EFCoreTests.RepositoryTests.gpiSyncTests;

public class GpiSyncClientEuropeRepositoryTests
{
    private readonly DataContext _dataContext;
    
    public GpiSyncClientEuropeRepositoryTests(IEngineDataContextTests prmEngineDataContextTests)
    {
        _dataContext = prmEngineDataContextTests.GetContext();

    }
    
    [Fact]
    public async Task GetCreatedClientEurope_ShouldReturnListOfObjects()
    {
        /// Step I - Ensure module is correctly configured 
        // Arrange
        var repository = new GpiSyncClientEuropeRepository(_dataContext);
        // Act
        var result = await repository.GetCreatedClientEurope(new ContextSession());
        // Assert
        Assert.NotNull(result);
        
        /// Step II reward LastCreate 2 hours should return Client2
        // Arrange
        _dataContext.EngineSyncStatus.Where(ess => ess.ModuleName.Equals("GpiEuropeClientModule"))
            .FirstOrDefault().LastCreate = DateTime.Now - TimeSpan.FromHours(2);     // now - 2 hours
        await _dataContext.SaveChangesAsync();
        // Act
        result = await repository.GetCreatedClientEurope(new ContextSession());
        // Assert res collection contain element with nom Client2
        Assert.NotNull(result);
        Assert.Single(result);
        Assert.Contains(result, c => c.Nom == "Client5");
        Assert.Contains(result, c => c.CodeGpi == "C0005");
        
        /// Step III New call should return 0 items
        // Act
        result = await repository.GetCreatedClientEurope(new ContextSession());
        // Assert
        Assert.NotNull(result);
        Assert.Empty(result);
    }
    
    [Fact]
    public async Task GetUpdatedClientEurope_ShouldReturnListOfObjects()
    {
        /// Step I - Ensure module is correctly configured 
        // Arrange
        var repository = new GpiSyncClientEuropeRepository(_dataContext);
        // Act
        var result = await repository.GetUpdatedClientEurope(new ContextSession());
        // Assert
        Assert.NotNull(result);
        
        /// Step II reward LastUpdate 2 hours should return Client2
        // Arrange
        _dataContext.EngineSyncStatus.Where(ess => ess.ModuleName.Equals("GpiEuropeClientModule"))
            .FirstOrDefault().LastUpdate = DateTime.Now - TimeSpan.FromHours(2);     // now - 2 hours
        await _dataContext.SaveChangesAsync();
        // Act
        result = await repository.GetUpdatedClientEurope(new ContextSession());
        // Assert res collection contain element with nom Client2
        Assert.NotNull(result);
        Assert.Single(result);
        Assert.Contains(result, c => c.Nom == "Client6");
        Assert.Contains(result, c => c.CodeGpi == "C0006");
        
        /// Step III New call should return 0 items
        // Act
        result = await repository.GetUpdatedClientEurope(new ContextSession());
        // Assert
        Assert.NotNull(result);
        Assert.Empty(result);
    }
}