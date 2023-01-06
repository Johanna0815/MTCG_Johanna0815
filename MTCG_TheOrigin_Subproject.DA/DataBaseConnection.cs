using Microsoft.VisualBasic;
using MTCG_TheOrigin;
using MTCG_TheOrigin_SubProject.Model;
using MTCG_TheOrigin_Subproject.BL;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection.Metadata;
using System.Runtime.CompilerServices;
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
            var connString = $"Host={server};Username={username};Password={password};Database={db}";

            var conn = new NpgsqlConnection(connString);
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

                // Console.WriteLine(await GetUserIDByusername(username, cmd));

                //SetUsersBalance
                SetUsersBalance(uid, cmd);
                SetAccessToken(uid, username, password, cmd);
            }
            else
            {
                Console.WriteLine("\t\r\nUser with same username already registered");
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
            cmd.CommandText = $"SELECT uid FROM users WHERE username = @username";
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
            string due_date;
            var cmd = new NpgsqlCommand($"SELECT u.uid, u.username, act.accessToken, act.due_date FROM users AS u JOIN accessTokens AS act ON u.uid = act.uid WHERE u.username = @username AND u.password = @password", con);
            //cmd.Parameters.AddRange
            cmd.Parameters.AddWithValue("username", username);
            cmd.Parameters.AddWithValue("username", password);
            cmd.Prepare();
            await using (var reader = await cmd.ExecuteReaderAsync())
                while (await reader.ReadAsync())
                {
                    uid = (int)reader.GetInt32("uid"); // try ?!
                    accessToken = (string)reader["accessToken"];
                    due_date = reader["due_date"].ToString(); // att: converting null literal or possibel null val to non_null
                    return $"{{\"msg\":\"Login successfull!\", \"uid\":{uid}, \"accessToken\": \"{accessToken}\", \"success\": true}}";
                }

            return $"{{\"msg\":\"Error. Login failed. Please check you credentials.\", \"success\": false}}";

        }


        public async Task<bool> ProofToken(string accessToken, NpgsqlCommand cmd)
        {
            string due_date;
            int uid;
            cmd.CommandText = $"SELECT u.uid, u.username, act.due_date FROM users AS u JSON accessTokens AS act ON u.uid = act.uid WHERE act.accessToken = @accessToken";
            cmd.Parameters.AddWithValue("accessToken", accessToken);
            cmd.Prepare();
            await using (var reader = await cmd.ExecuteReaderAsync())
                while (await reader.ReadAsync())
                {

                    // source https://stackoverflow.com/questions/36797517/sql-select-where-due-date-and-due-time-after-now-without-datetime
                    uid = (int)reader["uid"];
                    due_date = reader["due_date"].ToString();
                    if (DateTime.Now <= DateTime.Parse(due_date))
                    {
                        return true;

                    } else
                    {
                        return false;
                    }
                }
            return false;
        }


        public async Task<string> GetAccessToken(string username, string password, NpgsqlCommand cmd)
        {
            cmd.CommandText = $"SELECT act.accessToken FROM users AS u JSON accessTokens AS act ON u.uid WHERE u.username = @username AND u.password = @password";
            cmd.Parameters.AddWithValue("username", username);
            cmd.Parameters.AddWithValue("username", password);
            cmd.Prepare();
            await using (var reader = await cmd.ExecuteReaderAsync())
                while (await reader.ReadAsync())
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


        //BUG
        /// userStats sind eigentlich name, Elo, Wins, Losses. {swagger} 
        public async Task<UserProfile> GetUserProfile(int uid, NpgsqlCommand cmd)
        {
            cmd.CommandText = "SELECT * FROM userProfile WHERE uid = @uid";
            cmd.Parameters.AddWithValue("uid", uid);
            cmd.Prepare();

            UserProfile userProfile = new UserProfile();
            await using (var reader = await cmd.ExecuteReaderAsync())
                while (await reader.ReadAsync())
                {
                    userProfile.UID = (int)reader["UID"];
                    userProfile.Win = (int)reader["Win"];
                    userProfile.Elo = (int)reader["Elo"]; // unauffindbar.
                    userProfile.Deck = (int[])reader["Deck"]; // BUG. 
                    userProfile.Loos = (int)reader["Loos"];
                    // userProfile.
                }
            return userProfile;
        }





        //jeder user sollte mit 20 starten können.
        public void SetUsersBalance(int UId, NpgsqlCommand cmd)
        {
            lock (commandlock)
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
                cmd.ExecuteNonQueryAsync();
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



        //ohne passwort!
        public async Task<int> GetAccessToken(string username, NpgsqlCommand cmd)
        {
            int uid = await GetUserIDByusername(username, cmd);
            cmd.CommandText = $"SELECT coind FROM balances WHERE uid = @uid";
            cmd.Parameters.AddWithValue("uid", uid);
            cmd.Prepare();
            await using (var reader = await cmd.ExecuteReaderAsync())
                while (await reader.ReadAsync())
                    return reader.GetInt32(0);
            return 0;
        }


        public async Task<int> GetCoinsByUserID(int uid, NpgsqlCommand cmd)
        {
            cmd.CommandText = $"SELECT * FROM balances WHERE uid = @uid";
            cmd.Parameters.AddWithValue("uid", uid);
            cmd.Prepare();
            await using (var reader = await cmd.ExecuteReaderAsync())
                while (await reader.ReadAsync())
                    return (int)reader["coins"];
            return 0;
        }

        public async Task<int> NewPack(int uid, NpgsqlCommand cmd)
        {
            int newCoins = await GetCoinsByUserID(uid, cmd) - 5;
            lock (commandlock)
            {
                // HMACMD5.
                cmd.CommandText = $"UPDATE balances SET coins = @newCoins WHERE uid = @uid";
                cmd.Parameters.AddWithValue("newCoins", newCoins);
                cmd.Parameters.AddWithValue("uid", uid);
                cmd.Prepare();
                cmd.ExecuteNonQuery();
                return newCoins;
            }
        }

        public async Task<int[]> GetRandom(NpgsqlCommand cmd)
        {
            //cmd.UnknownResultTypeLis
            cmd.CommandText = $"SELECT count(*) FROM cardDeck";
            int[] cardIDArray = new int[5]; //
            cmd.Prepare();
            await using (var reader = await cmd.ExecuteReaderAsync())
                while (await reader.ReadAsync())
                {
                    decimal totalCount = (decimal)reader["count"];
                    for (int i = 0; i < cardIDArray.Length; i++)
                    {
                        Random random = new Random();
                        cardIDArray[i] = random.Next(1, (int)totalCount);
                    }
                }
            return cardIDArray;
        }

        public async Task<Card> GetCardByCardId(int CId, NpgsqlCommand cmd)
        {

            Console.WriteLine(CId);

            cmd.CommandText = $"SELECT CId, CardType, CardName, ElementType, Damage FROM CardDeck WHERE CId = @CId";
            cmd.Parameters.AddWithValue("CId", CId);
            cmd.Prepare();
            await using (var reader = await cmd.ExecuteReaderAsync())
                while (await reader.ReadAsync())
                {
                    Card card = new Card
                    {
                        CId = (int)reader["CId"],
                        CardType = (string)reader["CardType"],
                        CardName = (string)reader["CardName"],
                        ElementType = (string)reader["ElementType"],
                        Damage = (int)reader["Damage"] // auf decimal wechseln BUG

                    };
                    return card;


                }
            return new Card();
        }

        public void CardToUser(int UID, int[] cardIdArray, NpgsqlCommand cmd) // user UID class user 
        {

            lock (commandlock)
            {
                for (int i = 0; i < cardIdArray.Length; i++)
                {
                    int pivot = cardIdArray[i];
                    cmd.CommandText = $"INSERT INTO collections(UID, CId) VALUES (@UID, @CId)";
                    cmd.Parameters.Clear();
                    cmd.Parameters.AddWithValue("UID", UID);
                    cmd.Parameters.AddWithValue("CId", pivot);
                    cmd.Prepare();
                    var reader = cmd.ExecuteNonQuery;
                }
            }

        }


        public async Task<List<Card>> GenerateCardList(int[] cardIdArray, NpgsqlCommand cmd)
        {
            //lock (commandlock) || jeder kann erstellen zeitglecih.
            //{

            //}

            List<Card> list = new List<Card>();
            cmd.CommandText = $"SELECT CId, CardType, CardName, ElementType, Damage FROM CardDeck WHERE CId in (@1, @2, @3, @4, @5)";
            for (int i = 0; i < cardIdArray.Length; i++) // Overflowexception ? // BUG ?! 
            {
                cmd.Parameters.AddWithValue($"{i + 1}", cardIdArray[i]);

            }
            cmd.Prepare();
            await using (var reader = await cmd.ExecuteReaderAsync())
                while (await reader.ReadAsync())
                {
                    Card card = new Card
                    {
                        CId = (int)reader["CId"],
                        CardType = (string)reader["CardType"],
                        CardName = (string)reader["CardName"],
                        ElementType = (string)reader["ElementType"],
                        Damage = (int)reader["Damage"] // auf decimal wechseln BUG

                    };
                    list.Add(card);
                }

            return list;

        }



        public async Task<List<Card>> GetUsersCollection(int UID, NpgsqlCommand cmd)
        {

            List<Card> list = new List<Card>();
            cmd.CommandText = $"SELECT CId, CardType, CardName, ElementType, Damage FROM CardDeck WHERE CId in (@1, @2, @3, @4, @5)";
            for (int i = 0; i < cardIdArray.Length; i++) // Overflowexception ? // BUG ?! 
            {
                cmd.Parameters.AddWithValue($"{i + 1}", cardIdArray[i]);

            }
            cmd.Prepare();
            await using (var reader = await cmd.ExecuteReaderAsync())
                while (await reader.ReadAsync())
                {
                    Card card = new Card
                    {
                        CId = (int)reader["CId"],
                        CardType = (string)reader["CardType"],
                        CardName = (string)reader["CardName"],
                        ElementType = (string)reader["ElementType"],
                        Damage = (int)reader["Damage"] // auf decimal wechseln BUG

                    };
                    list.Add(card);
                }

            return list;
        }


        public async Task<UserProfile> GetUserProfile(int UID, NpgsqlCommand cmd)
        {

            UserProfile user = new UserProfile();
            cmd.CommandText = $"SELECT UserProfile.UID, UserProfile.Draw, UserProfile.Elo, UserProfile.Loos, UserProfile.Deck, UserProfile.Win, UserProfile.UserName";
            cmd.Parameters.AddWithValue("Uid", UID);
            cmd.Prepare();
            await using (var reader = await cmd.ExecuteReaderAsync())
                while (await reader.ReadAsync())
                {
                    //   UserProfile userProfile = new UserProfile();

                    user.UID = (int)reader["UID"];
                    user.UserName = (string)reader["UserName"];
                    user.Win = (int)reader["Win"];
                    user.Loos = (int)reader["Loos"];
                    user.Draw = (int)reader["Draw"];
                    user.Elo = (int)reader["Elo"];
                    user.Deck = (int[])reader["Deck"];

                }
            return user;
        }



        public void SyncUserProfile(UserProfile uprof, NpgsqlCommand cmd)
        {
            lock (commandlock)
            {
                cmd.CommandText = "UPDATE UserProfile SET Elo = @Elo, Win = @Win, Loos =  @Loos, Draw = @Draw WHERE Uid = @Uid"; // userprofile Proof 
                cmd.Parameters.AddWithValue("Elo", uprof.Elo); // ELO
                cmd.Parameters.AddWithValue("Win", uprof.Win);
                cmd.Parameters.AddWithValue("Draw", uprof.Draw);
                cmd.Parameters.AddWithValue("Loos", uprof.Loos);
                cmd.Prepare();
                cmd.ExecuteNonQuery();
            }
        }

        // addCoins
        public void AddCoins(int UID, int value, NpgsqlCommand cmd)
        {
            lock (commandlock)
            {
                cmd.CommandText = "UPDATE balances SET coins = coins + @value WHERE UID = @UID";
                cmd.Parameters.AddWithValue("UID", UID); // Elo
                cmd.Parameters.AddWithValue("value", value); // Win.
                cmd.Prepare();
                cmd.ExecuteNonQuery();
            }
        }



        // SetDeck
        public async Task<int[]> SetDeck(int UID, UserProfile user, NpgsqlCommand cmd)
        {

            await IfNotExistCreateUserProfile(UID, cmd);

            // cards zum user 
            bool UserHasCards = await




        }


        // userProfile a no 
        public async Task<string> IfNotExistCreateUserProfile(int UID, NpgsqlCommand cmd)
        {
            cmd.CommandText = "SELECT UID FROM UserProfile WHERE UID = @UID";
            cmd.Parameters.AddWithValue("UID", UID);
            cmd.Prepare();
            bool UserProfileExists = false;
            await using (var reader = await cmd.ExecuteReaderAsync())

                while (await reader.ReadAsync())
                {
                    if ((int)reader["UID"] == UID) UserProfileExists = true;
                }


            if (UserProfileExists == true)
                return "Profile already exists.";

            else
            {
                // anlegen TODO
                lock (commandlock)
                {
                    cmd.CommandText = "INSERT INTO UserProfile(UID, Elo, Draw, Win, Loos) VALUES (@UID, 100,0,0,0)";
                    cmd.Parameters.AddWithValue("UID", UID);
                    cmd.ExecuteNonQueryAsync();
                    return "UserProfile was created.";
                }

            }

            // TODO check if created   
        }



        //ToCheckUserHasCards
        public async Task<bool> ToCheckUserHasCards(int[] cd, int UID, NpgsqlCommand cmd)
        {
            cmd.CommandText = "SELECT CId FROM collections WHERE UID =@UID      ";
            cmd.Parameters.AddWithValue("UID", UID);
            cmd.Prepare();

            bool UserHasCards = true;
            var dict = new Dictionary<int, int>();
            await using (var reader = await cmd.ExecuteReaderAsync())
                while (await reader.ReadAsync())
                {
                    int cards = (int)reader["CId"];
                    if (!dict.ContainsKey(cards))
                    {
                        dict.Add(cards, 0);
                        //dict.Remove
                    }
                    dict[cards]++;
                }
            foreach (var cards in cd)
            {
                if (dict.ContainsKey(cards) == false) UserHasCards = false;
            }
            return UserHasCards;
        }

        //BattleStart
        public async void GoBattle(UserProfile user, NpgsqlCommand cmd)
        {
            //user starts in queue
            UserInQueue(user);
            if (Queue.UserWaitInQueue.Count >= 2)
            {

                // to CheckForSecond
                Battle battleResult = await CheckForSecond(cmd) // der 2. abcheckn.

                     // PutUserFromQueue
                     PutUserFromQueue(battleResult.userA);
                PutUserFromQueue(battleResult.userB);
                // sync with DB.
                SyncUserProfile(battleResult.userA, cmd);
                SyncUserProfile(battleResult.userB, cmd);
            }


        }

        public void UserInQueue(UserProfile user)
        {
            if (Queue.UserWaitInQueue.Where(u => u.UID == user.UID).FirstOrDefault() == null) Queue.UserWaitInQueue.Add(user); // .First
        }

        public void PutUserFromQueue(UserProfile user)
        {
            Queue.UserWaitInQueue.Remove(Queue.UserWaitInQueue.Find(u => u.UID == user.UID));
        }

        public async Task<Battle> CheckForSecond(NpgsqlCommand)
        {
            if (Queue.UserWaitInQueue.Count >= 2)
            {
               MTCG_TheOrigin_SubProject.BL.BattleLogic battle = new MTCG_TheOrigin_SubProject.BLBattleLogic();   //  BattleLogic // BUG PROBLEM mit einbinden. in using schon drin!
                Battle round = new Battle(); // add User to Battle
                round.userA = Queue.UserWaitInQueue[0];
                round.userB = Queue.UserWaitInQueue[1];
                Battle battleResult = await battle.
       

       


            }
        }




        internal static string GetStringSHA256(string text)
        {
            if (String.IsNullOrEmpty(text))
                return String.Empty;

            using (var sha = new SHA256Managed())
            {
                byte[] textD = System.Text.Encoding.UTF8.GetBytes(text);
                byte[] hash = sha.ComputeHash(textD);
                return BitConverter.ToString(hash).Replace("-", String.Empty);
            }
        }

    }
    
}
