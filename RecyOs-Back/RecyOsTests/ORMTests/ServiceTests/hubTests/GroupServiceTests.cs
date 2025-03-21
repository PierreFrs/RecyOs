using AutoMapper;
using Moq;
using RecyOs.ORM.DTO.hub;
using RecyOs.ORM.Entities;
using RecyOs.ORM.Entities.hub;
using RecyOs.ORM.Filters;
using RecyOs.ORM.Interfaces;
using RecyOs.ORM.Interfaces.hub;
using RecyOs.ORM.Mapping;
using RecyOs.ORM.Service.hub;


namespace RecyOsTests.ORMTests.ServiceTests.hubTests;

[Collection("GroupTests")]
public class GroupServiceTests
{
    private readonly Mock<IGroupRepository> _mockRepository;
    private readonly Mock<ITokenInfoService> _mockTokenInfoService;
    private readonly Mock<ICurrentContextProvider> _mockContextProvider;
    private readonly IMapper _mapper;
    private readonly GroupService _service;

    public GroupServiceTests()
    {
        _mockRepository = new Mock<IGroupRepository>();
        _mockTokenInfoService = new Mock<ITokenInfoService>();
        _mockContextProvider = new Mock<ICurrentContextProvider>();
        
        var configuration = new MapperConfiguration(cfg => cfg.AddProfile<GroupProfile>());
        _mapper = configuration.CreateMapper();

        _service = new GroupService(
            _mockContextProvider.Object,
            _mockRepository.Object, 
            _mapper, 
            _mockTokenInfoService.Object);
    }

    [Fact]
    public async Task CreateAsync_ShouldReturnGroupDto_WhenCreated()
    {
        // Arrange
        var createDto = new GroupDto { Name = "Test Group" };
        var expectedGroup = new Group { Id = 1, Name = "Test Group" };
        
        _mockTokenInfoService.Setup(x => x.GetCurrentUserName())
            .Returns("TestUser");
        
        _mockRepository.Setup(x => x.CreateAsync(It.IsAny<Group>(), It.IsAny<ContextSession>()))
            .ReturnsAsync(expectedGroup);

        // Act
        var result = await _service.CreateAsync(createDto);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(expectedGroup.Id, result.Id);
    }

    [Fact]
    public async Task UpdateAsync_ShouldReturnUpdatedGroupDto()
    {
        // Arrange
        const int id = 1;
        var updateDto = new GroupDto { Name = "Updated Group" };
        var expectedGroup = new Group { Id = id, Name = "Updated Group" };
        
        _mockTokenInfoService.Setup(x => x.GetCurrentUserName())
            .Returns("TestUser");
        
        _mockRepository.Setup(x => x.UpdateAsync(It.IsAny<Group>(), It.IsAny<ContextSession>()))
            .ReturnsAsync(expectedGroup);

        // Act
        var result = await _service.UpdateAsync(id, updateDto);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(expectedGroup.Id, result.Id);
        Assert.Equal(expectedGroup.Name, result.Name);
        _mockRepository.Verify(x => x.UpdateAsync(
            It.Is<Group>(g => 
                g.UpdatedBy == "TestUser" && 
                g.UpdatedAt.HasValue && 
                g.UpdatedAt.Value.Date == DateTime.Now.Date), 
            It.IsAny<ContextSession>()), 
            Times.Once);
    }

    [Fact]
    public async Task GetByIdAsync_ShouldReturnGroupDto()
    {
        // Arrange
        const int id = 1;
        var expectedGroup = new Group { Id = id, Name = "Test Group" };
        
        _mockRepository.Setup(x => x.GetByIdAsync(id, It.IsAny<ContextSession>(), false))
            .ReturnsAsync(expectedGroup);

        // Act
        var result = await _service.GetByIdAsync(id);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(expectedGroup.Id, result.Id);
        Assert.Equal(expectedGroup.Name, result.Name);
    }

