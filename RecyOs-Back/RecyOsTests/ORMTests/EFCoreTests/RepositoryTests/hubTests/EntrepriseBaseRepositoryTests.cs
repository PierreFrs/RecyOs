using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Moq;
using RecyOs.Helpers;
using RecyOs.ORM.EFCore.Repository.hub;
using RecyOs.ORM.Entities;
using RecyOs.ORM.Entities.hub;
using RecyOs.ORM.Filters.hub;
using RecyOs.ORM.Interfaces.hub;
using RecyOsTests.Interfaces;

namespace RecyOsTests.ORMTests.EFCoreTests.RepositoryTests.hubTests;

public class EntrepriseBaseRepositoryTests
{
    private readonly DataContext _context;
    private readonly Mock<IHttpContextAccessor> _mockHttpContextAccessor;
    
    public EntrepriseBaseRepositoryTests(IDataContextTests dataContextTests)
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
    public async Task Get_ById_ShouldReturnEntrepriseBase()
    {
        // Arrange
        var repository = new EntrepriseBaseRepository(_context, _mockHttpContextAccessor.Object);

        // Act
        var entrepriseBase = await repository.Get(1, new ContextSession());
        _context.Entry(entrepriseBase).State = EntityState.Detached;

        // Assert
        Assert.NotNull(entrepriseBase);
        Assert.Equal("056800659", entrepriseBase.Siren);
    }
    
    [Fact]
    public async Task Get_BySiren_ShouldReturnEntrepriseBase()
    {
        // Arrange
        var repository = new EntrepriseBaseRepository(_context, _mockHttpContextAccessor.Object);

        // Act
        var entrepriseBase = await repository.GetBySiren("056800659", new ContextSession());
        _context.Entry(entrepriseBase).State = EntityState.Detached;

        // Assert
        Assert.NotNull(entrepriseBase);
        Assert.Equal("056800659", entrepriseBase.Siren);
    }
    
    [Fact]
    public async Task GetFiltredListWithCount_ShouldReturnEntrepriseBaseList()
    {
        // Arrange
        var repository = new EntrepriseBaseRepository(_context, _mockHttpContextAccessor.Object);
        var filter = new EntrepriseBaseGridFilter { };

        // Act
        var entrepriseBaseList = await repository.GetFiltredListWithCount(filter, new ContextSession());
        int count = _context.EntrepriseBase.Count();
        foreach (var result in entrepriseBaseList.Item1)
        {
            _context.Entry(result).State = EntityState.Detached;
        }
        // Assert
        Assert.Equal(count, entrepriseBaseList.Item2);
    }
    
    [Fact]
    public async Task Exists_ShouldReturnTrue_WheObjectExists()
    {
        // Arrange
        var repository = new EntrepriseBaseRepository(_context, _mockHttpContextAccessor.Object);
        var entrepriseBase = new EntrepriseBase
        {
           Siren = "056800659"
        };

        // Act
        var exists = await repository.Exists(entrepriseBase, new ContextSession());

        // Assert
        Assert.True(exists);
    }
    
    [Fact]
    public async Task Exists_ShouldReturnFalse_WheObjectDoesNotExists()
    {
        // Arrange
        var repository = new EntrepriseBaseRepository(_context, _mockHttpContextAccessor.Object);
        var entrepriseBase = new EntrepriseBase
        {
           Siren = "000000000"
        };

        // Act
        var exists = await repository.Exists(entrepriseBase, new ContextSession(), true);

        // Assert
        Assert.False(exists);
    }
    
    [Fact]
    public void GetCurrentUserName_ShouldReturnCurrentUserName()
    {
        // Arrange
        var repository = new EntrepriseBaseRepository(_context, _mockHttpContextAccessor.Object);

        // Act
        var userName = repository.GetCurrentUserName();

        // Assert
        Assert.Equal("John DOE", userName);
    }
    
    /*********** Create ***********/
    [Fact]
    public async Task Create_ShouldReturnEntrepriseBase()
    {
        // Arrange
        var repository = new EntrepriseBaseRepository(_context, _mockHttpContextAccessor.Object);
        var entrepriseBase = new EntrepriseBase
        {
            Siren = "000000000",
            SirenFormate = "000 000 000",
            NomEntreprise = "Test",
            PersonneMorale = true,
            Denomination = "Test",
            Nom = null,
            Prenom = null,
            Sexe = null,
            CodeNaf = "64.19Z",
            LibelleCodeNaf = "Autres intermédiations monétaires",
            DomaineActivite = "Banques, assurances, services financiers",
            DateCreation = new DateTime(1958, 1, 1, 16, 23, 42, DateTimeKind.Utc),
            DateCreationFormate = "01/01/1958"
        };

        // Act
        var entrepriseBaseCreated = await repository.Create(entrepriseBase, new ContextSession());

        // Assert
        Assert.NotNull(entrepriseBaseCreated);
        Assert.Equal("000000000", entrepriseBaseCreated.Siren);
        
        // Clean
        _context.Entry(entrepriseBaseCreated).State = EntityState.Detached;
        
        _context.EntrepriseBase.Remove(entrepriseBaseCreated);
        await _context.SaveChangesAsync();
    }
    
    [Fact]
    public async Task Create_ExistingObject_ShouldReturnUpdatedEntrepriseBase()
    {
        // Arrange
        var repository = new EntrepriseBaseRepository(_context, _mockHttpContextAccessor.Object);
        var entrepriseBase = new EntrepriseBase
        {
            Siren = "056800659",
            SirenFormate = "056 800 659",
            NomEntreprise = "NEW SOCIETE GENERAL",
            PersonneMorale = true,
            Denomination = "SOCIETE GENERALE",
            Nom = null,
            Prenom = null,
            Sexe = null,
            CodeNaf = "64.19Z",
            LibelleCodeNaf = "Autres intermédiations monétaires",
            DomaineActivite = "Banques, assurances, services financiers",
            DateCreation = new DateTime(1958, 1, 1, 16, 23, 42, DateTimeKind.Utc),
            DateCreationFormate = "01/01/1958"
        };

        // Act
        var entrepriseBaseCreated = await repository.Create(entrepriseBase, new ContextSession());
        var obj = await repository.GetBySiren("056800659", new ContextSession());

        // Assert
        Assert.NotNull(entrepriseBaseCreated);
        Assert.Equal("056800659", entrepriseBaseCreated.Siren);
        Assert.Equal("NEW SOCIETE GENERAL", obj.NomEntreprise);
        Assert.Equal(1, obj.Id);
        Assert.Equal("Test engineer", obj.CreatedBy);
        
        // Clean
        _context.Entry(obj).State = EntityState.Detached;
        _context.Entry(entrepriseBaseCreated).State = EntityState.Detached;
    }

}