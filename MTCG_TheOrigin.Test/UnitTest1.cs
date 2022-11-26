using Moq;

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
            var battle = new BattleTest();

            Assert.IsNotNull(battle);
        }


        [Test]
        public void Battle_HasOnePlayer()
        {
            var battle = new Battle();
            battle.PlayerOne = new Mock<Player>().Object; //mit moq mit .Object bekommt man das Objekt dann. 

            Assert.IsNotNull(battle.PlayerOne);

            // bei objekten immer areSame verwenden, das eine bezieht sich aufs referenz und das andere aufs objekt
        }
    }

    internal class Player
    {

    }



    internal class Battle
    {
        public Battle()
        {




        }
    }
}