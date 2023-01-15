//using MTCG_TheOrigin_Subproject.DA;
//using MTCG_TheOrigin_SubProject.Model;
//using Npgsql;
//using System;
//using System.Threading;
//using Xunit;
//using MTCG_TheOrigin;



//namespace MTCG_TheOrigin.Test
//{
//    public class DataBaseConnectionTest //: IDisposable

//    {
//        //DataBaseConnectionTest testCase;

//        //public DataBaseConnectionTest()
//        //{
//        //    // setup the Test.
//        //    testCase= new DataBaseConnectionTest();
//        //    testCase.ProofToken_IsValid_ReturnsTrue();
//        //}
     


//        //[Fact]
//        //public async void TestDataBaseConnection_ReturnsTrue()
//        //{   //Arrange
//        //    //Act
//        //    //Assert
//        //    DataBaseConnection db = new DataBaseConnection();
//        //   // int[] deck = new int[4] { 10, 12, 44, 2 }; // No Duplicates
//        //    NpgsqlConnection con = await db.ConnectDB("localhost", "swe1user", "swe1pw", "mtcg_theOriginI");
//        //    var cmd = new NpgsqlCommand("", con);
//        //    bool result = db.DataBaseConnection();
//        //    Xunit.Assert.False(result);
//        //}


