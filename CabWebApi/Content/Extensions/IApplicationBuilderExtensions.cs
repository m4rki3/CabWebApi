using CabWebApi.Content.Middlewares;
using System.Runtime.CompilerServices;

namespace CabWebApi.Content.Extensions;
public static class IApplicationBuilderExtensions
{
    public static IApplicationBuilder UseConsoleLogging<TCategory>(
        this IApplicationBuilder builder,
        LogLevel logLevel)
        where TCategory : class
        =>
        builder.UseMiddleware<ConsoleLoggerMiddleware>(typeof(TCategory), logLevel);
}
