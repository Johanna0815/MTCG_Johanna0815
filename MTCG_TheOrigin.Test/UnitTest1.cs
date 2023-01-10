using Moq;
using MTCG_TheOrigin_Subproject.DA;
using Npgsql;

namespace MTCG_TheOrigin.Test
{
    public class Tests
    {
        [SetUp]
        public void Setup()
        {
        }


        //man möchte fragen, ob man ein Objekt aus der Klasse (Battle) machen kann. 
        [Test]
        public void Battle_Exist()
        {
            var battle = new Battle();

            Assert.IsNotNull(battle);
        }



        //[Test]
        //public async void SyncUser()
        //{
        //    DataBaseConnection db = new DataBaseConnection();
        //    Assert.Equals(10, user.UID);
        //}
        [Test] // DB-Tests in eigene unit. 
        public async void CheckIfContainsDuplicates_DeckWithDuplicatesReturnTrue()
        {
            DataBaseConnection db = new DataBaseConnection();
            int[] deck = new int[4] { 5, 5, 32, 12 }; // Duplicate inside
            NpgsqlConnection con = await db.ConnectDB("localhost", "swe1db", "swe1pw", "simpledatastore");// username swe1db oder postgres abchecken.
            var cmd = new NpgsqlCommand("", con);
            bool result = db.CheckForSecond(deck); // dependency BUG!! 
            Assert.True(result);
        
        }

        [Test]
        public async void CheckIfUserHasCards_IfUserHasCardsReturnTrue()
        {
            DataBaseConnection db = new DataBaseConnection();
            int[] deck = new int[4] { 5, 12, 32, 1 }; // no Duplicate 
            NpgsqlConnection con = await db.ConnectDB("localhost", "swe1db", "swe1pw", "simpledatastore");
            var cmd = new NpgsqlCommand("", con);


            bool result = await db.ToCheckUserHasCards(25, deck, cmd);
            Assert.True(result); // isTrue

        }




        [Test] // mit der queue abtesten! 
        public void Battle_HasOnePlayer()
        {
            var battle = new BattleLogic();
            battle.userA = new Mock<User>().Object; //mit moq mit .Object bekommt man das Objekt dann. 

            Assert.IsNotNull(battle.PlayerOne);

            // bei objekten immer areSame verwenden, das eine bezieht sich aufs referenz und das andere aufs objekt
        }
    }

  
}