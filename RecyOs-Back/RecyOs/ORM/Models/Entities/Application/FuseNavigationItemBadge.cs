using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RecyOs.ORM.Entities;

[Table("FuseNavigationItemBadge")]
public class FuseNavigationItemBadge: DeletableEntity
{
#nullable enable
    [Column("Title")]
    [StringLength(50)]
    public string? Title { get; set; }
    
    [Column("Classes")]
    [StringLength(255)]
    public string? Classes { get; set; }
#nullable disable
}