//        [Fact]
//        public async void ToCheckForDuplicates_DeckWithNoDuplicates_ReturnsFalse()
//        {
//            DataBaseConnection db = new DataBaseConnection();
//            int[] deck = new int[4] { 10, 12, 44, 2 }; // No Duplicates
//            NpgsqlConnection con = await db.ConnectDB("localhost", "swe1user", "swe1pw", "mtcg_theOriginI");
//            var cmd = new NpgsqlCommand("", con);
//            bool result = db.ToCheckForDuplicates(deck);
//            Xunit.Assert.False(result);
//        }
//        [Fact]
//        public async void ToCheckForDuplicates_DeckWithDuplicates_ReturnsTrue()
//        {
//            DataBaseConnection db = new DataBaseConnection();
//            int[] deck = new int[4] { 10, 2, 44, 2 }; // Duplicates
//            NpgsqlConnection con = await db.ConnectDB("localhost", "swe1user", "swe1pw", "mtcg_theOriginI");
//            var cmd = new NpgsqlCommand("", con);
//            bool result = db.ToCheckForDuplicates(deck);
//            Xunit.Assert.True(result);
//        }
//        [Fact]
//        public async void ToCheckUserHasCards_UserOwnsCards_ReturnsTrue()
//        {
//            DataBaseConnection db = new DataBaseConnection();
//            int[] deck = new int[4] { 4, 17, 9, 26 }; // No Duplicates
//            NpgsqlConnection con = await db.ConnectDB("localhost", "swe1user", "swe1pw", "mtcg_theOriginI");
//            var cmd = new NpgsqlCommand("", con);
//            bool result = await db.ToCheckUserHasCards( 25, deck, cmd);
//            Xunit.Assert.True(result);
//        }
//        [Fact]
//        public async void ToCheckUserHasCards_UserDoesNotOwnCards_ReturnsFalse()
//        {
//            DataBaseConnection db = new DataBaseConnection();
//            int[] deck = new int[4] { 10, 12, 45, 2 }; // DOES NOT OWN
//            NpgsqlConnection con = await db.ConnectDB("localhost", "swe1user", "swe1pw", "mtcg_theOriginI");
//            var cmd = new NpgsqlCommand("", con);
//            bool result = await db.ToCheckUserHasCards( 25, deck, cmd);
//            Xunit.Assert.False(result);
//        }
//        [Fact]
//        public async void SetDeck_UserOwnsAllCards_NoErrors()
//        {
//            DataBaseConnection db = new DataBaseConnection();
//            int[] deck = new int[4] { 17, 41, 32, 8 }; // DOES OWN & NO DUPLICATES
//            UserProfile user = new UserProfile();
//            user.deck = deck;
//            NpgsqlConnection con = await db.ConnectDB("localhost", "swe1user", "swe1pw", "mtcg_theOriginI");
//            var cmd = new NpgsqlCommand("", con);
//            int[] result = await db.SetDeck(25, user, cmd);
//            Xunit.Assert.Equal(4, result.Length); // BUG 
//        }
//        [Fact]
//        public async void SetDeck_UserDoesNotOwnCards_Error()
//        {
//            DataBaseConnection db = new DataBaseConnection();
//            int[] deck = new int[4] { 10, 12, 45, 2 }; // DOES NOT OWN
//            UserProfile user = new UserProfile();
//            user.deck = deck;
//            NpgsqlConnection con = await db.ConnectDB("localhost", "swe1user", "swe1pw", "mtcg_theOriginI");
//            var cmd = new NpgsqlCommand("", con);
//            int[] result = await db.SetDeck(25, user, cmd);
//            Xunit.Assert.Equal(0, result.Length); // RETURNS EMPTY ARRAY
//        }
//        [Fact]
//        public async void IfNotExistCreateUserProfile_UserProfileDoesExist()
//        {
//            DataBaseConnection db = new DataBaseConnection();
//            NpgsqlConnection con = await db.ConnectDB("localhost", "swe1user", "swe1pw", "mtcg_theOriginI");
//            var cmd = new NpgsqlCommand("", con);
//            string result = await db.IfNotExistCreateUserProfile(25, cmd);
//            Xunit.Assert.Equal("User Profile already Exists", result);
//        }
//        [Fact] // bug 
//        public async void SyncUser()
//        {
//            DataBaseConnection db = new DataBaseConnection();
//            NpgsqlConnection con = await db.ConnectDB("localhost", "swe1user", "swe1pw", "mtcg_theOriginI");
//            var cmd = new NpgsqlCommand("", con);
//             UserProfile user = await db.GetUserProfile(25, cmd);
//           // UserProfile user = await db.SyncUserProfile(username , cmd);
//            Xunit.Assert.Equal(25, user.uid);
//        }
//        [Fact] // BUG 
//        public async void GoBattle()
//        {
//            UserProfile userA = new UserProfile() { uid = 25, deck = new int[4] { 10, 12, 44, 2 }, elo = 100, win = 0, loos = 0, draw = 0 };
//            UserProfile userB = new UserProfile() { uid = 22, deck = new int[4] { 1, 6, 11, 16 }, elo = 100, win = 0, loos = 0, draw = 0 }; ;
//            Battle round = new Battle() { userA = userA, userB = userB };
//            BattleLogic battleLo = new BattleLogic();
//            round = await battleLo.GoBattle(round); // BUG 
//            bool matchSuccess = false;
//            if (userA.win > 0 || userA.loos > 0 || userA.draw > 0) matchSuccess = true;
//            Xunit.Assert.True(matchSuccess);
//        }
//        [Fact]
//        public void BattleWithTwoCards_SpellsInvolved_CAWins()
//        {
//            Card cA = new Card { cid = 1, card_type = "spell", damage = 25, element_type = "water", card_name = "spell" };
//            Card cB = new Card { cid = 2, card_type = "monster", damage = 40, element_type = "fire", card_name = "dragon" };
//            BattleLogic battle = new BattleLogic();
//            Xunit.Assert.Equal(1, battle.BattleWithTwoCards(cA, cB));
//        }
//        [Fact]
//        public void BattleWithTwoCards_NoSpellsInvolced_CBWins()
//        {
//            Card cA = new Card { cid = 1, card_type = "monster", damage = 11, element_type = "water", card_name = "goblin" };
//            Card cB = new Card { cid = 2, card_type = "monster", damage = 40, element_type = "fire", card_name = "dragon" };
//            BattleLogic battle = new BattleLogic();
//            Xunit.Assert.Equal(2, battle.BattleWithTwoCards(cA, cB));
//        }


