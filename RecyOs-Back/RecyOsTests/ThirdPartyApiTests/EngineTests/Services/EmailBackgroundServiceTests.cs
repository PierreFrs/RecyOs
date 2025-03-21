using System.Reflection;
using Microsoft.Extensions.Configuration;
using Moq;
using NLog;
using RecyOs.Engine.Alerts;
using RecyOs.Engine.Alerts.DTO;
using RecyOs.Engine.Alerts.Entities;
using RecyOs.Engine.Alerts.Interfaces;
using RecyOs.Engine.Interfaces;
using RecyOs.Engine.Tools.Interfaces;
using RecyOs.ORM.DTO;
using RecyOs.ORM.Interfaces;
using TaskStatus = RecyOs.Engine.Alerts.Entities.TaskStatus;

namespace RecyOsTests.EngineTests.Services;

public class EmailBackgroundServiceTests
{
    private readonly Mock<IEmailSender> _emailSenderMock;
    private readonly Mock<IEngineParametterService> _parameterServiceMock;
    private readonly Mock<IEngineMessageMailService> _messageMailServiceMock;
    private readonly Mock<IMigrationStatusService> _migrationStatusServiceMock;
    private readonly List<MessageMailDto> _pendingEmails;

    public EmailBackgroundServiceTests()
    {
        _emailSenderMock = new Mock<IEmailSender>();
        _parameterServiceMock = new Mock<IEngineParametterService>();
        _messageMailServiceMock = new Mock<IEngineMessageMailService>();
        _migrationStatusServiceMock = new Mock<IMigrationStatusService>();

        // Configuration des paramètres par défaut
        _parameterServiceMock.Setup(p => p.GetByNomAsync("EmailBackgroundService", "IsActive", false))
            .ReturnsAsync(new ParameterDto { Type = "boolean", Valeur = "true" });
        _parameterServiceMock.Setup(p => p.GetByNomAsync("EmailBackgroundService", "Interval", false))
            .ReturnsAsync(new ParameterDto { Type = "int", Valeur = "1" });

        // Configuration des emails en attente
        _pendingEmails = new List<MessageMailDto>
        {
            new() { Id = 1, Subject = "Test1", Status = TaskStatus.Pending },
            new() { Id = 2, Subject = "Test2", Status = TaskStatus.Pending }
        };
    }

    [Fact]
    public void ConstructorInitialize_WithDefaultParameters_ShouldCreateParameters()
    {
        // Arrange
        _parameterServiceMock.Setup(p => p.GetByNomAsync("EmailBackgroundService", "IsActive", false))
            .ReturnsAsync((ParameterDto?)null);
        _parameterServiceMock.Setup(p => p.GetByNomAsync("EmailBackgroundService", "Interval", false))
            .ReturnsAsync((ParameterDto?)null);

        // Act
        var service = new EmailBackgroundService(
            _emailSenderMock.Object,
            _parameterServiceMock.Object,
            _messageMailServiceMock.Object,
            _migrationStatusServiceMock.Object
        );

        //call private method Initialize
        var initializeMethod = typeof(EmailBackgroundService).GetMethod("Initialize", BindingFlags.NonPublic | BindingFlags.Instance);
        initializeMethod?.Invoke(service, null);

        // Assert
        _parameterServiceMock.Verify(p => p.CreateAsync(It.IsAny<ParameterDto>()), Times.Exactly(2));
    }

    [Fact]
    public async Task ExecuteAsync_WhenMigrationsNotCompleted_ShouldWait()
    {
        // Arrange
        var migrationCompletedCalled = 0;
        _migrationStatusServiceMock.Setup(m => m.IsMigrationCompleted)
            .Returns(() => {
                migrationCompletedCalled++;
                return migrationCompletedCalled >= 3;
            });

        var service = new EmailBackgroundService(
            _emailSenderMock.Object,
            _parameterServiceMock.Object,
            _messageMailServiceMock.Object,
            _migrationStatusServiceMock.Object
        );

        using var cts = new CancellationTokenSource();

        // Act
        var executeTask = Task.Run(async () => {
            MethodInfo? methodInfo = typeof(EmailBackgroundService).GetMethod("ExecuteAsync", 
                BindingFlags.NonPublic | BindingFlags.Instance);
            Assert.NotNull(methodInfo);
            await (Task)methodInfo.Invoke(service, new object[] { cts.Token });
        });

        // Attendre un peu pour laisser le temps aux vérifications de s'exécuter
        await Task.Delay(500);
        cts.Cancel();

        try 
        {
            await executeTask;
        }
        catch (Exception) 
        {
            // Ignorer l'exception de cancellation
        }

        // Assert
        Assert.True(migrationCompletedCalled >= 1);
        _migrationStatusServiceMock.Verify(m => m.IsMigrationCompleted, Times.AtLeast(1));
    }

