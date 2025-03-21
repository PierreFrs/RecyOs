using Microsoft.EntityFrameworkCore;
using RecyOs.Helpers;
using RecyOs.ORM.EFCore.Repository;
using RecyOs.ORM.Entities;
using RecyOs.ORM.Filters;
using RecyOsTests.Interfaces;

namespace RecyOsTests.ORMTests.EFCoreTests.RepositoryTests.FuseTests;

public class NotificationRepositoryTests
{
    private readonly DataContext _context;
    private readonly ContextSession _session;

    public NotificationRepositoryTests(IDataContextTests dataContextTests)
    {
        _context = dataContextTests.GetContext();
        _session = new ContextSession { UserId = 1 }; // Utilisateur de test
    }

    /*********** Getters ***********/
    [Fact]
    public async Task GetAsync_ValidId_ShouldReturnNotification()
    {
        // Arrange
        var repository = new NotificationRepository(_context);

        // Act
        var notification = await repository.GetAsync(1, _session);

        // Assert
        Assert.NotNull(notification);
        Assert.Equal("Test notification", notification.Title);
        Assert.Equal("Notification de test", notification.Description);
        Assert.False(notification.Read);
        Assert.Equal("fa-solid fa-bell", notification.Icon);
    }

    [Fact]
    public async Task GetAsync_InvalidId_ShouldReturnNull()
    {
        // Arrange
        var repository = new NotificationRepository(_context);

        // Act
        var notification = await repository.GetAsync(999, _session);

        // Assert
        Assert.Null(notification);
    }

    [Fact]
    public async Task GetListAsync_ShouldReturnAllNotifications()
    {
        // Arrange
        var repository = new NotificationRepository(_context);

        // Act
        var notifications = await repository.GetListAsync(_session);

        // Assert
        Assert.NotNull(notifications);
        Assert.True(notifications.Count() >= 6); // Vérifie qu'on a au moins toutes nos notifications de test
        Assert.True(notifications.First().Time >= notifications.Last().Time); // Vérifie le tri par date décroissant
    }

    [Fact]
    public async Task GetFilteredNotificationsWithCountAsync_ShouldReturnFilteredResults()
    {
        // Arrange
        var repository = new NotificationRepository(_context);
        var filter = new NotificationFilter
        {
            PageSize = 2,
            PageNumber = 1
        };

        // Act
        var notifications = await repository.GetFilteredNotificationsWithCountAsync(filter, _session);

        // Assert
        Assert.NotNull(notifications);
        Assert.Equal(2, notifications.Count());
    }

    [Fact]
    public async Task GetMyNotificationsAsync_ShouldReturnUserNotifications()
    {
        // Arrange
        var repository = new NotificationRepository(_context);

        // Act
        var notifications = await repository.GetMyNotificationsAsync(_session, null);

        // Assert
        Assert.NotNull(notifications);
        Assert.All(notifications, n => Assert.Equal(_session.UserId, n.UserId));
    }

    [Fact]
    public async Task GetMyNotificationsAsync_UnreadOnly_ShouldReturnUnreadNotifications()
    {
        // Arrange
        var repository = new NotificationRepository(_context);

        // Act
        var notifications = await repository.GetMyNotificationsAsync(_session, false);

        // Assert
        Assert.NotNull(notifications);
        Assert.All(notifications, n => Assert.False(n.Read));
    }

    [Fact]
    public async Task GetMyNotificationsAsync_ReadOnly_ShouldReturnReadNotifications()
    {
        // Arrange
        var repository = new NotificationRepository(_context);

        // Act
        var notifications = await repository.GetMyNotificationsAsync(_session, true);

        // Assert
        Assert.NotNull(notifications);
        Assert.All(notifications, n => Assert.True(n.Read));
    }

    /*********** Creators ***********/
    [Fact]
    public async Task CreateAsync_ValidNotification_ShouldCreateAndReturn()
    {
        // Arrange
        var repository = new NotificationRepository(_context);
        var newNotification = new Notification
        {
            Title = "New Test Notification",
            Description = "Test Description",
            Icon = "fa-solid fa-test",
            Time = DateTime.Now,
            Read = false,
            UserId = 1
        };

        // Act
        var createdNotification = await repository.CreateAsync(newNotification, _session);
        _context.Entry(createdNotification).State = EntityState.Detached;

        // Assert
        Assert.NotNull(createdNotification);
        Assert.NotEqual(0, createdNotification.Id);
        Assert.Equal(newNotification.Title, createdNotification.Title);
        Assert.Equal(newNotification.Description, createdNotification.Description);
    }

    /*********** Updaters ***********/
    [Fact]
    public async Task UpdateAsync_ValidNotification_ShouldUpdateAndReturn()
    {
        // Arrange
        var repository = new NotificationRepository(_context);
        var notification = await repository.GetAsync(1, _session);
        _context.Entry(notification).State = EntityState.Detached;
        notification.Title = "Updated Title";
        notification.Description = "Updated Description";

        // Act
        var updatedNotification = await repository.UpdateAsync(notification, _session);
        _context.Entry(updatedNotification).State = EntityState.Detached;

        // Assert
        Assert.NotNull(updatedNotification);
        Assert.Equal("Updated Title", updatedNotification.Title);
        Assert.Equal("Updated Description", updatedNotification.Description);
    }

    [Fact]
    public async Task UpdateAsync_NewNotification_ShouldCreateAndReturn()
    {
        // Arrange
        var repository = new NotificationRepository(_context);
        var newNotification = new Notification
        {
            Title = "New Notification via Update",
            Description = "Test Description",
            Icon = "fa-solid fa-test",
            Time = DateTime.Now,
            Read = false,
            UserId = 1
        };

        // Act
        var createdNotification = await repository.UpdateAsync(newNotification, _session);
        _context.Entry(createdNotification).State = EntityState.Detached;

        // Assert
        Assert.NotNull(createdNotification);
        Assert.NotEqual(0, createdNotification.Id);
        Assert.Equal(newNotification.Title, createdNotification.Title);
    }

    /*********** Deleters ***********/
    [Fact]
    public async Task DeleteAsync_ValidId_ShouldMarkAsDeleted()
    {
        // Arrange
        var repository = new NotificationRepository(_context);
        
        // Act
        await repository.DeleteAsync(1, _session);
        var deletedNotification = await repository.GetAsync(1, _session);

        // Assert
        Assert.Null(deletedNotification); // Car GetAsync ne retourne pas les éléments supprimés par défaut
        
        // Vérifie que l'élément existe toujours mais est marqué comme supprimé
        var deletedNotificationWithInclude = await repository.GetAsync(1, _session, true);
        Assert.NotNull(deletedNotificationWithInclude);
        Assert.True(deletedNotificationWithInclude.IsDeleted);
    }

    [Fact]
    public async Task DeleteAsync_InvalidId_ShouldNotThrowException()
    {
        // Arrange
        var repository = new NotificationRepository(_context);

        // Act
        await repository.DeleteAsync(999, _session);
        var deletedNotification = await repository.GetAsync(999, _session, true);

        // Assert
        Assert.Null(deletedNotification);
    }
}
