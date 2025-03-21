using AutoMapper;
using Moq;
using RecyOs.ORM.DTO.hub;
using RecyOs.ORM.Entities;
using RecyOs.ORM.Entities.hub;
using RecyOs.ORM.Filters.hub;
using RecyOs.ORM.Interfaces;
using RecyOs.ORM.Interfaces.hub;
using RecyOs.ORM.Service.hub;

namespace RecyOsTests.ORMTests.hubTests;

public class EtablissementFicheServiceTests
{
    private readonly Mock<ICurrentContextProvider> _mockCurrentContextProvider;
    private readonly IMapper _mapper;
    private readonly Mock<IEtablissementFicheRepository<EtablissementFiche>> _mockRepository;
    
    public EtablissementFicheServiceTests()
    {
        _mockCurrentContextProvider = new Mock<ICurrentContextProvider>();
        _mockRepository = new Mock<IEtablissementFicheRepository<EtablissementFiche>>();
        var mapperConfig = new MapperConfiguration(cfg =>
        {
            cfg.CreateMap<EtablissementFiche, EtablissementFicheDto>();
            cfg.CreateMap<EtablissementFicheDto, EtablissementFiche>();
        });
        _mapper = mapperConfig.CreateMapper();
    }
    
    [Fact]
    public async Task GetDataForGrid_ShouldReturnGridData()
    {
        // Arrange
        var filter = new EtablissementFicheGridFilter();
        var tuple = (new List<EtablissementFiche>(), 0);
        _mockRepository.Setup(repo => repo.GetFiltredListWithCount(filter, It.IsAny<ContextSession>(), false))
            .ReturnsAsync(tuple);
        var service = new EtablissementFicheService<EtablissementFiche>(_mockCurrentContextProvider.Object, _mockRepository.Object, _mapper);
        
        // Act
        var result = await service.GetDataForGrid(filter);
        
        // Assert
        Assert.NotNull(result);
        Assert.IsType<GridData<EtablissementFicheDto>>(result);
        _mockRepository.Verify(repo => repo.GetFiltredListWithCount(It.IsAny<EtablissementFicheGridFilter>(), It.IsAny<ContextSession>(), false), Times.Once);
    }
    
    [Fact]
    public async Task GetById_ShouldReturnEtablissementFicheDto()
    {
        // Arrange
        var etablissementFiche = new EtablissementFiche();
        _mockRepository.Setup(repo => repo.Get(It.IsAny<int>(), It.IsAny<ContextSession>(), false))
            .ReturnsAsync(etablissementFiche);
        var service = new EtablissementFicheService<EtablissementFiche>(_mockCurrentContextProvider.Object, _mockRepository.Object, _mapper);
        
        // Act
        var result = await service.GetById(1);
        
        // Assert
        Assert.NotNull(result);
        Assert.IsType<EtablissementFicheDto>(result);
        _mockRepository.Verify(repo => repo.Get(It.IsAny<int>(), It.IsAny<ContextSession>(), false), Times.Once);
    }
    
    [Fact]
    public async Task GetBySiret_ShouldReturnEtablissementFicheDto()
    {
        // Arrange
        var etablissementFiche = new EtablissementFiche();
        _mockRepository.Setup(repo => repo.GetBySiret(It.IsAny<string>(), It.IsAny<ContextSession>(), false))
            .ReturnsAsync(etablissementFiche);
        var service = new EtablissementFicheService<EtablissementFiche>(_mockCurrentContextProvider.Object, _mockRepository.Object, _mapper);
        
        // Act
        var result = await service.GetBySiret("123456789", false);
        
        // Assert
        Assert.NotNull(result);
        Assert.IsType<EtablissementFicheDto>(result);
        _mockRepository.Verify(repo => repo.GetBySiret(It.IsAny<string>(), It.IsAny<ContextSession>(), false), Times.Once);
    }
    
    [Fact]
    public async Task Delete_ShouldReturnTrue()
    {
        // Arrange
        _mockRepository.Setup(repo => repo.Delete(It.IsAny<int>(), It.IsAny<ContextSession>()));
        var service = new EtablissementFicheService<EtablissementFiche>(_mockCurrentContextProvider.Object, _mockRepository.Object, _mapper);
        
        // Act
        var result = await service.Delete(1);
        
        // Assert
        Assert.True(result);
        _mockRepository.Verify(repo => repo.Delete(It.IsAny<int>(), It.IsAny<ContextSession>()), Times.Once);
    }
    
    [Fact]
    public async Task Edit_ShouldReturnEtablissementFicheDto()
    {
        // Arrange
        var etablissementFiche = new EtablissementFiche();
        _mockRepository.Setup(repo => repo.UpdateAsync(It.IsAny<EtablissementFiche>(), It.IsAny<ContextSession>()))
            .ReturnsAsync(etablissementFiche);
        var service = new EtablissementFicheService<EtablissementFiche>(_mockCurrentContextProvider.Object, _mockRepository.Object, _mapper);
        
        // Act
        var result = await service.Edit(new EtablissementFicheDto());
        
        // Assert
        Assert.NotNull(result);
        Assert.IsType<EtablissementFicheDto>(result);
        _mockRepository.Verify(repo => repo.UpdateAsync(It.IsAny<EtablissementFiche>(), It.IsAny<ContextSession>()), Times.Once);
    }
    
    [Fact]
    public async Task Create_ShouldReturnEtablissementFicheDto()
    {
        // Arrange
        var etablissementFiche = new EtablissementFiche();
        _mockRepository.Setup(repo => repo.Create(It.IsAny<EtablissementFiche>(), It.IsAny<ContextSession>()))
            .ReturnsAsync(etablissementFiche);
        var service = new EtablissementFicheService<EtablissementFiche>(_mockCurrentContextProvider.Object, _mockRepository.Object, _mapper);
        
        // Act
        var result = await service.Create(new EtablissementFicheDto());
        
        // Assert
        Assert.NotNull(result);
        Assert.IsType<EtablissementFicheDto>(result);
        _mockRepository.Verify(repo => repo.Create(It.IsAny<EtablissementFiche>(), It.IsAny<ContextSession>()), Times.Once);
    }
}