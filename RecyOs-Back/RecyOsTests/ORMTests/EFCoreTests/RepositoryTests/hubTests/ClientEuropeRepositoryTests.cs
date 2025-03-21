using System.Security.Claims;
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

public class ClientEuropeRepositoryTests
{
    private readonly DataContext _context;
    private readonly Mock<IHttpContextAccessor> _mockHttpContextAccessor;
    
    public ClientEuropeRepositoryTests(IDataContextTests dataContextTests)
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
    public async Task Get_ById_ShouldReturnClientEurope()
    {
        // Arrange
        var repository = new ClientEuropeRepository(_context, _mockHttpContextAccessor.Object);
        var id = 1;
        
        // Act
        var result = await repository.Get(id, new ContextSession());
        _context.Entry(result).State = EntityState.Detached;
        
        // Assert
        Assert.NotNull(result);
        Assert.Equal(id, result.Id);
        Assert.Equal("BE0421064033", result.Vat);
        Assert.Equal("411107", result.CompteComptable);
    }
    
    [Fact]
    public async Task Get_ById_ShouldReturnNull_IfIdDoesntExist()
    {
        // Arrange
        var repository = new ClientEuropeRepository(_context, _mockHttpContextAccessor.Object);
        var id = 999;
        
        // Act
        var result = await repository.Get(id, new ContextSession());
        
        // Assert
        Assert.Null(result);
    }
    
    [Fact]
    public async Task GetByVat_ShouldReturnClientEurope()
    {
        // Arrange
        var repository = new ClientEuropeRepository(_context, _mockHttpContextAccessor.Object);
        var vat = "BE0421064033";
        
        // Act
        var result = await repository.GetByVat(vat, new ContextSession());
        _context.Entry(result).State = EntityState.Detached;
        
        // Assert
        Assert.NotNull(result);
        Assert.Equal(vat, result.Vat);
        Assert.Equal("411107", result.CompteComptable);
    }
    
    [Fact]
    public async Task GetByVat_ShouldReturnNull_IfVatDoesntExist()
    {
        // Arrange
        var repository = new ClientEuropeRepository(_context, _mockHttpContextAccessor.Object);
        var vat = "BE9999999999";
        
        // Act
        var result = await repository.GetByVat(vat, new ContextSession());
        
        // Assert
        Assert.Null(result);
    }
    
    [Fact]
    public async Task GetFiltredListWithCount_ShouldReturnClientEuropeList()
    {
        // Arrange
        var repository = new ClientEuropeRepository(_context, _mockHttpContextAccessor.Object);
        var filter = new ClientEuropeGridFilter
        {
            PageNumber = 0,
            PageSize = 10
        };
        
        
        // Act
        var result = await repository.GetFiltredListWithCount(filter, new ContextSession());
        foreach (var r in result.Item1)
        {
            _context.Entry(r).State = EntityState.Detached;
        }
        
        // Assert
        Assert.NotNull(result);
        int elementsCount = _context.ClientEurope.Count();
        Assert.Equal(elementsCount, result.Item2);
        Assert.Equal(elementsCount, result.Item1.Count());
    }
    
    [Fact]
    public async Task GetByCodeMkgt_ShouldReturnClientEurope()
    {
        // Arrange
        var repository = new ClientEuropeRepository(_context, _mockHttpContextAccessor.Object);
        var codeMkgt = "TESCLIE001";
        
        // Act
        var result = await repository.GetByCodeMkgt(codeMkgt, new ContextSession());
        _context.Entry(result).State = EntityState.Detached;
        
        // Assert
        Assert.NotNull(result);
        Assert.Equal(codeMkgt, result.CodeMkgt);
        Assert.Equal("411107", result.CompteComptable);
        Assert.Equal("BE0421064033", result.Vat);
    }
    
    [Fact]
    public async Task GetByCodeMkgt_ShouldReturnNull_IfCodeMkgtDoesntExist()
    {
        // Arrange
        var repository = new ClientEuropeRepository(_context, _mockHttpContextAccessor.Object);
        var codeMkgt = "TESCLIE999";
        
        // Act
        var result = await repository.GetByCodeMkgt(codeMkgt, new ContextSession());
        
        // Assert
        Assert.Null(result);
    }
    
    [Fact]
    public async Task GetByIdOdoo_ShouldReturnClientEurope()
    {
        // Arrange
        var repository = new ClientEuropeRepository(_context, _mockHttpContextAccessor.Object);
        var idOdoo = "9DOO0001";
        
        // Act
        var result = await repository.GetByIdOdoo(idOdoo, new ContextSession());
        _context.Entry(result).State = EntityState.Detached;
        
        // Assert
        Assert.NotNull(result);
        Assert.Equal(idOdoo, result.IdOdoo);
        Assert.Equal("411107", result.CompteComptable);
        Assert.Equal("BE0421064033", result.Vat);
    }
    
