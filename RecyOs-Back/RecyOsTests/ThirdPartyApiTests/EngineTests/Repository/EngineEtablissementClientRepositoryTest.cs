using Microsoft.Extensions.Configuration;
using Moq;
using RecyOs.Engine.Interfaces;
using RecyOs.Engine.Repository;
using RecyOs.Helpers;
using RecyOs.ORM.Entities.hub;
using RecyOsTests.Interfaces;
using RecyOsTests.Services;

namespace RecyOsTests.EngineTests.Repository;

public class EngineEtablissementClientRepositoryTest
{
    private readonly Mock<IDataContextEngine> _mockIDataContextEngine;
    private readonly Mock<IDataContextEngine> _mockIsolatedDataContextEngine;
    private readonly DataContext _dataContext;
    private readonly DataContext _isolatedDataContext;
    private readonly Mock<IConfiguration> _mockIConfiguration;

    public EngineEtablissementClientRepositoryTest(IEngineDataContextTests objEngineDataContextTests)
    {
        _mockIDataContextEngine = new Mock<IDataContextEngine>();
        _dataContext = objEngineDataContextTests.GetContext();
        _mockIDataContextEngine.Setup(m => m.GetContext()).Returns(_dataContext);
        _mockIConfiguration = new Mock<IConfiguration>();
        _mockIsolatedDataContextEngine = new Mock<IDataContextEngine>();
        _isolatedDataContext = new EngineDataContextTests().GetContext();
        _mockIsolatedDataContextEngine.Setup(m => m.GetContext()).Returns(_isolatedDataContext);
    }
    
    [Fact]
    public void GetCreatedEntities_WithNotExistModule_ShouldReturnNull()
    {
        // Arrange
        var repository = new EngineEtablissementClientRepository(_mockIDataContextEngine.Object, _mockIConfiguration.Object);
        // Act
        var result = repository.GetCreatedEntities("NotExistModule");
        // Assert no items
        Assert.Null(result);
    }

    /// ClientRepository` class. This method expects that the `GetCreatedEntities` method should always return a non-null collection.
    [Fact]
    public void GetCreatedEntities_WithMkgtClientModule_ShouldReturnNotNull()
    {
        /// Step I - Ensure module is correctly configured 
        // Arrange
        var repository = new EngineEtablissementClientRepository(_mockIDataContextEngine.Object, _mockIConfiguration.Object);
        // Act
        var result = repository.GetCreatedEntities("MkgtClientModule");
        // Assert
        Assert.NotNull(result);
        
        /// Step II reward LastCreate 2 hours should return Client1
        // Arrange 
        _dataContext.EngineSyncStatus.Where(ess => ess.ModuleName.Equals("MkgtClientModule"))
            .FirstOrDefault().LastCreate = DateTime.Now - TimeSpan.FromHours(2);     // now - 2 hours
        _dataContext.SaveChanges();
        // Act
        result = repository.GetCreatedEntities("MkgtClientModule");
        // Assert res collection contain element with nom Client1
        Assert.NotNull(result);
        Assert.Single(result);
        Assert.Contains(result, c => c.CodeMkgt == "TESCLI0001");
        Assert.NotNull(result[0].EntrepriseBase);
        
        /// Step III New call should return 0 items
        // Act
        result = repository.GetCreatedEntities("MkgtClientModule");
        // Assert
        Assert.NotNull(result);
        Assert.Empty(result);
    }
    
    [Fact]
    public void GetCreatedEntities_WithMkgtShipperClientModule_ShouldReturnNotNull()
    {
        /// Step I - Ensure module is correctly configured 
        // Arrange
        var repository = new EngineEtablissementClientRepository(_mockIsolatedDataContextEngine.Object, _mockIConfiguration.Object);
        // Act
        var result = repository.GetCreatedEntities("MkgtShipperClientModule");
        // Assert
        Assert.NotNull(result);
        
        /// Step II reward LastCreate 2 hours should return Client1
        // Arrange 
        _dataContext.EngineSyncStatus.Where(ess => ess.ModuleName.Equals("MkgtShipperClientModule"))
            .FirstOrDefault()
            .LastCreate = DateTime.Now - TimeSpan.FromHours(2); // now - 2 hours
        _dataContext.SaveChanges();
        // Act
        result = repository.GetCreatedEntities("MkgtShipperClientModule");
        // Assert res collection contain element with nom Client1
        Assert.NotNull(result);
        Assert.Single(result);
        Assert.Contains(result, c => c.CodeMkgt == "TESCLI0001");
        Assert.NotNull(result[0].EntrepriseBase);
        
        /// Step III New call should return same items
        // Act
        result = repository.GetCreatedEntities("MkgtShipperClientModule");
        // Assert
        Assert.NotNull(result);
        Assert.Single(result);
        Assert.Contains(result, c => c.CodeMkgt == "TESCLI0001");
        Assert.NotNull(result[0].EntrepriseBase);
    }

