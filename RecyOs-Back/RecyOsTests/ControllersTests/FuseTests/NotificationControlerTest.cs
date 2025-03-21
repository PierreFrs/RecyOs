using Microsoft.AspNetCore.Mvc;
using Moq;
using RecyOs.Controllers;
using RecyOs.ORM.DTO;
using RecyOs.ORM.Entities;
using RecyOs.ORM.Filters;
using RecyOs.ORM.Interfaces;

namespace RecyOsTests.ControllersTests.FuseTests;

[Collection("NotificationUnitTests")]
public class NotificationControllerTests
{
    private readonly Mock<INotificationService> _mockNotificationService;
    private readonly NotificationController _controller;

    public NotificationControllerTests()
    {
        _mockNotificationService = new Mock<INotificationService>();
        _controller = new NotificationController(_mockNotificationService.Object);
    }

    [Fact]
    public async Task Get_ReturnsNotFound_WhenNotificationIsNull()
    {
        // Arrange
        _mockNotificationService.Setup(s => s.GetById(It.IsAny<int>(), false))
            .ReturnsAsync((NotificationDto)null);

        // Act
        var result = await _controller.Get(1);

        // Assert
        Assert.IsType<NotFoundResult>(result);
    }

    [Fact]
    public async Task Get_ReturnsOk_WhenNotificationExists()
    {
        // Arrange
        var notification = new NotificationDto();
        _mockNotificationService.Setup(s => s.GetById(It.IsAny<int>(), false))
            .ReturnsAsync(notification);

        // Act
        var result = await _controller.Get(1);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(notification, okResult.Value);
    }

    [Fact]
    public async Task GetDataForGrid_ReturnsOk()
    {
        // Arrange
        var gridData = new GridData<NotificationDto>();
        _mockNotificationService.Setup(s => s.GetDataForGrid(It.IsAny<NotificationFilter>(), false))
            .ReturnsAsync(gridData);

        // Act
        var result = await _controller.GetDataForGrid(new NotificationFilter());

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(gridData, okResult.Value);
    }

    [Fact]
    public async Task GetMyNotifications_ReturnsOk()
    {
        // Arrange
        var notifications = new List<NotificationDto>();
        _mockNotificationService.Setup(s => s.GetMyNotifications(It.IsAny<bool?>(), false))
            .ReturnsAsync(notifications);

        // Act
        var result = await _controller.GetMyNotifications();

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(notifications, okResult.Value);
    }

    [Fact]
    public async Task Create_ReturnsOk()
    {
        // Arrange
        var notificationDto = new NotificationDto();
        _mockNotificationService.Setup(s => s.Create(It.IsAny<NotificationDto>()))
            .ReturnsAsync(notificationDto);

        // Act
        var result = await _controller.Create(new NotificationDto());

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(notificationDto, okResult.Value);
    }

    [Fact]
    public async Task MarkAsRead_ReturnsNotFound_WhenFalse()
    {
        // Arrange
        _mockNotificationService.Setup(s => s.MarkAsRead(It.IsAny<int>()))
            .ReturnsAsync(false);

        // Act
        var result = await _controller.MarkAsRead(1);

        // Assert
        Assert.IsType<NotFoundResult>(result);
    }

    [Fact]
    public async Task MarkAsRead_ReturnsOk_WhenTrue()
    {
        // Arrange
        _mockNotificationService.Setup(s => s.MarkAsRead(It.IsAny<int>()))
            .ReturnsAsync(true);

        // Act
        var result = await _controller.MarkAsRead(1);

        // Assert
        Assert.IsType<OkResult>(result);
    }

    [Fact]
    public async Task MarkAllAsRead_ReturnsOk()
    {
        // Act
        var result = await _controller.MarkAllAsRead();

        // Assert
        Assert.IsType<OkResult>(result);
        _mockNotificationService.Verify(s => s.MarkAllAsRead(), Times.Once);
    }

    [Fact]
    public async Task GetUnreadCount_ReturnsOk()
    {
        // Arrange
        const int count = 5;
        _mockNotificationService.Setup(s => s.GetUnreadCount())
            .ReturnsAsync(count);

        // Act
        var result = await _controller.GetUnreadCount();

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(count, okResult.Value);
    }

    [Fact]
    public async Task Delete_ReturnsNotFound_WhenNotificationIsNull()
    {
        // Arrange
        _mockNotificationService.Setup(s => s.GetById(It.IsAny<int>(), false))
            .ReturnsAsync((NotificationDto)null);

        // Act
        var result = await _controller.Delete(1);

        // Assert
        Assert.IsType<NotFoundResult>(result);
    }

    [Fact]
    public async Task Delete_ReturnsOk_WhenNotificationExists()
    {
        // Arrange
        var notification = new NotificationDto();
        _mockNotificationService.Setup(s => s.GetById(It.IsAny<int>(), false))
            .ReturnsAsync(notification);

        // Act
        var result = await _controller.Delete(1);

        // Assert
        Assert.IsType<OkResult>(result);
        _mockNotificationService.Verify(s => s.Delete(1), Times.Once);
    }

    [Fact]
    public async Task Update_ReturnsOk()
    {
        // Arrange
        var notificationDto = new NotificationDto();
        _mockNotificationService.Setup(s => s.Edit(It.IsAny<NotificationDto>()))
            .ReturnsAsync(notificationDto);

        // Act
        var result = await _controller.Update(new NotificationDto());

        // Assert
        Assert.IsType<OkObjectResult>(result);
    }
}
