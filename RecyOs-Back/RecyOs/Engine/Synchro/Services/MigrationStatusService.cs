using RecyOs.ORM.Interfaces;

namespace RecyOs.Engine.Services;

public class MigrationStatusService: IMigrationStatusService
{
    private bool _isMigrationCompleted = false;

    public bool IsMigrationCompleted => _isMigrationCompleted;

    public void SetMigrationCompleted()
    {
        _isMigrationCompleted = true;
    }
}