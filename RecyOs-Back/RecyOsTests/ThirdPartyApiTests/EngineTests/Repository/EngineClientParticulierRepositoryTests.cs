using Moq;
using RecyOs.Engine.Interfaces;
using RecyOs.Engine.Repository;
using RecyOs.Helpers;
using RecyOs.ORM.Entities.hub;
using RecyOsTests.Interfaces;
using RecyOsTests.Services;

namespace RecyOsTests.EngineTests.Repository;

public class EngineClientParticulierRepositoryTests
{
    private readonly Mock<IDataContextEngine> _mockIDataContextEngine;
    private readonly Mock<IDataContextEngine> _mockIsolatedDataContextEngine;
    private readonly DataContext _dataContext;
    private readonly DataContext _isolatedDataContext;
    
    public EngineClientParticulierRepositoryTests(IEngineDataContextTests objEngineDataContextTests)
    {
        _mockIDataContextEngine = new Mock<IDataContextEngine>();
        _dataContext = objEngineDataContextTests.GetContext();
        _mockIDataContextEngine.Setup(m => m.GetContext()).Returns(_dataContext);
        _mockIsolatedDataContextEngine = new Mock<IDataContextEngine>();
        _isolatedDataContext = new EngineDataContextTests().GetContext();
        _mockIsolatedDataContextEngine.Setup(m => m.GetContext()).Returns(_isolatedDataContext);
    }

    [Fact]
    public void GetCreatedEntities_WithNotExistModule_ShouldReturnNull()
    {
        // Arrange
        var repository = new EngineClientParticulierRepository(_mockIDataContextEngine.Object);
        // Act
        var result = repository.GetCreatedEntities("NotExistModule");
        // Assert no items
        Assert.Null(result);
    }

    [Fact]
    public void GetCreatedEntities_WithMkgtClientModule_ShouldReturnNotNull()
    {
        /// Step I - Ensure module is correctly configured 
        // Arrange
        var repository = new EngineClientParticulierRepository(_mockIDataContextEngine.Object);
        // Act
        var result = repository.GetCreatedEntities("MkgtClientParticulierModule");
        // Assert
        Assert.NotNull(result);
        
        /// Step II reward LastCreate 2 hours should return Caligula
        // Arrange 
        _dataContext.EngineSyncStatus.Where(ess => ess.ModuleName.Equals("MkgtClientParticulierModule"))
            .FirstOrDefault().LastCreate = DateTime.Now - TimeSpan.FromHours(2);     // now - 2 hours
        _dataContext.SaveChanges();
        // Act
        result = repository.GetCreatedEntities("MkgtClientParticulierModule");
        // Assert res collection contain element with nom Caligula
        Assert.NotNull(result);
        Assert.Single(result);
        Assert.Contains(result, c => c.CodeMkgt == "TESCLI0001");
        
        /// Step III New call should return 0 items
        // Act
        result = repository.GetCreatedEntities("MkgtClientParticulierModule");
        // Assert
        Assert.NotNull(result);
        Assert.Empty(result);
    }
    
    [Fact]
    public void GetCreatedEntities_WithMkgtShipperClientModule_ShouldReturnNotNull()
    {
        /// Step I - Ensure module is correctly configured 
        // Arrange
        var repository = new EngineClientParticulierRepository(_mockIsolatedDataContextEngine.Object);
        // Act
        var result = repository.GetCreatedEntities("MkgtShipperClientParticulierModule");
        // Assert
        Assert.NotNull(result);
        
        /// Step II reward LastCreate 2 hours should return Caligula
        // Arrange 
        _dataContext.EngineSyncStatus.Where(ess => ess.ModuleName.Equals("MkgtShipperClientParticulierModule"))
            .FirstOrDefault().LastCreate = DateTime.Now - TimeSpan.FromHours(2);     // now - 2 hours
        _dataContext.SaveChanges();
        // Act
        result = repository.GetCreatedEntities("MkgtShipperClientParticulierModule");
        // Assert res collection contain element with nom Caligula
        Assert.NotNull(result);
        Assert.Single(result);
        Assert.Contains(result, c => c.CodeMkgt == "TESCLI0001");
        
        /// Step III New call should return same items
        // Act
        result = repository.GetCreatedEntities("MkgtShipperClientParticulierModule");
        // Assert
        Assert.NotNull(result);
        Assert.Single(result);
        Assert.Contains(result, c => c.CodeMkgt == "TESCLI0001");
    }

    [Fact]
    public void GetUpdatedEntities_WithNotExistModule_ShouldReturnNull()
    {
        // Arrange
        var repository = new EngineClientParticulierRepository(_mockIDataContextEngine.Object);
        // Act
        var result = repository.GetUpdatedEntities("NotExistModule");
        // Assert no items
        Assert.Null(result);
    }
    
    [Fact]
    public void GetUpdatedEntities_WithMkgtClientModule_ShouldReturnNotNull()
    {
        /// Step I - Ensure module is correctly configured 
        // Arrange
        var repository = new EngineClientParticulierRepository(_mockIDataContextEngine.Object);
        // Act
        var result = repository.GetUpdatedEntities("MkgtClientParticulierModule");
        // Assert
        Assert.NotNull(result);
        
        /// Step II reward LastUpdate 2 hours should return Caligula
        // Arrange 
        _dataContext.EngineSyncStatus.Where(ess => ess.ModuleName.Equals("MkgtClientParticulierModule"))
            .FirstOrDefault().LastUpdate = DateTime.Now - TimeSpan.FromHours(2);     // now - 2 hours
        _dataContext.SaveChanges();
        // Act
        result = repository.GetUpdatedEntities("MkgtClientParticulierModule");
        // Assert res collection contain element with nom Caligula
        Assert.NotNull(result);
        Assert.Single(result);
        Assert.Contains(result, c => c.CodeMkgt == "TESCLI0002");
        
        /// Step III New call should return 0 items
        // Act
        result = repository.GetUpdatedEntities("MkgtClientParticulierModule");
        // Assert
        Assert.NotNull(result);
        Assert.Empty(result);
    }
    
