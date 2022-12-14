using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTCG_TheOrigin
{
    public class User //: IUser
    {
        
        public string _Name { get; set; }
        


        private string _Password { get; set; } // public ?


       

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
     



       
       



    
        //public void SetPassword( string password)
        //{
        //    this.Password = password;
        //    if (Password == null || Password.Length < 4)
        //    {
        //    throw new ArgumentException();
        //    }
        //    this.Password = password;
        //}

        

        // just the person whoms pw. 
        //public string GetPassword()
        //{
        //    return this.Password; // password
        //}

        public void DisplayInfoTest(string password)
        {
            Console.WriteLine("The password is {0}", password);
        }
        
        
    }
}
