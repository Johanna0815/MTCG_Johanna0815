using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTCG_TheOrigin_SubProject.Model
{
    public class UserProfile
    {
        public int UID { get; set; } = 0;
        public string UserName { get; set; }
        // public string Description { get; set; }
        public int Elo { get; set; } = 100;
        public int[] Deck { get; set;} = new int[0];
        public int Win { get; set; } = 0;
        public int Loos { get; set; } = 0;
        public int Draw { get; set; } = 0;

    }
}
