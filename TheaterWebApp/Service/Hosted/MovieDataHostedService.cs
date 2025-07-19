using TheaterWebApp.RestClient;

namespace TheaterWebApp.Service.Hosted;

public class MovieDataHostedService : IHostedService
{
    private readonly IWebHostEnvironment _env;
    private readonly ILogger<MovieDataHostedService> _logger;

    public MovieDataHostedService(IWebHostEnvironment env, ILogger<MovieDataHostedService> logger)
    {
        _env = env;
        _logger = logger;
    }
    
    public async Task StartAsync(CancellationToken cancellationToken)
    {
        var savePath = Path.Combine(_env.WebRootPath, "movie", "images");
        
        try
        {
            _logger.LogInformation("Movie API 데이터 수집 시작");
            // await MovieScrapper.GetMovieData(savePath);
            _logger.LogInformation("Movie API 데이터 수집 완료");
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Movie API 호출 중 에러 발생");
        }
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        // 앱 종료 시 작업이 필요하면 여기에 추가.
        return Task.CompletedTask;
    }
}