    [Fact]
    public async Task ExecuteAsync_WhenServiceNotActive_ShouldNotProcessEmails()
    {
        // Arrange
        _parameterServiceMock.Setup(p => p.GetByNomAsync("EmailBackgroundService", "IsActive", false))
            .ReturnsAsync(new ParameterDto { Type = "boolean", Valeur = "false" });
        _migrationStatusServiceMock.Setup(m => m.IsMigrationCompleted).Returns(true);

        var service = new EmailBackgroundService(
            _emailSenderMock.Object,
            _parameterServiceMock.Object,
            _messageMailServiceMock.Object,
            _migrationStatusServiceMock.Object
        );

        using var cts = new CancellationTokenSource();
        cts.CancelAfter(1000);

        // Act
        MethodInfo? methodInfo = typeof(EmailBackgroundService).GetMethod("ExecuteAsync", 
            BindingFlags.NonPublic | BindingFlags.Instance);
        Assert.NotNull(methodInfo);
        try
        {
            await (Task)methodInfo.Invoke(service, new object[] { cts.Token });
        }
        catch (TargetInvocationException ex) when (ex.InnerException is TaskCanceledException)
        {
            // Comportement attendu
        }

        // Assert
        _messageMailServiceMock.Verify(m => m.GetUnsentEmails(), Times.Never);
    }

    [Fact]
    public async Task ExecuteAsync_WithPendingEmails_ShouldProcessEmails()
    {
        // Arrange
        _migrationStatusServiceMock.Setup(m => m.IsMigrationCompleted).Returns(true);
        _messageMailServiceMock.Setup(m => m.GetUnsentEmails())
            .ReturnsAsync(_pendingEmails);
        _emailSenderMock.Setup(e => e.SendEmailAsync(It.IsAny<MessageMailDto>()))
            .ReturnsAsync(true);

        var service = new EmailBackgroundService(
            _emailSenderMock.Object,
            _parameterServiceMock.Object,
            _messageMailServiceMock.Object,
            _migrationStatusServiceMock.Object
        );

        using var cts = new CancellationTokenSource();
        cts.CancelAfter(1000);

        // Act
        MethodInfo? methodInfo = typeof(EmailBackgroundService).GetMethod("ExecuteAsync", 
            BindingFlags.NonPublic | BindingFlags.Instance);
        Assert.NotNull(methodInfo);
        try
        {
            await (Task)methodInfo.Invoke(service, new object[] { cts.Token });
        }
        catch (TargetInvocationException ex) when (ex.InnerException is TaskCanceledException)
        {
            // Comportement attendu
        }

        // Assert
        _messageMailServiceMock.Verify(m => m.GetUnsentEmails(), Times.AtLeastOnce);
        _emailSenderMock.Verify(e => e.SendEmailAsync(It.IsAny<MessageMailDto>()), 
            Times.Exactly(_pendingEmails.Count));
        _messageMailServiceMock.Verify(m => m.UpdateAsync(It.Is<MessageMailDto>(
            dto => dto.Status == TaskStatus.Completed)), 
            Times.Exactly(_pendingEmails.Count));
    }

