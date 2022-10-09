using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTCG_TheOrigin
{
    public class User
    {
        // fields nur in der Klasse ? 
        public string Name;
        private string _password; // mal sehen, ob es noch public muss sein. 

        // public string Password { get; set; }

        
        // private dann wenn es ins userprofil kommt, der wo bearbietet dort. 
        public void SetPassword( string password)
        {
            this.Password = password;
            if (Password == null || Password.Length < 4)
           // this.Password = password;
        }

        public string GetPassword()
        {
            return this.Password; // password
        }

        public void DisplayInfoTest(string password)
        {
            Console.WriteLine("The password is {0}", password);
        }

    }
}
