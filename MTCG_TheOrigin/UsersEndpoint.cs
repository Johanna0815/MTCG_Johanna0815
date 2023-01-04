using MTCG_TheOrigin;
using MTCG_TheOrigin_SubProject.HSL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using HttpMethod = MTCG_TheOrigin_SubProject.HSL.HttpMethod;

namespace MTCG_TheOrigin
{
    public class UsersEndpoint : IHttpEndpoint
    {

        public void HandleRequest(HttpRequest rq, HttpResponse rs)
        {

            // im main int
            switch(rq.Method)
            {
                case HttpMethod.POST:
                    CreateUser(rq, rs);
                    break;
                case HttpMethod.GET:
                    GetUsers(rq, rs); 
                    break;


            }
        }



        private void CreateUser(HttpRequest rq, HttpResponse rs)
        {
            try
            {
                var user = JsonSerializer.Deserialize<User>(rq.Content);


                // call BL fehlt no reinholen. 

                rs.ResponseCode = 201;
                rs.ResponseText = "OK";

            }
            catch(Exception) {
                rs.ResponseCode = 400;
                rs.Content = "Failed to create User! ";

            
            }
        }

        private void GetUsers(HttpRequest rq, HttpResponse rs)
        {
            // BL

            List<User> users = new List<User>();

            // kommt ja aus db ! 
            //users.Add(new User("Anton Erster", "1234"));
            //users.Add(new User("Berta Zweite", "0000"));

            rs.Content = JsonSerializer.Serialize(users);
            rs.ContentType= "application/json";
            rs.ResponseCode= 200;
            rs.ResponseText= "OK";


        }

    }



}