    [Fact]
    public void GetCreatedEntities_WithOdooClientModule_ShouldRetunNull()
    {
        /// Step I - Ensure module is correctly configured 
        // Arrange
        var repository = new EngineEtablissementClientRepository(_mockIDataContextEngine.Object, _mockIConfiguration.Object);
        // Act
        var result = repository.GetCreatedEntities("OdooClientModule");
        // Assert
        Assert.Null(result); 
    }

    [Fact]
    public void GetCreatedEntities_WithGpiClientModule_ShouldReturnNotNull()
    {
        /// Step I - Ensure module is correctly configured 
        // Arrange
        var repository = new EngineEtablissementClientRepository(_mockIDataContextEngine.Object, _mockIConfiguration.Object);
        // Act
        var result = repository.GetCreatedEntities("GpiClientModule");
        // Assert
        Assert.NotNull(result);
        
        /// Step II reward LastCreate 2 hours should return Client2
        // Arrange
        _dataContext.EngineSyncStatus.Where(ess => ess.ModuleName.Equals("GpiClientModule"))
            .FirstOrDefault()
            .LastCreate = DateTime.Now - TimeSpan.FromHours(2); // now - 2 hours
        _dataContext.SaveChanges();
        // Act
        result = repository.GetCreatedEntities("GpiClientModule");
        // Assert res collection contain element with nom Client2
        Assert.NotNull(result);
        Assert.Single(result);
        Assert.Contains(result, c => c.CodeGpi == "C00002");
        Assert.NotNull(result[0].EntrepriseBase);
        
        /// Step III New call should return 0 items
        // Act
        result = repository.GetCreatedEntities("GpiClientModule");
        // Assert
        Assert.NotNull(result);
        Assert.Empty(result);
    }

    [Fact]
    public void GetUpdatedEntities_WithNotExistModule_ShouldReturnNull()
    {
        // Arrange
        var repository = new EngineEtablissementClientRepository(_mockIDataContextEngine.Object, _mockIConfiguration.Object);
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
        var repository = new EngineEtablissementClientRepository(_mockIDataContextEngine.Object, _mockIConfiguration.Object);
        // Act
        var result = repository.GetUpdatedEntities("MkgtClientModule");
        // Assert
        Assert.NotNull(result);
        
        /// Step II reward LastCreate 2 hours should return Client1
        // Arrange 
        _dataContext.EngineSyncStatus.Where(ess => ess.ModuleName.Equals("MkgtClientModule"))
            .FirstOrDefault()
            .LastUpdate = DateTime.Now - TimeSpan.FromHours(2); // now - 2 hours
        _dataContext.SaveChanges();
        // Act
        result = repository.GetUpdatedEntities("MkgtClientModule");
        // Assert res collection contain element with nom Client1
        Assert.NotNull(result);
        Assert.Single(result);
        Assert.Contains(result, c => c.CodeMkgt == "TESCLI0003");
        Assert.NotNull(result[0].EntrepriseBase);
        
        /// Step III New call should return 0 items
        // Act
        result = repository.GetUpdatedEntities("MkgtClientModule");
        // Assert
        Assert.NotNull(result);
        Assert.Empty(result);
    }
    
