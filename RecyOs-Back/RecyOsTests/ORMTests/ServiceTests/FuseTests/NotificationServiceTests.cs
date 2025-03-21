using AutoMapper;
using Moq;
using RecyOs.Helpers;
using RecyOs.ORM.DTO;
using RecyOs.ORM.Entities;
using RecyOs.ORM.Filters;
using RecyOs.ORM.Interfaces;
using RecyOs.ORM.Service;

namespace RecyOsTests.ORMTests.ServiceTests.FuseTests;

public class NotificationServiceTests
{
    private readonly Mock<ICurrentContextProvider> _mockCurrentContextProvider;
    private readonly Mock<INotificationRepository<Notification>> _mockRepository;
    private readonly Mock<IMapper> _mockMapper;
    private readonly ContextSession _testSession;

    public NotificationServiceTests()
    {
        _mockCurrentContextProvider = new Mock<ICurrentContextProvider>();
        _mockRepository = new Mock<INotificationRepository<Notification>>();
        _mockMapper = new Mock<IMapper>();
        _testSession = new ContextSession { UserId = 1 };
        _mockCurrentContextProvider.Setup(x => x.GetCurrentContext()).Returns(_testSession);
    }

    [Fact]
    public async Task GetDataForGrid_WhenCalled_ReturnsGridData()
    {
        // Arrange
        var filter = new NotificationFilter { PageSize = 10, PageNumber = 1 };
        var notifications = new List<Notification>
        {
            new() { Id = 1, Title = "Test1", Description = "Desc1" },
            new() { Id = 2, Title = "Test2", Description = "Desc2" }
        };
        var notificationDtos = new List<NotificationDto>
        {
            new() { Id = 1, Title = "Test1", Description = "Desc1" },
            new() { Id = 2, Title = "Test2", Description = "Desc2" }
        };

        _mockRepository.Setup(x => x.GetFilteredNotificationsWithCountAsync(filter, _testSession, false))
            .ReturnsAsync(notifications);
        _mockMapper.Setup(x => x.Map<IEnumerable<NotificationDto>>(It.IsAny<IEnumerable<Notification>>()))
            .Returns(notificationDtos);

        var service = new NotificationService<Notification>(
            _mockCurrentContextProvider.Object,
            _mockRepository.Object,
            _mockMapper.Object);

        // Act
        var result = await service.GetDataForGrid(filter);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(2, result.Items.Count());
        Assert.Equal(10, result.Paginator.size);
        Assert.Equal(1, result.Paginator.page);
    }

    [Fact]
    public async Task GetById_WhenCalled_ReturnsNotification()
    {
        // Arrange
        var notification = new Notification { Id = 1, Title = "Test", Description = "Desc" };
        var notificationDto = new NotificationDto { Id = 1, Title = "Test", Description = "Desc" };

        _mockRepository.Setup(x => x.GetAsync(1, _testSession, false))
            .ReturnsAsync(notification);
        _mockMapper.Setup(x => x.Map<NotificationDto>(notification))
            .Returns(notificationDto);

        var service = new NotificationService<Notification>(
            _mockCurrentContextProvider.Object,
            _mockRepository.Object,
            _mockMapper.Object);

        // Act
        var result = await service.GetById(1);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(1, result.Id);
        Assert.Equal("Test", result.Title);
    }

    [Fact]
    public async Task Delete_WhenCalled_ReturnsTrue()
    {
        // Arrange
        _mockRepository.Setup(x => x.DeleteAsync(1, _testSession))
            .Returns(Task.CompletedTask);

        var service = new NotificationService<Notification>(
            _mockCurrentContextProvider.Object,
            _mockRepository.Object,
            _mockMapper.Object);

        // Act
        var result = await service.Delete(1);

        // Assert
        Assert.True(result);
        _mockRepository.Verify(x => x.DeleteAsync(1, _testSession), Times.Once);
    }

    [Fact]
    public async Task Edit_WhenCalled_ReturnsUpdatedNotification()
    {
        // Arrange
        var notificationDto = new NotificationDto { Id = 1, Title = "Updated", Description = "Desc" };
        var notification = new Notification { Id = 1, Title = "Updated", Description = "Desc" };

        _mockMapper.Setup(x => x.Map<Notification>(notificationDto)).Returns(notification);
        _mockRepository.Setup(x => x.UpdateAsync(notification, _testSession))
            .ReturnsAsync(notification);
        _mockMapper.Setup(x => x.Map<NotificationDto>(notification))
            .Returns(notificationDto);

        var service = new NotificationService<Notification>(
            _mockCurrentContextProvider.Object,
            _mockRepository.Object,
            _mockMapper.Object);

        // Act
        var result = await service.Edit(notificationDto);

        // Assert
        Assert.NotNull(result);
        Assert.Equal("Updated", result.Title);
    }