    [Fact]
    public async Task GetByIdOdoo_ShouldReturnNull_IfIdOdooDoesntExist()
    {
        // Arrange
        var repository = new ClientEuropeRepository(_context, _mockHttpContextAccessor.Object);
        var idOdoo = "9DOO9999";
        
        // Act
        var result = await repository.GetByIdOdoo(idOdoo, new ContextSession());
        
        // Assert
        Assert.Null(result);
    }
    
    [Fact]
    public void GetCurrentUserName_ShouldReturnCurrentUserName()
    {
        // Arrange
        var repository = new ClientEuropeRepository(_context, _mockHttpContextAccessor.Object);
        
        // Act
        var userName = repository.GetCurrentUserName();
        
        // Assert
        Assert.NotNull(userName);
        Assert.Equal("John DOE", userName);
    }
    
    [Fact]
    public async Task GetByCodeKerlog_ShouldReturnClientEurope()
    {
        // Arrange
        var repository = new ClientEuropeRepository(_context, _mockHttpContextAccessor.Object);
        var codeKerlog = "900001";
        
        // Act
        var result =await repository.GetByCodeKerlog(codeKerlog, new ContextSession());
        _context.Entry(result).State = EntityState.Detached;
        
        // Assert
        Assert.NotNull(result);
        Assert.Equal(codeKerlog, result.CodeKerlog);
        Assert.Equal("411107", result.CompteComptable);
        Assert.Equal("BE0421064033", result.Vat);
    }
    
    [Fact]
    public async Task GetByCodeKerlog_ShouldReturnNull_IfCodeKerlogDoesntExist()
    {
        // Arrange
        var repository = new ClientEuropeRepository(_context, _mockHttpContextAccessor.Object);
        var codeKerlog = "900999";
        
        // Act
        var result = await repository.GetByCodeKerlog(codeKerlog, new ContextSession());
        
        // Assert
        Assert.Null(result);
    }
    
    [Fact]
    public async Task Exists_ShouldReturnTrue_IfClientEuropeExists()
    {
        // Arrange
        var repository = new ClientEuropeRepository(_context, _mockHttpContextAccessor.Object);
        var clientEurope = new ClientEurope
        {
            Vat = "BE0421064033",
        };
        
        // Act
        var result = await repository.Exists(clientEurope, new ContextSession());
        
        // Assert
        Assert.True(result);
    }
    
    [Fact]
    public async Task Exists_ShouldReturnFalse_IfClientEuropeDoesntExist()
    {
        // Arrange
        var repository = new ClientEuropeRepository(_context, _mockHttpContextAccessor.Object);
        var clientEurope = new ClientEurope
        {
            Vat = "BE9999999999",
        };
        
        // Act
        var result = await repository.Exists(clientEurope, new ContextSession());
        
        // Assert
        Assert.False(result);
    }
    
    /*********** Create ***********/
    [Fact]
    public async Task Create_ShouldReturnClientEurope_IfClientEuropeDoesntExist()
    {
        // Arrange
        var repository = new ClientEuropeRepository(_context, _mockHttpContextAccessor.Object);
        var clientEurope = new ClientEurope
        {
            Vat = "BE8999999999",
            CodePostalFacturation = "12345",
            VilleFacturation = "MOUSCRON",
        };
        
        // Act
        var result = await repository.CreateAsync(clientEurope, new ContextSession());
        
        // Assert
        Assert.NotNull(result);
        Assert.Equal(clientEurope.Vat, result.Vat);
        Assert.Equal(clientEurope.CodePostalFacturation, result.CodePostalFacturation);
        
        _context.Entry(result).State = EntityState.Detached;
        
        var result2 = await repository.CreateAsync(clientEurope, new ContextSession());
        
        // Assert
        Assert.Equal(result.Id, result2.Id);
    }
    
    
    /*********** Update ***********/
    [Fact]
    public async Task Update_ShouldReturnClientEurope_IfClientEuropeExists()
    {
        // Arrange
        var repository = new ClientEuropeRepository(_context, _mockHttpContextAccessor.Object);

        var clientEurope = repository.Get(2, new ContextSession()).Result;
        var originalDateCreMkgt = clientEurope.DateCreMkgt;
        var originalDateCreOdoo = clientEurope.DateCreOdoo;
        var originalDateCre = clientEurope.CreateDate;
        var originalCreatedBy = clientEurope.CreatedBy;

        clientEurope.Nom = "Client Europe 2";
        clientEurope.Vat = "BE0407113453";
        clientEurope.DateCreMkgt = new DateTime(2021, 1, 1, 0, 0, 0, DateTimeKind.Utc);
        clientEurope.DateCreOdoo = new DateTime(2021, 1, 1, 0, 0, 0, DateTimeKind.Utc);
        clientEurope.CreateDate = new DateTime(2021, 1, 1, 0, 0, 0, DateTimeKind.Utc);

        
        var trackedEntities = _context.ChangeTracker.Entries<ClientEurope>();
        foreach (var entity in trackedEntities)
        {
            entity.State = EntityState.Detached; // Détacher l'entité pour éviter les erreurs de duplication
        }
        
        // Act
        await repository.UpdateAsync(clientEurope, new ContextSession());
        var result = await repository.Get(2, new ContextSession());
        
        // Assert
        Assert.NotNull(result);
        Assert.Equal(clientEurope.Nom, result.Nom);
        Assert.Equal("BE0407113453", result.Vat);
        Assert.Equal( originalDateCreMkgt, result.DateCreMkgt);
        Assert.Equal( originalDateCreOdoo, result.DateCreOdoo);
        Assert.Equal( originalDateCre, result.CreateDate);
        Assert.Equal( originalCreatedBy, result.CreatedBy);
    }