    [Fact]
    public void GetUpdatedEntities_WithMkgtShipperClientModule_ShouldReturnNotNull()
    {
        /// Step I - Ensure module is correctly configured 
        // Arrange
        var repository = new EngineEtablissementClientRepository(_mockIDataContextEngine.Object, _mockIConfiguration.Object);
        // Act
        var result = repository.GetUpdatedEntities("MkgtShipperClientModule");
        // Assert
        Assert.NotNull(result);
        
        /// Step II reward LastCreate 2 hours should return Client1
        // Arrange 
        _dataContext.EngineSyncStatus.Where(ess => ess.ModuleName.Equals("MkgtShipperClientModule"))
            .FirstOrDefault()
            .LastUpdate = DateTime.Now - TimeSpan.FromHours(2); // now - 2 hours
        _dataContext.SaveChanges();
        // Act
        result = repository.GetUpdatedEntities("MkgtShipperClientModule");
        // Assert res collection contain element with nom Client3
        Assert.NotNull(result);
        Assert.Single(result);
        Assert.Contains(result, c => c.CodeMkgt == "TESCLI0003");
        Assert.NotNull(result[0].EntrepriseBase);
        
        /// Step III New call should return 0 items
        // Act
        result = repository.GetUpdatedEntities("MkgtShipperClientModule");
        // Assert
        Assert.NotNull(result);
        Assert.Empty(result);
    }

    [Fact]
    public void GetUpdatedEntities_WithOdooClientModule_ShouldReturnNotNull()
    {
        /// Step I - Ensure module is correctly configured 
        // Arrange
        var repository = new EngineEtablissementClientRepository(_mockIDataContextEngine.Object, _mockIConfiguration.Object);
        // Act
        var result = repository.GetUpdatedEntities("OdooClientModule");
        // Assert
        Assert.NotNull(result);
        
        /// Step II reward LastCreate 2 hours should return Client2
        // Arrange
        _dataContext.EngineSyncStatus.Where(ess => ess.ModuleName.Equals("OdooClientModule"))
            .FirstOrDefault()
            .LastUpdate = DateTime.Now - TimeSpan.FromHours(2); // now - 2 hours
        _dataContext.SaveChanges();
        // Act
        result = repository.GetUpdatedEntities("OdooClientModule");
        // Assert res collection contain element with nom Client2
        Assert.NotNull(result);
        Assert.Single(result);
        Assert.Contains(result, c => c.IdOdoo == "000003");
        Assert.NotNull(result[0].EntrepriseBase);
        
        /// Step III New call should return 0 items
        // Act
        result = repository.GetUpdatedEntities("OdooClientModule");
        // Assert
        Assert.NotNull(result);
        Assert.Empty(result);
    }

    [Fact]
    public void GetUpdatedEntities_WithGpiClientModule_ShouldReturnNotNull()
    {
        /// Step I - Ensure module is correctly configured 
        // Arrange
        var repository = new EngineEtablissementClientRepository(_mockIDataContextEngine.Object, _mockIConfiguration.Object);
        // Act
        var result = repository.GetUpdatedEntities("GpiClientModule");
        // Assert
        Assert.NotNull(result);
        
        /// Step II reward LastCreate 2 hours should return Client2
        // Arrange
        _dataContext.EngineSyncStatus.Where(ess => ess.ModuleName.Equals("GpiClientModule"))
            .FirstOrDefault()
            .LastUpdate = DateTime.Now - TimeSpan.FromHours(2); // now - 2 hours
        _dataContext.SaveChanges();
        // Act
        result = repository.GetUpdatedEntities("GpiClientModule");
        // Assert res collection contain element with nom Client2
        Assert.NotNull(result);
        Assert.Single(result);
        Assert.Contains(result, c => c.CodeGpi == "C00003");
        Assert.NotNull(result[0].EntrepriseBase);
        
        /// Step III New call should return 0 items
        // Act
        result = repository.GetUpdatedEntities("GpiClientModule");
        // Assert
        Assert.NotNull(result);
        Assert.Empty(result);
    }
    
