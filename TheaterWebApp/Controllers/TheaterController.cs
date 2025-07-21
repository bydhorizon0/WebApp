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
    public IActionResult Index() => View();

    [HttpGet]
    public async Task<IActionResult> List([FromQuery] MovieSearchRequest request)
    {
        var pageResult = await _theaterService.GetAllMoviesAsync(request);
        return View(pageResult);
    }

    [HttpGet("{id:long}")]
    public async Task<IActionResult> Detail(long id)
    {
        var movieDetail = await _theaterService.GetMovieByIdAsync(id);
        return View(movieDetail);
    }

    [HttpPost("{movieId:long}")]
    public async Task<IActionResult> SaveComment(long movieId, CommentRequest request)
    {
        await _theaterService.SaveMovieCommentAsync(movieId, request);
        return RedirectToAction(nameof(Detail), new { id = movieId });
    }
}