using MTCG_TheOrigin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTCG_TheOrigin_SubProject.Model
{
    public class Damage : IDamage
    {



        // the damage of a card is constant and does not change
        //const int DamageOfCard = Damage.Equals(); // equals mit db abgliechen ?


        public bool IsDamaged;

        public int Value;

        bool IDamage.IsDamaged => throw new NotImplementedException();

        int IDamage.Value => throw new NotImplementedException();

        public Damage(bool isDamaged, int value) { 
            this.IsDamaged= isDamaged;
            this.Value= value;
        }


        //public Damage(Damage actualDamage) {
        //    DamageOfCard = actualDamage;

        //}

       
        public void Add(int addition)
        {

            Value += addition;
           // throw new NotImplementedException();
        }

        public void SetNoDamage()
        {
            if (IsDamaged)
            IsDamaged = false;
            Value = 0;
           // throw new NotImplementedException();
        }


        public int CompareWithOtherCard(IDamage? other)
        {
            // oder 1 true zurück?
            if (other == null) return 0;
            return -1;
        }

    }
}
