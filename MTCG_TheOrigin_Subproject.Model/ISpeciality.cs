using MTCG_TheOrigin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTCG_TheOrigin_SubProject.Model
{
    internal interface ISpeciality
    {

        void Apply(ICard doIt, ICard other, IDamage damage);
    }
}
