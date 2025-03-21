namespace RecyOs.ORM.Interfaces;

public interface IMigrationStatusService
{
    bool IsMigrationCompleted { get; }
    void SetMigrationCompleted();
}