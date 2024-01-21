using System.Net.Sockets;

namespace CabWebApi.Content.Middlewares;
public class WatchdogMiddleware
{
	private readonly string watchdogHost;
	private readonly int watchdogPort;
	private readonly RequestDelegate next;
	public WatchdogMiddleware(RequestDelegate next, string watchdogHost, int watchdogPort)
	{
		this.watchdogHost = watchdogHost;
		this.watchdogPort = watchdogPort;
		this.next = next;
	}
	public async Task InvokeAsync(HttpContext context)
	{
		using Socket client = new(SocketType.Stream, ProtocolType.Tcp);
		client.Connect(watchdogHost, watchdogPort);
		await next.Invoke(context);

		byte[] _ = Array.Empty<byte>();
		client.Send(_);
	}
}