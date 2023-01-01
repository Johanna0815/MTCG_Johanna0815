using MTCG_TheOrigin_SubProject.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTCG_TheOrigin_SubProject.BL
{
    public class BattleLogic
    {

        /// <summary>
        /// to get card for each user
        /// to shuffle decks. 
        /// </summary>
        /// <param name="battle"></param>
        /// <returns></returns>
        public async Task<Battle> StartBattle(Battle battle)
        {

           
            battle.userA.Deck = ShuffleDeck(battle.userA.Deck);
            battle.userB.Deck = ShuffleDeck(battle.userB.Deck   );

            battle = await BattleGo(battle);

            return battle;
        }

        

        async Task<Battle> BattleGo(Battle battle)
        {

            int[]   deckA = battle.userA.Deck;
            int[]   deckB = battle.userB.Deck;

            var cardDeck = await GetCardDeck();
            // cast array 

        }


        public async Task<List<CardDeck>> GetCardDeck()
        {
            CardDeck.AllCards.Clear(); // keine mehr
                                       // DatabaseGeneratedOption
            return CardDeck;
        }





        int[] ShuffleDeck(int[] deck)
        {
            Random random = new Random();
            deck = deck.OrderBy(x => x).ToArray();   // pivot aufs next ?.
            return deck;
        }


    }
}
