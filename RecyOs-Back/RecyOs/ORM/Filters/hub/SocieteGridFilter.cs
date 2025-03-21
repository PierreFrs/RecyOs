namespace RecyOs.ORM.Filters.hub;

public class SocieteGridFilter : BaseFilter
{
    #nullable enable
    public string? FilterBySocieteId { get; set; }
    public string? FilterByNom { get; set; }
    public string? FilterByIdOdoo { get; set; }
    public string? FilterIsDeleted { get; set; }
    #nullable disable
}
