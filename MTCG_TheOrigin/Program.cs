// See https://aka.ms/new-console-template for more information


// using MTCG_TheOrigin_SubProject.HSL;
using MTCG_TheOrigin;
using MTCG_TheOrigin_Subproject.DA;
using MTCG_TheOrigin_SubProject.HSL;
using MTCG_TheOrigin_SubProject.Model;
using System.Net;
using System.Reflection;

Console.WriteLine("Hello, World! Origin Main");





// hier dann den Server aufrufen

//80000 || loopback
var server = new HttpServer(IPAddress.Any, 10001);

// raus comm nomal!
 server.RegisterEndpoint("/users", new UsersEndpoint()); // die finden sich nciht, lässt sich nicht integrieren. // ned da 


//liste einbinden! 
//var response;
//IEnumerable<User> response = await DataBaseConnectionHandler.Register(if exists from users );


server.run();


static async Task<string> POST(string query, Credentials body, HttpBody request = null)
{
    string response = "string.Empty";
    bool await;


    switch(await)
    {
        case "/login":
           response = await DataBaseConnectionHandler.Login(username: body.username, password: body.password);
            break;
        case "/register":
            response = await DataBaseConnectionHandler.Register(body.username, body.password);
            break;
        case "/openPack":
           // response = await DataBaseConnectionHandler.
       // default: response = await DataBaseConnectionHandler.Register(username: body.username, password: body.password);

    }
}
