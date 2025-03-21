using System.Collections.Generic;
using RecyOs.ORM.DTO;

namespace RecyOs.Engine.Interfaces;

public interface IEngineSyncStatusService
{
    public EngineSyncStatusDto GetByModuleName(string prmValue);
    IList<EngineSyncStatusDto> GetEnabledModules();
}