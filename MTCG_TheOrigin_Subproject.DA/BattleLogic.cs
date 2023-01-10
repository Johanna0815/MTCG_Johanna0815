using MTCG_TheOrigin;
using MTCG_TheOrigin_Subproject.DA;
using MTCG_TheOrigin_SubProject.Model;
using Npgsql;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
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
            // cast array of ints into List of cards with info of cardType{damage...}
            CardDeck deA = new CardDeck();
            CardDeck deB = new CardDeck();

            deA.DeckCards = GetLookAtCardDeck(deckA, cardDeck); // BUG is not null ?
            deB.DeckCards = GetLookAtCardDeck(deckB, cardDeck);

            // if player has cards GOBattle
            int round = 0;
            bool goBattle = true;
           // Console.WriteLine(deckB);            
            while(goBattle )
            {
                if (round <= 100)
                {
                    Console.WriteLine();

                    

                    int WinnerOfRound = (int)BattleWithTwoCards(deA.DeckCards[0], deB.DeckCards[0]);

                    // draw
                    if (WinnerOfRound == 0)
                    {
                        Console.WriteLine($"{deA.DeckCards[0].CardName} WITH {deA.DeckCards[0].Damage} {deA.DeckCards[0].ElementType}-DAMAGE EQUALS {deB.DeckCards[0].CardName} WITH {deB.DeckCards[0].Damage} {deB.DeckCards[0].ElementType}-DAMAGE");
                        Console.WriteLine("It´s a DRAW.");


                        deA.DeckCards.RemoveAt(0);
                        deB.DeckCards.RemoveAt(0);

                        // deckkarten anziegen danach !
                        Console.WriteLine($"{battle.userA.UserName} HAS {deA.DeckCards.Count} CARDS IN DECK");
                        Console.WriteLine($"{battle.userB.UserName} HAS {deB.DeckCards.Count} Cards IN DECK");

                    }
                    // win
                    // case userA wins. Remove card from deckB
                    if (WinnerOfRound == 1)
                    {

                        Console.WriteLine($"{deA.DeckCards[0].CardName} WITH {deA.DeckCards[0].Damage} {deA.DeckCards[0].ElementType}-DAMAGE DESTROYED {deB.DeckCards[0].CardName} WITH {deB.DeckCards[0].Damage} {deB.DeckCards[0].ElementType}-DAMAGE");
                        Console.WriteLine($"{battle.userA.UserName} Winner of the ROUND.");

                        deA.DeckCards.Add(deB.DeckCards[0]);
                        deB.DeckCards.RemoveAt(0);

                        // deckkarten anziegen danach !
                        Console.WriteLine($"{battle.userA.UserName} HAS {deA.DeckCards.Count} CARDS IN DECK");
                        Console.WriteLine($"{battle.userB.UserName} HAS {deB.DeckCards.Count} Cards IN DECK");
                    }
                     // win
                    // case UserB wins. Remove card from DeckA
                    if (WinnerOfRound == 2)
                    {

                        Console.WriteLine($"{deB.DeckCards[0].CardName} WITH {deB.DeckCards[0].Damage} {deB.DeckCards[0].ElementType}-DAMAGE DESTROYED {deA.DeckCards[0].CardName} WITH {deA.DeckCards[0].Damage} {deA.DeckCards[0].ElementType}-DAMAGE");
                        Console.WriteLine($"{battle.userB.UserName} Winner of the ROUND.");


                        // deckkarten anziegen danach !
                        Console.WriteLine($"{battle.userA.UserName} HAS {deA.DeckCards.Count} CARDS IN DECK");
                        Console.WriteLine($"{battle.userB.UserName} HAS {deB.DeckCards.Count} Cards IN DECK");
                    }


                  
                    // next rounds. 
                    round++;

                }

                // loos
                // TODO 

            }
            return battle;
        }



        // calculate damage 
        // ElementType


        // multiplicates the damage
        public decimal DaamageCalculator(Card cardA, Card cardB)
        {
            if (cardA.CardType == "spell" || cardB.CardType == "spell")
            {
                switch(cardA.ElementType)
                {
                    case "water":
                        switch(cardB.ElementType) 
                        {
                            case "water":
                                return 1;
                            case "normal":
                                return 0.5m;
                            case "fire":
                                return 2;
                        }
                        break;
                   // default: 
                    // fire
                    case "fire":  
                      switch (cardB.ElementType)
                        {
                            case "water":
                                return 0.5m;
                            case "normal":
                                return 1;
                            case "fire":
                                return 2;
                        }
                        break;
                    // normal
                    case "normal":
                        switch(cardB.ElementType)
                        {
                            case "water":
                                return 2;
                            case "normal":
                                return 1;
                            case "fire":
                                return 0.5m;
                           
                        }
                        break;
                       // EditAndContinueLogEntry;
                  default:
                      //  Console.WriteLine(Card);
                        break;


                }
            }
            return 1;
        }

        public decimal BattleWithTwoCards(Card cardA, Card cardB)
        {

            // can be monster
            // can be spellCard.
            // The element type does not effect pure monster fights.

            // in this case monster must be involved. // and no monster and speLL?// elementType no effect
            if (cardA.CardType == "monster" && cardB.CardType == "monster")
            {
                // Card.Damage : cardA.Damage > cardB.Damage return 1 ? ReturnTypeEncoder 2;
                   if(cardA.Damage > cardB.Damage) return 1;
                if (cardA.Damage < cardB.Damage) return 2;
                if(cardA.Damage == cardB.Damage) return 3; // cardA not null here BUG. // bug im damage


                // 
            }
            // in this case Spell is involved.
            if (cardA.CardType == "spell" || cardB.CardType == "spell" )
            {
                decimal caA = cardA.Damage;
                if (caA * DaamageCalculator(cardA, cardB) > cardB.Damage)
                {

                    return 1;
                }
                if (caA * DaamageCalculator(cardA, cardB) <  cardB.Damage)
                {
                    return 2;
                }
                if (caA * DaamageCalculator(cardA, cardB) == cardB.Damage)
                {
                    return 0;
                }


            }


            return 0m;

        }

        public List<Card> GetLookAtCardDeck(int[] deck, List<Card> CD )
        {
            CardDeck cd = new CardDeck();
            foreach(int CId in deck)
            {
                foreach(var card in CD)
                {
                    if(card.CId == CId) cd.DeckCards.Add(card);
                }
            }
            return cd.DeckCards;
        }


        public async Task<List<Card>> GetCardDeck()
        {
            CardDeck.AllCards.Clear(); // keine mehr
                                       // DatabaseGeneratedOption


            DataBaseConnection db = new DataBaseConnection();
            NpgsqlConnection con = await db.ConnectDB("localhost", "swe1db", "pw1db", "db"); // db erst auf table !!!
            var cmd = new NpgsqlCommand("", con);
            cmd.CommandText = "SELECT * FROM CardDeck";
            await using(var reader = await cmd.ExecuteReaderAsync())
                while(await reader.ReadAsync())
                {
                    Card card = new Card();
                    card.CId = (int)reader["CId"];
                    //card.CId = (int)reader["CId"],
                    card.CardType = (string)reader["CardType"];
                    card.CardName = (string)reader["CardName"];
                    card.ElementType = (string)reader["ElementType"];
                    card.Damage = (int)reader["Damage"]; // auf decimal wechseln BUG
                    CardDeck.AllCards.Add(card);
                                      
                }
                return CardDeck.AllCards;
         }


        





        int[] ShuffleDeck(int[] deck)
        {
            Random random = new Random();
            deck = deck.OrderBy(x => x).ToArray();   // pivot aufs next ?.
            return deck;
        }


    }
}
