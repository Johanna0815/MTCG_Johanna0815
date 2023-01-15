using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using MTCG_TheOrigin_SubProject.Model;
using Npgsql;

namespace MTCG_TheOrigin_Subproject.DA
{
    public class BattleLogic
    {
        public async Task<Battle> GoBattle(Battle battle)
        {
            // FETCH CARDDECK
            // GetCardDeck();
            // SHUFFLE the DECKS
            battle.userA.deck = ShuffleDeck(battle.userA.deck);
            battle.userB.deck = ShuffleDeck(battle.userB.deck);
            // START BATTLE
            battle = await BattleStarted(battle);

            return battle;
        }
        async Task<Battle> BattleStarted(Battle battle)
        {
            // EXTRACT DECKS
            int[] deckA = battle.userA.deck;
            int[] deckB = battle.userB.deck;

            var CD = await GetCardDeck();



            // cast array of ints into List of cards with info of cardType{damage...} ...
            CardDeck deA = new CardDeck();
            CardDeck deB = new CardDeck();
            deA.DeckCards = GetLookAtCardDeck(deckA, CD);
            deB.DeckCards = GetLookAtCardDeck(deckB, CD);

            // go batteln
            int round = 1;
            bool roundGo = true;
           
            while (roundGo)
            {
                if (round <= 100)
                {
                   
                    Console.WriteLine($"Lets start with round: {round}");
                   

                    if (deA.DeckCards.Count > 0 && deB.DeckCards.Count > 0)
                    {
                        int RoundWinner = BattleWithTwoCards(deA.DeckCards[0], deB.DeckCards[0]);


                        // IN CASE OF USERA CARD WINS: ADD DEFEATED CARD TO DECKA AND REMOVE IT FROM DECKB
                        if (RoundWinner == 1)
                        {

                            Console.WriteLine($"{deA.DeckCards[0].card_name} with {deA.DeckCards[0].damage} {deA.DeckCards[0].element_type}-DAMAGE DEFEATS {deB.DeckCards[0].card_name} with {deB.DeckCards[0].damage} {deB.DeckCards[0].element_type}-DAMAGE");
                            Console.WriteLine($"{battle.userA.username} WINS the Round {round}!");

                            deA.DeckCards.Add(deB.DeckCards[0]);
                            deB.DeckCards.RemoveAt(0);

                            Console.WriteLine($"{battle.userA.username} has {deA.DeckCards.Count} Cards in Deck");
                            Console.WriteLine($"{battle.userB.username} has {deB.DeckCards.Count} Cards in Deck");

                        }
                        // IN CASE OF USERB CARD WINS: ADD DEFEATED CARD TO DECKB AND REMOVE IT FROM DECKA
                        if (RoundWinner == 2)
                        {

                            Console.WriteLine($"{deB.DeckCards[0].card_name} with {deB.DeckCards[0].damage} {deB.DeckCards[0].element_type}-DAMAGE DEFEATS {deA.DeckCards[0].card_name} with {deA.DeckCards[0].damage} {deA.DeckCards[0].element_type}-DAMAGE");
                            Console.WriteLine($"{battle.userB.username} WINS the Round {round}!");


                            deB.DeckCards.Add(deA.DeckCards[0]);
                            deA.DeckCards.RemoveAt(0);
                            Console.WriteLine($"{battle.userA.username} has {deA.DeckCards.Count} Cards in Deck");
                            Console.WriteLine($"{battle.userB.username} has {deB.DeckCards.Count} Cards in Deck");

                        }

                        // case: draw
                        if (RoundWinner == 0)
                        {
                           
                            Console.WriteLine($"{deA.DeckCards[0].card_name} with {deA.DeckCards[0].damage} {deA.DeckCards[0].element_type}-DAMAGE equals {deB.DeckCards[0].card_name} with the {deB.DeckCards[0].damage} {deB.DeckCards[0].element_type}-DAMAGE");
                            Console.WriteLine($"{ round} is a Draw!");
                           

                            deA.DeckCards.RemoveAt(0);
                            deB.DeckCards.RemoveAt(0);
                            Console.WriteLine($"{battle.userA.username} has {deA.DeckCards.Count} Cards in Deck");
                            Console.WriteLine($"{battle.userB.username} has {deB.DeckCards.Count} Cards in Deck");
                           
                        }
          
                        round++;
                    }
                    else
                    {
                        DataBaseConnection db = new DataBaseConnection();
                        NpgsqlConnection con = await db.ConnectDB("localhost", "swe1user", "swe1pw", "mtcg_theOriginI");
                        var cmd = new NpgsqlCommand("", con);
                        // case deckA no cards -> userB win
                        if (deA.DeckCards.Count == 0 && deB.DeckCards.Count > 0)
                        {
                           
                            Console.WriteLine($"{battle.userB.username} WINS this GAME in {round} ROUNDS.");
                            battle.userB.win++;
                            battle.userA.loos++;

                            battle.userB.elo += 20;
                            battle.userA.elo -= 15;

                            // ADD 2 coins TO WINNER
                            db.AddCoins(battle.userB.uid, 2, cmd);

                            roundGo = false;
                        }
                        // case deckB no cards -> userA win
                        if (deB.DeckCards.Count == 0 && deA.DeckCards.Count > 0)
                        {
                           
                            Console.WriteLine($"{battle.userA.username} WINS this GAME in {round} ROUNDS.");
                           

                            battle.userA.win++;
                            battle.userB.loos++;

                            battle.userA.elo += 20;
                            battle.userB.elo -= 15;

                            // ADD 2 coins TO WINNER
                            db.AddCoins(battle.userA.uid, 2, cmd);

                            roundGo = false;
                        }
                        // case both no cards
                        if (deA.DeckCards.Count == 0 && deB.DeckCards.Count == 0)
                        {
                            
                            Console.WriteLine($"{battle.userA.username} and {battle.userB.username}  NO Cards left! This Game is a Draw!");
                            
                            battle.userA.draw++;
                            battle.userB.draw++;

                            roundGo = false;
                        }
                    }
                }
                else
                {
                    Console.WriteLine(" Round 100....so it is a DRAW!.");
                    battle.userA.draw++;
                    battle.userB.draw++;
                    roundGo = false;
                }
            }
            return battle;
        }




