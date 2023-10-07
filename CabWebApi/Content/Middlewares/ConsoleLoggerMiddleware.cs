using Microsoft.Extensions.Logging.Console;
using Microsoft.Extensions.Logging.Debug;
using Microsoft.Extensions.Options;

namespace CabWebApi.Content.Middlewares;
public class ConsoleLoggerMiddleware
{
    private readonly RequestDelegate next;
    private readonly LogLevel logLevel;
    private readonly ILogger logger;
    public ConsoleLoggerMiddleware(RequestDelegate next, Type category, LogLevel logLevel )
    {
        this.next = next;
        this.logLevel = logLevel;
        var loggerFactory = LoggerFactory.Create(builder =>
        {
            builder.AddConsole();
            builder.SetMinimumLevel(logLevel);
        });
        logger = loggerFactory.CreateLogger(category);
    }
    public async Task InvokeAsync(HttpContext context)
    {
        logger.Log(logLevel, "message");
        await next.Invoke(context);
    }
}
