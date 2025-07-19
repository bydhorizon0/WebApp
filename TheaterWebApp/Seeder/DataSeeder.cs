using Microsoft.AspNetCore.Components.Web;
using TheaterWebApp.Contexts;
using TheaterWebApp.Entities;
using TheaterWebApp.RestClient;

namespace TheaterWebApp.Seeder;

public class DataSeeder
{
    private readonly TheaterContext _context;
    private readonly IWebHostEnvironment _env;
    private readonly MovieScrapper _movieScrapper;

    public DataSeeder(TheaterContext context, IWebHostEnvironment env)
    {
        _context = context;
        _env = env;
        _movieScrapper = new MovieScrapper();
    }

    public static void SeedUsers(TheaterContext context)
    {
        if (!context.Users.Any())
        {
            context.Users.AddRange(new List<User>
            {
                new User { Nickname = "jack99", Email = "jack99@gmail.com", Password = "123123" },
                new User { Nickname = "jack55", Email = "jack55@gmail.com", Password = "123123" },
            });

            context.SaveChanges();
        }
    }

    public async Task SeedMoviesAsync()
    {
        var movieInfos = await _movieScrapper.GetMovieData();
        string imageAccessPath = "/movie/images";

        var movieEntities = movieInfos.Select(m =>
            new Movie
            {
                Title = m.Title,
                Description = m.Description,
                ReleaseDate = m.ReleaseDate,
                MovieImages = new List<MovieImage>
                {
                    new MovieImage
                    {
                        ImageName = m.Title.Trim(),
                        Path = $"{imageAccessPath}/{m.Title.Trim()}.jpg",
                    }
                }
            }
        );
        await _context.Movies.AddRangeAsync(movieEntities);
        await _context.SaveChangesAsync();

        string imageSavePath = Path.Combine(_env.WebRootPath, "movie", "images");
        foreach (var movieInfo in movieInfos)
        {
            await _movieScrapper.DownloadMovieImageAsync(imageSavePath, movieInfo.ImageUrl, movieInfo.Title);
        }
    }
}