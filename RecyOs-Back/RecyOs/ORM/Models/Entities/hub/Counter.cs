using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RecyOs.ORM.Entities.hub;

[Table("counter")]
public class Counter : BaseEntity
{
    [Required]
    [Column("name")]
    [StringLength(50)]
    public string Name { get; set; }
    
    [Required]
    [Column("value")]
    public int Value { get; set; }
    
    [Required]
    [Column("description")]
    [StringLength(255)]
    public string Description { get; set; }
}