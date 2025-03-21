using Microsoft.Extensions.Configuration;
using Moq;
using RecyOs.Engine.Interfaces;
using RecyOs.Engine.Repository;
using RecyOs.Helpers;
using RecyOs.ORM.Entities.hub;
using RecyOsTests.Interfaces;
using RecyOsTests.Services;

namespace RecyOsTests.EngineTests.Repository;

public class EngineEuropeClientRepositoryTests
{
    private readonly Mock<IDataContextEngine> _mockIDataContextEngine;
    private readonly Mock<IDataContextEngine> _mockIsolatedDataContextEngine;
    private readonly DataContext _dataContext;
    private readonly DataContext _isolatedDataContext;

    public EngineEuropeClientRepositoryTests(IEngineDataContextTests objEngineDataContextTests)
    {
        _mockIDataContextEngine = new Mock<IDataContextEngine>();
        _dataContext = objEngineDataContextTests.GetContext();
        _mockIDataContextEngine.Setup(x => x.GetContext()).Returns(_dataContext);
        _mockIsolatedDataContextEngine = new Mock<IDataContextEngine>();
        _isolatedDataContext = new EngineDataContextTests().GetContext();
        _mockIsolatedDataContextEngine.Setup(m => m.GetContext()).Returns(_isolatedDataContext);
    }

    /// <summary>
    /// The GetCreatedEntities_WithNotExistModule_ShouldReturnNull method returns null when called with a non-existent module name.
    /// </summary>
    /// <remarks>
    /// This method is a unit test that checks the behavior of the GetCreatedEntities method in the EngineEuropeClientRepository class.
    /// The test verifies that the method correctly returns null when called with a module name that does not exist.
    /// The test uses the Xunit Fact attribute to indicate that it is a test method.
    /// </remarks>
    [Fact]
    public void GetCreatedEntities_WithNotExistModule_ShouldReturnNull()
    {
        // Arrange
        var repository = new EngineEuropeClientRepository(_mockIDataContextEngine.Object);
        // Act
        var res =  repository.GetCreatedEntities("NotExistModule");
        // Assert no items
        Assert.Null(res);
    }


    /// <summary>
    /// Returns the created entities for the specified module, using the MkgtEuropeClientModule configuration.
    /// </summary>
    /// <returns>A collection of created entities for the given module.</returns>
    [Fact]
    public void GetCreatedEntities_WithMkgtEuropeClientModule_ShouldReturnNotNull()
    {
        /// Step I - Ensure module is correctly configured 
        // Arrange
        var repository = new EngineEuropeClientRepository(_mockIDataContextEngine.Object);
        // Act
        var res = repository.GetCreatedEntities("MkgtEuropeClientModule");
        // Assert
        Assert.NotNull(res);
        
        /// Step II reward LastCreate 2 hours should return Client1
        // Arrange 
        _dataContext.EngineSyncStatus.Where(ess => ess.ModuleName.Equals("MkgtEuropeClientModule"))
            .FirstOrDefault().LastCreate = DateTime.Now - TimeSpan.FromHours(2);     // now - 2 hours
        _dataContext.SaveChanges();
        // Act
        res = repository.GetCreatedEntities("MkgtEuropeClientModule");
        // Assert res collection contain element with nom Client1
        Assert.NotNull(res);
        Assert.Single(res);
        Assert.Contains(res, c => c.Nom == "Client1");
        
        /// Step III New call should return 0 items
        // Act
        res = repository.GetCreatedEntities("MkgtEuropeClientModule");
        // Assert
        Assert.NotNull(res);
        Assert.Empty(res);
    }

    /// <summary>
    /// Unit test method to verify that the GetCreatedEntities method returns when the OdooEuropeClientModule is used. </summary>
    /// <remarks>
    /// The GetCreatedEntities method should return a collection of created entities for the specified module.
    /// Odoo creations is not writen by engine so it need to return null
    /// </remarks>
    /// <returns>Void</returns>
    [Fact]
    public void GetCreatedEntities_WithOdooEuropeClientModule_ShouldReturnNull()
    {
        // Arrange
        var repository = new EngineEuropeClientRepository(_mockIDataContextEngine.Object);
        // Act
        var res = repository.GetCreatedEntities("OdooEuropeClientModule");
        // Assert
        Assert.Null(res);
    }