    [Fact]
    public async Task ExecuteAsync_WhenEmailSendingFails_ShouldUpdateStatusToFailed()
    {
        // Arrange
        _migrationStatusServiceMock.Setup(m => m.IsMigrationCompleted).Returns(true);
        _messageMailServiceMock.Setup(m => m.GetUnsentEmails())
            .ReturnsAsync(_pendingEmails);
        _emailSenderMock.Setup(e => e.SendEmailAsync(It.IsAny<MessageMailDto>()))
            .ReturnsAsync(false);
        _emailSenderMock.Setup(e => e.GetLastError())
            .Returns("Test error");

        var service = new EmailBackgroundService(
            _emailSenderMock.Object,
            _parameterServiceMock.Object,
            _messageMailServiceMock.Object,
            _migrationStatusServiceMock.Object
        );

        using var cts = new CancellationTokenSource();

        // Act
        var executeTask = Task.Run(async () => {
            MethodInfo? methodInfo = typeof(EmailBackgroundService).GetMethod("ExecuteAsync", 
                BindingFlags.NonPublic | BindingFlags.Instance);
            Assert.NotNull(methodInfo);
            await (Task)methodInfo.Invoke(service, new object[] { cts.Token });
        });

        // Attendre un peu pour laisser le temps aux vérifications de s'exécuter
        await Task.Delay(500);
        cts.Cancel();

        try 
        {
            await executeTask;
        }
        catch (Exception) 
        {
            // Ignorer l'exception de cancellation
        }

        // Assert
        _messageMailServiceMock.Verify(m => m.UpdateAsync(It.Is<MessageMailDto>(
            dto => dto.Status == TaskStatus.Failed && dto.Error == "Test error")), 
            Times.Exactly(_pendingEmails.Count));
    }

    [Fact]
    public void Constructor_WithInvalidIsActiveType_ShouldDefaultToFalse()
    {
        // Arrange
        _parameterServiceMock.Setup(p => p.GetByNomAsync("EmailBackgroundService", "IsActive", false))
            .ReturnsAsync(new ParameterDto { Type = "string", Valeur = "true" }); // Type invalide

        // Act
        var service = new EmailBackgroundService(
            _emailSenderMock.Object,
            _parameterServiceMock.Object,
            _messageMailServiceMock.Object,
            _migrationStatusServiceMock.Object
        );

        // Vérifier que le service est inactif en essayant d'envoyer des emails
        _migrationStatusServiceMock.Setup(m => m.IsMigrationCompleted).Returns(true);
        _messageMailServiceMock.Setup(m => m.GetUnsentEmails())
            .ReturnsAsync(_pendingEmails);

        using var cts = new CancellationTokenSource();
        var executeTask = Task.Run(async () => {
            MethodInfo? methodInfo = typeof(EmailBackgroundService).GetMethod("ExecuteAsync", 
                BindingFlags.NonPublic | BindingFlags.Instance);
            Assert.NotNull(methodInfo);
            await (Task)methodInfo.Invoke(service, new object[] { cts.Token });
        });

        // Attendre un peu
        Task.Delay(500).Wait();
        cts.Cancel();

        // Assert
        _messageMailServiceMock.Verify(m => m.GetUnsentEmails(), Times.Never);
    }

    [Fact]
    public void Constructor_WithInvalidIntervalType_ShouldDefaultTo3600()
    {
        // Arrange
        _parameterServiceMock.Setup(p => p.GetByNomAsync("EmailBackgroundService", "Interval", false))
            .ReturnsAsync(new ParameterDto { Type = "string", Valeur = "60" }); // Type invalide

        // Act
        var service = new EmailBackgroundService(
            _emailSenderMock.Object,
            _parameterServiceMock.Object,
            _messageMailServiceMock.Object,
            _migrationStatusServiceMock.Object
        );

        // Assert - Vérifier que l'intervalle par défaut est utilisé
        _migrationStatusServiceMock.Setup(m => m.IsMigrationCompleted).Returns(true);
        _messageMailServiceMock.Setup(m => m.GetUnsentEmails())
            .ReturnsAsync(_pendingEmails);

        using var cts = new CancellationTokenSource();
        var executeTask = Task.Run(async () => {
            MethodInfo? methodInfo = typeof(EmailBackgroundService).GetMethod("ExecuteAsync", 
                BindingFlags.NonPublic | BindingFlags.Instance);
            Assert.NotNull(methodInfo);
            await (Task)methodInfo.Invoke(service, new object[] { cts.Token });
        });

        // Attendre un peu moins que l'intervalle par défaut
        Task.Delay(100).Wait();
        cts.Cancel();

        // Vérifier qu'un seul cycle a eu le temps de s'exécuter
        _messageMailServiceMock.Verify(m => m.GetUnsentEmails(), Times.Once);
    }
}
