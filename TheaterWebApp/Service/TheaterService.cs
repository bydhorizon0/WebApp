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

    public async Task<PageResult<MovieViewModel>> GetAllMoviesAsync(MovieSearchRequest request)
    {
        var query = _context.Movies
            .Include(m => m.MovieImages)
            .Include(m => m.MovieGenres)
            .AsQueryable();

        // 검색 조건
        if (request.Type.ToLower() == "title")
        {
            query = query.Where(m => m.Title.ToLower().Contains(request.Keyword));
        }
        else if (request.Type.ToLower() == "description")
        {
            query = query.Where(m => m.Description.ToLower().Contains(request.Keyword.ToLower()));
        }
        else
        {
            query = query.Where(m => m.Title.ToLower().Contains(request.Keyword.ToLower()));
        }

        int totalCount = await query.CountAsync();
        // 페이지네이션
        int skip = (request.Page - 1) * request.Size;

        var movies = await query
            .OrderByDescending(m => m.ReleaseDate)
            .Skip(skip)
            .Take(request.Size)
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

        return new PageResult<MovieViewModel>
        {
            Items = movies,
            TotalCount = totalCount,
            TotalPages = (int)Math.Ceiling((double)totalCount / request.Size),
            CurrentPage = request.Page,
            SearchKeyword = request.Keyword,
            SearchType = request.Type,  
        };
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