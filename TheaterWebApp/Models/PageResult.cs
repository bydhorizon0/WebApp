namespace TheaterWebApp.Models;

public class PageResult<T>
{
    public ICollection<T> Items { get; set; } = new  List<T>();
    public int TotalCount { get; set; }
    public int TotalPages { get; set; }
    public int CurrentPage { get; set; }
    public string SearchKeyword { get; set; } = string.Empty;
    public string SearchType { get; set; } = string.Empty;
}