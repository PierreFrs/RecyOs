using System.Security.Claims;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Moq;
using RecyOs.Helpers;
using RecyOs.ORM.EFCore.Repository.hub;
using RecyOs.ORM.Entities;
using RecyOs.ORM.Entities.hub;
using RecyOs.ORM.Filters.hub;
using RecyOsTests.Interfaces;

namespace RecyOsTests.ORMTests.EFCoreTests.RepositoryTests.hubTests;

public class EtablissementFicheRepositoryTests
{
    private readonly DataContext _context;
    private readonly Mock<IHttpContextAccessor> _mockHttpContextAccessor;

    public EtablissementFicheRepositoryTests(IDataContextTests dataContextTests)
    {
        _context = dataContextTests.GetContext();
        _mockHttpContextAccessor = new Mock<IHttpContextAccessor>();
    
        
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
    public async Task Get_ById_ShouldReturnEtablissementFiche()
    {
        // Arrange
        var repository = new EtablissementFicheRepository(_context, _mockHttpContextAccessor.Object);
        var id = 1;
        
        // Act
        var result = await repository.Get(id, new ContextSession());
        _context.Entry(result).State = EntityState.Detached;
        
        // Assert
        Assert.NotNull(result);
        Assert.Equal(id, result.Id);
        Assert.Equal("05680065900858", result.Siret);
    }
    
    [Fact]
    public async Task Get_ById_ShouldReturnNull_IfIdDoesntExist()
    {
        // Arrange
        var repository = new EtablissementFicheRepository(_context, _mockHttpContextAccessor.Object);
        var id = 999;
        
        // Act
        var result = await repository.Get(id, new ContextSession());
        
        // Assert
        Assert.Null(result);
    }
    
    [Fact]
    public async Task GetBySiret_ShouldReturnEtablissementFiche()
    {
        // Arrange
        var repository = new EtablissementFicheRepository(_context, _mockHttpContextAccessor.Object);
        var siret = "05680065900858";
        
        // Act
        var result = await repository.GetBySiret(siret, new ContextSession());
        _context.Entry(result).State = EntityState.Detached;
        
        // Assert
        Assert.NotNull(result);
        Assert.Equal(siret, result.Siret);
        Assert.Equal(1, result.Id);
        Assert.Equal("012345", result.CodePostal);
    }
    
    [Fact]
    public async Task GetFiltredListWithCount_ShouldReturnEtablissementFicheList()
    {
        // Arrange
        var repository = new EtablissementFicheRepository(_context, _mockHttpContextAccessor.Object);
        var filter = new EtablissementFicheGridFilter();
        int elementsCount = await _context.EtablissementFiche.CountAsync();
        
        // Act
        var result = await repository.GetFiltredListWithCount(filter, new ContextSession());
        foreach (var r in result.Item1)
        {
            _context.Entry(r).State = EntityState.Detached;
        }
        
        // Assert
        Assert.Equal(elementsCount, result.Item2);
        Assert.Equal(elementsCount, result.Item1.Count());
    }

    [Fact]
    public void GetCurrentUserName_ShouldReturnCurrentUserName()
    {
        // Arrange
        var repository = new EtablissementFicheRepository(_context, _mockHttpContextAccessor.Object);
        
        // Act
        var result = repository.GetCurrentUserName();
        
        // Assert
        Assert.NotNull(result);
        Assert.Equal("John DOE", result);
    }

    [Fact]
    public async Task Exists_ShouldReturnTrue_IfEtablissementFicheExists()
    {
        // Arrange
        var repository = new EtablissementFicheRepository(_context, _mockHttpContextAccessor.Object);
        var etablissementFiche = new EtablissementFiche
        {
            Siret = "05680065900858",
        };

        // Act
        var result = await repository.Exists(etablissementFiche, new ContextSession());

        // Assert
        Assert.True(result);

    }
    
    [Fact]
    public async Task Exists_ShouldReturnFalse_IfEtablissementFicheDoesntExist()
    {
        // Arrange
        var repository = new EtablissementFicheRepository(_context, _mockHttpContextAccessor.Object);
        var etablissementFiche = new EtablissementFiche
        {
            Siret = "99999999999999",
        };

        // Act
        var result = await repository.Exists(etablissementFiche, new ContextSession());

        // Assert
        Assert.False(result);

    }
    
    /*********** Create ***********/
    [Fact]
    public async Task Create_ShouldReturnEtablissementFiche_IfEtablissementFicheDoesntExist()
    {
        // Arrange
        var repository = new EtablissementFicheRepository(_context, _mockHttpContextAccessor.Object);
        var etablissementFiche = new EtablissementFiche
        {
            Siret = "99999999999999",
            CodePostal = "12345"
        };

        // Act
        var result = await repository.Create(etablissementFiche, new ContextSession());

        // Assert
        Assert.NotNull(result);
        Assert.Equal(etablissementFiche.Siret, result.Siret);
        Assert.Equal(etablissementFiche.CodePostal, result.CodePostal);
    }
    
    [Fact]
    public async Task Create_ShouldReturnEtablissementFiche_IfEtablissementFicheExists()
    {
        // Arrange
        var repository = new EtablissementFicheRepository(_context, _mockHttpContextAccessor.Object);
        var etablissementFiche = new EtablissementFiche
        {
            Siret = "05680065900859",
            CodePostal = "12345"
        };

        // Act
        var result = await repository.Create(etablissementFiche, new ContextSession());

        // Assert
        Assert.NotNull(result);
        Assert.Equal(etablissementFiche.Siret, result.Siret);
        Assert.Equal("12345", result.CodePostal);
    }
}