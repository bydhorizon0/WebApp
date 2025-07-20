using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TheaterWebApp.Models;
using TheaterWebApp.Service;

namespace TheaterWebApp.Controllers;

[Authorize]
[Route("[controller]/[action]")]
public class TheaterController : Controller
{
    private readonly ILogger<TheaterController> _logger;
    private readonly ITheaterService _theaterService;

    public TheaterController(ILogger<TheaterController> logger, ITheaterService theaterService)
    {
        _logger = logger;
        _theaterService = theaterService;
    }
    
    [AllowAnonymous]
    [HttpGet("/")]
    public async Task<IActionResult> Index() => View();

    [HttpGet("/list")]
    public async Task<ActionResult> List([FromQuery] MovieSearchRequest request)
    {
        var pageResult = await _theaterService.GetAllMoviesAsync(request);
        return View(pageResult);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult> Detail(int id)
    {
        var movieDetail = await _theaterService.GetMovieByIdAsync(id);
        return View(movieDetail);
    }
}