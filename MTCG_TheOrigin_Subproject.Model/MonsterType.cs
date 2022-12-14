using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace MTCG_TheOrigin_SubProject.Model
{

    // all possible MonsterTypes
    public enum MonsterType
    {
        Goblin,
        Dragon,
        Wizzard,
        Ork,
        Knight,
       // WaterSpell,
        Kraken,
        FireElve
        
    }

    static class MonsterTypeMethods
    {
        // mit parameter als = mt
        public static string GetString(this MonsterType mt)
        {
            var name = mt.ToString();

            return name;
        }

        public static string GetDefaultDamageType(this MonsterType mt)
        {
            string damageType;
            switch (mt)
            {

                case MonsterType.Goblin:
                //  case MonsterType.WaterSpell:
                case MonsterType.Dragon:
                case MonsterType.Ork:
                case MonsterType.FireElve:
                case MonsterType.Wizzard:
                case MonsterType.Knight:
                case MonsterType.Kraken:
                    damageType = DamageType.Normal.ToString();
                    break;
                default:
                    damageType = DamageType.Normal.ToString();
                    break;
            }
            return damageType;

          



            }

        public static MonsterType? GetType(string type)
        {
            if (Enum.TryParse(type, out MonsterType enumType)) return enumType;
            return null;
        }
     }

 }

