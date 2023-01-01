using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace MTCG_TheOrigin
{
    public class Card //: ICard
    {

        public int CId { get; set; }
        public string CardType { get; set; }

        public string CardName { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
       // public int Damage { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public string ElementType { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public int Damage { get; set; }


        // Damage of a card is constant and does not change!!
        //public Card()
        //{
        //    Damage= 0;
        //}

        //public Card(string CardName, string element)
        //{

        //    this.CardName = CardName; 
        //    this.ElementType = element;
        //   // this.Damage = damageOfCard;


        //}


        // abchecken, dass mind 5 auf der hand sind ?
        /*
        public void SetCardpieces(int CardPieces)
        {
            if (MinLengthAttribute < 6)
            {
                throw new Exception("Length should be 5!");

            }
            this.SetCardpieces = CardPieces;
        }

        */




    }
}
