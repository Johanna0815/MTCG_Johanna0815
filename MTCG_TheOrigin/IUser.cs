using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTCG_TheOrigin
{
    // wegen dem anderen Projekt einbinden eher public!
    public interface IUser
    {

       // private string Name;
        // public string Password;

        public string Name { get; set; }
        public string Password { get; set; }
        
        /*
        public string name { 
            get { return this.Name;}
            set { this.Name = value; }
        }


        public string password
        {
            get { return this.Password; }
            set { this.Password = value; }
        }

        */ 

    }
}
