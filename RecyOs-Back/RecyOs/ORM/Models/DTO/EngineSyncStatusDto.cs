using System;

namespace RecyOs.ORM.DTO;

public class EngineSyncStatusDto
{
    public string ModuleName { get; set; }
#nullable enable
    public DateTime? LastCreate { get; set; }
    public DateTime? LastUpdate { get; set; }
    public bool? IdCreatedByDest { get; set; }
#nullable disable
    public bool SyncCre { get; set; }
    public bool SyncUpd { get; set; }
    public bool ModuleEnabled { get; set; }
    
}