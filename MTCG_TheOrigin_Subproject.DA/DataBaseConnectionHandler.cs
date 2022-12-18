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
            NpgsqlConnection con = await db.ConnectDB("localhost", "swe1db", "1234", "simpledatabasestore");
            con.Open(); // pq
            db.Register(username, password, con);
            return "{\"msg\": \"Login successfull!\", \"success\": true}";

        }







    }
}
