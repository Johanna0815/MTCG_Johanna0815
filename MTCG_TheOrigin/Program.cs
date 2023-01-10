// See https://aka.ms/new-console-template for more information
using MTCG_TheOrigin;
using MTCG_TheOrigin_Subproject.DA;
using MTCG_TheOrigin_SubProject.Model;
using System.Net;
using System.Reflection;
using System.Net.Sockets;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net.Security;
using System.Text;
using System.Text.Json;

//Console.WriteLine("Hello, World! Origin Main");

public class Program
{
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    public static TcpListener tcpListener_;
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

    //ServerCertificateSelectionCallback.

    static void Main (string[] args)
    {
         Server().Wait();

    }
   

    static async Task Server()
    {

        int port = 10001;
        if (tcpListener_ == null) tcpListener_ = new TcpListener(IPAddress.Any, port);
        tcpListener_.Start();
        Console.WriteLine("Welcome to MTCG" );
        
        while(true)
        {
            var client = await tcpListener_.AcceptTcpClientAsync();
            _ = Task.Factory.StartNew(async (state) => // ? 
            {
                TcpClient clientA = (TcpClient)state; // state as 
                using (clientA)
                {
                    bool end = true;
                    while (end)
                    {
                        using (var newservice = clientA.GetStream())
                        {

                            string FirstLine, body = null;
                            byte[] msg = new byte[1024]; // resize ? 
                            newservice.Read(msg, 0, msg.Length);
                            string request = Encoding.UTF8.GetString(msg).TrimEnd('\0');
                            string requestType, query;
                            string jsonString = "";
                            Credentials jsonBody;
                            HttpBody requestBody;
                            using (StringReader reader = new StringReader(request))
                            {
                                string line = await reader.ReadLineAsync();
                                //GCCollectionMode
                                //global::System.Console.WriteLine(line);
                                Console.WriteLine(line);
                                FirstLine = line;
                                bool bodyContent = false;
                                requestType = FirstLine.Split(' ')[0];// reader.ReadLine();
                                query = FirstLine.Split(' ')[1];
                                while(line != null)
                                {
                                    line = await reader.ReadLineAsync();
                                    Console.WriteLine(line);
                               
                                if ( line == "{" || bodyContent)
                                    {
                                        bodyContent = true;
                                        jsonString += line;
                                        if (line == "}") bodyContent = false;
                                    }
                                }


                                body = await reader.ReadLineAsync(); // reader not null ? solve ' '
                                Console.WriteLine(body);
                                Console.WriteLine(jsonString);
                                jsonBody = JsonSerializer.Deserialize<Credentials>(jsonString);
                                requestBody = JsonSerializer.Deserialize<HttpBody>(jsonString);
                                Console.WriteLine(jsonBody.AccessToken); // TEST



                            }
                            switch (requestType.ToLower())
                            {
                                case "post":
                                    string response = await POST(query, jsonBody, requestBody);
                                    Console.WriteLine("Response: " + response);
                                    string rs =
                                    @$"HTTP/1.1 200 OK
                                    Last-Modified: 07.01.2023
                                    Content-Length:
                                    Content-Type: application/json
                                    Connection: Closed


                                    {response}";
                                    byte[] bytesSend = System.Text.Encoding.ASCII.GetBytes(rs);//new byte[1];
                                    newservice.Write(bytesSend, 0, bytesSend.Length);
                                    break;

                                    // case "get":

                            }



                        }





                    }
                    clientA?.Close(); // possible null 

                }

            }, client);



        }


    }





    static async Task<string> POST(string query, Credentials body, HttpBody? request = null) // bug ? 
    {
        string response = "string.Empty";
        //  bool await;


        switch (query)
        {
            case "/login": // BUG 
                response = await DataBaseConnectionHandler.Login(username: body.UserName, password: body.Password);
                break;
            case "/register":
                response = await DataBaseConnectionHandler.Register(body.UserName, body.Password);
                break;
            case "/newPack":
                response = await DataBaseConnectionHandler.NewPack(username: body.UserName, password: body.Password, accessToken: body.AccessToken);
                break;
            case "/setDeck":
                response = await DataBaseConnectionHandler.SetDeck(request.Deck, username: body.UserName, password: body.Password, accessToken: body.AccessToken);
                break;
                // "/goBattle
                // response = await DataBaseConnectionHandler.
                // default: response = await DataBaseConnectionHandler.Register(username: body.username, password: body.password);

                // setDeck

        }
        return response;
    }


    // Request without Datas inside the body, get the datats returned in the response
    static public async Task<string> GET(string query, Credentials body)
    {
        string response = "string.Empty";
        switch(query)
        {
            case "getAccessToken":
                response = await DataBaseConnectionHandler.GetAccessToken(username: body.UserName, password: body.Password);
                break;
           // case "getUserUserProfile":
             //   response = await DataBaseConnectionHandler.
            //    break;


        }

        return response;
    }


}






// hier dann den Server aufrufen

//80000 || loopback
//var server = new HttpServer(IPAddress.Any, 10001);

//// raus comm nomal!
// server.RegisterEndpoint("/users", new UsersEndpoint()); // die finden sich nciht, lässt sich nicht integrieren. // ned da 


//liste einbinden! 
//var response;
//IEnumerable<User> response = await DataBaseConnectionHandler.Register(if exists from users );


//compare stats in score-board.!

//server.run();



