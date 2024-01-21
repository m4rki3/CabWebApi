using CabWebApi.Content.Middlewares;

namespace CabWebApi.Content.Extensions;
public static class ApplicationBuilderExtensions
{
	public static IApplicationBuilder UseConsoleLogging<TCategoryName>(
		this IApplicationBuilder builder, LogLevel minLogLevel
	) where TCategoryName : class =>
		builder.UseMiddleware<ConsoleLoggerMiddleware>(typeof(TCategoryName), minLogLevel);

	public static IApplicationBuilder UseWatchdog(
		this IApplicationBuilder builder, string watchdogHost, int watchdogPort) =>
		builder.UseMiddleware<WatchdogMiddleware>(watchdogHost, watchdogPort);
}