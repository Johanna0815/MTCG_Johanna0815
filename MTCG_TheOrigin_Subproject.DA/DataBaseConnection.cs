using Microsoft.VisualBasic;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace MTCG_TheOrigin_Subproject.DA
{
    public class DataBaseConnection
    {

       // string connectionString = Configur


        static object commandlock = new object();
        public async Task<NpgsqlConnection> ConnectDB(string server, string username, string password, string db)
        {

            // db weg ?
            var connString = $"Host={server};Username={username};Password={password};Database={db}";

            var conn = new Npgsql.NpgsqlConnection(connString);
            await conn.OpenAsync();
            return conn;

        }


        public async void Register(string username, string password, NpgsqlConnection con)
        {

            //  await con.OpenAsync() ;

            // var cmd = new Npgsql.NpgsqlCommand("", con) ; // NpgsqlCommand
            var cmd = new NpgsqlCommand("", con);
            int uid = await GetUserIDByusername(username, cmd);
            if (uid == 0)
            {
                lock (commandlock)
                {
                    cmd.CommandText = $"INSERT INTO users(username, password) VALUES(@username, @password)";
                    cmd.Parameters.AddWithValue("username", username);
                    cmd.Parameters.AddWithValue("password", password);
                    cmd.Prepare();
                    cmd.ExecuteNonQueryAsync();
                }


                lock (commandlock)
                {
                    uid = GetUserIDByusername(username, cmd).Result;

                }

                Console.WriteLine(await GetUserIDByusername(username, cmd));

                //SetUserBalance
            }
            else
            {
                Console.WriteLine("Choose different Username, already token!!!");
            }

        }




        

        /// <summary>
        /// // prepare Beispiel !
        /// </summary>
        /// <param name="username"></param>
        /// <param name="cmd"></param>
        //        using (SqlConnection connection = new SqlConnection(connectionString))
        //{
        //    connection.Open();
        //    SqlCommand command = new SqlCommand(null, connection);

        // Create and prepare an SQL statement.
        //command.CommandText =
        //        "INSERT INTO Region (RegionID, RegionDescription) " +
        //        "VALUES (@id, @desc)";
        //    SqlParameter idParam = new SqlParameter("@id", SqlDbType.Int, 0);
        //SqlParameter descParam =
        //    new SqlParameter("@desc", SqlDbType.Text, 100);
        //idParam.Value = 20;
        //    descParam.Value = "First Region";
        //    command.Parameters.Add(idParam);
        //    command.Parameters.Add(descParam);

        //    // Call Prepare after setting the Commandtext and Parameters.
        //    command.Prepare();
        //    command.ExecuteNonQuery();

        //    // Change parameter values and call ExecuteNonQuery.
        //    command.Parameters[0].Value = 21;
        //    command.Parameters[1].Value = "Second Region";
        //    command.ExecuteNonQuery();
        //}



        public async Task<int> GetUserIDByusername(string username, NpgsqlCommand cmd)
        {
            // cmd.CommandText = username ;
            cmd.CommandText = $"Select uid FROM users WHERE username = @username";
            cmd.Parameters.AddWithValue("username", username);

                        //cmd.GetType();
           
              cmd.Prepare();  // set parameters 
           // await using (var reader = await cmd.GetType)
            await using (var reader = await cmd.ExecuteReaderAsync())
            {
                while (await reader.ReadAsync())
                    return reader.GetInt32(0);
                {
                    reader.Close();
                }

            }

            return 0;
        }


        public async Task<string> LoginByCredentials(string username, string password, NpgsqlConnection con)
        {
            int uid;
            string accessToken;
            string dueDate;
            var cmd = new NpgsqlCommand($"SELECT u.uid, u.username, act.accessToken, act.dueDate FROM users AS u JOIN accessTokens AS act ON u.uid = act.uid WHERE u.username = @username AND u.password = @password", con);
            //cmd.Parameters.AddRange
            cmd.Parameters.AddWithValue("username", username);
            cmd.Parameters.AddWithValue("username", password);
            cmd.Prepare();
            await using (var reader = await cmd.ExecuteReaderAsync())
                while (await reader.ReadAsync())
                {
                    uid = (int)reader.GetInt32("uid"); // try ?!
                    accessToken = (string)reader["accessToken"];
                    dueDate = reader["dueDate"].ToString(); // att: converting null literal or possibel null val to non_null
                    return $"{{\"msg\":\"Login successfull!\", \"uid\":{uid}, \"accessToken\": \"{accessToken}\", \"success\": true}}";
                }

            return $"{{\"msg\":\"Error. Login failed. Please check you credentials.\", \"success\": false}}";

        }


        public async Task<bool> ProofToken(string accessToken, NpgsqlCommand cmd)
        {
            string dueDate;
            int uid;
            cmd.CommandText = $"SELECT u.uid, u.username, act.dueDate FROM users AS u JSON accessTokens AS act ON u.uid = act.uid WHERE act.accessToken = @accessToken";
            cmd.Parameters.AddWithValue("accessToken", accessToken);
            cmd.Prepare();
            await using (var reader = await cmd.ExecuteReaderAsync())
                while (await reader.ReadAsync())
                {
                    uid = (int)reader["uid"];
                    dueDate = reader["dueDate"].ToString();
                    if (DateTime.Now <= DateTime.Parse(dueDate))
                    {
                        return true;

                    } else
                    {
                        return false;
                    }
                }
            return false;
            //  return -1;
        }


        public async Task<string> GetAccessToken(string username, string password, NpgsqlCommand cmd)
        {
            cmd.CommandText = $"SELECT act.accessToken FROM users AS u JSON accessTokens AS act ON u.uid WHERE u.username = @username AND u.password = @password";
            cmd.Parameters.AddWithValue("username", username);
            cmd.Parameters.AddWithValue("username", password);
            cmd.Prepare();
            await using (var reader = await cmd.ExecuteReaderAsync())
                while (await reader.ReadAsync() )
                {
                    return (string)reader["accessToken"];

                }
            // pq 
            return "wrong credentials";
        }

        //private string connString = null!;
        //public DataBaseConnection() { 

        //CreateDBIfNotExists();
        //}



        /// <summary>
        /// check if table already exists 
        /// </summary>
        //void CreateDBIfNotExists();


        //// user Calss bug !
        //bool AddUser();


        // UserCardStack rename ! // bug mit reinhlen, add prohect -.- 
        //public bool AddPackage(List<UserCardStack> cards)
        //{
        //    using var conn = DataBaseConnection(connString);
        //    var transaction = BeginnTransaction(conn);
        //    if (transaction == null)
        //        return false;
        //    try
        //    {
        //        using var 
        //            // id fehlt no 

        //    }
        //}





        //jeder user sollte mit 20 starten können.
        public void SetUserBalance(int UId, NpgsqlCommand cmd)
        {
            lock(commandlock)
            {
                cmd.CommandText = $"INSERT INTO balances(uid, coins) VALUES(@uid, 20)";
                cmd.Parameters.AddWithValue("uid", UId);
                cmd.Prepare();
                cmd.ExecuteNonQueryAsync();


            }
        }

        public void SetAccessToken(int uid, string username, string password, NpgsqlCommand cmd)
        {
            lock (commandlock)
            {

                cmd.CommandText = $"INSERT INTO accessTokens(uid, accessToken, dueDate) VALUES(@uid, 'ddmmyyy')";
                cmd.Parameters.AddWithValue("uid", uid);
                cmd.Prepare();
                //cmd.CommandText = $"SELECT act.accessToken FROM users AS u JSON accessTokens AS act ON u.uid WHERE u.username = @username AND u.password = @password";
                //cmd.Parameters.AddWithValue("username", username);
                //cmd.Parameters.AddWithValue("username", password);
                //cmd.Prepare();
                //await using (var reader = await cmd.ExecuteReaderAsync())
                //    while (await reader.ReadAsync())
                //    {
                //        return (string)reader["accessToken"];

                //    }
            }
        }





    }
    
}
