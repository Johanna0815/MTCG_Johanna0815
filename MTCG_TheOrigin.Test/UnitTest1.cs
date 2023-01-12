//using Moq;
//using MTCG_TheOrigin_Subproject.DA;
//using MTCG_TheOrigin_SubProject.DA;
//using Npgsql;



//namespace MTCG_TheOrigin.Test
//{
//    public class Tests
//    {
//        [SetUp]
//        public void Setup()
//        {

//        }


//        //man möchte fragen, ob man ein Objekt aus der Klasse (Battle) machen kann. 
//        [Test]
//        public void Battle_Exist()
//        {
//            var battle = new BattleLogic();

//            Assert.IsNotNull(battle);
//        }



//        //[Test]
//        //public async void SyncUser()
//        //{
//        //    DataBaseConnection db = new DataBaseConnection();
//        //    Assert.Equals(10, user.UID);
//        //}
//        [Test] // DB-Tests in eigene unit. 
//        public async bool CheckIfContainsDuplicates_DeckWithDuplicatesReturnTrue()
//        {
//            DataBaseConnection db = new DataBaseConnection();
//            int[] deck = new int[4] { 5, 5, 32, 12 }; // Duplicate inside
//            NpgsqlConnection con = await db.ConnectDB("localhost", "swe1user", "swe1pw", "mtcg_theorigin");//  username checked abchecken.
//            var cmd = new NpgsqlCommand("", con);
//            bool result = db.CheckForSecond(deck); // dependency BUG!! 
//            Assert.True(result);
        
//        }

//        [Test]
//        public async bool CheckIfUserHasCards_IfUserHasCardsReturnTrue()
//        {
//            DataBaseConnection db = new DataBaseConnection();
//            int[] deck = new int[4] { 5, 12, 32, 1 }; // no Duplicate 
//            NpgsqlConnection con = await db.ConnectDB("localhost", "swe1user", "swe1pw", "mtcg_theorigin");
//            var cmd = new NpgsqlCommand("", con);


//            bool result = await db.ToCheckUserHasCards( deck, 25, cmd);
//            Assert.True(result); // isTrue

//        }




//        //[Test] // mit der queue abtesten! 
//        //public void Battle_HasOnePlayer()
//        //{
//        //    var battle = new BattleLogic();
//        //    battle.userA = new Mock<User>().Object; //mit moq mit .Object bekommt man das Objekt dann. 

//        //    Assert.IsNotNull(battle.PlayerOne);

//        //    // bei objekten immer areSame verwenden, das eine bezieht sich aufs referenz und das andere aufs objekt
//        //}
//    }

  
//}