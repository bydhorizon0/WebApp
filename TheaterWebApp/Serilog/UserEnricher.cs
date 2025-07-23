using System.Security.Claims;
using Serilog.Core;
using Serilog.Events;

namespace TheaterWebApp.Serilog;

public class UserEnricher : ILogEventEnricher
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public UserEnricher(IHttpContextAccessor httpContextAccessor) => _httpContextAccessor = httpContextAccessor;
    
    public void Enrich(LogEvent logEvent, ILogEventPropertyFactory propertyFactory)
    {
        var context = _httpContextAccessor.HttpContext;

        var email = context?.User.Claims
            .FirstOrDefault(c => c.Type == ClaimTypes.Email)
            ?.Value ?? "UnknownEmail";

        string ip = context?.Connection.RemoteIpAddress?.ToString() ?? "UnknownIP";

        string userAgent = context?.Request.Headers["User-Agent"].ToString() ?? "UnknownUserAgent";
        
        logEvent.AddOrUpdateProperty(propertyFactory.CreateProperty("Email", email));
        logEvent.AddOrUpdateProperty(propertyFactory.CreateProperty("UserIP", ip));
        logEvent.AddOrUpdateProperty(propertyFactory.CreateProperty("User-Agent", userAgent));
    }
}