    [Fact]
    public void GetCreatedEntities_WithGpiEuropeClientModule_ShouldReturnNotNull()
    {
        /// Step I - Ensure module is correctly configured
        // Arrange
        var repository = new EngineEuropeClientRepository(_mockIDataContextEngine.Object);
        // Act
        var res = repository.GetCreatedEntities("GpiEuropeClientModule");
        // Assert
        Assert.NotNull(res);
        
        /// Step II reward LastCreate 2 hours should return Client2
        // Arrange
        _dataContext.EngineSyncStatus.Where(ess => ess.ModuleName.Equals("GpiEuropeClientModule"))
            .FirstOrDefault().LastCreate = DateTime.Now - TimeSpan.FromHours(2);     // now - 2 hours
        _dataContext.SaveChanges();
        // Act
        res = repository.GetCreatedEntities("GpiEuropeClientModule");
        // Assert res collection contain element with nom Client5
        Assert.NotNull(res);
        Assert.Single(res);
        Assert.Contains(res, c => c.Nom == "Client5");
        
        /// Step III New call should return 0 items
        // Act
        res = repository.GetCreatedEntities("GpiEuropeClientModule");
        // Assert
        Assert.NotNull(res);
        Assert.Empty(res);
    }
    
    [Fact]
    public void GetCreatedEntities_WithMkgtShipperEuropeClientModule_ShouldReturnNotNull()
    {
        /// Step I - Ensure module is correctly configured
        // Arrange
        var repository = new EngineEuropeClientRepository(_mockIsolatedDataContextEngine.Object);
        // Act
        var res = repository.GetCreatedEntities("MkgtShipperEuropeClientModule");
        // Assert
        Assert.NotNull(res);
        
        /// Step II reward LastCreate 2 hours should return Client1
        // Arrange
        _dataContext.EngineSyncStatus.Where(ess => ess.ModuleName.Equals("MkgtShipperEuropeClientModule"))
            .FirstOrDefault().LastCreate = DateTime.Now - TimeSpan.FromHours(2);     // now - 2 hours
        _dataContext.SaveChanges();
        // Act
        res = repository.GetCreatedEntities("MkgtShipperEuropeClientModule");
        // Assert res collection contain element with nom Client2
        Assert.NotNull(res);
        Assert.Single(res);
        Assert.Contains(res, c => c.Nom == "Client1");
        
        /// Step III New call should return the same
        // Act
        res = repository.GetCreatedEntities("MkgtShipperEuropeClientModule");
        // Assert
        Assert.NotNull(res);
        Assert.Single(res);
        Assert.Contains(res, c => c.Nom == "Client1");

    }

    /// <summary>
    /// Tests the behavior of the GetUpdatedEntities method when the specified module does not exist.
    /// </summary>
    /// <remarks>
    /// This test verifies that the GetUpdatedEntities method returns null when attempting to retrieve entities from a non-existent module.
    /// </remarks>
    [Fact]
    public void GetUpdatedEntities_WithNotExistModule_ShouldReturnNull()
    {
        // Arrange
        var repository = new EngineEuropeClientRepository(_mockIDataContextEngine.Object);
        // Act
        var res = repository.GetUpdatedEntities("NotExistModule");
        // Assert
        Assert.Null(res);
    }