    [Fact]
    public void GetUpdatedEntities_WithDashDocClientModule_ShouldReturnNotNull()
    {
        /// Step I - Ensure module is correctly configured 
        // Arrange
        var repository = new EngineEtablissementClientRepository(_mockIDataContextEngine.Object, _mockIConfiguration.Object);
        // Act
        var result = repository.GetUpdatedEntities("DashDocClientModule");
        // Assert
        Assert.NotNull(result);
        
        /// Step II reward LastCreate 2 hours should return Client2
        // Arrange
        _dataContext.EngineSyncStatus.Where(ess => ess.ModuleName.Equals("DashDocClientModule"))
            .FirstOrDefault()
            .LastUpdate = DateTime.Now - TimeSpan.FromHours(2); // now - 2 hours
        _dataContext.SaveChanges();
        // Act
        result = repository.GetUpdatedEntities("DashDocClientModule");
        // Assert res collection contain element with nom Client2
        Assert.NotNull(result);
        Assert.Single(result);
        Assert.Contains(result, c => c.IdDashdoc == 1235483);
        Assert.NotNull(result[0].EntrepriseBase);
        
        /// Step III New call should return 0 items
        // Act
        result = repository.GetUpdatedEntities("DashDocClientModule");
        // Assert
        Assert.NotNull(result);
        Assert.Empty(result);
    }

    [Fact]
    public void GetUpdatedEntities_WithHubSpotClientModule_ShouldReturnNotNull()
    {
        /// Step I - Ensure module is correctly configured 
        // Arrange
        var repository = new EngineEtablissementClientRepository(_mockIDataContextEngine.Object, _mockIConfiguration.Object);
        // Act
        var result = repository.GetUpdatedEntities("HubSpotClientModule");
        // Assert
        Assert.NotNull(result);
        
        /// Step II reward LastCreate 2 hours should return Client2
        // Arrange
        _dataContext.EngineSyncStatus.Where(ess => ess.ModuleName.Equals("HubSpotClientModule"))
            .FirstOrDefault()
            .LastUpdate = DateTime.Now - TimeSpan.FromHours(2); // now - 2 hours
        _dataContext.SaveChanges();
        // Act
        result = repository.GetUpdatedEntities("HubSpotClientModule");
        // Assert res collection contain element with nom Client2
        Assert.NotNull(result);
        Assert.Single(result);
        Assert.Contains(result, c => c.IdHubspot == "8965879040");
        Assert.NotNull(result[0].EntrepriseBase);
        
        /// Step III New call should return 0 items
        // Act
        result = repository.GetUpdatedEntities("HubSpotClientModule");
        // Assert
        Assert.NotNull(result);
        Assert.Empty(result);
    }
    
            [Fact]
        public async Task GetMontantGarantieForEntreprise_WithCouverture_ShouldReturnFormattedAmount()
        {
            // Arrange
            var repository = new EngineEtablissementClientRepository(_mockIDataContextEngine.Object, _mockIConfiguration.Object);
            

            // Act
            var result = await repository.GetMontantGarantieForEntreprise("056800659");

            // Assert
            Assert.Equal("100 000 €", result);
        }

        [Fact]
        public async Task GetMontantGarantieForEntreprise_WithTotalGuaranteeNDCover_ShouldReturnFormattedAmount()
        {
            // Arrange
            var repository = new EngineEtablissementClientRepository(_mockIDataContextEngine.Object, _mockIConfiguration.Object);

            _mockIConfiguration.Setup(x => x.GetSection("CoverPolicies:NDCoverFrance:CoverAmount").Value)
                               .Returns("10000");

            // Act
            var result = await repository.GetMontantGarantieForEntreprise("200017598");

            // Assert
            Assert.Equal("10 000 €", result);
        }

        [Fact]
        public async Task GetMontantGarantieForEntreprise_WithNoGuaranteeNDCover_ShouldReturnZero()
        {
            // Arrange
            var repository = new EngineEtablissementClientRepository(_mockIDataContextEngine.Object, _mockIConfiguration.Object);

            // Act
            var result = await repository.GetMontantGarantieForEntreprise("948224746");

            // Assert
            Assert.Equal("0 €", result);
        }

        [Fact] public async Task GetMontantGarantieForEntreprise_WithInvalidNDCover_ShouldReturnZero()
        {
            // Arrange
            var repository = new EngineEtablissementClientRepository(_mockIDataContextEngine.Object, _mockIConfiguration.Object);

            // Act
            var result = await repository.GetMontantGarantieForEntreprise("258796413");

            // Assert
            Assert.Equal("0 €", result);
        }
        
