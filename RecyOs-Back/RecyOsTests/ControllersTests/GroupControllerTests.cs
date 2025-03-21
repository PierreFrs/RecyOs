using Microsoft.AspNetCore.Mvc;
using Moq;
using RecyOs.Controllers.ClientsControllers;
using RecyOs.ORM.DTO.hub;
using RecyOs.ORM.Interfaces.hub;
using RecyOs.ORM.Filters;
using RecyOs.ORM.Entities;
using RecyOs.ORM.DTO;

namespace RecyOsTests.ORMTests.ControllerTests.hubTests;

[Collection("GroupTests")]
public class GroupControllerTests
{
    private readonly Mock<IGroupService> _mockService;
    private readonly GroupController _controller;

    public GroupControllerTests()
    {
        _mockService = new Mock<IGroupService>();
        _controller = new GroupController(_mockService.Object);
    }

    [Fact]
    public async Task GetAll_ShouldReturnOkResult_WithGroups()
    {
        // Arrange
        var groups = new List<GroupDto>
        {
            new() { Id = 1, Name = "Group 1" },
            new() { Id = 2, Name = "Group 2" }
        };
        _mockService.Setup(x => x.GetListAsync())
            .ReturnsAsync(groups);

        // Act
        var result = await _controller.GetAll();

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var returnValue = Assert.IsType<List<GroupDto>>(okResult.Value);
        Assert.Equal(2, returnValue.Count);
    }

    [Fact]
    public async Task GetById_ShouldReturnOkResult_WithGroup()
    {
        // Arrange
        const int id = 1;
        var group = new GroupDto { Id = id, Name = "Test Group" };
        _mockService.Setup(x => x.GetByIdAsync(id))
            .ReturnsAsync(group);

        // Act
        var result = await _controller.GetById(id);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var returnValue = Assert.IsType<GroupDto>(okResult.Value);
        Assert.Equal(id, returnValue.Id);
    }

    [Fact]
    public async Task Create_ShouldReturnOkResult_WithCreatedGroup()
    {
        // Arrange
        var groupDto = new GroupDto { Name = "New Group" };
        var createdGroup = new GroupDto { Id = 1, Name = "New Group" };
        _mockService.Setup(x => x.CreateAsync(groupDto))
            .ReturnsAsync(createdGroup);

        // Act
        var result = await _controller.Create(groupDto);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var returnValue = Assert.IsType<GroupDto>(okResult.Value);
        Assert.Equal(createdGroup.Id, returnValue.Id);
    }

    [Fact]
    public async Task Update_ShouldReturnOkResult_WithUpdatedGroup()
    {
        // Arrange
        const int id = 1;
        var groupDto = new GroupDto { Name = "Updated Group" };
        var updatedGroup = new GroupDto { Id = id, Name = "Updated Group" };
        _mockService.Setup(x => x.UpdateAsync(id, groupDto))
            .ReturnsAsync(updatedGroup);

        // Act
        var result = await _controller.Update(id, groupDto);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var returnValue = Assert.IsType<GroupDto>(okResult.Value);
        Assert.Equal(updatedGroup.Id, returnValue.Id);
    }

    [Fact]
    public async Task Delete_ShouldReturnOkResult_WithTrue()
    {
        // Arrange
        const int id = 1;
        _mockService.Setup(x => x.DeleteAsync(id))
            .ReturnsAsync(true);

        // Act
        var result = await _controller.Delete(id);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        Assert.True((bool)okResult.Value);
    }

    [Fact]
    public async Task GetFilteredListWithClients_ReturnsOk()
    {
        // Arrange
        var filter = new GroupFilter 
        { 
            PageNumber = 1,
            PageSize = 10,
            FilteredByNom = "Test"
        };
        
        _mockService.Setup(x => x.GetFilteredListWithClientsAsync(It.IsAny<GroupFilter>()))
            .ReturnsAsync(new GridData<GroupDto>());

        // Act
        var result = await _controller.GetFilteredListWithClients(filter);

        // Assert
        Assert.IsType<OkObjectResult>(result.Result);
    }

    [Fact]
    public async Task GetFilteredListWithClients_WithNullFilter_ReturnsOk()
    {
        // Arrange
        _mockService.Setup(x => x.GetFilteredListWithClientsAsync(It.IsAny<GroupFilter>()))
            .ReturnsAsync(new GridData<GroupDto>());

        // Act
        var result = await _controller.GetFilteredListWithClients(null);

        // Assert
        Assert.IsType<OkObjectResult>(result.Result);
    }

    [Fact]
    public async Task GetByName_ShouldReturnOkResult_WithGroup()
    {
        // Arrange
        const string name = "Test Group";
        var group = new GroupDto { Id = 1, Name = name };
        _mockService.Setup(x => x.GetByNameAsync(name))
            .ReturnsAsync(group);

        // Act
        var result = await _controller.GetByName(name);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var returnValue = Assert.IsType<GroupDto>(okResult.Value);
        Assert.Equal(name, returnValue.Name);
    }
}