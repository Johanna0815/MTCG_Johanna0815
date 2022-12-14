using MTCG_TheOrigin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTCG_TheOrigin_SubProject.Model
{
    public interface IEffect
    {

       /// <summary>
       /// 
       /// Effect anwenden ! auf MonsterCard.
       /// </summary>
       /// <param name="doIt"></param>
        void Apply(ICard doIt);


        /// <summary>
        /// 
        /// 
        /// </summary>
        /// <param name="doIt"></param>
        void Drop(ICard doIt);

    }
}