    [Fact]
    public async Task Create_WhenCalled_ReturnsNewNotification()
    {
        // Arrange
        var notificationDto = new NotificationDto { Title = "New", Description = "Desc" };
        var notification = new Notification { Title = "New", Description = "Desc" };

        _mockMapper.Setup(x => x.Map<Notification>(notificationDto)).Returns(notification);
        _mockRepository.Setup(x => x.CreateAsync(It.IsAny<Notification>(), _testSession, false))
            .ReturnsAsync(notification);
        _mockMapper.Setup(x => x.Map<NotificationDto>(notification))
            .Returns(notificationDto);

        var service = new NotificationService<Notification>(
            _mockCurrentContextProvider.Object,
            _mockRepository.Object,
            _mockMapper.Object);

        // Act
        var result = await service.Create(notificationDto);

        // Assert
        Assert.NotNull(result);
        Assert.Equal("New", result.Title);
        Assert.False(notification.Read);
    }

    [Fact]
    public async Task GetMyNotifications_WhenCalled_ReturnsUserNotifications()
    {
        // Arrange
        var notifications = new List<Notification>
        {
            new() { Id = 1, UserId = 1, Title = "Test1" },
            new() { Id = 2, UserId = 1, Title = "Test2" }
        };
        var notificationDtos = new List<NotificationDto>
        {
            new() { Id = 1, UserId = 1, Title = "Test1" },
            new() { Id = 2, UserId = 1, Title = "Test2" }
        };

        _mockRepository.Setup(x => x.GetMyNotificationsAsync(_testSession, null, false))
            .ReturnsAsync(notifications);
        _mockMapper.Setup(x => x.Map<IEnumerable<NotificationDto>>(notifications))
            .Returns(notificationDtos);

        var service = new NotificationService<Notification>(
            _mockCurrentContextProvider.Object,
            _mockRepository.Object,
            _mockMapper.Object);

        // Act
        var result = await service.GetMyNotifications();

        // Assert
        Assert.NotNull(result);
        Assert.Equal(2, result.Count());
    }

    [Fact]
    public async Task MarkAsRead_WhenValidNotification_ReturnsTrue()
    {
        // Arrange
        var notificationDto = new NotificationDto { Id = 1, UserId = 1, Read = false };
        var notification = new Notification { Id = 1, UserId = 1, Read = false };

        _mockRepository.Setup(x => x.GetAsync(1, _testSession, false))
            .ReturnsAsync(notification);
        _mockMapper.Setup(x => x.Map<NotificationDto>(notification))
            .Returns(notificationDto);
        _mockMapper.Setup(x => x.Map<Notification>(notificationDto))
            .Returns(notification);
        _mockRepository.Setup(x => x.UpdateAsync(It.IsAny<Notification>(), _testSession))
            .ReturnsAsync(notification);

        var service = new NotificationService<Notification>(
            _mockCurrentContextProvider.Object,
            _mockRepository.Object,
            _mockMapper.Object);

        // Act
        var result = await service.MarkAsRead(1);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public async Task MarkAllAsRead_WhenCalled_ReturnsTrue()
    {
        // Arrange
        var notifications = new List<Notification>
        {
            new() { Id = 1, UserId = 1, Read = false },
            new() { Id = 2, UserId = 1, Read = false }
        };

        _mockRepository.Setup(x => x.GetMyNotificationsAsync(_testSession, false, false))
            .ReturnsAsync(notifications);
        _mockRepository.Setup(x => x.UpdateAsync(It.IsAny<Notification>(), _testSession))
            .ReturnsAsync((Notification n, ContextSession s) => n);

        var service = new NotificationService<Notification>(
            _mockCurrentContextProvider.Object,
            _mockRepository.Object,
            _mockMapper.Object);

        // Act
        var result = await service.MarkAllAsRead();

        // Assert
        Assert.True(result);
        _mockRepository.Verify(x => x.UpdateAsync(It.IsAny<Notification>(), _testSession), Times.Exactly(2));
    }

    [Fact]
    public async Task GetUnreadCount_WhenCalled_ReturnsCount()
    {
        // Arrange
        var notifications = new List<Notification>
        {
            new() { Id = 1, UserId = 1, Read = false },
            new() { Id = 2, UserId = 1, Read = false }
        };

        _mockRepository.Setup(x => x.GetMyNotificationsAsync(_testSession, false, false))
            .ReturnsAsync(notifications);

        var service = new NotificationService<Notification>(
            _mockCurrentContextProvider.Object,
            _mockRepository.Object,
            _mockMapper.Object);

        // Act
        var result = await service.GetUnreadCount();

        // Assert
        Assert.Equal(2, result);
    }
} 