    [Fact]
    public async Task GetListAsync_ShouldReturnGroupDtoList()
    {
        // Arrange
        var groups = new List<Group>
        {
            new() { Id = 1, Name = "Group 1" },
            new() { Id = 2, Name = "Group 2" }
        };
        
        _mockRepository.Setup(x => x.GetListAsync(false))
            .ReturnsAsync(groups);

        // Act
        var result = await _service.GetListAsync();

        // Assert
        Assert.NotNull(result);
        Assert.Equal(2, result.Count);
    }

    [Fact]
    public async Task DeleteAsync_ShouldReturnTrue_WhenDeleted()
    {
        // Arrange
        const int id = 1;
        _mockRepository.Setup(x => x.DeleteAsync(id, It.IsAny<ContextSession>()))
            .ReturnsAsync(true);

        // Act
        var result = await _service.DeleteAsync(id);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public async Task GetFilteredListWithClientsAsync_ShouldReturnGridData()
    {
        // Arrange
        var filter = new GroupFilter 
        { 
            PageNumber = 1,
            PageSize = 10,
            FilteredByNom = "Test"
        };

        var groups = new List<Group>
        {
            new() { Id = 1, Name = "Test Group 1" },
            new() { Id = 2, Name = "Test Group 2" }
        };

        const int totalCount = 2;

        _mockRepository.Setup(x => x.GetFilteredListWithClientsAsync(
            It.IsAny<GroupFilter>(), 
            It.IsAny<ContextSession>(),
            It.IsAny<bool>()))
            .ReturnsAsync((groups, totalCount));

        // Act
        var result = await _service.GetFilteredListWithClientsAsync(filter);

        // Assert
        Assert.NotNull(result);
        Assert.NotNull(result.Items);
        Assert.NotNull(result.Paginator);
        Assert.Equal(totalCount, result.Paginator.length);
        Assert.Equal(filter.PageSize, result.Paginator.size);
        Assert.Equal(filter.PageNumber, result.Paginator.page);
        Assert.Equal(2, result.Items.Count());
        Assert.Equal((filter.PageNumber - 1) * filter.PageSize, result.Paginator.startIndex);
        Assert.Equal((int)Math.Ceiling(totalCount / (double)filter.PageSize), result.Paginator.lastPage);
    }

    [Fact]
    public async Task GetFilteredListWithClientsAsync_WithEmptyResult_ShouldReturnEmptyGridData()
    {
        // Arrange
        var filter = new GroupFilter 
        { 
            PageNumber = 1,
            PageSize = 10,
            FilteredByNom = "NonExistent"
        };

        var groups = new List<Group>();
        const int totalCount = 0;

        _mockRepository.Setup(x => x.GetFilteredListWithClientsAsync(
            It.IsAny<GroupFilter>(),
            It.IsAny<ContextSession>(),
            It.IsAny<bool>()))
            .ReturnsAsync((groups, totalCount));

        // Act
        var result = await _service.GetFilteredListWithClientsAsync(filter);

        // Assert
        Assert.NotNull(result);
        Assert.Empty(result.Items);
        Assert.NotNull(result.Paginator);
        Assert.Equal(0, result.Paginator.length);
        Assert.Equal(filter.PageSize, result.Paginator.size);
        Assert.Equal(filter.PageNumber, result.Paginator.page);
        Assert.Equal(0, result.Paginator.lastPage);
        Assert.Equal(0, result.Paginator.startIndex);
    }

    [Fact]
    public async Task GetByNameAsync_ShouldReturnGroupDto()
    {
        // Arrange
        const string name = "Test Group";
        var expectedGroup = new Group { Id = 1, Name = name };  

        _mockRepository.Setup(x => x.GetByNameAsync(name, It.IsAny<ContextSession>()))
            .ReturnsAsync(expectedGroup);

        // Act
        var result = await _service.GetByNameAsync(name);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(expectedGroup.Id, result.Id);
        Assert.Equal(expectedGroup.Name, result.Name);
    }    
} 