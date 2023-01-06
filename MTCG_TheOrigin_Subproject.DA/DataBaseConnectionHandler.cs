﻿using MTCG_TheOrigin;
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

        public static async Task<string> NewPack(string username = "", string password = "", string accessToken = "")
        {
            if(username != null && password != null || accessToken != null)
            {
                string LoginResponse = await Login(username, password, accessToken);
                ResponseJson json = JsonSerializer.Deserialize<ResponseJson>(LoginResponse); 
                if(json.success == true)
                {
                    DataBaseConnection db = new DataBaseConnection();
                    NpgsqlConnection con = await db.ConnectDB("localhost", "swe1db", "swe1pw", "simpledatastore");
                    var cmd = new NpgsqlCommand("", con);


                    // buyPack
                    var CoinStat = await db.NewPack(json.uid, cmd);
                    //get card ids
                    var cardIdArray = await db.GetRandom(cmd);
                    // put tu user

                    // getCardList
                    List<Card> CardList = await db.GenerateCardList(cardIdArray, cmd);
                    string response = JsonSerializer.Serialize<List<Card>>(CardList);

                    return $"{{\"msg\": \"Pack opened successful.\", \"success\": true, \"coins\": {CoinStat}, \"data\": {response}}}";

                }
            }
            return "{\"msg\": \"To purchase Pack failed!\", \"success\": false}";
        }


        
        public static async Task<string> GetAccessToken(string username = "", string password = "")
        {
            if (username != null && password != null)
            {

                string LoginResponse = await Login(username, password);
                ResponseJson json = JsonSerializer.Deserialize<ResponseJson>(LoginResponse);
                if(json.success == true)
                {
                    DataBaseConnection db = new DataBaseConnection();
                    NpgsqlConnection con = await db.ConnectDB("localhost", "swe1db", "swe1pw", "simpledatastore");
                    var cmd = new NpgsqlCommand("", con);
                    var accessToken = await db.GetAccessToken(username, password, cmd);

                    return $"{{\"msg\": \"AccessToken successful in use.\", \"success:\": true, \"data\": {accessToken}}}";
                }
                return $"{{\"msg\": \"Authentication failed\", \"success\": false}}";
            }
            return $"{{\"msg\": \"Authentication failed\", \"success\": false}}";
        }
            

          


        



    }
}
