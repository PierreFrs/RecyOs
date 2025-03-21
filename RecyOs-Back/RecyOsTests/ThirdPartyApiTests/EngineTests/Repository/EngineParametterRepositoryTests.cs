using Microsoft.EntityFrameworkCore;
using Moq;
using RecyOs.Engine.Interfaces;
using RecyOs.Engine.Repository;
using RecyOs.Helpers;
using RecyOs.ORM.Entities;
using RecyOsTests.Interfaces;

namespace RecyOsTests.ThirdPartyApiTests.EngineTests.Repository;

public class EngineParametterRepositoryTests
{
    private readonly DataContext _context;
    private readonly EngineParametterRepository _repository;

    public EngineParametterRepositoryTests(IEngineDataContextTests dataContextTests)
    {
        _context = dataContextTests.GetContext();
        var mockDataContextEngine = new Mock<IDataContextEngine>();
        mockDataContextEngine.Setup(x => x.GetContext()).Returns(_context);
        _repository = new EngineParametterRepository(mockDataContextEngine.Object);
    }

    [Fact]
    public async Task GetAsync_ValidId_ReturnsParameter()
    {
        // Arrange
        const int parameterId = 1;

        // Act
        var parameter = await _repository.GetAsync(parameterId);

        // Assert
        Assert.NotNull(parameter);
        Assert.Equal("Param1", parameter.Nom);
        Assert.Equal("Value1", parameter.Valeur);
        Assert.Equal("Test", parameter.Module);
    }

    [Fact]
    public async Task GetAsync_InvalidId_ReturnsNull()
    {
        // Arrange
        const int invalidId = 999;

        // Act
        var parameter = await _repository.GetAsync(invalidId);

        // Assert
        Assert.Null(parameter);
    }

    [Fact]
    public async Task GetAsync_DeletedParameter_NotReturnedByDefault()
    {
        // Arrange
        var parameter = await _repository.GetAsync(2);
        parameter.IsDeleted = true;
        await _context.SaveChangesAsync();

        // Act
        var result = await _repository.GetAsync(parameter.Id);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task GetAsync_DeletedParameter_ReturnedWhenIncludeDeleted()
    {
        // Arrange
        var parameter = await _repository.GetAsync(3);
        parameter.IsDeleted = true;
        await _context.SaveChangesAsync();

        // Act
        var result = await _repository.GetAsync(parameter.Id, true);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(parameter.Id, result.Id);
    }

    [Fact]
    public async Task GetByNomAsync_ValidModuleAndName_ReturnsParameter()
    {
        // Arrange
        const string module = "Test";
        const string nom = "Param1";

        // Act
        var parameter = await _repository.GetByNomAsync(module, nom);

        // Assert
        Assert.NotNull(parameter);
        Assert.Equal(nom, parameter.Nom);
        Assert.Equal(module, parameter.Module);
        Assert.Equal("Value1", parameter.Valeur);
    }

    [Fact]
    public async Task GetByNomAsync_InvalidModule_ReturnsNull()
    {
        // Arrange
        const string invalidModule = "InvalidModule";
        const string nom = "Param1";

        // Act
        var parameter = await _repository.GetByNomAsync(invalidModule, nom);

        // Assert
        Assert.Null(parameter);
    }

    [Fact]
    public async Task GetByNomAsync_InvalidName_ReturnsNull()
    {
        // Arrange
        const string module = "Test";
        const string invalidNom = "InvalidParam";

        // Act
        var parameter = await _repository.GetByNomAsync(module, invalidNom);

        // Assert
        Assert.Null(parameter);
    }

    [Fact]
    public async Task CreateAsync_ValidParameter_CreatesAndReturnsParameter()
    {
        // Arrange
        var newParameter = new Parameter
        {
            Nom = "NewParam",
            Valeur = "NewValue",
            Module = "Test",
            CreatedBy = "Test engineer",
            CreateDate = DateTime.Now
        };

        // Act
        var createdParameter = await _repository.CreateAsync(newParameter);

        // Assert
        Assert.NotNull(createdParameter);
        Assert.NotEqual(0, createdParameter.Id);
        Assert.Equal(newParameter.Nom, createdParameter.Nom);
        Assert.Equal(newParameter.Valeur, createdParameter.Valeur);
        Assert.Equal(newParameter.Module, createdParameter.Module);

        // Verify in database
        var dbParameter = await _context.Parameters.FindAsync(createdParameter.Id);
        Assert.NotNull(dbParameter);
        Assert.Equal(newParameter.Nom, dbParameter.Nom);
    }
}
