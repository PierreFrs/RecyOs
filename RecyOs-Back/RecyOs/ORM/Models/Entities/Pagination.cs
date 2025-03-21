namespace RecyOs.ORM.Entities;

public class Pagination
{
    public int length { get; set; }
    public int size { get; set; }
    public int page { get; set; }
    public int lastPage { get; set; }
    public int startIndex { get; set; }
    public int cost { get; set; }
}