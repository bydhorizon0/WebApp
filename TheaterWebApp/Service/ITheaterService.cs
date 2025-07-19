using TheaterWebApp.Models;

namespace TheaterWebApp.Service;

public interface ITheaterService
{
    Task<PageResult<MovieViewModel>> GetAllMoviesAsync(MovieSearchRequest request);
    Task<MovieDetailViewModel> GetMovieByIdAsync(int id);
}