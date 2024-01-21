namespace CabWebApi.Content.Middlewares;
public class ConsoleLoggerMiddleware
{
	private readonly RequestDelegate next;
	private readonly ILogger logger;
	public ConsoleLoggerMiddleware(RequestDelegate next, Type category, LogLevel logLevel)
	{
		this.next = next;
		var loggerFactory = LoggerFactory.Create(builder =>
		{
			builder.AddConsole();
			builder.SetMinimumLevel(logLevel);
		});
		logger = loggerFactory.CreateLogger(category);
	}
	public async Task InvokeAsync(HttpContext context)
	{
		string path = context.Request.Path.ToString();
		string method = context.Request.Method;
		string? query = context.Request.Query.ToString();
		logger.Log(LogLevel.Information, $"Request: {method} - {path} - {query}");
		await next.Invoke(context);

		int code = context.Response.StatusCode;
		string? body = context.Response.Body.ToString();
		logger.Log(LogLevel.Information, $"Responce: {code} - {body}");
	}
}