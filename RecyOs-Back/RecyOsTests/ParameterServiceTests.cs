using AutoMapper;
using Moq;
using RecyOs.ORM.DTO;
using RecyOs.ORM.Entities;
using RecyOs.ORM.Filters;
using RecyOs.ORM.Interfaces;
using RecyOs.ORM.Interfaces.IParameters;
using RecyOs.ORM.Service;

namespace RecyOsTests;

public class ParameterServiceTests
{
    private readonly Mock<ICurrentContextProvider> _currentContextProviderMock;
    private readonly Mock<IParameterRepository<Parameter>> _parameterRepositoryMock;
    private readonly IMapper _mapper;
    
    public ParameterServiceTests()
    {
        _currentContextProviderMock = new Mock<ICurrentContextProvider>();
        _parameterRepositoryMock = new Mock<IParameterRepository<Parameter>>();
        var mapperConfig = new MapperConfiguration(cfg =>
        {
            cfg.CreateMap<Parameter, ParameterDto>().ReverseMap();
        });
        _mapper = mapperConfig.CreateMapper();
    }
    
    /* Getters */
    [Fact]
    public async Task GetAsync_ReturnsParameterDto_WhenGetAsyncIsSuccessful()
    {
        // Arrange
        var parameter = new Parameter
        {
            Id = 1,
            Nom = "Param1",
            Valeur = "Value1",
            Module = "Test",
            CreatedBy = "Test engineer"
        };
        _parameterRepositoryMock.Setup(x => x.GetAsync(1, It.IsAny<ContextSession>(), false))
            .ReturnsAsync(parameter);
        var parameterService = new ParameterService<Parameter>(
            _currentContextProviderMock.Object,
            _parameterRepositoryMock.Object,
            _mapper);
        
        // Act
        var result = await parameterService.GetById(1);
        
        // Assert
        Assert.NotNull(result);
        Assert.Equal("Param1", result.Nom);
        Assert.Equal("Value1", result.Valeur);
        Assert.Equal("Test", result.Module);
    }
    
    [Fact]
    public async Task GetAsync_ReturnsNull_WhenGetAsyncIsUnsuccessful()
    {
        // Arrange
        _parameterRepositoryMock.Setup(x => x.GetAsync(0, It.IsAny<ContextSession>(), false))
            .ReturnsAsync((Parameter)null);
        var parameterService = new ParameterService<Parameter>(
            _currentContextProviderMock.Object,
            _parameterRepositoryMock.Object,
            _mapper);
        
        // Act
        var result = await parameterService.GetById(0);
        
        // Assert
        Assert.Null(result);
    }
    
    [Fact]
    public async Task GetByNomAsync_ReturnsParameterDto_WhenGetByNomAsyncIsSuccessful()
    {
        // Arrange
        var parameter = new Parameter
        {
            Id = 1,
            Nom = "Param1",
            Valeur = "Value1",
            Module = "Test",
            CreatedBy = "Test engineer"
        };
        _parameterRepositoryMock.Setup(x => x.GetByNom("Test", "Param1", It.IsAny<ContextSession>(), false))
            .ReturnsAsync(parameter);
        var parameterService = new ParameterService<Parameter>(
            _currentContextProviderMock.Object,
            _parameterRepositoryMock.Object,
            _mapper);
        
        // Act
        var result = await parameterService.GetByNom("Test", "Param1");
        
        // Assert
        Assert.NotNull(result);
        Assert.Equal("Param1", result.Nom);
        Assert.Equal("Value1", result.Valeur);
        Assert.Equal("Test", result.Module);
    }
    
    [Fact]
    public async Task GetByNomAsync_ReturnsNull_WhenGetByNomAsyncIsUnsuccessful()
    {
        // Arrange
        _parameterRepositoryMock.Setup(x => x.GetByNom("Test", "Param10", It.IsAny<ContextSession>(), false))
            .ReturnsAsync((Parameter)null);
        var parameterService = new ParameterService<Parameter>(
            _currentContextProviderMock.Object,
            _parameterRepositoryMock.Object,
            _mapper);
        
        // Act
        var result = await parameterService.GetByNom("Test", "Param10");
        
        // Assert
        Assert.Null(result);
    }
    