        public int BattleWithTwoCards(Card cardA, Card cardB)
        {

            // without spell
            if (cardA.card_type == "monster" && cardB.card_type == "monster")
            {
                if (cardA.damage > cardB.damage) return 1;
                if (cardA.damage < cardB.damage) return 2;
                if (cardA.damage == cardB.damage) return 0; // bug im damage
            }
            // with spell
            if (cardA.card_type == "spell" || cardB.card_type == "spell")
            {
                decimal c1 = cardA.damage;
                if (c1 * DaamageCalculator(cardA, cardB) > cardB.damage)
                {
                    return 1;
                }
                if (c1 * DaamageCalculator(cardA, cardB) < cardB.damage)
                {
                    return 2;
                }
                if (c1 * DaamageCalculator(cardA, cardB) == cardB.damage)
                {
                    return 0;
                }
            }
            return 0;
        }
        decimal DaamageCalculator(Card cardA, Card cardB)
        {
            if (cardA.card_type == "spell" || cardB.card_type == "spell")
            {
                switch (cardA.element_type)
                {
                    case "water":
                        switch (cardB.element_type)
                        {
                            case "water":
                                return 1;
                            case "fire":
                                return 2;
                            case "normal":
                                return 0.5m;
                        }
                        break;
                    case "fire":
                        switch (cardB.element_type)
                        {
                            case "water":
                                return 0.5m;
                            case "fire":
                                return 1;
                            case "normal":
                                return 2;
                        }
                        break;
                    case "normal":
                        switch (cardB.element_type)
                        {
                            case "water":
                                return 2;
                            case "fire":
                                return 0.5m;
                            case "normal":
                                return 1;
                        }
                        break;
                    default: // "normal"
                        break;
                }
            }
            return 1;
        }
      
        public async Task<List<Card>> GetCardDeck()
        {
            CardDeck.AllCards.Clear();
            DataBaseConnection db = new DataBaseConnection();
            NpgsqlConnection con = await db.ConnectDB("localhost", "swe1user", "swe1pw", "mtcg_theOriginI");
            var cmd = new NpgsqlCommand("", con);
            cmd.CommandText = "SELECT * FROM carddeck";
            await using (var reader = await cmd.ExecuteReaderAsync())
                while (await reader.ReadAsync())
                {
                    Card card = new Card();
                    card.cid = (int)reader["cid"];
                    card.card_type = (string)reader["card_type"];
                    card.card_name = (string)reader["card_name"];
                    card.element_type = (string)reader["element_type"];
                    card.damage = (int)reader["damage"]; // no cast.
                    CardDeck.AllCards.Add(card);
                }
            return CardDeck.AllCards;
        }
        public List<Card> GetLookAtCardDeck(int[] deck, List<Card> CD)
        {
            CardDeck cd = new CardDeck();
            foreach (int cid in deck)
            {
                foreach (var card in CD)
                {
                    if (card.cid == cid) cd.DeckCards.Add(card);
                }
            }
            return cd.DeckCards;
        }




        int[] ShuffleDeck(int[] deck)
        {
            Random random = new Random();
            deck = deck.OrderBy(x => random.Next()).ToArray();
            return deck;
        }

    }
}
