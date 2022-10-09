using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTCG_TheOrigin
{
    public class Card
    {

        // abchecken, dass mind 5 auf der hand sind ?
        public void SetCardpieces(int CardPieces)
        {
            if (MinLengthAttribute < 6)
            {
                throw new Exception("Length should be 5!");

            }
            this.SetCardpieces = CardPieces;
        }


    }
}