    [Fact]
    public async Task GetDataForGrid_ReturnsGridData_WhenGetDataForGridAsyncIsSuccessful()
    {
        // Arrange
        var parameterFilter = new ParameterFilter
        {
            PageNumber = 1,
            PageSize = 10
        };
        var tuple = new Tuple<IEnumerable<Parameter>, int>(new List<Parameter>
        {
            new Parameter
            {
                Id = 1,
                Nom = "Param1",
                Valeur = "Value1",
                Module = "Test",
                CreatedBy = "Test engineer"
            },
            new Parameter
            {
                Id = 2,
                Nom = "Param2",
                Valeur = "Value2",
                Module = "Test",
                CreatedBy = "Test engineer"
            }
        }, 2);
        _parameterRepositoryMock.Setup(x => x.GetDataForGrid(parameterFilter, It.IsAny<ContextSession>(), false))
            .ReturnsAsync((tuple.Item1, tuple.Item2));
        var parameterService = new ParameterService<Parameter>(
            _currentContextProviderMock.Object,
            _parameterRepositoryMock.Object,
            _mapper);
        
        // Act
        var result = await parameterService.GetDataForGrid(parameterFilter);
        
        // Assert
        Assert.NotNull(result);
        Assert.IsType<GridData<ParameterDto>>(result);
        Assert.Equal(2, result.Items.Count());
        Assert.Equal(2, result.Paginator.length);
    }
    
    [Fact]
    public async Task GetDataForGrid_ReturnsEmptyGridData_WhenGetDataForGridAsyncIsUnsuccessful()
    {
        // Arrange
        var parameterFilter = new ParameterFilter
        {
            PageNumber = 10,
            PageSize = 2
        };
        var tuple = new Tuple<IEnumerable<Parameter>, int>(new List<Parameter>(), 0);
        _parameterRepositoryMock.Setup(x => x.GetDataForGrid(parameterFilter, It.IsAny<ContextSession>(), false))
            .ReturnsAsync((tuple.Item1, tuple.Item2));
        var parameterService = new ParameterService<Parameter>(
            _currentContextProviderMock.Object,
            _parameterRepositoryMock.Object,
            _mapper);
        
        // Act
        var result = await parameterService.GetDataForGrid(parameterFilter);
        
        // Assert
        Assert.NotNull(result);
        Assert.IsType<GridData<ParameterDto>>(result);
        Assert.Empty(result.Items);
        Assert.Equal(0, result.Paginator.length);
    }
    
    /* Creators */
    [Fact]
    public async Task CreateAsync_ReturnsParameterDto_WhenCreateAsyncIsSuccessful()
    {
        // Arrange
        var parameterDto = new ParameterDto
        {
            Nom = "Param1",
            Valeur = "Value1",
            Module = "Test",
        };
        var parameter = new Parameter
        {
            Id = 1,
            Nom = "Param1",
            Valeur = "Value1",
            Module = "Test",
            CreatedBy = "Test engineer"
        };
        _parameterRepositoryMock.Setup(x => x.CreateAsync(It.IsAny<Parameter>(), It.IsAny<ContextSession>()))
            .ReturnsAsync(parameter);
        var parameterService = new ParameterService<Parameter>(
            _currentContextProviderMock.Object,
            _parameterRepositoryMock.Object,
            _mapper);
        
        // Act
        var result = await parameterService.CreateAsync(parameterDto);
        
        // Assert
        Assert.NotNull(result);
        Assert.Equal("Param1", result.Nom);
        Assert.Equal("Value1", result.Valeur);
        Assert.Equal("Test", result.Module);
    }
    
