using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
namespace Watchdog;

public class Program
{
	private static object consoleColorLock = new();
	
	public static void Main(string[] args)
	{
		string? host = Environment.GetEnvironmentVariable("WATCHDOG_HOST");
		if (host is null)
			throw new Exception("'WATCHDOG_HOST' environment variable is required.");
		
		string? portStr = Environment.GetEnvironmentVariable("WATCHDOG_PORT");
		if (portStr is null)
			throw new Exception("'WATCHDOG_HOST' environment variable is required.");
		
		int port = int.Parse(portStr);
		IPAddress address = Dns.GetHostEntry(host).AddressList.First();
		IPEndPoint serverEndPoint = new(address, port);
		using Socket server = new(SocketType.Stream, ProtocolType.Tcp);
		server.Bind(serverEndPoint);
		server.Listen();
		Action<object?> clientAction = new(socketObj =>
		{
			if (socketObj is not Socket socket)
				return;

			Stopwatch stopwatch = new();
			stopwatch.Start();
			byte[] _ = Array.Empty<byte>();
			socket.Receive(_);
			stopwatch.Stop();
			long elapsed = stopwatch.ElapsedMilliseconds;
			switch (elapsed)
			{
				case >= 1000:
					Critical("Application performance is low.");
					break;
				case < 1000 and >= 500:
					Warn("Application performance degraded.");
					break;
				default:
					Info("Application performance is stable.");
					break;
			}
			socket.Close();
		});

		do
		{
			Socket client = server.Accept();
			Task execution = new(clientAction, client);
			execution.Start();
		} while (true);
	}
	
	private static void Info(in string message)
	{
		lock (consoleColorLock)
		{
			Console.BackgroundColor = ConsoleColor.DarkGreen;
			Console.ForegroundColor = ConsoleColor.White;
			Console.Write("***");
			Console.ResetColor();
			Console.Write(message);
			Console.WriteLine();
		}
	}

	private static void Warn(in string message)
	{
		lock (consoleColorLock)
		{
			Console.BackgroundColor = ConsoleColor.Yellow;
			Console.ForegroundColor = ConsoleColor.White;
			Console.Write("***");
			Console.ResetColor();
			Console.Write(message);
			Console.WriteLine();
		}
	}

	private static void Critical(in string message)
	{
		lock (consoleColorLock)
		{
			Console.BackgroundColor = ConsoleColor.Red;
			Console.ForegroundColor = ConsoleColor.White;
			Console.Write("***");
			Console.ResetColor();
			Console.Write(message);
			Console.WriteLine();
		}
	}
}

// static string GetHealthLog(long time) => time switch
// {
// 	>= 1000 => "Application is unhealthy",
// 	< 1000 and >= 500 => "Application is degraded",
// 	_ => "Application is healthy"
// };