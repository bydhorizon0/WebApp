using Microsoft.EntityFrameworkCore;
using TheaterWebApp.Contexts;
using TheaterWebApp.Models;

namespace TheaterWebApp.Service;

public class TheaterService : ITheaterService
{
    private readonly ILogger<TheaterService> _logger;
    private readonly TheaterContext _context;

    public TheaterService(ILogger<TheaterService> logger, TheaterContext context)
    {
        _logger = logger;
        _context = context;
    }

    public async Task<List<MovieViewModel>> GetAllMoviesAsync()
    {
        return await _context.Movies
            .Select(m => new MovieViewModel
            {
                Id = m.Id,
                Title = m.Title,
                Description = m.Description,
                ReleaseDate = m.ReleaseDate,
                MainGenre = m.MovieGenres.FirstOrDefault().Genre.ToString(),
                MainImage = m.MovieImages.Select(mi => new MovieImageModel { ImageName = mi.ImageName, Path = mi.Path })
                    .FirstOrDefault(),
                CreatedAt = m.CreatedAt,
                UpdatedAt = m.UpdatedAt
            })
            .ToListAsync();
    }

    public async Task<MovieDetailViewModel> GetMovieByIdAsync(int id)
    {
        return await _context.Movies
            .Select(m => new MovieDetailViewModel
            {
                Id = m.Id,
                Title = m.Title,
                Description = m.Description,
                ReleaseDate = m.ReleaseDate,
                MovieImages = m.MovieImages
                    .Select(mi => new MovieImageModel { ImageName = mi.ImageName, Path = mi.Path }).ToList(),
                Genres = m.MovieGenres.Select(genre => genre.Genre.ToString()).ToList(),
                Comments = m.Comments
                    .Where(c => c.ParentCommendId == null)
                    .Select(c => new CommentModel
                    {
                        Id = c.Id,
                        Content = c.Content,
                        UserNickname = c.User.Nickname,
                        ParentCommentId = null,
                        CreatedAt = c.CreatedAt,
                        UpdatedAt = c.UpdatedAt,
                        NestedComment =
                            c.NestedComments.Select(nc => new CommentModel
                            {
                                Id = nc.Id,
                                Content = nc.Content,
                                UserNickname = nc.User.Nickname,
                                ParentCommentId = nc.ParentCommendId,
                                CreatedAt = nc.CreatedAt,
                                UpdatedAt = nc.UpdatedAt,
                            }).ToList()
                    }
                ).ToList(),
                MovieRating = 0.0,
            })
            .FirstOrDefaultAsync(m => m.Id == id) ?? throw new KeyNotFoundException($"Movie with id {id} not found");
    }
}