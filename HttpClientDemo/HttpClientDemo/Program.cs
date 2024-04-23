using System.Net;
using System.Net.Sockets;
using System.Text;

namespace HttpClientDemo
{
	class Program
	{
		public static void Main()
		{
			var httpServer = new HttpServer(12345);

			httpServer.Start();
		}

	}
}
