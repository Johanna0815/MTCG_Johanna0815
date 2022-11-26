using System.Net.Sockets;

namespace MTCG_TheOrigin_SubProject.HSL
{
    public class HttpProcessor
    {
        private TcpClient clientSocket;

        public HttpProcessor(TcpClient clientSocket)
        {
            this.clientSocket = clientSocket;
        }




        public void run()
        {

            var reader = new StreamReader(clientSocket.GetStream());
            var request = new HttpRequest(reader); // HttpRequest // ist eine Hilfsklasse
            request.Parse();


            //.. Application anwendungsspezifische Verarbeitung
            // unterschiedlichen Endpunkte
            // BL - Aufrufe usw.... 

            Thread.Sleep(10000);

            var writer = new StreamWriter(clientSocket.GetStream()) { AutoFlush = true };
            var response = new HttpResponse(writer); // nur HttpResponseMessage nicht ? 
            response.ResponseCode = 200;
            response.ResponseText = "OK";
            response.ResponseContent = "<html><body> Hello Writer here we are! </body></hmtl>";
            response.Process();
        }
    }
}