using Moq;
using RecyOs.Engine.Alerts.Entities;
using RecyOs.Engine.Alerts.Repositories;
using RecyOs.Engine.Interfaces;
using RecyOs.Helpers;
using RecyOsTests.Interfaces;
using TaskStatus = RecyOs.Engine.Alerts.Entities.TaskStatus;

namespace RecyOsTests.EngineTests.Repository;

public class EngineMessageMailRepositoryTests
{
    private readonly Mock<IDataContextEngine> _mockIDataContextEngine;
    private readonly DataContext _dataContext;

    public EngineMessageMailRepositoryTests(IEngineDataContextTests prmEngineDataContextTests)
    {
        _mockIDataContextEngine = new Mock<IDataContextEngine>();
        _dataContext = prmEngineDataContextTests.GetContext();
        _mockIDataContextEngine.Setup(x => x.GetContext()).Returns(_dataContext);
    }

    [Fact]
    public async Task GetPendingMessagesAsync_ShouldReturnPendingMessages()
    {
        // Arrange
        var repository = new EngineMessageMailRepository(_mockIDataContextEngine.Object);

        // Act
        var result = await repository.GetPendingMessagesAsync();

        // Assert
        Assert.NotNull(result);
        Assert.All(result, message => Assert.Equal(TaskStatus.Pending, message.Status));
    }

    [Fact]
    public async Task GetByStatusAsync_ValidStatus_ShouldReturnMatchingMessages()
    {
        // Arrange
        var repository = new EngineMessageMailRepository(_mockIDataContextEngine.Object);
        var status = TaskStatus.Completed;

        // Act
        var result = await repository.GetByStatusAsync(status);

        // Assert
        Assert.NotNull(result);
        Assert.All(result, message => Assert.Equal(status, message.Status));
    }

    [Fact]
    public async Task GetFailedMessagesAsync_ShouldReturnFailedMessages()
    {
        // Arrange
        var repository = new EngineMessageMailRepository(_mockIDataContextEngine.Object);

        // Act
        var result = await repository.GetFailedMessagesAsync();

        // Assert
        Assert.NotNull(result);
        Assert.All(result, message => Assert.Equal(TaskStatus.Failed, message.Status));
    }

    [Fact]
    public async Task UpdateStatusAsync_ValidId_ShouldUpdateStatus()
    {
        // Arrange
        var repository = new EngineMessageMailRepository(_mockIDataContextEngine.Object);
        var id = 1;
        var newStatus = TaskStatus.Completed;

        // Act
        await repository.UpdateStatusAsync(id, newStatus);
        var updatedMessage = await repository.GetByIdAsync(id);

        // Assert
        Assert.NotNull(updatedMessage);
        Assert.Equal(newStatus, updatedMessage.Status);
    }

    [Fact]
    public async Task UpdateStatusAsync_InvalidId_ShouldThrowException()
    {
        // Arrange
        var repository = new EngineMessageMailRepository(_mockIDataContextEngine.Object);
        var invalidId = 999;

        // Act & Assert
        await Assert.ThrowsAsync<KeyNotFoundException>(() => 
            repository.UpdateStatusAsync(invalidId, TaskStatus.Completed));
    }

    [Fact]
    public async Task CreateAsync_ValidEntity_ShouldCreateAndReturnEntity()
    {
        // Arrange
        var repository = new EngineMessageMailRepository(_mockIDataContextEngine.Object);
        var newMessage = new MessageMail
        {
            Subject = "Test Subject",
            From = "test@test.com",
            To = "recipient@test.com",
            Status = TaskStatus.Pending,
            Priority = 1,
            DateCreated = DateTime.UtcNow,
            IsDeleted = false,
            DateSent = DateTime.UtcNow,
            Error = "Test error",
            Body = "Test body",
            Cc = "test@test.com",
            Bcc = "test@test.com"
        };

        // Act
        var result = await repository.CreateAsync(newMessage);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(newMessage.Subject, result.Subject);
        Assert.Equal(newMessage.From, result.From);
    }

    [Fact]
    public async Task GetByIdAsync_ValidId_ShouldReturnMessage()
    {
        // Arrange
        var repository = new EngineMessageMailRepository(_mockIDataContextEngine.Object);
        var id = 1;

        // Act
        var result = await repository.GetByIdAsync(id);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(id, result.Id);
    }

    [Fact]
    public async Task GetByIdAsync_InvalidId_ShouldReturnNull()
    {
        // Arrange
        var repository = new EngineMessageMailRepository(_mockIDataContextEngine.Object);
        var invalidId = 999;

        // Act
        var result = await repository.GetByIdAsync(invalidId);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task DeleteAsync_ValidId_ShouldDeleteMessage()
    {
        // Arrange
        var repository = new EngineMessageMailRepository(_mockIDataContextEngine.Object);
        var id = 3;

        // Act
        await repository.DeleteAsync(id);
        var deletedMessage = await repository.GetByIdAsync(id);

        // Assert
        Assert.Null(deletedMessage);
    }

    [Fact]
    public async Task UpdateAsync_ValidEntity_ShouldUpdateAndReturnEntity()
    {
        // Arrange
        var repository = new EngineMessageMailRepository(_mockIDataContextEngine.Object);
        var messageMail = new MessageMail();
        messageMail.Id = 4;
        messageMail.Subject = "Updated Subject";
        messageMail.From = "test@test.com";
        messageMail.To = "recipient@test.com";
        messageMail.Status = TaskStatus.Pending;
        messageMail.Priority = 1;
        messageMail.DateCreated = DateTime.UtcNow;

        // Act
        var result = await repository.UpdateAsync(messageMail);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(messageMail.Subject, result.Subject);
        Assert.Equal(messageMail.From, result.From);
        Assert.Equal(messageMail.Status, result.Status);
        _mockIDataContextEngine.Verify(x => x.GetContext(), Times.Once);
    }
}
