using MTCG_TheOrigin_SubProject.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTCG_TheOrigin
{
    public interface ICard
    {

        public string CardName { get; set; }

       // public int Damage { get; set; }    
        public string ElementType { get; set; }

        int Damage { get; set; }
        DamageType Type { get; }



        ////IENumerable<> out 
        //IEnumerable<ISpeciality> out Specialities { get; }
        //// IPlayerLog Log { get; set; }

        //IDamage CalculateDamage(ICard doit)
        //{
        //    // der player muss sich erst noch einloggen.
        //    //Log.AddNewCardLog(this.ToString()!);
            
        //}



       




    }
}
