using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTCG_TheOrigin
{
    public interface IUser
    {
        
        // unique Username
        public string Name { get; set; }
        public string Password { get; set; }




        public bool IsRegistered();

        public string ToRegister();


        //to manage his(user) cards/ trade or whatever. [DataType Card]
        public string CardManager();
        

    }
}
