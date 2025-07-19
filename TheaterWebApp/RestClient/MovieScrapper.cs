using System.Text.Json;

namespace TheaterWebApp.RestClient;

public record MovieInfo
{
    public string Title { get; set; }
    public string Description { get; set; }
    public DateTime ReleaseDate { get; set; }
    public string ImageUrl { get; set; }
}

public class MovieScrapper
{
    private static readonly HttpClient client = new HttpClient();
    private static readonly string IMAGE_BASE_URL = "https://image.tmdb.org/t/p/w500/{0}";
    private static readonly SemaphoreSlim _semaphore = new SemaphoreSlim(1, 8);

    public async Task<List<MovieInfo>> GetMovieData()
    {
        var movieInfos = new List<MovieInfo>();

        foreach (var page in Enumerable.Range(1, 5))
        {
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri(
                    $"https://api.themoviedb.org/3/discover/movie?include_adult=false&include_video=false&language=ko-KR&page={page}&sort_by=popularity.desc&with_original_language=en"),
                Headers =
                {
                    { "Accept", "application/json" },
                    {
                        "Authorization",
                        "Bearer eyJhbGciOiJIUzI1NiJ9.eyJhdWQiOiIwOTQxMmE1ZDNiNTczNzMzOWI1MzAxMWJmYjhlZjA2NyIsIm5iZiI6MTc0OTIyNzY3NC40ODg5OTk4LCJzdWIiOiI2ODQzMTg5YWE2YmQ0MTQ4YzI0NTY4ZDQiLCJzY29wZXMiOlsiYXBpX3JlYWQiXSwidmVyc2lvbiI6MX0.4j0_0RgGuQdgWsMbYSe3oHVZe-Id3i6i2EP5vLVpujM"
                    },
                },
            };

            using var response = await client.SendAsync(request);

            response.EnsureSuccessStatusCode();
            var body = await response.Content.ReadAsStringAsync();
            var jsonElement = JsonSerializer.Deserialize<JsonElement>(body);

            if (jsonElement.TryGetProperty("results", out var results))
            {
                foreach (var movie in results.EnumerateArray())
                {
                    var movieInfo = new MovieInfo();
                    if (movie.TryGetProperty("title", out var titleElement))
                    {
                        string title = titleElement.ToString() ?? "";
                        movieInfo.Title = title;
                    }
                    
                    if (movie.TryGetProperty("overview", out var overviewElement))
                    {
                        string overview = overviewElement.GetString() ?? "";
                        movieInfo.Description = overview;
                    }

                    if (movie.TryGetProperty("poster_path", out var posterElement))
                    {
                        string posterPath = posterElement.GetString() ?? "";
                        string imageUrl = string.Format(IMAGE_BASE_URL, posterPath.TrimStart('/'));
                        movieInfo.ImageUrl = imageUrl;
                    }
                    
                    if (movie.TryGetProperty("release_date", out var releaseDateElement))
                    {
                        string releaseDateStr = releaseDateElement.GetString() ?? "";
                        DateTime.TryParse(releaseDateStr, out DateTime releaseDate);
                        movieInfo.ReleaseDate = releaseDate;
                    }
                    
                    movieInfos.Add(movieInfo);
                }
            }
        }

        return movieInfos;
    }

    public async Task DownloadMovieImageAsync(string savePath, string imageUrl, string title)
    {
        await _semaphore.WaitAsync();

        try
        {
            if (!Directory.Exists(savePath))
                Directory.CreateDirectory(savePath);
            
            string saveFilePath = Path.Combine(savePath, $"{RemoveInvalidFileNameChars(title)}.jpg");
            
            var imageBytes = await client.GetByteArrayAsync(imageUrl);
            await File.WriteAllBytesAsync(saveFilePath, imageBytes);
        }
        catch (Exception e)
        {
            Console.WriteLine($"[error] {e.Message}]");
        }
        finally
        {
            _semaphore.Release();
        }
    }

    static string RemoveInvalidFileNameChars(string filename)
    {
        var invalidChars = Path.GetInvalidFileNameChars();
        return new string(filename.Where(c => !invalidChars.Contains(c)).ToArray());
    }
}