    [Fact]
    public void GetUpdatedEntities_WithMkgtEuropeClientModule_ShouldReturnNotNull()
    {
        /// Step I - Ensure module is correctly configured
        // Arrange
        var repository = new EngineEuropeClientRepository(_mockIDataContextEngine.Object);
        // Act
        var res = repository.GetUpdatedEntities("MkgtEuropeClientModule");
        // Assert
        Assert.NotNull(res);
        Assert.Empty(res);
        
        /// Step II reward LastUpdate 2 hours should return Client1
        // Arrange 
        _dataContext.EngineSyncStatus.Where(ess => ess.ModuleName.Equals("MkgtEuropeClientModule"))
            .FirstOrDefault()
            .LastUpdate = DateTime.Now - TimeSpan.FromHours(2); // now - 2 hours
        _dataContext.ClientEurope.Where(x => x.Id == 1)
            .FirstOrDefault()
            .UpdatedAt = DateTime.Now; // now
        _dataContext.SaveChanges();
       
        // Act
        res = repository.GetUpdatedEntities("MkgtEuropeClientModule");
        // Assert res collection contain element with nom Client1
        Assert.NotNull(res);
        Assert.Single(res);
        Assert.Contains(res, c => c.Nom == "Client1");
        
        /// Step III New call should return 0 items
        // Act
        res = repository.GetUpdatedEntities("MkgtEuropeClientModule");
        // Assert
        Assert.NotNull(res);
        Assert.Empty(res);
    }

    [Fact]
    public void GetUpdatedEntities_WithOdooEuropeClientModule_ShouldReturnNotNull()
    {
        var lmockIsolatedDataContextEngine = new Mock<IDataContextEngine>();
        var lisolatedDataContext = new EngineDataContextTests().GetContext();
        lmockIsolatedDataContextEngine.Setup(m => m.GetContext()).Returns(lisolatedDataContext);
        
        /// Step I - Ensure module is correctly configured
        // Arrange
        var repository = new EngineEuropeClientRepository(lmockIsolatedDataContextEngine.Object);
        // Act
        var res = repository.GetUpdatedEntities("OdooEuropeClientModule");
        // Assert
        Assert.NotNull(res);
        Assert.Empty(res);
        
        /// Step II reward LastUpdate 2 hours should return Client 2
        // Arrange 
        lisolatedDataContext.EngineSyncStatus.Where(ess => ess.ModuleName.Equals("OdooEuropeClientModule"))
            .FirstOrDefault()
            .LastUpdate = DateTime.Now - TimeSpan.FromHours(2); // now - 2 hours
        lisolatedDataContext.ClientEurope.Where(x => x.Id == 2)
            .FirstOrDefault()
            .UpdatedAt = DateTime.Now - TimeSpan.FromHours(1); // now
        lisolatedDataContext.SaveChanges();
        // Act
        res = repository.GetUpdatedEntities("OdooEuropeClientModule");
        // Assert res collection contain element with nom Client2
        Assert.NotNull(res);
        Assert.Single(res);
        Assert.Contains(res, c => c.Nom == "Client2");
        
        /// Step III New call should return 0 items
        // Act
        res = repository.GetUpdatedEntities("OdooEuropeClientModule");
        // Assert
        Assert.NotNull(res);
        Assert.Empty(res);
    }

    [Fact]
    public void GetUpdatedEntities_WithGpiEuropeClientModule_ShouldReturnNotNull()
    {
        /// Step I - Ensure module is correctly configured
        // Arrange
        var repository = new EngineEuropeClientRepository(_mockIDataContextEngine.Object);
        // Act
        var res = repository.GetUpdatedEntities("GpiEuropeClientModule");
        // Assert
        Assert.NotNull(res);
        
        /// Step II reward LastUpdate 2 hours should return Client2
        // Arrange 
        _dataContext.EngineSyncStatus.Where(ess => ess.ModuleName.Equals("GpiEuropeClientModule"))
            .FirstOrDefault()
            .LastUpdate = DateTime.Now - TimeSpan.FromHours(2); // now - 2 hours
        _dataContext.ClientEurope.Where(x => x.Id == 6)
            .FirstOrDefault()
            .UpdatedAt = DateTime.Now; // now
        _dataContext.SaveChanges();
        // Act
        res = repository.GetUpdatedEntities("GpiEuropeClientModule");
        // Assert res collection contain element with nom Client2
        Assert.NotNull(res);
        Assert.Single(res);
        Assert.Contains(res, c => c.Nom == "Client6");
        
        /// Step III New call should return 0 items
        // Act
        res = repository.GetUpdatedEntities("GpiEuropeClientModule");
        // Assert
        Assert.NotNull(res);
        Assert.Empty(res);
    }
    
