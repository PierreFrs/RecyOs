using RecyOs.Engine.Services;

namespace RecyOsTests.ThirdPartyApiTests.EngineTests.Services;

public class MigrationStatusServiceTests
{
    private readonly MigrationStatusService _service;

    public MigrationStatusServiceTests()
    {
        _service = new MigrationStatusService();
    }

    [Fact]
    public void IsMigrationCompleted_InitialState_ReturnsFalse()
    {
        // Act & Assert
        Assert.False(_service.IsMigrationCompleted);
    }

    [Fact]
    public void SetMigrationCompleted_SetsStatusToTrue()
    {
        // Act
        _service.SetMigrationCompleted();

        // Assert
        Assert.True(_service.IsMigrationCompleted);
    }

    [Fact]
    public void IsMigrationCompleted_AfterMultipleSetCalls_RemainsTrue()
    {
        // Act
        _service.SetMigrationCompleted();
        _service.SetMigrationCompleted();
        _service.SetMigrationCompleted();

        // Assert
        Assert.True(_service.IsMigrationCompleted);
    }

    [Fact]
    public void IsMigrationCompleted_NewInstance_ReturnsFalse()
    {
        // Arrange
        var newService = new MigrationStatusService();

        // Act & Assert
        Assert.False(newService.IsMigrationCompleted);
    }
}