    [Fact]
    public async Task CreateAsync_ReturnsNull_WhenCreateAsyncIsUnsuccessful()
    {
        // Arrange
        var parameterDto = new ParameterDto
        {
            Nom = "Param1",
            Valeur = "Value1",
            Module = "Test",
        };
        _parameterRepositoryMock.Setup(x => x.CreateAsync(It.IsAny<Parameter>(), It.IsAny<ContextSession>()))
            .ReturnsAsync((Parameter)null);
        var parameterService = new ParameterService<Parameter>(
            _currentContextProviderMock.Object,
            _parameterRepositoryMock.Object,
            _mapper);
        
        // Act
        var result = await parameterService.CreateAsync(parameterDto);
        
        // Assert
        Assert.Null(result);
    }
    
    /* Updaters */
    [Fact]
    public async Task UpdateAsync_ReturnsParameterDto_WhenUpdateAsyncIsSuccessful()
    {
        // Arrange
        var parameterDto = new ParameterDto
        {
            Id = 1,
            Nom = "Param1",
            Valeur = "Value1",
            Module = "Test",
        };
        var parameter = new Parameter
        {
            Id = 1,
            Nom = "Param1",
            Valeur = "Value1",
            Module = "Test",
            CreatedBy = "Test engineer"
        };
        _parameterRepositoryMock.Setup(x => x.UpdateAsync(It.IsAny<Parameter>(), It.IsAny<ContextSession>()))
            .ReturnsAsync(parameter);
        var parameterService = new ParameterService<Parameter>(
            _currentContextProviderMock.Object,
            _parameterRepositoryMock.Object,
            _mapper);
        
        // Act
        var result = await parameterService.UpdateAsync(parameterDto);
        
        // Assert
        Assert.NotNull(result);
        Assert.Equal("Param1", result.Nom);
        Assert.Equal("Value1", result.Valeur);
        Assert.Equal("Test", result.Module);
    }
    
    [Fact]
    public async Task UpdateAsync_ReturnsNull_WhenUpdateAsyncIsUnsuccessful()
    {
        // Arrange
        var parameterDto = new ParameterDto
        {
            Id = 1,
            Nom = "Param1",
            Valeur = "Value1",
            Module = "Test",
        };
        _parameterRepositoryMock.Setup(x => x.UpdateAsync(It.IsAny<Parameter>(), It.IsAny<ContextSession>()))
            .ReturnsAsync((Parameter)null);
        var parameterService = new ParameterService<Parameter>(
            _currentContextProviderMock.Object,
            _parameterRepositoryMock.Object,
            _mapper);
        
        // Act
        var result = await parameterService.UpdateAsync(parameterDto);
        
        // Assert
        Assert.Null(result);
    }
    
    /* Deleters */
    [Fact]
    public async Task DeleteAsync_ReturnsTrue_WhenDeleteAsyncIsSuccessful()
    {
        // Arrange
        _parameterRepositoryMock.Setup(x => x.DeleteAsync(1, It.IsAny<ContextSession>()))
            .ReturnsAsync(true);
        var parameterService = new ParameterService<Parameter>(
            _currentContextProviderMock.Object,
            _parameterRepositoryMock.Object,
            _mapper);
        
        // Act
        var result = await parameterService.DeleteAsync(1);
        
        // Assert
        Assert.True(result);
    }
    
    [Fact]
    public async Task DeleteAsync_ReturnsFalse_WhenDeleteAsyncIsUnsuccessful()
    {
        // Arrange
        _parameterRepositoryMock.Setup(x => x.DeleteAsync(0, It.IsAny<ContextSession>()))
            .ReturnsAsync(false);
        var parameterService = new ParameterService<Parameter>(
            _currentContextProviderMock.Object,
            _parameterRepositoryMock.Object,
            _mapper);
        
        // Act
        var result = await parameterService.DeleteAsync(0);
        
        // Assert
        Assert.False(result);
    }
}