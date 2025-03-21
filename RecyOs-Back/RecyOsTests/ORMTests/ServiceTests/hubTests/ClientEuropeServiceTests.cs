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

public class ClientEuropeServiceTests
{
    private readonly Mock<ICurrentContextProvider> _mockCurrentContextProvider;
    private readonly Mock<IGroupRepository> _mockGroupRepository;
    private readonly IMapper _mapper;
    private readonly Mock<IClientEuropeRepository<ClientEurope>> _mockRepository;
    
    public ClientEuropeServiceTests()
    {
        _mockCurrentContextProvider = new Mock<ICurrentContextProvider>();
        _mockGroupRepository = new Mock<IGroupRepository>();
        _mockRepository = new Mock<IClientEuropeRepository<ClientEurope>>();
        var mapperConfig = new MapperConfiguration(cfg =>
        {
            cfg.CreateMap<ClientEurope, ClientEuropeDto>();
            cfg.CreateMap<ClientEuropeDto, ClientEurope>();
            cfg.CreateMap<Group, GroupDto>();
        });
        _mapper = mapperConfig.CreateMapper();
    }
    
    [Fact]
    public async Task GetDataForGrid_ShouldReturnGridData()
    {
        // Arrange
        var tuple = (new List<ClientEurope>(), 0);
        _mockRepository.Setup(repo => repo.GetFiltredListWithCount(It.IsAny<ClientEuropeGridFilter>(), It.IsAny<ContextSession>(), false))
            .ReturnsAsync(tuple);
        var service = new ClientEuropeService<ClientEurope>(_mockCurrentContextProvider.Object, _mapper, _mockRepository.Object, _mockGroupRepository.Object);
        
        // Act
        var result = await service.GetGridDataAsync(new ClientEuropeGridFilter());
        
        // Assert
        Assert.NotNull(result);
        Assert.IsType<GridData<ClientEuropeDto>>(result);
        _mockRepository.Verify(repo => repo.GetFiltredListWithCount(It.IsAny<ClientEuropeGridFilter>(), It.IsAny<ContextSession>(), false), Times.Once);
    }
    
    [Fact]
    public async Task GetById_ShouldReturnClientEuropeDto()
    {
        // Arrange
        var clientEurope = new ClientEurope();
        _mockRepository.Setup(repo => repo.GetById(It.IsAny<int>(), It.IsAny<ContextSession>(), false))
            .ReturnsAsync(clientEurope);
        var service = new ClientEuropeService<ClientEurope>(_mockCurrentContextProvider.Object, _mapper, _mockRepository.Object, _mockGroupRepository.Object);
        
        // Act
        var result = await service.GetById(1);
        
        // Assert
        Assert.NotNull(result);
        Assert.IsType<ClientEuropeDto>(result);
        _mockRepository.Verify(repo => repo.GetById(It.IsAny<int>(), It.IsAny<ContextSession>(), false), Times.Once);
    }
    
    [Fact]
    public async Task GetByVatNumber_ShouldReturnClientEuropeDto()
    {
        // Arrange
        var clientEurope = new ClientEurope();
        _mockRepository.Setup(repo => repo.GetByVat(It.IsAny<string>(), It.IsAny<ContextSession>(), false))
            .ReturnsAsync(clientEurope);
        var service = new ClientEuropeService<ClientEurope>(_mockCurrentContextProvider.Object, _mapper, _mockRepository.Object, _mockGroupRepository.Object);
        
        // Act
        var result = await service.GetByVat("BE123456789");
        
        // Assert
        Assert.NotNull(result);
        Assert.IsType<ClientEuropeDto>(result);
        _mockRepository.Verify(repo => repo.GetByVat(It.IsAny<string>(), It.IsAny<ContextSession>(), false), Times.Once);
    }
    
    [Fact]
    public async Task GetByCodeKerlog_ShouldReturnClientEuropeDto()
    {
        // Arrange
        var clientEurope = new ClientEurope();
        _mockRepository.Setup(repo => repo.GetByCodeKerlog(It.IsAny<string>(), It.IsAny<ContextSession>(), false))
            .ReturnsAsync(clientEurope);
        var service = new ClientEuropeService<ClientEurope>(_mockCurrentContextProvider.Object, _mapper, _mockRepository.Object, _mockGroupRepository.Object);
        
        // Act
        var result = await service.GetByCodeKerlog("123456789");
        
        // Assert
        Assert.NotNull(result);
        Assert.IsType<ClientEuropeDto>(result);
        _mockRepository.Verify(repo => repo.GetByCodeKerlog(It.IsAny<string>(), It.IsAny<ContextSession>(), false), Times.Once);
    }
    
