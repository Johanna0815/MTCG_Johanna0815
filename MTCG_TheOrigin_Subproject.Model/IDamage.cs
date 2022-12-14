using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTCG_TheOrigin_SubProject.Model
{
    public interface IDamage
    {

        bool IsDamaged
        {

            get;
        }

        int Value { get; }



        /// <summary>
        /// 
        /// 
        /// </summary>
        /// <param name="addition"></param>
        // add damage to the damage.
        void Add(int addition);

        /// <summary>
        /// 
        /// base damage reboot // für neues spiel ? 
        /// </summary>
        void SetNoDamage();
    }
}
