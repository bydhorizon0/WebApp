namespace TheaterWebApp.Models;

public class MovieSearchRequest
{
    private string _keyword = string.Empty;

    public string Keyword
    {
        get => _keyword;
        set => _keyword = value ?? string.Empty;
    }

    private string _type = string.Empty;

    public string Type
    {
        get => _type;
        set => _type = value ?? string.Empty;
    }
    
    private int _page = 1;
    public int Page
    {
        get => _page;
        set => _page = value > 0 ? value : 1;
    }

    private int _size = 12;

    public int Size
    {
        get => _size;
        set => _size = value > 0 ? value : 12;
    }
}