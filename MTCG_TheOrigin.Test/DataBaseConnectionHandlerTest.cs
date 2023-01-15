//using MTCG_TheOrigin_Subproject.DA;
//using MTCG_TheOrigin_SubProject.Model;
//using Npgsql;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using Xunit;

//namespace MTCG_TheOrigin.Test
//{
//    public class DataBaseConnectionHandlerTest
//    {
//        [Fact]
//        public void PassingTest() => Xunit.Assert.Equal(4, Add(2, 2));
//        [Fact]
//        public void FailingTest()
//        {
//            Xunit.Assert.Equal(5, Add(2, 2));
//        }

//        int Add(int x, int y)
//        {
//            return x + y;
//        }

//        //[Fact]
//        //public void Adding


//        [Fact]
//        public async void SetDeck_UserOwnsAllCards_NoErrors()
//        {
//            DataBaseConnection db = new DataBaseConnection();
//            int[] deck = new int[5] { 1, 17, 41, 32, 8 }; // DOES OWN & NO DUPLICATES
//            UserProfile user = new UserProfile();
//            user.deck = deck;
//            NpgsqlConnection con = await db.ConnectDB("localhost", "swe1user", "swe1pw", "mtcg_theOriginI");
//            var cmd = new NpgsqlCommand("", con);
//            int[] result = await db.SetDeck(25, user, cmd);
//            Xunit.Assert.Equal(4, result.Length); // BUG 
//        }


//        [Fact] // BUG 
//        public async void GoBattle()
//        {
//            UserProfile userA = new UserProfile() { uid = 25, deck = new int[4] { 10, 12, 44, 2 }, elo = 100, win = 0, loos = 0, draw = 0 };
//            UserProfile userB = new UserProfile() { uid = 22, deck = new int[4] { 1, 6, 11, 16 }, elo = 100, win = 0, loos = 0, draw = 0 }; ;
//            Battle round = new Battle() { userA = userA, userB = userB };
//            BattleLogic battle = new BattleLogic();
//            round = await battle.GoBattle(round); // BUG 
//            bool roundGo = false;
//            if (userA.win > 0 || userA.loos > 0 || userA.draw > 0) roundGo = true;
//            Xunit.Assert.True(roundGo);
//        }


//    }
//}
