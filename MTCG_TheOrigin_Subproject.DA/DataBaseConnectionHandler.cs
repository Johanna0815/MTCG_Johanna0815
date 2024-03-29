﻿using MTCG_TheOrigin_Subproject.DA;
using MTCG_TheOrigin_SubProject.Model;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace MTCG_TheOrigin_Subproject.DA
{
   public class DataBaseConnectionHandler
    {
        public static async Task<string> Register(string username, string password)
        {



            DataBaseConnection db = new DataBaseConnection();
            NpgsqlConnection con = await db.ConnectDB("localhost", "swe1user", "swe1pw", "mtcg_theOriginI");
            db.Register(username, password, con);
            //conn.Close();
            return "{\"msg\": \"User successfully created\", \"success\": true}";
        }
        public static async Task<string> Login(string username = "", string password = "", string access_token = "")
        {
            DataBaseConnection db = new DataBaseConnection();
            NpgsqlConnection con = await db.ConnectDB("localhost", "swe1user", "swe1pw", "mtcg_theOriginI");
            if (access_token != null)
            {
                var cmd = new NpgsqlCommand("", con);
                bool isValid = await db.ProofToken(access_token, cmd);
                if (isValid == true)
                {
                    return "{\"msg\": \"access_token valid.\", \"success\": true}";
                }
                else
                {
                    return "{\"msg\": \"access_token not valid! Please Login again!\", \"success\": false}";
                }
            }
            else if (username != null && password != null)
            {
                string response = await db.LoginByCredentials(username, password, con);
                return response;
            }
            return "none";
        }


        public static async Task<string> NewPack(string username = "", string password = "", string access_token = "")
        {
            if (username != null && password != null ^ access_token != null)
            {
                string loginResponse = await Login(username, password, access_token);
                ResponseJson json = JsonSerializer.Deserialize<ResponseJson>(loginResponse);
                if (json.success == true)
                {
                    DataBaseConnection db = new DataBaseConnection();
                    NpgsqlConnection con = await db.ConnectDB("localhost", "swe1user", "swe1pw", "mtcg_theOriginI");
                    var cmd = new NpgsqlCommand("", con);
                    
                    
                    
                    //reduce coins by 5
                    var newCoins = await db.NewPack(json.uid, cmd);
                    //get random card id's (cid)
                    var cardIdArray = await db.GetRandom(cmd);


                    //assign cards to user
                    db.CardToUser(json.uid, cardIdArray, cmd);
                    //get CardList
                    List<Card> CardList = await db.GenerateCardList(cardIdArray, cmd);
                    string response = JsonSerializer.Serialize<List<Card>>(CardList);

                    return $"{{\"msg\": \"Pack purchase successful.\", \"success\": true, \"coins\": {newCoins}, \"data\": {response}}}";
                }
            }
            return "{\"msg\": \"Pack purchase failed!\", \"success\": false}";
        }



        public static async Task<string> ShowCollection(string username = "", string password = "", string access_token = "")
        {
            if (username != null && password != null ^ access_token != null)
            {
                string loginResponse = await Login(username, password, access_token);
                ResponseJson json = JsonSerializer.Deserialize<ResponseJson>(loginResponse);
                if (json.success == true)
                {
                    DataBaseConnection db = new DataBaseConnection();
                    NpgsqlConnection con = await db.ConnectDB("localhost", "swe1user", "swe1pw", "mtcg_theOriginI");
                    var cmd = new NpgsqlCommand("", con);

                    //get CardList
                    List<Card> CardList = await db.GetUsersCollection(json.uid, cmd);

                    string response = JsonSerializer.Serialize<List<Card>>(CardList);

                    return $"{{\"msg\": \"Successfully retrieved Collection.\", \"success\": true, \"data\": {response}}}";
                }
            }
            return "{\"msg\": \"Collection not available! Error!\", \"success\": false}";
        }
        public static async Task<string> SetDeck(int[] deck, string username = "", string password = "", string access_token = "")
        {
            if (username != null && password != null ^ access_token != null)
            {
                string loginResponse = await Login(username, password, access_token);
                ResponseJson json = JsonSerializer.Deserialize<ResponseJson>(loginResponse);
                if (json.success == true)
                {
                    DataBaseConnection db = new DataBaseConnection();
                    NpgsqlConnection con = await db.ConnectDB("localhost", "swe1user", "swe1pw", "mtcg_theOriginI");
                    var cmd = new NpgsqlCommand("", con);

                    UserProfile user = new UserProfile();
                    user.deck = deck;
                    int[] confirmedDeck = await db.SetDeck(json.uid, user, cmd);

                    return $"{{\"msg\": \"Successfully Updated Deck\", \"success\": true, \"data\": {confirmedDeck.ToString()}}}";
                }
            }
            return $"{{\"msg\": \"To Update Deck failed make sure you own the cards and don't have any duplicates.\", \"success\": false, \"deck\": []}}";
        }
        public static async Task<string> GoBattle(string username = "", string password = "", string access_token = "")
        {
            if (username != null && password != null ^ access_token != null)
            {
                string loginResponse = await Login(username, password, access_token);
                ResponseJson json = JsonSerializer.Deserialize<ResponseJson>(loginResponse);
                if (json.success == true)
                {
                    DataBaseConnection db = new DataBaseConnection();
                    NpgsqlConnection con = await db.ConnectDB("localhost", "swe1user", "swe1pw", "mtcg_theOriginI");
                    var cmd = new NpgsqlCommand("", con);

                    //Step_1: GetUserProfile
                    UserProfile user = await db.GetUserProfile(json.uid, cmd);

                    //Step_2: Queue
                    db.GoBattle(user, cmd);


                    return $"{{\"msg\": \"Searching for Battle\", \"success\": true}}";
                }
            }
            return "{\"msg\": \"Searching for Battle failed\", \"success\": false}";
        }
        public static async Task<string> TradeOffer(int receiver_uid, int[] a_receive, int[] b_receive, string action, int tradeoffer_id = -1, string username = "", string password = "", string access_token = "")
        {
            if (username != null && password != null ^ access_token != null)
            {
                string loginResponse = await Login(username, password, access_token);
                ResponseJson json = JsonSerializer.Deserialize<ResponseJson>(loginResponse);
                if (json.success == true)
                {
                    DataBaseConnection db = new DataBaseConnection();
                    NpgsqlConnection con = await db.ConnectDB("localhost", "swe1user", "swe1pw", "mtcg_theOriginI");
                    var cmd = new NpgsqlCommand("", con);
                    switch (action)
                    {
                        case "accept":
                            string accept_response = await db.AcceptTradeoffer(json.uid, tradeoffer_id, cmd);
                            return accept_response;
                        case "decline":
                            string decline_response = await db.DeclineTradeoffer(json.uid, tradeoffer_id, cmd);
                            return decline_response;
                        case "generate":
                            string generate_response = db.GenerateTradeoffer(json.uid, receiver_uid, a_receive, b_receive, cmd);
                            return generate_response;
                    }
                    return "";
                }
                return "";
            }
            return "";

        }
        public static async Task<string> GetTradeoffers(string username = "", string password = "", string access_token = "")
        {
            if (username != null && password != null ^ access_token != null)
            {
                string loginResponse = await Login(username, password, access_token);
                ResponseJson json = JsonSerializer.Deserialize<ResponseJson>(loginResponse);
                if (json.success == true)
                {
                    DataBaseConnection db = new DataBaseConnection();
                    NpgsqlConnection con = await db.ConnectDB("localhost", "swe1user", "swe1pw", "mtcg_theOriginI");
                    var cmd = new NpgsqlCommand("", con);
                    var tradeoffers = db.GetTradeoffers(json.uid, cmd);

                    string response = JsonSerializer.Serialize<List<TradeOffer>>(tradeoffers.TradeOfferList);
                    return $"{{\"msg\": \"Successfully retrieved tradeoffers!\", \"success\": true, \"data\": {response}}}";
                }


                return $"{{\"msg\": \"Authentication failed\", \"success\": false}}";
            }
            return $"{{\"msg\": \"Authentication failed\", \"success\": false}}";
        }

        public static async Task<string> GetUserProfile(string username = "", string password = "", string access_token = "")
        {
            if (username != null && password != null ^ access_token != null)
            {
                string loginResponse = await Login(username, password, access_token);
                ResponseJson json = JsonSerializer.Deserialize<ResponseJson>(loginResponse);
                if (json.success == true)
                {
                    DataBaseConnection db = new DataBaseConnection();
                    NpgsqlConnection con = await db.ConnectDB("localhost", "swe1user", "swe1pw", "mtcg_theOriginI");
                    var cmd = new NpgsqlCommand("", con);
                    var userprofile = await db.GetUserprofile(json.uid, cmd);

                    string response = JsonSerializer.Serialize<UserProfile>(userprofile);
                    return $"{{\"msg\": \"Successfully retrieved Userprofile!\", \"success\": true, \"data\": {response}}}";
                }


                return $"{{\"msg\": \"Authentication failed\", \"success\": false}}";
            }
            return $"{{\"msg\": \"Authentication failed\", \"success\": false}}";
        }


        public static async Task<string> GetAccessToken(string username = "", string password = "")
        {
            if (username != null && password != null)
            {
                string loginResponse = await Login(username, password);
                ResponseJson json = JsonSerializer.Deserialize<ResponseJson>(loginResponse);
                if (json.success == true)
                {
                    DataBaseConnection db = new DataBaseConnection();
                    NpgsqlConnection con = await db.ConnectDB("localhost", "swe1user", "swe1pw", "mtcg_theOriginI");
                    var cmd = new NpgsqlCommand("", con);
                    var access_token = await db.GetAccessToken(username, password, cmd);

                    return $"{{\"msg\": \"Successfully retrieved access_token!\", \"success\": true, \"data\": {access_token}}}";
                }


                return $"{{\"msg\": \"Authentication failed\", \"success\": false}}";
            }
            return $"{{\"msg\": \"Authentication failed\", \"success\": false}}";
        }
    }
}
