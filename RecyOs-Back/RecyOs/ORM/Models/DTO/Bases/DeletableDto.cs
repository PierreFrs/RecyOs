namespace RecyOs.ORM.DTO;

public class DeletableDto : BaseDto
{
    public bool IsDeleted { get; set; } = false;
}