    [Fact]
    public void GetUpdatedEntities_WithHubSpotEuropeClientModule_ShouldReturnNotNull()
    {
        /// Step I - Ensure module is correctly configured
        // Arrange
        var repository = new EngineEuropeClientRepository(_mockIDataContextEngine.Object);
        // Act
        var res = repository.GetUpdatedEntities("HubSpotEuropeClientModule");
        // Assert
        Assert.NotNull(res);
        
        /// Step II reward LastUpdate 2 hours should return Client2
        // Arrange 
        _dataContext.EngineSyncStatus.Where(ess => ess.ModuleName.Equals("HubSpotEuropeClientModule"))
            .FirstOrDefault()
            .LastUpdate = DateTime.Now - TimeSpan.FromHours(2); // now - 2 hours
        _dataContext.ClientEurope.Where(x => x.Id == 3)
            .FirstOrDefault()
            .UpdatedAt = DateTime.Now; // now
        _dataContext.SaveChanges();
        // Act
        res = repository.GetUpdatedEntities("HubSpotEuropeClientModule");
        // Assert res collection contain element with nom Client3
        Assert.NotNull(res);
        Assert.Single(res);
        Assert.Contains(res, c => c.Nom == "Client3");
        
        /// Step III New call should return 0 items
        // Act
        res = repository.GetUpdatedEntities("HubSpotEuropeClientModule");
        // Assert
        Assert.NotNull(res);
        Assert.Empty(res);
    }
    
    [Fact]
    public void GetUpdatedEntities_WithDashDocEuropeClientModule_ShouldReturnNotNull()
    {
        /// Step I - Ensure module is correctly configured
        // Arrange
        var repository = new EngineEuropeClientRepository(_mockIDataContextEngine.Object);
        // Act
        var res = repository.GetUpdatedEntities("DashDocEuropeClientModule");
        // Assert
        Assert.NotNull(res);
        
        /// Step II reward LastUpdate 2 hours should return Client2
        // Arrange 
        _dataContext.EngineSyncStatus.Where(ess => ess.ModuleName.Equals("DashDocEuropeClientModule"))
            .FirstOrDefault()
            .LastUpdate = DateTime.Now - TimeSpan.FromHours(2); // now - 2 hours
        _dataContext.ClientEurope.Where(x => x.Id == 4)
            .FirstOrDefault()
            .UpdatedAt = DateTime.Now; // now
        _dataContext.SaveChanges();
        // Act
        res = repository.GetUpdatedEntities("DashDocEuropeClientModule");
        // Assert res collection contain element with nom Client4
        Assert.NotNull(res);
        Assert.Single(res);
        Assert.Contains(res, c => c.Nom == "Client4");
        
        /// Step III New call should return 0 items
        // Act
        res = repository.GetUpdatedEntities("DashDocEuropeClientModule");
        // Assert
        Assert.NotNull(res);
        Assert.Empty(res);
    }
    
    [Fact]
    public void GetUpdatedEntities_WithMkgtShipperEuropeClientModule_ShouldReturnNotNull()
    {
        /// Step I - Ensure module is correctly configured
        // Arrange
        var repository = new EngineEuropeClientRepository(_mockIDataContextEngine.Object);
        // Act
        var res = repository.GetUpdatedEntities("MkgtShipperEuropeClientModule");
        // Assert
        Assert.NotNull(res);
        
        /// Step II reward LastUpdate 2 hours should return Client3
        // Arrange 
        _dataContext.EngineSyncStatus.Where(ess => ess.ModuleName.Equals("MkgtShipperEuropeClientModule"))
            .FirstOrDefault()
            .LastUpdate = DateTime.Now - TimeSpan.FromDays(2); // now - 2 days
        _dataContext.SaveChanges();
        // Act
        res = repository.GetUpdatedEntities("MkgtShipperEuropeClientModule");
        // Assert res collection contain element with nom Client3
        Assert.NotNull(res);
        Assert.Single(res);
        Assert.Contains(res, c => c.Nom == "Client3");
        
        /// Step III New call should return 0 items
        // Act
        res = repository.GetUpdatedEntities("MkgtShipperEuropeClientModule");
        // Assert
        Assert.NotNull(res);
        Assert.Empty(res);
    }
    
