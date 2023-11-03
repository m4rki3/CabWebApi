using CabWebApi.Content.Middlewares;
using System.Runtime.CompilerServices;

namespace CabWebApi.Content.Extensions;
public static class IApplicationBuilderExtensions
{
    public static IApplicationBuilder UseConsoleLogging<TCategoryName>(
        this IApplicationBuilder builder, LogLevel minLogLevel
    ) where TCategoryName : class =>
        builder.UseMiddleware<ConsoleLoggerMiddleware>(typeof(TCategoryName), minLogLevel);
}