    [Fact]
    public void GetUpdatedEntities_WithMkgtShipperClientModule_ShouldReturnNotNull()
    {
        /// Step I - Ensure module is correctly configured 
        // Arrange
        var repository = new EngineClientParticulierRepository(_mockIDataContextEngine.Object);
        // Act
        var result = repository.GetUpdatedEntities("MkgtShipperClientParticulierModule");
        // Assert
        Assert.NotNull(result);
        
        /// Step II reward LastUpdate 2 hours should return Caligula
        // Arrange 
        _dataContext.EngineSyncStatus.Where(ess => ess.ModuleName.Equals("MkgtShipperClientParticulierModule"))
            .FirstOrDefault().LastUpdate = DateTime.Now - TimeSpan.FromHours(2);     // now - 2 hours
        _dataContext.SaveChanges();
        // Act
        result = repository.GetUpdatedEntities("MkgtShipperClientParticulierModule");
        // Assert res collection contain element with nom Caligula
        Assert.NotNull(result);
        Assert.Single(result);
        Assert.Contains(result, c => c.CodeMkgt == "TESCLI0002");
        
        /// Step III New call should return 0 items
        // Act
        result = repository.GetUpdatedEntities("MkgtShipperClientParticulierModule");
        // Assert
        Assert.NotNull(result);
        Assert.Empty(result);
    }
    
    [Fact]
    public void GetCreatedEntities_WithOdooClientModule_ShouldReturnNull()
    {
        // Arrange
        var repository = new EngineClientParticulierRepository(_mockIDataContextEngine.Object);
        // Act
        var result = repository.GetCreatedEntities("OdooClientParticulierModule");
        // Assert no items
        Assert.Null(result);
    }
    
    [Fact]
    public void GetUpdatedEntities_WithOdooClientModule_ShouldReturnNotNull()
    {
        /// Step I - Ensure module is correctly configured 
        // Arrange
        var repository = new EngineClientParticulierRepository(_mockIDataContextEngine.Object);
        // Act
        var result = repository.GetUpdatedEntities("OdooClientParticulierModule");
        // Assert no items
        Assert.NotNull(result);
        
        /// Step II reward LastUpdate 2 hours should return Caligula
        // Arrange 
        _dataContext.EngineSyncStatus.Where(ess => ess.ModuleName.Equals("OdooClientParticulierModule"))
            .FirstOrDefault().LastUpdate = DateTime.Now - TimeSpan.FromHours(2);     // now - 2 hours
        _dataContext.SaveChanges();
        // Act
        result = repository.GetUpdatedEntities("OdooClientParticulierModule");
        // Assert res collection contain element with id odoo ODOO0001
        Assert.NotNull(result);
        Assert.Single(result);
        Assert.Contains(result, c => c.IdOdoo == "ODOO0001");
        
        /// Step III New call should return 0 items
        // Act
        result = repository.GetUpdatedEntities("OdooClientParticulierModule");
        // Assert
        Assert.NotNull(result);
        Assert.Empty(result);
        
    }
    
    [Fact]
    public void CallBackDestIdCreation_WithNotExistModule_ShouldReturnNull()
    {
        // Arrange
        var repository = new EngineClientParticulierRepository(_mockIDataContextEngine.Object);
        // Act
        var result = repository.CallBackDestIdCreation("NotExistModule", new List<ClientParticulier>());
        // Assert no items
        Assert.Null(result);
    }
    
    [Fact]
    public void CallBackDestIdCreation_WithMkgtClientParticulierModule_ShouldThrowNotSupportedException()
    {
        // Arrange
        var repository = new EngineClientParticulierRepository(_mockIDataContextEngine.Object);
        // Act
        // Assert
        Assert.Throws<NotSupportedException>(() => repository.CallBackDestIdCreation("MkgtClientParticulierModule", new List<ClientParticulier>()));
    }
    
    [Fact]
    public void CallBackDestIdCreation_WithOdooClientParticulierModule_ShouldThrowNotSupportedException()
    {
        // Arrange
        var repository = new EngineClientParticulierRepository(_mockIDataContextEngine.Object);
        // Act
        // Assert
        Assert.Throws<NotSupportedException>(() => repository.CallBackDestIdCreation("OdooClientParticulierModule", new List<ClientParticulier>()));
    }
    
    [Fact]
    public void CallBackDestIdCreation_WithMkgtShipperClientParticulierModule_ShouldReturnUpdatedItems()
    {
        // Arrange
        var repository = new EngineClientParticulierRepository(_mockIDataContextEngine.Object);
        var items = new List<ClientParticulier>
        {
            new ClientParticulier
            {
                Id = 3,
                CodeMkgt = "TESCLI0001",
                IdOdoo = "ODOO0001",
                IdShipperDashdoc = 987654321
            }
        };
        // Act
        var result = repository.CallBackDestIdCreation("MkgtShipperClientParticulierModule", items);
        var chekedInDb = _dataContext.ClientParticuliers.Where(c => c.IdShipperDashdoc == 987654321).ToList();
        // Assert
        Assert.NotNull(result);
        Assert.Single(result);
        Assert.Contains(chekedInDb, c => c.IdShipperDashdoc == 987654321);
    }
}