using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using TheaterWebApp.Contexts;
using TheaterWebApp.Service;
using Xunit.Abstractions;

namespace TheaterWebAppTest;

public class UnitTest1
{
    private readonly ITestOutputHelper _output;
    private readonly TheaterContext _context;
    private readonly ITheaterService _theaterService;

    public UnitTest1(ITestOutputHelper output)
    {
        _output = output;
        
        var options = new DbContextOptionsBuilder<TheaterContext>()
            .UseMySQL("server=localhost;user=root;password=root123;database=efdb")
            .EnableSensitiveDataLogging()
            .LogTo(_output.WriteLine)
            .Options;
        
        _context = new TheaterContext(options, null);
        
        _theaterService = new TheaterService(null,  _context);
    }
    
    [Fact]
    public async Task Test1()
    {
        var movies = await _theaterService.GetAllMoviesAsync();

        _output.WriteLine("Hello");
        foreach (var movie in movies)
        {
            _output.WriteLine(movie.ToString());
        }
    }

    [Fact]
    public async Task Test2()
    {
        var movieDetail = await _theaterService.GetMovieByIdAsync(1);
        
        _output.WriteLine(movieDetail.ToString());
    }
}