    [Fact]
    public async Task GetByCodeMkgt_ShouldReturnClientEuropeDto()
    {
        // Arrange
        var clientEurope = new ClientEurope();
        _mockRepository.Setup(repo => repo.GetByCodeMkgt(It.IsAny<string>(), It.IsAny<ContextSession>(), false))
            .ReturnsAsync(clientEurope);
        var service = new ClientEuropeService<ClientEurope>(_mockCurrentContextProvider.Object, _mapper, _mockRepository.Object, _mockGroupRepository.Object);
        
        // Act
        var result = await service.GetByCodeMkgt("123456789");
        
        // Assert
        Assert.NotNull(result);
        Assert.IsType<ClientEuropeDto>(result);
        _mockRepository.Verify(repo => repo.GetByCodeMkgt(It.IsAny<string>(), It.IsAny<ContextSession>(), false), Times.Once);
    }
    
    [Fact]
    public async Task GetByIdOdoo_ShouldReturnClientEuropeDto()
    {
        // Arrange
        var clientEurope = new ClientEurope();
        _mockRepository.Setup(repo => repo.GetByIdOdoo(It.IsAny<string>(), It.IsAny<ContextSession>(), false))
            .ReturnsAsync(clientEurope);
        var service = new ClientEuropeService<ClientEurope>(_mockCurrentContextProvider.Object, _mapper, _mockRepository.Object, _mockGroupRepository.Object);
        
        // Act
        var result = await service.GetByIdOdoo("12");
        
        // Assert
        Assert.NotNull(result);
        Assert.IsType<ClientEuropeDto>(result);
        _mockRepository.Verify(repo => repo.GetByIdOdoo(It.IsAny<string>(), It.IsAny<ContextSession>(), false), Times.Once);
    }

    [Fact]
    public async Task GetGroup_ShouldReturnGroupDto()
    {
        // Arrange
        var group = new Group();
        var clientEurope = new ClientEurope { GroupId = 1 };
        _mockRepository.Setup(repo => repo.GetById(It.IsAny<int>(), It.IsAny<ContextSession>(), false))
            .ReturnsAsync(clientEurope);
        _mockGroupRepository.Setup(repo => repo.GetByIdAsync(It.IsAny<int>(), It.IsAny<ContextSession>(), false))
            .ReturnsAsync(group);
        var service = new ClientEuropeService<ClientEurope>(_mockCurrentContextProvider.Object, _mapper, _mockRepository.Object, _mockGroupRepository.Object);
        
        // Act
        var result = await service.GetGroup(1);

        // Assert
        Assert.NotNull(result);
        Assert.IsType<GroupDto>(result);
        _mockGroupRepository.Verify(repo => repo.GetByIdAsync(It.IsAny<int>(), It.IsAny<ContextSession>(), false), Times.Once);
    }

    [Fact]
    public async Task GetGroup_ShouldReturnNull_IfGroupIsNotFound()
    {
        // Arrange
        var clientEurope = new ClientEurope { GroupId = 1 };
        _mockRepository.Setup(repo => repo.GetById(It.IsAny<int>(), It.IsAny<ContextSession>(), false))
            .ReturnsAsync(clientEurope);
        _mockGroupRepository.Setup(repo => repo.GetByIdAsync(It.IsAny<int>(), It.IsAny<ContextSession>(), false))
            .ReturnsAsync((Group)null);
        var service = new ClientEuropeService<ClientEurope>(_mockCurrentContextProvider.Object, _mapper, _mockRepository.Object, _mockGroupRepository.Object);
        
        // Act
        var result = await service.GetGroup(1);

        // Assert
        Assert.Null(result);
    }
    
