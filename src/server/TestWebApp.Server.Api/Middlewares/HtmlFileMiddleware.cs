using System;

namespace TestWebApp.Server.Api.Middlewares;

public class HtmlFileMiddleware(ILogger<HtmlFileMiddleware> logger, RequestDelegate next, IWebHostEnvironment env)
{
    private readonly ILogger<HtmlFileMiddleware> _logger = logger;

    private const string _fallbackPath = "/404.html";

    private readonly RequestDelegate _next = next;

    private readonly string _webRootPath = env.WebRootPath;

    public async Task InvokeAsync(HttpContext context)
    {
        _logger.LogInformation("Applying html file middleware");
        var method = context.Request.Method;
        if (!string.Equals(method, "GET", StringComparison.OrdinalIgnoreCase))
        {
            await _next(context);
            return;
        }

        var path = context.Request.Path;
        if (!path.HasValue || path.StartsWithSegments("/api"))
        {
            await _next(context);
            return;
        }

        _logger.LogDebug("Applying html file middleware [{path}]", path);
        var extension = Path.GetExtension(path);
        if (string.IsNullOrEmpty(extension))
        {
            context.Request.Path = GetNewPath([
                string.IsNullOrEmpty(path) ? string.Empty : Path.ChangeExtension(path.Value.TrimEnd('/'), "html"),
                Path.Combine(path, "index.html"),
            ]);
        }
        else if (extension == "html" || extension == "htm")
        {
            context.Request.Path = GetNewPath([path]);
        }

        await _next(context);
    }

    private string GetNewPath(string[] candidates)
    {
        foreach (var path in candidates)
        {
            _logger.LogDebug("Checking path {path}", path);
            if (string.IsNullOrEmpty(path))
            {
                continue;
            }
            if (File.Exists(Path.Combine(_webRootPath, path.TrimStart('/'))))
            {
                _logger.LogDebug("Exists!");
                return path;
            }
        }
        _logger.LogDebug("Any candidates do not exist");
        return _fallbackPath;
    }
}

public static class HtmlFileMiddlewareExtensions
{
    public static IApplicationBuilder UseHtmlFileMiddleware(this IApplicationBuilder builder)
    {
        builder.UseMiddleware<HtmlFileMiddleware>();
        return builder;
    }
}
