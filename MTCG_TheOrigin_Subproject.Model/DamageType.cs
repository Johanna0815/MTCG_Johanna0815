using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace MTCG_TheOrigin_SubProject.Model
{
    

        public enum DamageType
        {
            Water,
            Fire,
            Normal

        }


    static class DamageTypeMethods
    {

        // für den parameter dann = dt
        public static string GetString(this DamageType dt)
        {
            var name = dt.ToString();
            if (name == "Normal") name = "Regular";
            return name;
        }


        public static DamageType? GetType(string type)
        {
            // if (type == null) return null;
            if (type == "Regular") return DamageType.Normal;
            if (Enum.TryParse(type, out DamageType enumType)) return enumType;
            return null;
        }



    }
      

    
}
