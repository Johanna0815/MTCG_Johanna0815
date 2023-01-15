using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using MTCG_TheOrigin_SubProject.Model;
using Npgsql;


namespace MTCG_TheOrigin_Subproject.DA
{
    public class DataBaseConnection
    {
        static object commandlock = new object();
        public async Task<NpgsqlConnection> ConnectDB(string server, string username, string password, string database)
        {
            var connString = $"Host={server};Username={username};Password={password};Database={database}";

            var con = new NpgsqlConnection(connString);
            await con.OpenAsync();
            return con;
        }
        public async void Register(string username, string password, NpgsqlConnection con)
        {
           // try {
                var cmd = new NpgsqlCommand("", con);
                
                // to check if user is existing
                int uid = await GetUserIDByUsername(username, cmd);
                if (uid == 0)  // uid is 0 returned, if user not exists.
                {
                    lock (cmd)
                    {
                        // generate user.
                        cmd.CommandText = $"INSERT INTO users(username, password) VALUES(@username, @password)";
                        cmd.Parameters.AddWithValue("username", username);
                        cmd.Parameters.AddWithValue("password", password);
                        cmd.Prepare();
                        cmd.ExecuteNonQueryAsync();
                    }
               
            
                    lock (cmd)
                    {
                         Thread.Sleep(100);
                        uid = GetUserIDByUsername(username, cmd).Result;
                    }
                
                    SetUsersBalance(uid, cmd);
                    SetAccessToken(uid, username, password, cmd); // bug kommt keinen token setzen.
                    //Console.WriteLine(await GetUserBalance(username, cmd));
                }
                else
                {
                    Console.WriteLine("Username is already taken. Choose another username!");
                }

            //}catch (Exception ae)
            //{
            //    Console.WriteLine(  $" {ae} occured.");

            //}
        }












        public async Task<string> LoginByCredentials(string username, string password, NpgsqlConnection con)
        {
            int uid;
            string access_token;
            string due_date;
            var cmd = new NpgsqlCommand($"SELECT u.uid, u.username, act.access_token, act.due_date FROM users AS u JOIN access_tokens AS act ON u.uid = act.uid WHERE u.username = @username AND u.password = @password", con);
            cmd.Parameters.AddWithValue("username", username);
            cmd.Parameters.AddWithValue("password", password);
            cmd.Prepare();


            await using (var reader = await cmd.ExecuteReaderAsync())
                while (await reader.ReadAsync())
                {
                    uid = (int)reader["uid"];
                    access_token = (string)reader["access_token"];
                    due_date = reader["due_date"].ToString();


                    return $"{{\"msg\":\"Login successful.\", \"uid\": {uid}, \"access_token\": \"{access_token}\", \"success\": true}}";
                }
            return $"{{\"msg\":\"Login failed! Please check your credentials.\", \"success\": false}}";

        }


