using MTCG_TheOrigin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTCG_TheOrigin_SubProject.Model
{
    public class UserCardStack : ICard, IUser
    {
        public string Name { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public string Password { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public string CardName { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public int Damage { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public string ElementType { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public string CardManager()
        {
            throw new NotImplementedException();
        }

        public bool IsRegistered()
        {
            throw new NotImplementedException();
        }

        public string ToRegister()
        {
            throw new NotImplementedException();
        }

        // a User has  multiple cards in his stack
        public UserCardStack(Card[] cardList, string user)
        {
            //Card[] CardStack = new UserCardStack[];
            //CardStack[0] = new UserCardStack[] { Goblin, Dragon };




            throw new NotImplementedException();
        }

    }
}
