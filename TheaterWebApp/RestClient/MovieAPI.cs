using System.Text.Json;

namespace TheaterWebApp.RestClient;

public class MovieAPI
{
    private static readonly string IMAGE_BASE_URL = "https://image.tmdb.org/t/p/w500/{0}";
    private static readonly string SAVE_PATH = "/Users/inandexhale95/Documents/dotnet-projects/Syntax/Basic/Images";
    
    static async Task GetMovieData()
    {
        var downloadTasks = new List<Task>();
        
        var client = new HttpClient();
        
        foreach (var page in Enumerable.Range(1, 10))
        {
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri($"https://api.themoviedb.org/3/discover/movie?include_adult=false&include_video=false&language=ko-KR&page={page}&sort_by=popularity.desc"),
                Headers =
                {
                    { "Accept", "application/json" },
                    { "Authorization", "Bearer eyJhbGciOiJIUzI1NiJ9.eyJhdWQiOiIwOTQxMmE1ZDNiNTczNzMzOWI1MzAxMWJmYjhlZjA2NyIsIm5iZiI6MTc0OTIyNzY3NC40ODg5OTk4LCJzdWIiOiI2ODQzMTg5YWE2YmQ0MTQ4YzI0NTY4ZDQiLCJzY29wZXMiOlsiYXBpX3JlYWQiXSwidmVyc2lvbiI6MX0.4j0_0RgGuQdgWsMbYSe3oHVZe-Id3i6i2EP5vLVpujM" },
                },
            };
            
            using (var response = await client.SendAsync(request))
            {
                response.EnsureSuccessStatusCode();
                var body = await response.Content.ReadAsStringAsync();
                var jsonElement = JsonSerializer.Deserialize<JsonElement>(body);

                if (jsonElement.TryGetProperty("results", out var results))
                {
                    foreach (var movie in results.EnumerateArray())
                    {
                        string title = movie.TryGetProperty("title", out var titleElement)
                            ? titleElement.GetString() ?? ""
                            : "";
                        
                        if (movie.TryGetProperty("poster_path", out var posterElement))
                        {
                            string posterPath = posterElement.GetString() ?? throw new Exception("Poster path not found");
                            
                            string imageUrl = string.Format(IMAGE_BASE_URL, posterPath.TrimStart('/'));
                            downloadTasks.Add(DownloadMovieImage(imageUrl, title));
                        }
                    }
                }
            }
        }
        await Task.WhenAll(downloadTasks);
    }

    static async Task DownloadMovieImage(string imageUrl, string title)
    {
        if (!Directory.Exists(SAVE_PATH))
            Directory.CreateDirectory(SAVE_PATH);
        
        string saveFilePath = Path.Combine(SAVE_PATH, $"{title}.jpg");
        
        using HttpClient client = new HttpClient();
        var imageBytes = await client.GetByteArrayAsync(imageUrl);
        await File.WriteAllBytesAsync(saveFilePath, imageBytes);
    }
}