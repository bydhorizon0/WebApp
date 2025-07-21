using TheaterWebApp.Models;

namespace TheaterWebApp.Service;

public interface ITheaterService
{
    Task<PageResult<MovieViewModel>> GetAllMoviesAsync(MovieSearchRequest request);
    Task<MovieDetailViewModel> GetMovieByIdAsync(long id);
    Task SaveMovieCommentAsync(long movieId, CommentRequest comment);
}