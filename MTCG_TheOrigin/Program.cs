// See https://aka.ms/new-console-template for more information

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
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Runtime.Serialization.Json;



namespace MTCG_TheOrigin
{
    class Program
    {
        static TcpListener tcplistener_;

       
        static void Main(string[] args)
        {
            Server().Wait();
        }

        public static async Task Server()
        {
            int port = 10001;
            if (tcplistener_ == null) tcplistener_ = new TcpListener(IPAddress.Any, port);
            tcplistener_.Start();



            Console.WriteLine("Start MTCG...");
            while (true)
            {
                var client = await tcplistener_.AcceptTcpClientAsync();
                _ = Task.Factory.StartNew(async (state) =>
                {
                    TcpClient clientA = (TcpClient)state;
                    using (clientA)
                    {
                        bool ende = true;
                        while (ende)
                        {
                            using (var newservice = clientA.GetStream())
                            {
                                string Firstline, body = "";
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
                                    Console.WriteLine(line);
                                    Firstline = line;
                                    bool bodyContent = false;
                                    requestType = Firstline.Split(' ')[0];
                                    query = Firstline.Split(' ')[1];
                                    while (line != null)
                                    {
                                        line = await reader.ReadLineAsync();
                                        Console.WriteLine(line);
                                        if (line == "{" || bodyContent)
                                        {
                                            bodyContent = true;
                                            jsonString += line;
                                            if (line == "}") bodyContent = false;
                                        }
                                    }

                                    body = await reader.ReadLineAsync();
                                    Console.WriteLine(body);
                                    Console.WriteLine(jsonString);
                                    jsonBody = JsonSerializer.Deserialize<Credentials>(jsonString);
                                    requestBody = JsonSerializer.Deserialize<HttpBody>(jsonString);
                                    
                                    Console.WriteLine(jsonBody.access_token);
                                }

                                switch (requestType.ToLower())
                                {
                                    case "post":
                                        string response = await POST(query, jsonBody, requestBody);
                                        Console.WriteLine("Response: " + response);
                                        string res =
                                        @$"HTTP/1.1 200 OK
                                        Last-Modified: 01.01.2023
                                        Content-Length:
                                        Content-Type: application/json
                                        Connection: Closed

                                        {response}";
                                        byte[] bytesToSend = Encoding.ASCII.GetBytes(res);
                                        newservice.Write(bytesToSend, 0, bytesToSend.Length);
                                        break;
                                    case "get":
                                        string GETresponse = await GET(query, jsonBody);
                                        Console.WriteLine("Response: " + GETresponse);
                                        string GETres =
                                        @$"HTTP/1.1 200 OK
                                        Last-Modified: 01.01.2023
                                        Content-Length:
                                        Content-Type: application/json
                                        Connection: Closed

                                        {GETresponse}";
                                        byte[] bytesToSend_GET = Encoding.ASCII.GetBytes(GETres);
                                        newservice.Write(bytesToSend_GET, 0, bytesToSend_GET.Length);
                                        break;
                                }

                            }
                        }
                        clientA?.Close();
                    }

                }, client);

            }
        }

        static public async Task<string> POST(string query, Credentials body, HttpBody requestBody = null)
        {
            string response = "string.Empty";
            switch (query)
            {
                case "/login":
                    response = await DataBaseConnectionHandler.Login(username: body.username, password: body.password, access_token: body.access_token);
                    break;
                case "/register":
                    response = await DataBaseConnectionHandler.Register(body.username, body.password);
                    break;
                case "/newPack":
                    response = await DataBaseConnectionHandler.NewPack(username: body.username, password: body.password, access_token: body.access_token);
                    break;
                case "/setDeck":
                    response = await DataBaseConnectionHandler.SetDeck(requestBody.deck, username: body.username, password: body.password, access_token: body.access_token);
                    break;
                case "/GoBattle":
                    response = await DataBaseConnectionHandler.GoBattle(username: body.username, password: body.password, access_token: body.access_token);
                    break;
                case "/generateTradeoffer":
                    string actionGenerate = "generate";
                    response = await DataBaseConnectionHandler.TradeOffer(requestBody.receiver_uid, requestBody.a_receive, requestBody.b_receive, actionGenerate, username: body.username, password: body.password, access_token: body.access_token);
                    break;
                case "/declineTradeoffer":
                    string actionDecline = "delete";
                    response = await DataBaseConnectionHandler.TradeOffer(requestBody.receiver_uid, requestBody.a_receive, requestBody.b_receive, actionDecline, requestBody.tradeoffer_id, username: body.username, password: body.password, access_token: body.access_token);
                    break;
                case "/acceptTradeoffer":
                    string actionAccept = "accept";
                    response = await DataBaseConnectionHandler.TradeOffer(requestBody.receiver_uid, requestBody.a_receive, requestBody.b_receive, actionAccept, requestBody.tradeoffer_id, username: body.username, password: body.password, access_token: body.access_token);
                    break;
            }


            //query != "/startBattle") 
            return response;
        }



        static public async Task<string> GET(string query, Credentials body)
        {
            string response = "string.Empty";
            switch (query)
            {
                case "/getCollection":
                    response = await DataBaseConnectionHandler.ShowCollection(username: body.username, password: body.password, access_token: body.access_token);
                    break;
                case "/getTradeoffers":
                    response = await DataBaseConnectionHandler.GetTradeoffers(username: body.username, password: body.password, access_token: body.access_token);
                    break;
                case "/getUserprofile":
                    response = await DataBaseConnectionHandler.GetUserProfile(username: body.username, password: body.password, access_token: body.access_token);
                    break;
                case "/getAccessToken":
                    response = await DataBaseConnectionHandler.GetAccessToken(username: body.username, password: body.password);
                    break;
            }
            return response;
        }
       
    }
}