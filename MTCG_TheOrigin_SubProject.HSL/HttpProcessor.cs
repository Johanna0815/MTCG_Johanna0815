using System.Net.Sockets;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MTCG_TheOrigin_SubProject.HSL
{
    public class HttpProcessor
    {
        private TcpClient clientSocket;
        private HttpServer httpServer;

        public HttpProcessor(HttpServer httpServer, TcpClient clientSocket)
        {
            this.httpServer = httpServer;
            this.clientSocket = clientSocket;
        }




        public void run()
        {

            var reader = new StreamReader(clientSocket.GetStream());
            var request = new HttpRequest(reader);
            request.Parse();


            //.. Application anwendungsspezifische Verarbeitung
            // unterschiedlichen Endpunkte
            // BL - Aufrufe usw.... 



            var writer = new StreamWriter(clientSocket.GetStream()) { AutoFlush = true };
            var response = new HttpResponse(writer); // nur HttpResponseMessage nicht ? 


            IHttpEndpoint endpoint;
            httpServer.Endpoints.TryGetValue(request.Path, out endpoint); // nullexception! /users bene
                                                                          //try
                                                                          //{

            // user unhandled - is aber au ned drin

            // II. Abänderung. v
            //  var endpoint = httpServer.Endpoints[request.Path[1]]; // NullReferenceException. /bug
            if (endpoint != null)
            {
                endpoint.HandleRequest(request, response);
            }
            else
            {

                // oder hier gleich Response "400 " mal schauen. 




                //Console.WriteLine($"{ex} occured - endpoint HAndleRequest");
                response.ResponseCode = 404;
                response.ResponseText = "BAD REQUEST";
                response.Content = "<html><body>Endpoint not found! </body></hmtl>";
                // response.Headers.Add("Content-Length", response.Content.Length.ToString());
                response.Headers.Add("Content-Type", "text/plain");



                // Thread.Sleep(10000);
                //response.ResponseCode = 200;
                //    response.ResponseText = "OK";
                //    response.Content = "<html><body> Hello Writer here we are! </body></hmtl>";
                //    response.Headers.Add("Content-Length", response.Content.Length.ToString());
                //    response.Headers.Add("Content-Type", "text/plain");
            }

            response.Process();






            //catch (Exception ex)
            //{
            //    Console.WriteLine($"{ex} occured - endpoint HAndleRequest");
            //    response.ResponseCode = 404;
            //    response.ResponseText = "BAD REQUEST";
            //    response.Content = "<html><body>Endpoint not found! </body></hmtl>";
            //    //response.Headers.Add("Content-Length", response.Content.Length.ToString());
            //    response.Headers.Add("Content-Type", "text/plain");
            //}


        }
    }
}
