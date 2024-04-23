using System.Net;
using System.Net.Sockets;
using System.Text;

namespace HttpClientDemo
{
	class Program
	{
		static async Task Main()
		{
			//await ReadData();

			const string NewLine = "\r\n";

			TcpListener tcpListener = new TcpListener(
				IPAddress.Loopback, 12345);
			tcpListener.Start();

			//daemon // service
			while (true)
			{
				var client = tcpListener.AcceptTcpClient();
				using (var stream = client.GetStream())
				{

					byte[] buffer = new byte[1000000];
					var length = stream.Read(buffer, 0, buffer.Length);

					string requestString =
								Encoding.UTF8.GetString(buffer, 0, length);

					Console.WriteLine(requestString);

					string html = $"<h1>Hello from AtanasServer {DateTime.Now}</h1>";

					string response = "HTTP/1.1 200 OK" + NewLine +
								 "Server: AtanasServer 2024" + NewLine +
								 //"Location: https://www.google.com" + NewLine +
								 "Content-Type: text/html; charset=utf-8" + NewLine +
								 "Contnet-Length: " + html.Length + NewLine +
								 NewLine +
								 html + NewLine;

					byte[] responseBytes = Encoding.UTF8.GetBytes(response);
					stream.Write(responseBytes);

					Console.WriteLine(new string('=', 70));
				}
            }
		}


		//We are the client
		public static async Task ReadData()
		{
			Console.OutputEncoding = Encoding.UTF8;
			string url = "https://softuni.bg/";
			HttpClient httpClient = new HttpClient();
			//httpClient.DefaultRequestHeaders.Add("X-Test", "test...");
			//var html = await httpClient.GetStringAsync(url);
			var response = await httpClient.GetAsync(url);
			Console.WriteLine(response.StatusCode);
			Console.WriteLine(string.Join(Environment.NewLine, response.Headers.Select(x => x.Key + ": " + x.Value.First())));
            //Console.WriteLine(response);
        }
	}
}
