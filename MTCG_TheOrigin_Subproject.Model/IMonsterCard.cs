using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTCG_TheOrigin_SubProject.Model
{
    public interface IMonsterCard
    {


        MonsterType monsterType { get; }
        IEnumerable<IEffect> Effects { get; }






    }

    
}
