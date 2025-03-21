using AutoMapper;
using Microsoft.AspNetCore.Http;
using Moq;
using RecyOs.ORM.DTO.hub;
using RecyOs.ORM.Entities;
using RecyOs.ORM.Entities.hub;
using RecyOs.ORM.Filters.hub;
using RecyOs.ORM.Interfaces;
using RecyOs.ORM.Interfaces.hub;
using RecyOs.ORM.Service.hub;

namespace RecyOsTests.ORMTests.hubTests;

public class EntrepriseBaseServiceTests
{
    private readonly Mock<ICurrentContextProvider> _mockCurrentContextProvider;
    private readonly IMapper _mapper;
    private readonly Mock<IEntrepriseBaseRepository<EntrepriseBase>> _mockRepository;
    
    public EntrepriseBaseServiceTests()
    {
        _mockCurrentContextProvider = new Mock<ICurrentContextProvider>();
        _mockRepository = new Mock<IEntrepriseBaseRepository<EntrepriseBase>>();
        var mapperConfig = new MapperConfiguration(cfg =>
        {
            cfg.CreateMap<EntrepriseBase, EntrepriseBaseDto>();
            cfg.CreateMap<EntrepriseBaseDto, EntrepriseBase>();
        });
        _mapper = mapperConfig.CreateMapper();
    }
    
    [Fact]
    public async Task GetDataForGrid_ShouldReturnGridData()
    {
        // Arrange
        var filter = new EntrepriseBaseGridFilter();
        var tuple = (new List<EntrepriseBase>(), 0);
        _mockRepository.Setup(repo => repo.GetFiltredListWithCount(filter, It.IsAny<ContextSession>(), false))
            .ReturnsAsync(tuple);
        var service = new EntrepriseBaseService<EntrepriseBase>(_mockCurrentContextProvider.Object, _mockRepository.Object, _mapper);
        
        // Act
        var result = await service.GetDataForGrid(filter);
        
        // Assert
        Assert.NotNull(result);
        Assert.IsType<GridData<EntrepriseBaseDto>>(result);
        _mockRepository.Verify(repo => repo.GetFiltredListWithCount(It.IsAny<EntrepriseBaseGridFilter>(), It.IsAny<ContextSession>(), false), Times.Once);
    }
    
    [Fact]
    public async Task GetById_ShouldReturnEntrepriseBaseDto()
    {
        // Arrange
        var entrepriseBase = new EntrepriseBase();
        _mockRepository.Setup(repo => repo.Get(It.IsAny<int>(), It.IsAny<ContextSession>(), false))
            .ReturnsAsync(entrepriseBase);
        var service = new EntrepriseBaseService<EntrepriseBase>(_mockCurrentContextProvider.Object, _mockRepository.Object, _mapper);
        
        // Act
        var result = await service.GetById(1);
        
        // Assert
        Assert.NotNull(result);
        Assert.IsType<EntrepriseBaseDto>(result);
        _mockRepository.Verify(repo => repo.Get(It.IsAny<int>(), It.IsAny<ContextSession>(), false), Times.Once);
    }
    
    [Fact]
    public async Task GetBySiren_ShouldReturnEntrepriseBaseDto()
    {
        // Arrange
        var entrepriseBase = new EntrepriseBase();
        _mockRepository.Setup(repo => repo.GetBySiren(It.IsAny<string>(), It.IsAny<ContextSession>(), false))
            .ReturnsAsync(entrepriseBase);
        var service = new EntrepriseBaseService<EntrepriseBase>(_mockCurrentContextProvider.Object, _mockRepository.Object, _mapper);
        
        // Act
        var result = await service.GetBySiren("123456789");
        
        // Assert
        Assert.NotNull(result);
        Assert.IsType<EntrepriseBaseDto>(result);
        _mockRepository.Verify(repo => repo.GetBySiren(It.IsAny<string>(), It.IsAny<ContextSession>(), false), Times.Once);
    }
    
    [Fact]
    public async Task Delete_ShouldReturnTrue()
    {
        // Arrange
        _mockRepository.Setup(repo => repo.Delete(It.IsAny<int>(), It.IsAny<ContextSession>()));
        var service = new EntrepriseBaseService<EntrepriseBase>(_mockCurrentContextProvider.Object, _mockRepository.Object, _mapper);
        
        // Act
        var result = await service.Delete(1);
        
        // Assert
        Assert.True(result);
        _mockRepository.Verify(repo => repo.Delete(It.IsAny<int>(), It.IsAny<ContextSession>()), Times.Once);
    }
    
    [Fact]
    public async Task Edit_ShouldReturnEntrepriseBaseDto()
    {
        // Arrange
        var entrepriseBase = new EntrepriseBase();
        _mockRepository.Setup(repo => repo.Update(It.IsAny<EntrepriseBase>(), It.IsAny<ContextSession>()))
            .ReturnsAsync(entrepriseBase);
        var service = new EntrepriseBaseService<EntrepriseBase>(_mockCurrentContextProvider.Object, _mockRepository.Object, _mapper);
        
        // Act
        var result = await service.Edit(new EntrepriseBaseDto());
        
        // Assert
        Assert.NotNull(result);
        Assert.IsType<EntrepriseBaseDto>(result);
        _mockRepository.Verify(repo => repo.Update(It.IsAny<EntrepriseBase>(), It.IsAny<ContextSession>()), Times.Once);
    }
    
    [Fact]
    public async Task Create_ShouldReturnEntrepriseBaseDto()
    {
        // Arrange
        var entrepriseBase = new EntrepriseBase();
        _mockRepository.Setup(repo => repo.Create(It.IsAny<EntrepriseBase>(), It.IsAny<ContextSession>()))
            .ReturnsAsync(entrepriseBase);
        var service = new EntrepriseBaseService<EntrepriseBase>(_mockCurrentContextProvider.Object, _mockRepository.Object, _mapper);
        
        // Act
        var result = await service.Create(new EntrepriseBaseDto());
        
        // Assert
        Assert.NotNull(result);
        Assert.IsType<EntrepriseBaseDto>(result);
        _mockRepository.Verify(repo => repo.Create(It.IsAny<EntrepriseBase>(), It.IsAny<ContextSession>()), Times.Once);
    }
}
    