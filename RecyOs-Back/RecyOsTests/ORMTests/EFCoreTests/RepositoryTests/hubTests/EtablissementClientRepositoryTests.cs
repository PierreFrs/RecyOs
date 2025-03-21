using System.Security.Claims;
using AutoMapper;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Moq;
using RecyOs.Helpers;
using RecyOs.ORM.EFCore.Repository.hub;
using RecyOs.ORM.Entities;
using RecyOs.ORM.Entities.hub;
using RecyOs.ORM.Filters.hub;
using RecyOs.ORM.Interfaces.hub;
using RecyOsTests.Interfaces;

namespace RecyOsTests.ORMTests.EFCoreTests.RepositoryTests.hubTests;

[Collection("RepositoryTests")]
public class EtablissementClientRepositoryTests
{
    private readonly DataContext _context;
    private readonly Mock<IHttpContextAccessor> _mockHttpContextAccessor;
    private readonly Mock<IEntrepriseBaseRepository<EntrepriseBase>> _mockBaseRepos;
    private readonly Mock<IEtablissementFicheRepository<EtablissementFiche>> _mockEtablissementFicheRepos;
    private readonly Mock<IMapper> _mockMapper;
    
    public EtablissementClientRepositoryTests(IDataContextTests dataContextTests)
    {
        _context = dataContextTests.GetContext();
        _mockHttpContextAccessor = new Mock<IHttpContextAccessor>();
        _mockBaseRepos = new Mock<IEntrepriseBaseRepository<EntrepriseBase>>();
        _mockEtablissementFicheRepos = new Mock<IEtablissementFicheRepository<EtablissementFiche>>();
        _mockMapper = new Mock<IMapper>();
        
        // Créer un faux ClaimsPrincipal
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, "John DOE")
        };
        var identity = new ClaimsIdentity(claims, "TestAuthType");
        var claimsPrincipal = new ClaimsPrincipal(identity);
        _mockHttpContextAccessor.Setup(_ => _.HttpContext.User).Returns(claimsPrincipal);
    }
    
    /*********** Geters ***********/
    [Fact]
    public async Task Get_ById_ShouldReturnEtablissementClient()
    {
        // Arrange
        var repository = new EtablissementClientRepository(_context, _mockHttpContextAccessor.Object, _mockBaseRepos.Object, _mockEtablissementFicheRepos.Object ,_mockMapper.Object);
        
        // Act
        var etablissementClient = await repository.Get(1, new ContextSession());
        _context.Entry(etablissementClient).State = EntityState.Detached;
        
        // Assert
        Assert.NotNull(etablissementClient);
        Assert.Equal("Etablissement 1", etablissementClient.Nom);
        Assert.Equal( "05680065900858", etablissementClient.Siret);
    }
    
    [Fact]
    public async Task Get_BySiret_ShouldReturnEtablissementClient()
    {
        // Arrange
        var repository = new EtablissementClientRepository(_context, _mockHttpContextAccessor.Object, _mockBaseRepos.Object, _mockEtablissementFicheRepos.Object , _mockMapper.Object);
        
        // Act
        var etablissementClient = await repository.GetBySiret("05680065900858", new ContextSession());
        _context.Entry(etablissementClient).State = EntityState.Detached;
        
        // Assert
        Assert.NotNull(etablissementClient);
        Assert.Equal("Etablissement 1", etablissementClient.Nom);
        Assert.Equal( "05680065900858", etablissementClient.Siret);
    }
    
    [Fact]
    public async Task Get_ByCodeMkgt_ShouldReturnEtablissementClient()
    {
        // Arrange
        var repository = new EtablissementClientRepository(_context, _mockHttpContextAccessor.Object, _mockBaseRepos.Object, _mockEtablissementFicheRepos.Object , _mockMapper.Object);
        
        // Act
        var etablissementClient = await repository.GetByCodeMkgt("TESCLI0001", new ContextSession());
        _context.Entry(etablissementClient).State = EntityState.Detached;
        
        // Assert
        Assert.NotNull(etablissementClient);
        Assert.Equal("Etablissement 1", etablissementClient.Nom);
        Assert.Equal( "05680065900858", etablissementClient.Siret);
    }
    
    [Fact]
    public async Task Get_ByCodeKerlog_ShouldReturnEtablissementClient()
    {
        // Arrange
        var repository = new EtablissementClientRepository(_context, _mockHttpContextAccessor.Object, _mockBaseRepos.Object, _mockEtablissementFicheRepos.Object ,_mockMapper.Object);
        
        // Act
        var etablissementClient = await repository.GetByCodeKerlog("000001", new ContextSession());
        _context.Entry(etablissementClient).State = EntityState.Detached;
        
        // Assert
        Assert.NotNull(etablissementClient);
        Assert.Equal("Etablissement 1", etablissementClient.Nom);
        Assert.Equal( "05680065900858", etablissementClient.Siret);
    }
    
    [Fact]
    public async Task Get_ByIdOdoo_ShouldReturnEtablissementClient()
    {
        // Arrange
        var repository = new EtablissementClientRepository(_context, _mockHttpContextAccessor.Object, _mockBaseRepos.Object, _mockEtablissementFicheRepos.Object ,_mockMapper.Object);
        
        // Act
        var etablissementClient = await repository.GetByIdOdoo("ODOO0001", new ContextSession());
        _context.Entry(etablissementClient).State = EntityState.Detached;
        
        // Assert
        Assert.NotNull(etablissementClient);
        Assert.Equal("Etablissement 1", etablissementClient.Nom);
        Assert.Equal( "05680065900858", etablissementClient.Siret);
    }
    
    [Fact]
    public async Task GetByCodeGpi_ShouldReturnEtablissementClient()
    {
        // Arrange
        var repository = new EtablissementClientRepository(_context, _mockHttpContextAccessor.Object, _mockBaseRepos.Object, _mockEtablissementFicheRepos.Object ,_mockMapper.Object);
        
        // Act
        var etablissementClient = await repository.GetByCodeGpi("GPI0001", new ContextSession());
        _context.Entry(etablissementClient).State = EntityState.Detached;
        
        // Assert
        Assert.NotNull(etablissementClient);
        Assert.Equal("Etablissement 9", etablissementClient.Nom);
        Assert.Equal( "02526474600016", etablissementClient.Siret);
    }
    
    [Fact]
    public void GetCurrentUserName_ShouldReturnCurrentUserName()
    {
        // Arrange
        var repository = new EtablissementClientRepository(_context, _mockHttpContextAccessor.Object, _mockBaseRepos.Object, _mockEtablissementFicheRepos.Object ,_mockMapper.Object);
        
        // Act
        var userName = repository.GetCurrentUserName();
        
        // Assert
        Assert.NotNull(userName);
        Assert.Equal("John DOE", userName);
    }
    
    [Fact]
    public async Task GetFiltredListWithCount_ShouldReturnEtablissementClientList()
    {
        // Arrange
        var repository = new EtablissementClientRepository(_context, _mockHttpContextAccessor.Object, _mockBaseRepos.Object, _mockEtablissementFicheRepos.Object ,_mockMapper.Object);
        var filter = new EtablissementClientGridFilter { };
        int elementsCount = await _context.EtablissementClient.CountAsync();
        
        // Act
        var etablissementClientList = await repository.GetFiltredListWithCount(filter, new ContextSession());

        foreach (var etablissement in etablissementClientList.Item1)
        {
            _context.Entry(etablissement).State = EntityState.Detached;
        }
        
        // Assert
        Assert.Equal(elementsCount, etablissementClientList.Item2);
    }
    
    [Fact]
    public async Task Exists_ShouldReturnTrue_WhenEtablissementClientSameId()
    {
        // Arrange
        var repository = new EtablissementClientRepository(_context, _mockHttpContextAccessor.Object, _mockBaseRepos.Object, _mockEtablissementFicheRepos.Object ,_mockMapper.Object);
        var etablissementClient = new EtablissementClient
        {
            Id = 1,
        };
        
        // Act
        var exists = await repository.Exists(etablissementClient, new ContextSession());
        
        // Assert
        Assert.True(exists);
    }
    
    [Fact]
    public async Task Exists_ShouldReturnTrue_WhenEtablissementClientSameSiret()
    {
        // Arrange
        var repository = new EtablissementClientRepository(_context, _mockHttpContextAccessor.Object, _mockBaseRepos.Object, _mockEtablissementFicheRepos.Object ,_mockMapper.Object);
        var etablissementClient = new EtablissementClient
        {
            Siret = "05680065900858",
        };
        
        // Act
        var exists = await repository.Exists(etablissementClient, new ContextSession());
        
        // Assert
        Assert.True(exists);
    }
    
    [Fact]
    public async Task Exists_ShouldReturnFalse_WhenEtablissementClientDifferentIdOrSiret()
    {
        // Arrange
        var repository = new EtablissementClientRepository(_context, _mockHttpContextAccessor.Object, _mockBaseRepos.Object, _mockEtablissementFicheRepos.Object ,_mockMapper.Object);
        var etablissementClient = new EtablissementClient
        {
            Id = 1962112,
            Siret = "75480065100858",
        };
        
        // Act
        var exists = await repository.Exists(etablissementClient, new ContextSession());
        
        // Assert
        Assert.False(exists);
    }

    [Fact]
    public async Task GetEtablissementGroupBySirenAsync_ShouldReturnEtablissementClientList_IfSirenIsValid()
    {
        // Arrange
        var repository = new EtablissementClientRepository(_context, _mockHttpContextAccessor.Object, _mockBaseRepos.Object, _mockEtablissementFicheRepos.Object, _mockMapper.Object);

        // Act
        var etablissementClientList = await repository.GetEtablissementGroupBySirenAsync("056800659", new ContextSession(), false);

        // Assert
        Assert.NotNull(etablissementClientList);
        Assert.Equal(1, etablissementClientList.Count());
    }
    
    /*********** Adders ***********/
    [Fact]
    public async Task Create_ShouldReturnEtablissementClient_WhenEtablissementClientIsValid()
    {
        // Arrange
        var repository = new EtablissementClientRepository(_context, _mockHttpContextAccessor.Object, _mockBaseRepos.Object, _mockEtablissementFicheRepos.Object ,_mockMapper.Object);
        var etablissementClient = new EtablissementClient
        {
            Nom = "Etablissement 5",
            Siret = "11000201100044",
            Siren = "110002011",
        };
        
        // Act
        var result = await repository.CreateIfDoesntExistAsync(etablissementClient, new ContextSession());
        var obj = await repository.GetBySiret("11000201100044", new ContextSession());
        
        // Assert
        Assert.NotNull(result);
        Assert.Equal("Etablissement 5", obj.Nom);
        Assert.Equal("11000201100044", obj.Siret);
        Assert.Equal("John DOE", obj.CreatedBy);

        etablissementClient = new EtablissementClient
        {
            Nom = "Etablissement 5",
            Siret = "11000201100044",
            Siren = "110002011",
        };
        
        var result2 = await repository.CreateIfDoesntExistAsync(etablissementClient, new ContextSession()); // Retenter l'ajout doit renvoyer null
        
        // Assert
        Assert.Equal(result.Id, result2.Id);
        
        // Clean
        _context.Entry(etablissementClient).State = EntityState.Detached;
        _context.Entry(obj).State = EntityState.Detached;
    }

    [Fact]
    public async Task CreateFull_ShouldReturnEtablissementClient_WhenEtablissementClientIsValid()
    {
        // Arrange
        var repository =
            new EtablissementClientRepository(_context, _mockHttpContextAccessor.Object, _mockBaseRepos.Object, _mockEtablissementFicheRepos.Object ,_mockMapper.Object);

        var etablissementClient = new EtablissementClient
        {
            Nom = "Etablissement 3",
            Siret = "92240296100037",
            Siren = "922402961",
        };

        var entrepriseBase = new EntrepriseBase
        {
            Siren = "922402961",
        };

        var etablissementFiche = new EtablissementFiche
        {
            Siret = "92240296100037",
        };

        // Act
        var result = await repository.Create(etablissementClient, entrepriseBase, etablissementFiche,
            new ContextSession(), false);

        // Assert
        Assert.NotNull(result);
        Assert.Equal("Etablissement 3", result.Nom);
        Assert.Equal("92240296100037", result.Siret);
        
        // Clean
        _context.Entry(result).State = EntityState.Detached;
    }
    
    /*********** Updaters ***********/
    [Fact]
    public async Task Update_ShouldReturnEtablissementClient_WhenEtablissementClientIsValid()
    {
        // Arrange
        var repository = new EtablissementClientRepository(_context, _mockHttpContextAccessor.Object, _mockBaseRepos.Object, _mockEtablissementFicheRepos.Object ,_mockMapper.Object);

        var obj = await repository.Get(1, new ContextSession());
        var originalDateCreMkgt = obj.DateCreMKGT;
        var originalDateCreOdoo = obj.DateCreOdoo;
        var originalDateCre = obj.CreateDate;
        var originalCreatedBy = obj.CreatedBy;
        
        obj.Nom = "Etablissement 4";
        obj.Siret = "01234567890123";
        obj.DateCreMKGT = new DateTime(2021, 1, 1, 0, 0, 0, DateTimeKind.Utc);
        obj.DateCreOdoo = new DateTime(2021, 1, 1, 0, 0, 0, DateTimeKind.Utc);
        obj.CreateDate = new DateTime(2021, 1, 1, 0, 0, 0, DateTimeKind.Utc);
        
        var trackedEntities = _context.ChangeTracker.Entries<EtablissementClient>();
        foreach (var entity in trackedEntities)
        {
            entity.State = EntityState.Detached; // Détacher l'entité pour éviter les erreurs de duplication
        }
        
        // Act
        await repository.UpdateAsync(obj, new ContextSession());
        var result = await repository.Get(1, new ContextSession());
        
        // Assert
        Assert.NotNull(result);
        Assert.Equal(obj.Nom, result.Nom);
        Assert.Equal("05680065900858", result.Siret);
        Assert.Equal( originalDateCreMkgt, result.DateCreMKGT);
        Assert.Equal( originalDateCreOdoo, result.DateCreOdoo);
        Assert.Equal( originalDateCre, result.CreateDate);
        Assert.Equal( originalCreatedBy, result.CreatedBy);
    }

    [Fact]
    public async Task Update_ShouldNotUpdateIdHubspot_IfItIsAlreadySetPreviously()
    {
        // Arrange
        var repository = new EtablissementClientRepository(_context, _mockHttpContextAccessor.Object, _mockBaseRepos.Object, _mockEtablissementFicheRepos.Object ,_mockMapper.Object);

        var obj = await repository.Get(1, new ContextSession());
        obj.IdHubspot = "NewId";
        
        var trackedEntities = _context.ChangeTracker.Entries<EtablissementClient>();
        foreach (var entity in trackedEntities)
        {
            entity.State = EntityState.Detached; // Détacher l'entité pour éviter les erreurs de duplication
        }
        
        // Act
        await repository.UpdateAsync(obj, new ContextSession());
        var result = await repository.Get(1, new ContextSession());
        
        // Assert
        Assert.NotNull(result);
        Assert.Equal(obj.IdHubspot, result.IdHubspot);
    }
    
    [Fact]
    public async Task Update_ShouldNotUpdateIdOdoo_IfItIsAlreadySetPreviously()
    {
        // Arrange
        var repository = new EtablissementClientRepository(_context, _mockHttpContextAccessor.Object, _mockBaseRepos.Object, _mockEtablissementFicheRepos.Object ,_mockMapper.Object);

        var obj = await repository.Get(1, new ContextSession());
        obj.IdOdoo = "NewId";
        
        var trackedEntities = _context.ChangeTracker.Entries<EtablissementClient>();
        foreach (var entity in trackedEntities)
        {
            entity.State = EntityState.Detached; // Détacher l'entité pour éviter les erreurs de duplication
        }
        
        // Act
        await repository.UpdateAsync(obj, new ContextSession());
        var result = await repository.Get(1, new ContextSession());
        
        // Assert
        Assert.NotNull(result);
        Assert.Equal(obj.IdOdoo, result.IdOdoo);
    }
    
    [Fact]
    public async Task Update_ShouldNotUpdateCodeMKGT_IfItIsAlreadySetPreviously()
    {
        // Arrange
        var repository = new EtablissementClientRepository(_context, _mockHttpContextAccessor.Object, _mockBaseRepos.Object, _mockEtablissementFicheRepos.Object ,_mockMapper.Object);

        var obj = await repository.Get(1, new ContextSession());
        obj.CodeMkgt = "NewCodeMkgt";
        
        var trackedEntities = _context.ChangeTracker.Entries<EtablissementClient>();
        foreach (var entity in trackedEntities)
        {
            entity.State = EntityState.Detached; // Détacher l'entité pour éviter les erreurs de duplication
        }
        
        // Act
        await repository.UpdateAsync(obj, new ContextSession());
        var result = await repository.Get(1, new ContextSession());
        
        // Assert
        Assert.NotNull(result);
        Assert.Equal(obj.CodeMkgt, result.CodeMkgt);
    }
    
    [Fact]
    public async Task Update_ShouldNotUpdateCodeGPI_IfItIsAlreadySetPreviously()
    {
        // Arrange
        var repository = new EtablissementClientRepository(_context, _mockHttpContextAccessor.Object, _mockBaseRepos.Object, _mockEtablissementFicheRepos.Object ,_mockMapper.Object);

        var obj = await repository.Get(1, new ContextSession());
        
        obj.CodeGpi = "NewCodeGpi";
        
        var trackedEntities = _context.ChangeTracker.Entries<EtablissementClient>();
        foreach (var entity in trackedEntities)
        {
            entity.State = EntityState.Detached; // Détacher l'entité pour éviter les erreurs de duplication
        }
        
        // Act
        await repository.UpdateAsync(obj, new ContextSession());
        var result = await repository.Get(1, new ContextSession());
        
        // Assert
        Assert.NotNull(result);
        Assert.Equal(obj.CodeGpi, result.CodeGpi);
    }
    
    [Fact]
    public async Task Update_ShouldNotUpdateFrnCodeGpi_IfItIsAlreadySetPreviously()
    {
        // Arrange
        var repository = new EtablissementClientRepository(_context, _mockHttpContextAccessor.Object, _mockBaseRepos.Object, _mockEtablissementFicheRepos.Object ,_mockMapper.Object);

        var obj = await repository.Get(1, new ContextSession());
        
        obj.FrnCodeGpi = "NewCodeGpi";
        
        var trackedEntities = _context.ChangeTracker.Entries<EtablissementClient>();
        foreach (var entity in trackedEntities)
        {
            entity.State = EntityState.Detached; // Détacher l'entité pour éviter les erreurs de duplication
        }
        
        // Act
        await repository.UpdateAsync(obj, new ContextSession());
        var result = await repository.Get(1, new ContextSession());
        
        // Assert
        Assert.NotNull(result);
        Assert.Equal(obj.FrnCodeGpi, result.FrnCodeGpi);
    }

    [Fact]
    public async Task Update_TestCreationOf_ExternalIds()
    {
        // Arrange
        var repository = new EtablissementClientRepository(_context, _mockHttpContextAccessor.Object, _mockBaseRepos.Object, _mockEtablissementFicheRepos.Object ,_mockMapper.Object);
        var obj = await repository.Get(2, new ContextSession());
        var originalDateCreMkgt = obj.DateCreMKGT;
        var originalDateCreOdoo = obj.DateCreOdoo;
        var originalDateCre = obj.CreateDate;
        var originalCreatedBy = obj.CreatedBy;
        var originalDateCreDashdoc = obj.DateCreDashdoc;
        
        obj.CodeMkgt = "NEWCODE0002";
        obj.IdOdoo = "-1";
        obj.IdDashdoc = -1;
        var trackedEntities = _context.ChangeTracker.Entries<EtablissementClient>();
        foreach (var entity in trackedEntities)
        {
            entity.State = EntityState.Detached; // Détacher l'entité pour éviter les erreurs de duplication
        }
        
        // Act
        await repository.UpdateAsync(obj, new ContextSession());
        
        var result = await repository.Get(2, new ContextSession());
        
        // Assert
        Assert.NotNull(result);
        Assert.Equal( "NEWCODE0002", result.CodeMkgt);
        Assert.NotEqual( originalDateCreMkgt, result.DateCreMKGT);
        Assert.NotEqual( originalDateCreOdoo, result.DateCreOdoo);
        Assert.NotEqual( originalDateCreDashdoc, result.DateCreDashdoc);
        Assert.Equal( originalDateCre, result.CreateDate);
        Assert.Equal( originalCreatedBy, result.CreatedBy);
    }

    [Fact]
    public async Task Update_TestNoCreationOf_ExternalIds()
    {
        // Arrange
        var repository = new EtablissementClientRepository(_context, _mockHttpContextAccessor.Object, _mockBaseRepos.Object, _mockEtablissementFicheRepos.Object ,_mockMapper.Object);
        var obj = await repository.Get(9, new ContextSession());
        var originalDateCreMkgt = obj.DateCreMKGT;
        var originalDateCreOdoo = obj.DateCreOdoo;
        var originalDateCre = obj.CreateDate;
        var originalCreatedBy = obj.CreatedBy;
        
        obj.Nom = "Etablissement 270";
        var trackedEntities = _context.ChangeTracker.Entries<EtablissementClient>();
        foreach (var entity in trackedEntities)
        {
            entity.State = EntityState.Detached; // Détacher l'entité pour éviter les erreurs de duplication
        }
        
        // Act
        await repository.UpdateAsync(obj, new ContextSession());
        
        var result = await repository.Get(9, new ContextSession());
        
        // Assert
        Assert.NotNull(result);
        Assert.Equal( "Etablissement 270", result.Nom);
        Assert.Equal( originalDateCreMkgt, result.DateCreMKGT);
        Assert.Equal( originalDateCreOdoo, result.DateCreOdoo);
        Assert.Equal( originalDateCre, result.CreateDate);
        Assert.Equal( originalCreatedBy, result.CreatedBy);
    }
    
    [Fact]
    public async Task UpdateWithNewSiretAsync_ShouldReturnEtablissementWithNewSiret_WhenEtablissementIsValid()
    {
        // Arrange
        var repository = new EtablissementClientRepository(_context, _mockHttpContextAccessor.Object, _mockBaseRepos.Object, _mockEtablissementFicheRepos.Object ,_mockMapper.Object);

        var clientToUpdate = new EtablissementClient
        {
            Id = 10,
            Nom = "Etablissement 4",
            Siret = "05680065900858",// Act
            Siren = "056800659",
        };

        _context.Add(clientToUpdate);
        await _context.SaveChangesAsync();

        var clientWithUpdatedData = new EtablissementClient
        {
            Id = 10,
            Nom = "Etablissement 5",
            Siret = "05680065900858",
            Siren = "056800659",
        };
        
        var trackedEntities = _context.ChangeTracker.Entries<EtablissementClient>();
        foreach (var entity in trackedEntities)
        {
            entity.State = EntityState.Detached; // Détacher l'entité pour éviter les erreurs de duplication
        }
        
        // Act
        await repository.UpdateWithNewSiretAsync(clientWithUpdatedData, new ContextSession());
        
        await _context.SaveChangesAsync();
       
        var result = await _context.EtablissementClient.FirstOrDefaultAsync(ec => ec.Siret == clientWithUpdatedData.Siret);
        
        // Assert
        Assert.NotNull(result);
        Assert.Equal(result.Siret, clientToUpdate.Siret);
        Assert.Equal( result.Nom, clientWithUpdatedData.Nom);
    }
}