    [Fact]
    public async Task Delete_ShouldReturnTrue_IfEntityIsDeleted()
    {
        // Arrange
        var clientEurope = new ClientEurope();
        clientEurope.Client = true;
        _mockRepository.Setup(repo => repo.GetById(It.IsAny<int>(), It.IsAny<ContextSession>(), It.IsAny<bool>()))
            .ReturnsAsync(clientEurope);
        _mockRepository.Setup(repo => repo.UpdateAsync(clientEurope, It.IsAny<ContextSession>()))
            .ReturnsAsync(clientEurope);

        var service = new ClientEuropeService<ClientEurope>(_mockCurrentContextProvider.Object, _mapper, _mockRepository.Object, _mockGroupRepository.Object);
        
        // Act
        var result = await service.DeleteAsync(1, true, false);
        
        // Assert
        Assert.True(result);
        _mockRepository.Verify(repo => repo.Delete(It.IsAny<int>(), It.IsAny<ContextSession>()), Times.Once);
    }
    
    [Fact]
    public async Task Update_ShouldReturnClientEuropeDto()
    {
        // Arrange
        var clientEurope = new ClientEurope();
        _mockRepository.Setup(repo => repo.UpdateAsync(It.IsAny<ClientEurope>(), It.IsAny<ContextSession>()))
            .ReturnsAsync(clientEurope);
        var service = new ClientEuropeService<ClientEurope>(_mockCurrentContextProvider.Object, _mapper, _mockRepository.Object, _mockGroupRepository.Object);
        
        // Act
        var result = await service.Update(new ClientEuropeDto());
        
        // Assert
        Assert.NotNull(result);
        Assert.IsType<ClientEuropeDto>(result);
        _mockRepository.Verify(repo => repo.UpdateAsync(It.IsAny<ClientEurope>(), It.IsAny<ContextSession>()), Times.Once);
    }
    
    [Fact]
    public async Task Create_ShouldReturnClientEuropeDto()
    {
        // Arrange
        var clientEurope = new ClientEurope();
        _mockRepository.Setup(repo => repo.CreateAsync(It.IsAny<ClientEurope>(), It.IsAny<ContextSession>()))
            .ReturnsAsync(clientEurope);
        var service = new ClientEuropeService<ClientEurope>(_mockCurrentContextProvider.Object, _mapper, _mockRepository.Object, _mockGroupRepository.Object);
        
        // Act
        var result = await service.Create(new ClientEuropeDto());
        
        // Assert
        Assert.NotNull(result);
        Assert.IsType<ClientEuropeDto>(result);
        _mockRepository.Verify(repo => repo.CreateAsync(It.IsAny<ClientEurope>(), It.IsAny<ContextSession>()), Times.Once);
    }

    [Fact]
    public async Task CreateFromScratch_ShouldReturnClientEuropeDto_IfEntityIsCreated()
    {
        // Arrange
        var clientEurope = new ClientEurope();
        _mockRepository.Setup(repo => repo.CreateAsync(It.IsAny<ClientEurope>(), It.IsAny<ContextSession>()))
            .ReturnsAsync(clientEurope);
        var service = new ClientEuropeService<ClientEurope>(_mockCurrentContextProvider.Object, _mapper, _mockRepository.Object, _mockGroupRepository.Object);

        // Act
        var result = await service.CreateFromScratchAsync("BE123456789");

        // Assert
        Assert.NotNull(result);
        Assert.IsType<ClientEuropeDto>(result);
        _mockRepository.Verify(repo => repo.CreateAsync(It.IsAny<ClientEurope>(), It.IsAny<ContextSession>()), Times.Once);
    }

    [Fact]
    public async Task CreateFromScratch_ShouldThrowException_IfEntityIsNotCreated()
    {
        // Arrange
        _mockRepository.Setup(repo => repo.CreateAsync(It.IsAny<ClientEurope>(), It.IsAny<ContextSession>()))
            .ReturnsAsync((ClientEurope)null);
        var service = new ClientEuropeService<ClientEurope>(_mockCurrentContextProvider.Object, _mapper, _mockRepository.Object, _mockGroupRepository.Object);

        // Act
        var exception = await Assert.ThrowsAsync<Exception>(() => service.CreateFromScratchAsync("BE123456789"));

        // Assert
        Assert.NotNull(exception);
        Assert.Equal("Impossible de créer l'établissement.", exception.Message);
        _mockRepository.Verify(repo => repo.CreateAsync(It.IsAny<ClientEurope>(), It.IsAny<ContextSession>()), Times.Once);
    }
}