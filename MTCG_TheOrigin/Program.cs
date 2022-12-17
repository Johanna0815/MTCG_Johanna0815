// See https://aka.ms/new-console-template for more information


// using MTCG_TheOrigin_SubProject.HSL;
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


server.run();





// wollte etwas testen, aber das als Subproject zu testen "would cause a circular dependency!)
// MTCG_TheOrigin_SubProject.Model.UserCardStack testen = new UserCardStack();


