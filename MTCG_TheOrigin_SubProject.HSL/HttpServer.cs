using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace MTCG_TheOrigin_SubProject.HSL
{
    public class HttpServer
    {


        private readonly int port = 8000; // default wert, muss im Constructor sonst ein wert sein!
        private readonly IPAddress ipAddress;

        private TcpListener httpListener;

        public Dictionary<string, IHttpEndpoint> Endpoints { get; private set; } = new Dictionary<string, IHttpEndpoint>();

        public HttpServer(IPAddress address, int port)
        {
            this.ipAddress = address;
            this.port = port;

            httpListener = new TcpListener(ipAddress, port);
        }


        // kein beenden und stoppen noch nicht drin, es fehlen noch zwei Funktionen ! 
        public void run()
        {

            httpListener.Start();
            while (true)
            {
                Console.WriteLine("Waiting for new client request...");
                var clientSocket = httpListener.AcceptTcpClient();
                var httpProcessor = new HttpProcessor(this, clientSocket);
                // alt:   var httpProcessor = new HttpProcessor(clientSocket); // oder das vom Thread ableiten !
                Task.Factory.StartNew(() =>
                {
                    httpProcessor.run();
                });
            }
        }

        public void RegisterEndpoint(string path, IHttpEndpoint endpoint)
        {
            Endpoints.Add(path, endpoint);
        }



    }
}
