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

        public int TradeofferID { get; set; }
        public int SenderUID { get; set; }
        public int ReceiverUID { get; set; }

        public int[] A_receive { get; set; }
        
        public int[] B_receive { get; set; }
        
        public string Status { get; set; }



    }
}
