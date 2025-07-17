using TheaterWebApp.Models;

namespace TheaterWebApp.Service;

public interface ITheaterService
{
    Task<List<MovieViewModel>> GetAllMoviesAsync();
    Task<MovieDetailViewModel> GetMovieByIdAsync(int id);
}