using MTCG_TheOrigin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTCG_TheOrigin_SubProject.Model
{
    public class SpellCard // : ICard, ISpellCard
    {
        public int Damage { get; set; }
        public DamageType Type { get; set; }  
      //  public IEnumerable<IESpeciality> Specialities { get; set; }
        //public IPlayerLog Log { get; set; }


       // public IEnumerable<Card> Cards { get; set;}

        //public SpellCard(int damage, DamageType damageType, IEnumerable<ISpeciality> specialities, IPlayerLog log) {
        //    Damage= damage;
        //    Type= damageType;
        //    Specialities = specialities;
        //    Log = log;
        //}


        public override string ToString()
        {
            return $"{Type.GetString()} Spell";
        }

    }
}
