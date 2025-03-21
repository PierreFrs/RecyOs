using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RecyOs.ORM.Entities;

[Table("EngineSyncStatus")]
public class EngineSyncStatus : DeletableEntity
{
    [Column("module_name")]
    [StringLength(255)]
    [Required]
    public string ModuleName { get; set; }
#nullable enable
    [Column("last_create")]
    public DateTime? LastCreate { get; set; }
    
    [Column("last_update")]
    public DateTime? LastUpdate { get; set; }
    
    [Column("id_created_by_dest")]
    public bool? IdCreatedByDest { get; set; }
#nullable disable
    [Column("sync_cre")]
    [Required]
    public bool SyncCre { get; set; }
    
    [Column("sync_upd")] 
    [Required] 
    public bool SyncUpd { get; set; }
    
    [Column("module_enabled")]
    [Required]
    public bool ModuleEnabled { get; set; }
}