//        [Fact]
//        public void DaamageCalculator_()
//        {
//            Card c1 = new Card { cid = 1, card_type = "spell", damage = 11, element_type = "water", card_name = "spell" };
//            Card c2 = new Card { cid = 2, card_type = "monster", damage = 40, element_type = "fire", card_name = "dragon" };
//            BattleLogic battle = new BattleLogic();
//            Xunit.Assert.Equal(2, battle.BattleWithTwoCards(c1, c2));
//        }
//        [Fact]
//        public async void ToCheckUserHasCards_ReturnsTrue()
//        {
//            DataBaseConnection db = new DataBaseConnection();
//            NpgsqlConnection con = await db.ConnectDB("localhost", "swe1user", "swe1pw", "mtcg_theOriginI");
//            var cmd = new NpgsqlCommand("", con);
//            int[] cards = new int[3] { 6, 12, 18 };
//            bool UserOwnsCard = await db.ToCheckUserHasCards(25, cards, cmd);
//            Xunit.Assert.True(UserOwnsCard);
//        }
//        [Fact]
//        public async void ToCheckUserHasCards_ReturnsFalse()
//        {
//            DataBaseConnection db = new DataBaseConnection();
//            NpgsqlConnection con = await db.ConnectDB("localhost", "swe1user", "swe1pw", "mtcg_theOriginI");
//            var cmd = new NpgsqlCommand("", con);
//            int[] cards = new int[4] { 6, 12, 18, 45 };
//            bool UserOwnsCard = await db.ToCheckUserHasCards( 25, cards, cmd);
//            Xunit.Assert.False(UserOwnsCard);
//        }
//        [Fact]
//        public async void GetTradeoffers_UserHasTradeoffers_ReturnsTrue()
//        {
//            DataBaseConnection db = new DataBaseConnection();
//            NpgsqlConnection con = await db.ConnectDB("localhost", "swe1user", "swe1pw", "mtcg_theOriginI");
//            var cmd = new NpgsqlCommand("", con);
//            int uid = 22;
//            var tradeoffers = db.GetTradeoffers(uid, cmd);
//            Xunit.Assert.True(tradeoffers.TradeOfferList.Count > 0);
//        }
//        [Fact]
//        public async void GetTradeoffers_UserDoNotHaveTradeoffers_ReturnsFalse()
//        {
//            DataBaseConnection db = new DataBaseConnection();
//            NpgsqlConnection con = await db.ConnectDB("localhost", "swe1user", "swe1pw", "mtcg_theOriginI");
//            var cmd = new NpgsqlCommand("", con);
//            int uid = 25;
//            var tradeoffers = db.GetTradeoffers(uid, cmd);
//            Xunit.Assert.False(tradeoffers.TradeOfferList.Count > 0);
//        }
//        [Fact]
//        public async void IfNotExistCreateUserProfile_AlreadyExists()
//        {
//            DataBaseConnection db = new DataBaseConnection();
//            NpgsqlConnection con = await db.ConnectDB("localhost", "swe1user", "swe1pw", "mtcg_theOriginI");
//            var cmd = new NpgsqlCommand("", con);
//            int uid = 25;
//            string response = await db.IfNotExistCreateUserProfile(uid, cmd);
//            string expectedResponse = "User Profile already Exists";
//            Xunit.Assert.Equal(expectedResponse, response);
//        }
//        [Fact]
//        public async void GetUsersCollection_UserHasCollections_ReturnsTrue()
//        {
//            DataBaseConnection db = new DataBaseConnection();
//            NpgsqlConnection con = await db.ConnectDB("localhost", "swe1user", "swe1pw", "mtcg_theOriginI");
//            var cmd = new NpgsqlCommand("", con);
//            int uid = 25;
//            var CardList = await db.GetUsersCollection(uid, cmd);
//            Xunit.Assert.True(CardList.Count > 0);
//        }
//        [Fact]
//        public async void GetUsersCollection_UserDoesNotHaveCollection_ReturnsFalse()
//        {
//            DataBaseConnection db = new DataBaseConnection();
//            NpgsqlConnection con = await db.ConnectDB("localhost", "swe1user", "swe1pw", "mtcg_theOriginI");
//            var cmd = new NpgsqlCommand("", con);
//            int uid = 1;
//            var CardList = await db.GetUsersCollection(uid, cmd);
//            Xunit.Assert.False(CardList.Count > 0);
//        }
//        [Fact]
//        public async void GetCardByCardID_ReturnsTrue()
//        {
//            DataBaseConnection db = new DataBaseConnection();
//            NpgsqlConnection con = await db.ConnectDB("localhost", "swe1user", "swe1pw", "mtcg_theOriginI");
//            var cmd = new NpgsqlCommand("", con);
//            int cid = 1;
//            var card = await db.GetCardByCardID(cid, cmd);
//            Xunit.Assert.True(card.card_name == "goblin");
//        }
//        [Fact]
//        public async void GetCardByCardID_ReturnsFalse()
//        {
//            DataBaseConnection db = new DataBaseConnection();
//            NpgsqlConnection con = await db.ConnectDB("localhost", "swe1user", "swe1pw", "mtcg_theOriginI");
//            var cmd = new NpgsqlCommand("", con);
//            int cid = 1;
//            var card = await db.GetCardByCardID(cid, cmd);
//            Xunit.Assert.False(card.card_name == "dragon");
//        }
//        [Fact]
//        public async void GetCoinsByUserid_ReturnsTrue()
//        {
//            DataBaseConnection db = new DataBaseConnection();
//            NpgsqlConnection con = await db.ConnectDB("localhost", "swe1user", "swe1pw", "mtcg_theOriginI");
//            var cmd = new NpgsqlCommand("", con);
//            int uid = 6;
//            int coins = await db.GetCoinsByUserID(uid, cmd);
//            Xunit.Assert.True(coins == 20);
//        }
//        [Fact]
//        public async void GetCoinsByUserid_ReturnsFalse()
//        {
//            DataBaseConnection db = new DataBaseConnection();
//            NpgsqlConnection con = await db.ConnectDB("localhost", "swe1user", "swe1pw", "mtcg_theOriginI");
//            var cmd = new NpgsqlCommand("", con);
//            int uid = 6;
//            int coins = await db.GetCoinsByUserID(uid, cmd);
//            Xunit.Assert.False(coins == 30);
//        }
//        [Fact]
//        public async void ProofToken_IsValid_ReturnsTrue()
//        {
//            DataBaseConnection db = new DataBaseConnection();
//            NpgsqlConnection con = await db.ConnectDB("localhost", "swe1user", "swe1pw", "mtcg_theOriginI");
//            var cmd = new NpgsqlCommand("", con);
//            string accessToken = "8D906C4004A240B5A7E5A10746E5E702077D1F081D122F891F4576A2C630496E";
//            bool valid = await db.ProofToken(accessToken, cmd);
//            Xunit.Assert.True(valid);
//        }
//        [Fact]
//        public async void GetUserProfile_ReturnsTrue()
//        {
//            DataBaseConnection db = new DataBaseConnection();
//            NpgsqlConnection con = await db.ConnectDB("localhost", "swe1user", "swe1pw", "mtcg_theOriginI");
//            var cmd = new NpgsqlCommand("", con);
//            int uid = 25;
//            var userProfile = await db.GetUserProfile(uid, cmd);
//            Xunit.Assert.True(userProfile.uid == uid);
//        }
//        [Fact]
//        public async void GetUserProfile_ReturnsFalse()
//        {
//            DataBaseConnection db = new DataBaseConnection();
//            NpgsqlConnection con = await db.ConnectDB("localhost", "swe1user", "swe1pw", "mtcg_theOriginI");
//            var cmd = new NpgsqlCommand("", con);
//            int uid = 100;
//            var userProfile = await db.GetUserProfile(uid, cmd);
//            Xunit.Assert.False(userProfile.uid == uid);
//        }

//        //public void Dispose()
//        //{
//        //    // to close test
//        //    // to close DataBase.
//        //    throw new NotImplementedException();
//        //}
//    }
//}
