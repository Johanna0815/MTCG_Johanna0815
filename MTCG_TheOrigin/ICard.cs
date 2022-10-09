using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTCG_TheOrigin
{
    public interface ICard
    {

        public string Name { get; set; }

        // die liste noch abändern. // statt card doch lieber string absoecihern ? Card()
        var amountOfCards = new List<Card> 
        {
           
        };

        // a user can manage his cards // Methode, dass user cards aufnehemn, mehr und weniger machen kann ?
        public void CardManager(User userManager, Card userCardManager)
        {
            
       

        }




    }
}