    [Fact]
    public void CallBackDestIdCreation_WithMkgtEuropeClientModule_ShouldTrowNotSupportedException()
    {
        // Arrange
        var repository = new EngineEuropeClientRepository(_mockIDataContextEngine.Object);

        // Act & Assert
        Assert.Throws<NotSupportedException>(() => 
            repository.CallBackDestIdCreation("MkgtEuropeClientModule", new List<ClientEurope>())
        );
    }
    
    [Fact]
    public void CallBackDestIdCreation_WithOdooEuropeClientModule_ShouldTrowNotSupportedException()
    {
        // Arrange
        var repository = new EngineEuropeClientRepository(_mockIDataContextEngine.Object);

        // Act & Assert
        Assert.Throws<NotSupportedException>(() => 
            repository.CallBackDestIdCreation("OdooEuropeClientModule", new List<ClientEurope>())
        );
    }
    
    [Fact]
    public void CallBackDestIdCreation_WithGpiEuropeClientModule_ShouldTrowNotSupportedException()
    {
        // Arrange
        var repository = new EngineEuropeClientRepository(_mockIDataContextEngine.Object);

        // Act & Assert
        Assert.Throws<NotSupportedException>(() => 
            repository.CallBackDestIdCreation("GpiEuropeClientModule", new List<ClientEurope>())
        );
    }
    
    [Fact]
    public void CallBackDestIdCreation_WithHubSpotEuropeClientModule_ShouldTrowNotSupportedException()
    {
        // Arrange
        var repository = new EngineEuropeClientRepository(_mockIDataContextEngine.Object);

        // Act & Assert
        Assert.Throws<NotSupportedException>(() => 
            repository.CallBackDestIdCreation("HubSpotEuropeClientModule", new List<ClientEurope>())
        );
    }
    
    [Fact]
    public void CallBackDestIdCreation_WithDashDocEuropeClientModule_ShouldTrowNotSupportedException()
    {
        // Arrange
        var repository = new EngineEuropeClientRepository(_mockIDataContextEngine.Object);

        // Act & Assert
        Assert.Throws<NotSupportedException>(() => 
            repository.CallBackDestIdCreation("DashDocEuropeClientModule", new List<ClientEurope>())
        );
    }
    
    [Fact]
    public void CallBackDestIdCreation_WithNotExistModule_ShouldReturnNull()
    {
        // Arrange
        var repository = new EngineEuropeClientRepository(_mockIDataContextEngine.Object);

        // Act
        var res = repository.CallBackDestIdCreation("NotExistModule", new List<ClientEurope>());

        // Assert
        Assert.Null(res);
    }

    [Fact]
    public void CallBackDestIdCreation_WithMkgtShipperEuropeClientModule_ShouldUpdateDestId()
    {
        // Arrange
        var repository = new EngineEuropeClientRepository(_mockIDataContextEngine.Object);
        var res = new List<ClientEurope>
        {
            new ClientEurope
            {
                Id = 1,
                Nom = "Client1",
                CodeMkgt = "CLIENTHAUB59",
                IdShipperDashdoc = 123456798
            }
        };
        
        // Act
        var result = repository.CallBackDestIdCreation("MkgtShipperEuropeClientModule", res);
        var chekedInDb = _dataContext.ClientEurope.Where(x => x.Id == 1).FirstOrDefault();
        
        // Assert
        Assert.NotNull(result);
        Assert.Equal(123456798, chekedInDb.IdShipperDashdoc);
    }
}