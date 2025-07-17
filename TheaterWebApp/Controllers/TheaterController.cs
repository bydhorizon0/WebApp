using Microsoft.AspNetCore.Mvc;
using TheaterWebApp.Models;
using TheaterWebApp.Service;

namespace TheaterWebApp.Controllers;

[Route("[controller]/[action]")]
public class TheaterController : ControllerBase
{
    private readonly ILogger<TheaterController> _logger;
    private readonly ITheaterService _theaterService;

    public TheaterController(ILogger<TheaterController> logger, ITheaterService theaterService)
    {
        _logger = logger;
        _theaterService = theaterService;
    }

    [HttpGet]
    public async Task<ActionResult<List<MovieViewModel>>> GetMovies()
    {
        var allMovies = await _theaterService.GetAllMoviesAsync();
        return Ok(allMovies);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<MovieDetailViewModel>> GetMovie(int id)
    {
        var movieDetail = await _theaterService.GetMovieByIdAsync(id);
        return Ok(movieDetail);
    }
}