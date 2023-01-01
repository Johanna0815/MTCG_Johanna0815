using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTCG_TheOrigin_Subproject.DA
{
    public class DataBaseConnectionHandler
    {

        public static async Task<string> Register(string username, string password)
        {
            DataBaseConnection db = new DataBaseConnection();
            NpgsqlConnection con = await db.ConnectDB("localhost", "swe1db", "swe1pw", "simpledatastore"); 
           // con.Open(); // pq
            db.Register(username, password, con);
            return "{\"msg\": \"Login successfull!\", \"success\": true}";
        }

        

        public static async Task<string> Login(string username = "", string password = "", string accessToken = "")
        {
            DataBaseConnection db = new DataBaseConnection();
            NpgsqlConnection con = await db.ConnectDB("localhost", "swe1db", "swe1pw", "simpledatastore"); // simpledatastroe - weg!
                                                                                                         // 

            if (accessToken != null)
            {
                var cmd = new NpgsqlCommand("", con);
                bool isProofed = await db.ProofToken(accessToken, cmd);
                if (isProofed == true)
                {
                    return "{\"msg\": \"Login successfull!\", \"success\": true}";
                    // if isProofed 
                }
                // is false
                Console.WriteLine("not succeeded");

            }
            
                ////
                ///TODO login 
        
            // is false
            return null;
        }




        // TODO
        //public static async Task<string> GetAccessToken(string username = "", string password = "")
        //{
        //    if (username != null && password != null)
        //    {
               
        //    }
        //    //  var cmd = new NpgsqlCommand("", con);

        //    return await todo;


        //}



    }
}
