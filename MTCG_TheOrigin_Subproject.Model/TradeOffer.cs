using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace MTCG_TheOrigin_SubProject.Model
{
    public class TradeOffer
    {

        public int TradeOfferID { get; set; }
        public int Sender_uid { get; set; }
        public int ReceiverUID { get; set; }

        public int[] I_receive { get; set; }
        
        public int[] U_receive { get; set; }
        
        public string Status { get; set; }



    }
}
