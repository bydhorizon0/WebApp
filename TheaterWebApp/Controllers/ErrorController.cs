using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace TheaterWebApp.Controllers;

[Route("[controller]")]
public class ErrorController : Controller
{
    private readonly ILogger<ErrorController> _logger;

    public ErrorController(ILogger<ErrorController> logger) => _logger = logger;
    
    [Route("{statusCode:int}")]
    public IActionResult HttpStatusCodeHandler(int statusCode)
    {
        ViewBag.StatusCode = statusCode;
        
        return View("GlobalError");
    }

    /// <summary>
    /// ASP.NET Core에서 처리되지 않은 예외가 발생했을 때 실행된다.
    /// </summary>
    /// <returns></returns>
    [Route("Exception")]
    public IActionResult ExceptionHandler()
    {
        var exceptionFeature = HttpContext.Features.Get<IExceptionHandlerFeature>();
        if (exceptionFeature != null)
        {
            _logger.LogError(exceptionFeature.Error, "Unhandled exception");
        }
        
        return View("UnhandledError");
    }
    
    public IActionResult Crash() => throw new Exception("일부러 발생한 예외.");
}