        [Fact]
        public async Task GetMontantGarantieForEntreprise_WithNoCouvertureAndNoNDCover_ShouldReturnZero()
        {
            // Arrange
            var repository = new EngineEtablissementClientRepository(_mockIDataContextEngine.Object, _mockIConfiguration.Object);

            // Act
            var result = await repository.GetMontantGarantieForEntreprise("123456789");

            // Assert
            Assert.Equal("0 €", result);
        }
        
        [Fact]
        public void CallBackDestIdCreation_WithMkgtClientModule_ShouldTrowNotSupportedException()
        {
            // Arrange
            var repository = new EngineEtablissementClientRepository(_mockIDataContextEngine.Object, _mockIConfiguration.Object);
            var items = new List<EtablissementClient>();

            // Act & Assert
            Assert.Throws<NotSupportedException>(() => repository.CallBackDestIdCreation("MkgtClientModule", items));
        }
        
        [Fact]
        public void CallBackDestIdCreation_WithOdooClientModule_ShouldTrowNotSupportedException()
        {
            // Arrange
            var repository = new EngineEtablissementClientRepository(_mockIDataContextEngine.Object, _mockIConfiguration.Object);
            var items = new List<EtablissementClient>();

            // Act & Assert
            Assert.Throws<NotSupportedException>(() => repository.CallBackDestIdCreation("OdooClientModule", items));
        }
        
        [Fact]
        public void CallBackDestIdCreation_WithGpiClientModule_ShouldTrowNotSupportedException()
        {
            // Arrange
            var repository = new EngineEtablissementClientRepository(_mockIDataContextEngine.Object, _mockIConfiguration.Object);
            var items = new List<EtablissementClient>();

            // Act & Assert
            Assert.Throws<NotSupportedException>(() => repository.CallBackDestIdCreation("GpiClientModule", items));
        }
        
        [Fact]
        public void CallBackDestIdCreation_WithDashDocClientModule_ShouldTrowNotSupportedException()
        {
            // Arrange
            var repository = new EngineEtablissementClientRepository(_mockIDataContextEngine.Object, _mockIConfiguration.Object);
            var items = new List<EtablissementClient>();

            // Act & Assert
            Assert.Throws<NotSupportedException>(() => repository.CallBackDestIdCreation("DashDocClientModule", items));
        }
        
        [Fact]
        public void CallBackDestIdCreation_WithHubSpotClientModule_ShouldTrowNotSupportedException()
        {
            // Arrange
            var repository = new EngineEtablissementClientRepository(_mockIDataContextEngine.Object, _mockIConfiguration.Object);
            var items = new List<EtablissementClient>();

            // Act & Assert
            Assert.Throws<NotSupportedException>(() => repository.CallBackDestIdCreation("HubSpotClientModule", items));
        }
        
        [Fact]
        public void CallBackDestIdCreation_WithNotExistModule_ShouldReturnNull()
        {
            // Arrange
            var repository = new EngineEtablissementClientRepository(_mockIDataContextEngine.Object, _mockIConfiguration.Object);
            var items = new List<EtablissementClient>();

            // Act
            var result = repository.CallBackDestIdCreation("NotExistModule", items);
            // Assert
            Assert.Null(result);
        }
        
        [Fact]
        public void CallBackDestIdCreation_WithMkgtShipperClientModule_ShouldUpdateDestId()
        {
            // Arrange
            var repository = new EngineEtablissementClientRepository(_mockIDataContextEngine.Object, _mockIConfiguration.Object);
            var items = new List<EtablissementClient>
            {
                new EtablissementClient
                {
                    Id = 1,
                    CodeMkgt = "TESCLI0001",
                    IdShipperDashdoc = 123456789,
                    Siret = "05680065900858"
                }
            };

            // Act
            var result = repository.CallBackDestIdCreation("MkgtShipperClientModule", items);
            var chekedInDb = _dataContext.EtablissementClient.FirstOrDefault(x => x.CodeMkgt == "TESCLI0001");
            // Assert
            Assert.NotNull(result);
            Assert.Equal(123456789, chekedInDb.IdShipperDashdoc);
        }
    }