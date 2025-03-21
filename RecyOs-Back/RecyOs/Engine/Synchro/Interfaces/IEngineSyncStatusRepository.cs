using System.Collections.Generic;
using RecyOs.ORM.Entities;

namespace RecyOs.Engine.Interfaces;

public interface IEngineSyncStatusRepository
{
    public EngineSyncStatus GetByModuleName(string prmValue);
    IList<EngineSyncStatus> GetEnabledModules();
}