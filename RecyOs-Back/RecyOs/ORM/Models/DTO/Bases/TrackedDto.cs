using System;

namespace RecyOs.ORM.DTO;

public class TrackedDto : DeletableDto
{
#nullable enable
    public DateTime? CreateDate { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public string? CreatedBy { get; set; }
    public string? UpdatedBy { get; set; }
#nullable disable
}