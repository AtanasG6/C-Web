using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace HttpClientDemo
{
	public class HttpServer
	{
		private readonly IPAddress ipAddress;
		private readonly int port;
		private readonly TcpListener serverListenter;
		private const string NewLine = "\r\n";
		private bool clientConnected = false;

		public HttpServer(int port)
		{
			this.ipAddress = IPAddress.Parse("127.0.0.1");
			this.port = port;

			this.serverListenter = new TcpListener(this.ipAddress, port);
		}

		public HttpServer(string ipAddress, int port)
		{
			this.ipAddress = IPAddress.Parse(ipAddress);
			this.port = port;

			this.serverListenter = new TcpListener(this.ipAddress, port);
		}

		public void Start()
		{
			Console.WriteLine($"Server started on port {port}.");
			Console.WriteLine("Listening for requests...");

			// daemon // service
			while (true)
			{
				this.serverListenter.Start();

				var connection = serverListenter.AcceptTcpClient();

				if (!clientConnected)
				{
					Console.WriteLine("Client connected");
					clientConnected = true;
				}

				var networkStream = connection.GetStream();

				WriteResponse(networkStream, "<h1>Hello there!</h1>");

				connection.Close();
			}
		}

		private void WriteResponse(NetworkStream networkStream, string message)
		{
			var contentLength = Encoding.UTF8.GetByteCount(message);

			var response = "HTTP/1.1 200 OK" + NewLine +
						   "Server: AtanasServer 2024" + NewLine +
						   "Content-Type: text/html; charset=UTF-8" + NewLine +
						   $"Content-Length: {contentLength}" + NewLine +
						   NewLine +
						   message + NewLine;

			var responseBytes = Encoding.UTF8.GetBytes(response);

			networkStream.Write(responseBytes, 0, responseBytes.Length);
		}
	}
}