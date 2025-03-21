using Microsoft.AspNetCore.Http;
using Moq;
using RecyOs.Engine.Interfaces;
using RecyOs.Helpers;
using RecyOs.ORM.EFCore.Repository.gpiSync;
using RecyOs.ORM.Entities;
using RecyOsTests.Interfaces;

namespace RecyOsTests.ORMTests.EFCoreTests.RepositoryTests.gpiSyncTests;

public class GpiSyncEtablissementClientRepositoryTests
{
    private readonly DataContext _dataContext;
    
    public GpiSyncEtablissementClientRepositoryTests(IEngineDataContextTests prmEngineDataContextTests)
    {
        _dataContext = prmEngineDataContextTests.GetContext();

    }
    
    [Fact]
    public async Task GetCreatedEtablissementClient_ShouldReturnListOfObjects()
    {
        /// Step I - Ensure module is correctly configured 
        // Arrange
        var repository = new GpiSyncEtablissementClientRepository(_dataContext);
        // Act
        var result = await repository.GetCreatedEtablissementClient(new ContextSession());
        // Assert
        Assert.NotNull(result);
        
        /// Step II reward LastCreate 2 hours should return Client2
        // Arrange
        _dataContext.EngineSyncStatus.Where(ess => ess.ModuleName.Equals("GpiClientModule"))
            .FirstOrDefault().LastCreate = DateTime.Now - TimeSpan.FromHours(2);     // now - 2 hours
        await _dataContext.SaveChangesAsync();
        // Act
        result = await repository.GetCreatedEtablissementClient(new ContextSession());
        // Assert res collection contain element with nom Client2
        Assert.NotNull(result);
        Assert.Single(result);
        Assert.Contains(result, c => c.CodeGpi == "C00002");
        Assert.NotNull(result[0].EntrepriseBase);
        
        /// Step III New call should return 0 items
        // Act
        result = await repository.GetCreatedEtablissementClient(new ContextSession());
        // Assert
        Assert.NotNull(result);
        Assert.Empty(result);
    }
    
    [Fact]
    public async Task GetUpdatedEtablissementClient_ShouldReturnListOfObjects()
    {
        /// Step I - Ensure module is correctly configured 
        // Arrange
        var repository = new GpiSyncEtablissementClientRepository(_dataContext);
        // Act
        var result = await repository.GetUpdatedEtablissementClient(new ContextSession());
        // Assert
        Assert.NotNull(result);
        
        /// Step II reward LastUpdate 2 hours should return Client1
        // Arrange 
        _dataContext.EngineSyncStatus.Where(ess => ess.ModuleName.Equals("GpiClientModule"))
            .FirstOrDefault().LastUpdate = DateTime.Now - TimeSpan.FromHours(2);     // now - 2 hours
        await _dataContext.SaveChangesAsync();
        // Act
        result = await repository.GetUpdatedEtablissementClient(new ContextSession());
        // Assert res collection contain element with nom Client1
        Assert.NotNull(result);
        Assert.Single(result);
        Assert.Contains(result, c => c.CodeGpi == "C00003");
        Assert.NotNull(result[0].EntrepriseBase);
        
        /// Step III New call should return 0 items
        // Act
        result = await repository.GetUpdatedEtablissementClient(new ContextSession());
        // Assert
        Assert.NotNull(result);
        Assert.Empty(result);
    }
}