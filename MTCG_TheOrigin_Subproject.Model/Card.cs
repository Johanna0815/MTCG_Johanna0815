using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace MTCG_TheOrigin
{
    public class Card
    {

        public int CId { get; set; }
        public string CardType { get; set; }

        public string CardName { get; set; }
       
        public string ElementType { get;  set; }

        public int Damage { get; set; }

    }
}
