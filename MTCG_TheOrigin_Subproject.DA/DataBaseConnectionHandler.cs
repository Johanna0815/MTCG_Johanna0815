using MTCG_TheOrigin_SubProject.Model;
using Npgsql;
using System.Text.Json;

namespace MTCG_TheOrigin_Subproject.DA
{
    public class DataBaseConnectionHandler
    {

        public static async Task<string> Register(string username, string password)
        {
            DataBaseConnection db = new DataBaseConnection();
            NpgsqlConnection con = await db.ConnectDB("localhost", "swe1user", "swe1pw", "mtcg_theOriginI"); // swe1db ? 
           // con.Open(); // pq
            db.Register(username, password, con); // db change !
            return "{\"MSG\": \"Login successfull!\", \"Success\": true}";
        }

        

        public static async Task<string> Login(string username = "", string password = "", string accessToken = "")
        {
            DataBaseConnection db = new DataBaseConnection();
            NpgsqlConnection con = await db.ConnectDB("localhost", "swe1user", "swe1pw", "mtcg_theOriginI"); // simpledatastroe - weg!
                                                                                                         // 

            if (accessToken != null)
            {
                var cmd = new NpgsqlCommand("", con);
                bool isProofed = await db.ProofToken(accessToken, cmd);
                if (isProofed == true)
                {
                    return "{\"MSG\": \"Login successfull!\", \"Success\": true}";
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
                if(json.Success == true)
                {
                    DataBaseConnection db = new DataBaseConnection();
                    NpgsqlConnection con = await db.ConnectDB("localhost", "swe1user", "swe1pw", "mtcg_theOriginI");
                    var cmd = new NpgsqlCommand("", con);


                    // buyPack
                    var CoinStat = await db.NewPack(json.UID, cmd);
                    //get card ids
                    var cardIdArray = await db.GetRandom(cmd);
                    // put tu user

                    // getCardList
                    List<Card> CardList = await db.GenerateCardList(cardIdArray, cmd);
                    string response = JsonSerializer.Serialize<List<Card>>(CardList);

                    return $"{{\"MSG\": \"Pack opened successful.\", \"Success\": true, \"Coins\": {CoinStat}, \"Data\": {response}}}";

                }
            }
            return "{\"MSG\": \"To purchase Pack failed!\", \"Success\": false}";
        }


        
        public static async Task<string> GetAccessToken(string username = "", string password = "")
        {
            if (username != null && password != null)
            {

                string LoginResponse = await Login(username, password);
                ResponseJson json = JsonSerializer.Deserialize<ResponseJson>(LoginResponse);
                if(json.Success == true)
                {
                    DataBaseConnection db = new DataBaseConnection();
                    NpgsqlConnection con = await db.ConnectDB("localhost", "swe1user", "swe1pw", "mtcg_theOriginI");
                    var cmd = new NpgsqlCommand("", con);
                    var accessToken = await db.GetAccessToken(username, password, cmd);

                    return $"{{\"MSG\": \"AccessToken successful in use.\", \"Success:\": true, \"Data\": {accessToken}}}";
                }
                return $"{{\"MSG\": \"Authentication failed\", \"Success\": false}}";
            }
            return $"{{\"MSG\": \"Authentication failed\", \"Success\": false}}";
        }



        public static async Task<string> SetDeck(int[] deck, string username = "", string password = "", string accessToken = "") // bug { { } }
        {
            if (username != null && password != null || accessToken != null)
            {
                string LoginResponse = await Login(username, password, accessToken);
                ResponseJson json = JsonSerializer.Deserialize<ResponseJson>(LoginResponse);
                if (json.Success == true)
                {
                    DataBaseConnection db = new DataBaseConnection();
                    NpgsqlConnection con = await db.ConnectDB("localhost", "swe1user", "swe1pw", "mtcg_theOriginI");
                    var cmd = new NpgsqlCommand("", con);

                    UserProfile userP = new UserProfile();
                    userP.Deck = deck;
                    int[] checkedDeck = await db.SetDeck(json.UID, userP, cmd);

                    return $"{{\"MSG\": \"The deck has been successfully configured\", \"Success\": true, \"Data\": {checkedDeck.ToString()}}}";

                    //var accessToken = await db.GetAccessToken(username, password, cmd);

                }
            }
            return $"{{\"MSG\": \"he provided deck did not include the required amount of cards\", \"Success\": false, \"Deck\": []}}";
        }




        
        // goBattle
        // generateTradeOffer


    }
}