        public async Task<bool> ProofToken(string access_token, NpgsqlCommand cmd)
        {
            string due_date;
            int uid;
            cmd.CommandText = $"SELECT u.uid, u.username, act.due_date FROM users AS u JOIN access_tokens AS act ON u.uid = act.uid WHERE act.access_token = @access_token";
            cmd.Parameters.AddWithValue("access_token", access_token);
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
                    }
                    else
                    {
                        return false;
                    }
                }
            return false;
        }


        public async Task<string> GetAccessToken(string username, string password, NpgsqlCommand cmd)
        {
            cmd.CommandText = $"SELECT act.access_token FROM users AS u JOIN access_tokens AS act ON u.uid = act.uid WHERE u.username = @username AND u.password = @password";
            cmd.Parameters.AddWithValue("username", username);
            cmd.Parameters.AddWithValue("password", password);
            cmd.Prepare();
            await using (var reader = await cmd.ExecuteReaderAsync())
                while (await reader.ReadAsync())
                {
                    return (string)reader["access_token"]; //bug
                }
            return "credentials wrong<-";
        }
        public async Task<int> GetUserIDByUsername(string username, NpgsqlCommand cmd)
        {
            cmd.CommandText = $"SELECT uid FROM users WHERE username = @username";
            cmd.Parameters.AddWithValue("username", username);
            cmd.Prepare();
            await using (var reader = await cmd.ExecuteReaderAsync())
            {
                try
                {
                    while (await reader.ReadAsync())
                        return reader.GetInt32(0);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                    throw;
                }
                finally
                {
                    reader.Close();
                }

            }

            return 0;
        }
        public void SetUsersBalance(int uid, NpgsqlCommand cmd)
        {
            lock (commandlock)
            {
                cmd.CommandText = $"INSERT INTO balances(uid, coins) VALUES(@uid, 20)";
                cmd.Parameters.AddWithValue("uid", uid);
                cmd.Prepare();
                cmd.ExecuteNonQueryAsync();
            }
        }
        public async void SetAccessToken(int uid, string username, string password, NpgsqlCommand cmd)
        {
            Thread.Sleep(200);
                string hash = GetStringSHA256(uid + username + password);
                cmd.CommandText = $"INSERT INTO access_tokens(uid, access_token, due_date) VALUES(@uid, @hash, '2023-05-05')";
                cmd.Parameters.AddWithValue("uid", uid);
                cmd.Parameters.AddWithValue("hash", hash);
                cmd.Prepare();
               await cmd.ExecuteNonQueryAsync();
            
        }

        // without password
        public async Task<int> GetAccessToken(string username, NpgsqlCommand cmd)
        {
            int uid = await GetUserIDByUsername(username, cmd);
            cmd.CommandText = $"SELECT coins FROM balances WHERE uid = @uid";
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
          


            //reduce balance by 5
            int newCoins = await GetCoinsByUserID(uid, cmd) - 5;
            lock (commandlock)
            {
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
            cmd.CommandText = $"SELECT count(*) FROM carddeck";
            int[] cardIdArray = new int[5];
            cmd.Prepare();
            await using (var reader = await cmd.ExecuteReaderAsync())
                while (await reader.ReadAsync())
                {
                    long totalCount = (long)reader["count"];
                    for (int i = 0; i < cardIdArray.Length; i++)
                    {
                        Random random = new Random();
                        cardIdArray[i] = random.Next(1, (int)totalCount);
                    }
                }
            return cardIdArray;
        }
        public async Task<Card> GetCardByCardID(int cid, NpgsqlCommand cmd)
        {
            Console.WriteLine(cid);
            cmd.CommandText = $"SELECT cid, card_type, card_name, element_type, damage FROM carddeck WHERE cid = @cid";
            cmd.Parameters.AddWithValue("cid", cid);
            cmd.Prepare();
            await using (var reader = await cmd.ExecuteReaderAsync())
                while (await reader.ReadAsync())
                {
                    Card card = new Card
                    {
                        cid = (int)reader["cid"],
                        card_type = (string)reader["card_type"],
                        card_name = (string)reader["card_name"],
                        element_type = (string)reader["element_type"],
                        damage = (int)reader["damage"]
                    };
                    return card;
                }
            return new Card();
        }


        public void CardToUser(int uid, int[] cardIdArray, NpgsqlCommand cmd)
        {
            lock (commandlock)
            {
                for (int i = 0; i < cardIdArray.Length; i++)
                {
                    int curr = cardIdArray[i];
                    cmd.CommandText = $"INSERT INTO collections(uid, cid) VALUES (@uid, @cid)";
                    cmd.Parameters.Clear();
                    cmd.Parameters.AddWithValue("uid", uid);
                    cmd.Parameters.AddWithValue("cid", curr);
                    cmd.Prepare();
                    var reader = cmd.ExecuteNonQuery();
                }
            }

        }

        public async Task<List<Card>> GenerateCardList(int[] cardIdArray, NpgsqlCommand cmd)
        {
            List<Card> CardList = new List<Card>();
            cmd.CommandText = $"SELECT cid, card_type, card_name, element_type, damage FROM carddeck WHERE cid in (@1, @2, @3, @4, @5)";
            for (int i = 0; i < cardIdArray.Length; i++)
            {
                cmd.Parameters.AddWithValue($"{i + 1}", cardIdArray[i]);
            }
            cmd.Prepare();
            await using (var reader = await cmd.ExecuteReaderAsync())
                while (await reader.ReadAsync())
                {
                    Card card = new Card
                    {
                        cid = (int)reader["cid"],
                        card_type = (string)reader["card_type"],
                        card_name = (string)reader["card_name"],
                        element_type = (string)reader["element_type"],
                        damage = (int)reader["damage"]
                    };
                    CardList.Add(card);
                }
            return CardList;
        }
        public async Task<List<Card>> GetUsersCollection(int uid, NpgsqlCommand cmd)
        {
            List<Card> CardList = new List<Card>();
            cmd.CommandText = $"SELECT cid, card_type, card_name, element_type, damage FROM carddeck WHERE cid in (SELECT cid FROM collections WHERE uid = @uid)";
            cmd.Parameters.AddWithValue("uid", uid);
            cmd.Prepare();
            await using (var reader = await cmd.ExecuteReaderAsync())
                while (await reader.ReadAsync())
                {
                    Card card = new Card
                    {
                        cid = (int)reader["cid"],
                        card_type = (string)reader["card_type"],
                        card_name = (string)reader["card_name"],
                        element_type = (string)reader["element_type"],
                        damage = (int)reader["damage"]
                    };
                    CardList.Add(card);
                }
            return CardList;
        }


        public async Task<UserProfile> GetUserProfile(int uid, NpgsqlCommand cmd)
        {
            UserProfile user = new UserProfile();
            cmd.CommandText = "SELECT userprofile.uid, userprofile.elo, userprofile.deck, userprofile.win, userprofile.loos, userprofile.draw, users.username  FROM userprofile JOIN users ON userprofile.uid = users.uid WHERE userprofile.uid = @uid";
            cmd.Parameters.AddWithValue("uid", uid);
            cmd.Prepare();
            await using (var reader = await cmd.ExecuteReaderAsync())
                while (await reader.ReadAsync())
                {

                    user.uid = (int)reader["uid"];
                    user.username = (string)reader["username"];
                    user.elo = (int)reader["elo"];
                    user.deck = (int[])reader["deck"];
                    user.win = (int)reader["win"];
                    user.loos = (int)reader["loos"];
                    user.draw = (int)reader["draw"];
                }
            return user;

        }
        public void SyncUserProfile(UserProfile user, NpgsqlCommand cmd)
        {
            lock (commandlock)
            {
                cmd.CommandText = "UPDATE userprofile SET elo = @elo, win = @win, loos = @loos, draw = @draw WHERE uid = @uid";
                cmd.Parameters.AddWithValue("elo", user.elo); // elo

                cmd.Parameters.AddWithValue("win", user.win); // win
                cmd.Parameters.AddWithValue("loos", user.loos); // loos
                cmd.Parameters.AddWithValue("draw", user.draw); // draw
                cmd.Parameters.AddWithValue("uid", user.uid); // draw
                cmd.Prepare();
                cmd.ExecuteNonQuery();
            }
        }


        // mandatory feature. {be able to add virtual coins to your system}
        public void AddCoins(int uid, int value, NpgsqlCommand cmd)
        {
            lock (commandlock)
            {
                cmd.CommandText = "UPDATE balances SET coins = coins + @value WHERE uid = @uid";
                cmd.Parameters.AddWithValue("uid", uid); // elo
                cmd.Parameters.AddWithValue("value", value); // win
                cmd.Prepare();
                cmd.ExecuteNonQuery();
            }
        }


        public async Task<string> IfNotExistCreateUserProfile(int uid, NpgsqlCommand cmd)
        {
            cmd.CommandText = "SELECT uid FROM userprofile WHERE uid = @uid";
            cmd.Parameters.AddWithValue("uid", uid);
            cmd.Prepare();
            bool UserProfileUidExists = false;
            await using (var reader = await cmd.ExecuteReaderAsync())
                while (await reader.ReadAsync())
                {
                    if ((int)reader["uid"] == uid) UserProfileUidExists = true;
                }
            if (UserProfileUidExists == true)
            {
                return "User Profile already Exists";
            }            
           else
            {
                lock (commandlock)
                {
                    cmd.CommandText = "INSERT INTO userprofile (uid, elo, win, loos, draw) VALUES (@uid, 100, 0,0,0)";
                    cmd.Parameters.AddWithValue("uid", uid);
                    cmd.ExecuteNonQueryAsync();
                    return "User Profile did not exist before and was created";
                }

            }

        }


        public async Task<int[]> SetDeck(int uid, UserProfile user, NpgsqlCommand cmd)
        {
            await IfNotExistCreateUserProfile(uid, cmd);
            if (ToCheckForDuplicates(user.deck) == false)
            {
                bool DoesUserOwnCards = await ToCheckUserHasCards(uid, user.deck, cmd);
                if (DoesUserOwnCards == true)
                {
                    lock (commandlock)
                    {
                        cmd.CommandText = "UPDATE userprofile SET deck = ARRAY[@cid1,@cid2,@cid3,@cid4] WHERE uid = @uid";
                        cmd.Parameters.AddWithValue("uid", uid);
                        for (int i = 0; i < user.deck.Length; i++)
                        {
                            cmd.Parameters.AddWithValue($"cid{i + 1}", user.deck[i]);
                        }
                        cmd.Prepare();
                        cmd.ExecuteNonQuery();
                        return user.deck;
                    }
                }
                else
                {
                    int[] emptyArray = new int[0];
                    return emptyArray;
                }
            }
            else
            {
                int[] emptyArray = new int[0];
                return emptyArray;
            }
        }

        public bool ToCheckForDuplicates(int[] deck)
        {
            var dict = new Dictionary<int, int>();
            foreach (var card in deck)
            {
                if (!dict.ContainsKey(card))
                {
                    dict.Add(card, 0);
                }
                dict[card]++;
            }
            bool hasDouplicates;
            if (dict.Count == 4)
            {
                hasDouplicates = false;
            }
            else
            {
                hasDouplicates = true;
            }
            return hasDouplicates;
        }




        public async Task<bool> ToCheckUserHasCards(int uid, int[] deck, NpgsqlCommand cmd)
        {
            cmd.CommandText = $"SELECT cid FROM collections WHERE uid = @uid AND cid IN (@cid1, @cid2, @cid3, @cid4)";
            cmd.Parameters.AddWithValue("uid", uid);
            for (int i = 0; i < deck.Length; i++)
            {
                cmd.Parameters.AddWithValue($"cid{i + 1}", deck[i]);
            }
            cmd.Prepare();
            bool UserOwnsCard = true;

            var dict = new Dictionary<int, int>();
            await using (var reader = await cmd.ExecuteReaderAsync())
                while (await reader.ReadAsync())
                {
                    int card = (int)reader["cid"];
                    if (!dict.ContainsKey(card))
                    {
                        dict.Add(card, 0);
                    }
                    dict[card]++;
                }
            foreach (var card in deck)
            {
                if (dict.ContainsKey(card) == false) UserOwnsCard = false;
            }
            return UserOwnsCard;
        }




        public async void GoBattle(UserProfile user, NpgsqlCommand cmd)
        {
            // add user to Queue
            UserInQueue(user);
            if (Queue.UserWaitInQueue.Count >= 2)
            {
                Battle battleResult = await CheckForSecond(cmd);
                RemoveUserFromQueue(battleResult.userA);
                RemoveUserFromQueue(battleResult.userB);
                // syncinc with DB
                SyncUserProfile(battleResult.userA, cmd);
                SyncUserProfile(battleResult.userB, cmd);
            }

        }


        public void UserInQueue(UserProfile user)
        {
            if (Queue.UserWaitInQueue.Where(u => u.uid == user.uid).FirstOrDefault() == null) Queue.UserWaitInQueue.Add(user);
        }
        public void RemoveUserFromQueue(UserProfile user)
        {
          _=   Queue.UserWaitInQueue.Remove(Queue.UserWaitInQueue.Find(u => u.uid == user.uid));
        }
        public async Task<Battle> CheckForSecond(NpgsqlCommand cmd)
        {
            if (Queue.UserWaitInQueue.Count >= 2)
            {
                BattleLogic battle = new BattleLogic();
                Battle battleRound = new Battle(); // ADD USERS TO MATCH
                battleRound.userA = Queue.UserWaitInQueue[0];
                battleRound.userB = Queue.UserWaitInQueue[1];
                Battle battleResults = await battle.GoBattle(battleRound); // START BATTLE
                return battleResults;
            }
            else return null;
        }
        public TradeOffers GetTradeoffers(int uid, NpgsqlCommand cmd)
        {
            lock (commandlock)
            {

                TradeOffers tradeoffers = new TradeOffers();
                cmd.CommandText = "SELECT * FROM tradeoffers WHERE receiver_uid = @uid";
                cmd.Parameters.AddWithValue("uid", uid);
                cmd.Prepare();

                using (var reader = cmd.ExecuteReader())
                    while (reader.Read())
                    {
                        TradeOffer trade = new TradeOffer();
                        trade.tradeoffer_id = (int)reader["tradeoffer_id"];
                        trade.sender_uid = (int)reader["sender_uid"];
                        trade.receiver_uid = (int)reader["receiver_uid"];
                        trade.a_receive = (int[])reader["a_receive"];
                        trade.b_receive = (int[])reader["b_receive"];
                        trade.status = (string)reader["status"];

                        tradeoffers.TradeOfferList.Add(trade);

                    }
                return tradeoffers;
            }

        }
        public TradeOffer GetTradeofferByTradeoffer_id(int tradeoffer_id, NpgsqlCommand cmd)
        {
            lock (commandlock)
            {
                TradeOffer tradeoffer = new TradeOffer();
                cmd.CommandText = "SELECT * FROM tradeoffers WHERE tradeoffer_id = @tradeoffer_id";
                cmd.Parameters.AddWithValue("tradeoffer_id", tradeoffer_id);
                cmd.Prepare();

                using (var reader = cmd.ExecuteReader())
                    while (reader.Read())
                    {
                        tradeoffer.tradeoffer_id = (int)reader["tradeoffer_id"];
                        tradeoffer.sender_uid = (int)reader["sender_uid"];
                        tradeoffer.receiver_uid = (int)reader["receiver_uid"];
                        tradeoffer.a_receive = (int[])reader["a_receive"];
                        tradeoffer.b_receive = (int[])reader["b_receive"];
                        tradeoffer.status = (string)reader["status"];
                    }
                return tradeoffer;
            }

        }
        public async Task<string> AcceptTradeoffer(int uid, int tradeoffer_id, NpgsqlCommand cmd)
        {
            cmd.CommandText = "UPDATE tradeoffers SET status = @status WHERE tradeoffer_id = @tradeoffer_id AND recipient_uid = @uid";
            string status = "accepted";
            cmd.Parameters.AddWithValue("status", status);
            cmd.Parameters.AddWithValue("uid", uid);
            cmd.Parameters.AddWithValue("tradeoffer_id", tradeoffer_id);
            cmd.Prepare();
            await cmd.ExecuteNonQueryAsync();

            TradeOffer tradeoffer = GetTradeofferByTradeoffer_id(tradeoffer_id, cmd);
            // check for cards to user receive, ELSE no  swap
            if (tradeoffer.b_receive.Length > 0)
            {
                SwapCardsOwner(tradeoffer.receiver_uid, tradeoffer.sender_uid, tradeoffer.b_receive, cmd);
            }

            if (tradeoffer.a_receive.Length > 0)
            {
                SwapCardsOwner(tradeoffer.sender_uid, tradeoffer.receiver_uid, tradeoffer.a_receive, cmd);
            }
            return "Tradeoffer successfully accepted!";
        }


        // Swap cards to newOwner
        public string SwapCardsOwner(int uid, int sender_uid, int[] cards, NpgsqlCommand cmd)
        {
            lock (commandlock)
            {
                string cardsCountString = "";
                for (int i = 1; i <= cards.Length; i++)
                {
                    if (i != cards.Length) cardsCountString += $"@c{i},";
                    else cardsCountString += $"@c{i}";
                }
                string commandText = $"UPDATE collections SET uid = @uid WHERE cid IN ({cardsCountString}) AND uid = @sender_uid";
                cmd.CommandText = commandText;
                cmd.Parameters.AddWithValue("cards", cards);
                for (int i = 0; i < cards.Length; i++)
                {
                    cmd.Parameters.AddWithValue($"c{i + 1}", cards[i]);
                }
                cmd.Parameters.AddWithValue("uid", uid);
                cmd.Parameters.AddWithValue("sender_uid", sender_uid);
                cmd.Prepare();

                cmd.ExecuteNonQuery();

                return "Tradeoffer successfully accepted!";
            }

        }
        public async Task<string> DeclineTradeoffer(int uid, int tradeoffer_id, NpgsqlCommand cmd)
        {
            cmd.CommandText = "UPDATE tradeoffers SET status = @status WHERE tradeoffer_id = @tradeoffer_id";
            string status = "declined";
            cmd.Parameters.AddWithValue("status", status);
            cmd.Parameters.AddWithValue("tradeoffer_id", tradeoffer_id);
            cmd.Prepare();

            await cmd.ExecuteNonQueryAsync();

            return "Tradeoffer successfully declined!";
        }
        public string GenerateTradeoffer(int sender_uid, int receiver_uid, int[] a_receive, int[] b_receive, NpgsqlCommand cmd)
        {
            lock (commandlock)
            {
                cmd.CommandText = "INSERT INTO tradeoffers (sender_uid, receiver_uid, a_receive, b_receive, status) VALUES (@sender_uid, @receiver_uid, @a_receive, @b_receive, @status)";

                cmd.Parameters.AddWithValue("sender_uid", sender_uid);
                cmd.Parameters.AddWithValue("receiver_uid", receiver_uid);
                cmd.Parameters.AddWithValue("a_receive", a_receive);
                cmd.Parameters.AddWithValue("b_receive", b_receive);
                string status = "pending";
                cmd.Parameters.AddWithValue("status", status);
                cmd.Prepare();

                cmd.ExecuteNonQuery();

                return "Tradeoffer successfully created!";
            }

        }
        public async Task<bool> CheckIfUserOwnsCard(int[] cards, int uid, NpgsqlCommand cmd)
        {
            cmd.CommandText = "SELECT cid FROM collections WHERE uid = @uid";
            cmd.Parameters.AddWithValue("uid", uid);
            cmd.Prepare();
            bool UserOwnsCard = true;

            var dict = new Dictionary<int, int>();
            await using (var reader = await cmd.ExecuteReaderAsync())
                while (await reader.ReadAsync())
                {
                    int card = (int)reader["cid"];
                    if (!dict.ContainsKey(card))
                    {
                        dict.Add(card, 0);
                    }
                    dict[card]++;
                }
            foreach (var card in cards)
            {
                if (dict.ContainsKey(card) == false) UserOwnsCard = false;
            }
            return UserOwnsCard;
        }
        public async Task<UserProfile> GetUserprofile(int uid, NpgsqlCommand cmd)
        {
            cmd.CommandText = "SELECT * FROM userprofile WHERE uid = @uid";
            cmd.Parameters.AddWithValue("uid", uid);
            cmd.Prepare();

            UserProfile userprofile = new UserProfile();
            await using (var reader = await cmd.ExecuteReaderAsync())
                while (await reader.ReadAsync())
                {
                    userprofile.uid = (int)reader["uid"];
                    userprofile.elo = (int)reader["elo"];
                    userprofile.deck = (int[])reader["deck"];
                    userprofile.win = (int)reader["win"];
                    userprofile.loos = (int)reader["loos"];
                    userprofile.draw = (int)reader["draw"];
                }
            return userprofile;
        }




        // source https://www.delftstack.com/howto/csharp/csharp-sha256/
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