    [Fact]
    public async Task Update_CodesCli_ShouldNotChangeDateCre()
    {
        // Arrange
        var repository = new ClientEuropeRepository(_context, _mockHttpContextAccessor.Object);
        var clientEurope = repository.Get(3, new ContextSession()).Result;
        var originalDateCreMkgt = clientEurope.DateCreMkgt;
        var originalDateCreOdoo = clientEurope.DateCreOdoo;
        var originalDateCre = clientEurope.CreateDate;
        var originalCreatedBy = clientEurope.CreatedBy;
        
        clientEurope.CodeMkgt = null;
        clientEurope.IdOdoo = null;
        
        var trackedEntities = _context.ChangeTracker.Entries<ClientEurope>();
        foreach (var entity in trackedEntities)
        {
            entity.State = EntityState.Detached; // Détacher l'entité pour éviter les erreurs de duplication
        }
        
        // Act
        await repository.UpdateAsync(clientEurope, new ContextSession());
        var result = await repository.Get(clientEurope.Id, new ContextSession());
        _context.Entry(result).State = EntityState.Detached;
        
        // Assert
        Assert.NotNull(result);
        Assert.Equal(clientEurope.Vat, result.Vat);
        Assert.Equal( originalDateCreMkgt, result.DateCreMkgt);
        Assert.Equal( originalDateCreOdoo, result.DateCreOdoo);
        Assert.Equal( originalDateCre, result.CreateDate);
        Assert.Equal( originalCreatedBy, result.CreatedBy);
    }

    [Fact]
    public async Task Update_CodesCli_ShouldChangeDateUpdate()
    {
        // Arrange
        var repository = new ClientEuropeRepository(_context, _mockHttpContextAccessor.Object);
        var clientEurope = repository.Get(4, new ContextSession()).Result;
        var originalDateCreMkgt = clientEurope.DateCreMkgt;
        var originalDateCreOdoo = clientEurope.DateCreOdoo;
        var originalDateCre = clientEurope.CreateDate;
        var originalCreatedBy = clientEurope.CreatedBy;
        var originalUpdatedBy = clientEurope.UpdatedBy;
        var originalUpdatedAt = clientEurope.UpdatedAt;
        
        clientEurope.VilleFacturation = "MOUSCRON";
        // Act
        var trackedEntities = _context.ChangeTracker.Entries<ClientEurope>();
        foreach (var entity in trackedEntities)
        {
            entity.State = EntityState.Detached; // Détacher l'entité pour éviter les erreurs de duplication
        }
        await repository.UpdateAsync(clientEurope, new ContextSession());
        var result = await repository.Get(clientEurope.Id, new ContextSession());
        _context.Entry(result).State = EntityState.Detached;
        
        // Assert
        Assert.NotNull(result);
        Assert.Equal(clientEurope.Vat, result.Vat);
        Assert.Equal( originalDateCreMkgt, result.DateCreMkgt);
        Assert.Equal( originalDateCreOdoo, result.DateCreOdoo);
        Assert.Equal( originalDateCre, result.CreateDate);
        Assert.Equal( originalCreatedBy, result.CreatedBy);
        Assert.NotEqual( originalUpdatedBy, result.UpdatedBy);
        Assert.NotEqual( originalUpdatedAt, result.UpdatedAt);
    }
    
}