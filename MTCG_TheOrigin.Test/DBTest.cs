//using MTCG_TheOrigin_Subproject.DA;
//using Npgsql;
//using System;
//using System.Collections.Generic;
//using System.Data.Common;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace MTCG_TheOrigin.Test
//{
//    public class DBTest
//    {

//        [Test]
//        [TestCase(TestName = "DBConnection checked", Description = 
//            "DataBaseConection is connected so it is  true")]
//        public void ConnectDB()
//        {
           
//            DataBaseConnection db = new DataBaseConnection();
//           Assert.IsTrue( true);
//        }




//        //[Test]
//        //public async bool CheckIfContainsDuplicates_DeckWithDuplicatesReturnTrue()
//        //{
//        //    DataBaseConnection db = new DataBaseConnection();
//        //    int[] deck = new int[4] { 5, 5, 32, 12 }; // Duplicate inside
//        //    NpgsqlConnection con = await db.ConnectDB("localhost", "swe1user", "swe1pw", "mtcg_theOriginI");//  username checked abchecken.
//        //    var cmd = new NpgsqlCommand("", con);
//        //    bool result = db.CheckForSecond(deck); // dependency BUG!! 
//        //    Assert.True(result);

//        //}


//        //[Test]
//        //public async bool CheckIfUserHasCards_IfUserHasCardsReturnTrue()
//        //{
//        //    DataBaseConnection db = new DataBaseConnection();
//        //    int[] deck = new int[4] { 5, 12, 32, 1 }; // no Duplicate 
//        //    NpgsqlConnection con = await db.ConnectDB("localhost", "swe1user", "swe1pw", "mtcg_theOriginI");
//        //    var cmd = new NpgsqlCommand("", con);


//        //    bool result = await db.ToCheckUserHasCards(deck, 25, cmd);
//        //    Assert.True(result); // isTrue

//        //}


//    }
//}
