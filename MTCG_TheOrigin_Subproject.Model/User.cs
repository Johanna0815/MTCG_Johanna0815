using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTCG_TheOrigin
{
    public class User //: IUser
    {
        
        public string Username { get; set; }
        


        public string Password { get; set; } // public ?


        public User(string username, string password)
        {
            Username = username;
            Password = password;
        }

       

       public bool IsRegistered()
        {
            throw new NotImplementedException();
        }

        public string ToRegister()
        {
            throw new NotImplementedException();
        }

        public string CardManager()
        {
            throw new NotImplementedException();
        }
     




        public void DisplayInfoTest(string password)
        {
            Console.WriteLine("The password is {0}", password);
        }
        
        
    }
}
