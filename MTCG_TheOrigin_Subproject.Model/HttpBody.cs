using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace MTCG_TheOrigin_SubProject.Model
{
    public class HttpBody
    {
        [JsonInclude]
        public int[] Deck { get; set; }
        [JsonInclude]
        public int TradeofferID { get;set; }
        [JsonInclude]
        public int ReceiverUID { get; set; }
        [JsonInclude]
        public int[] I_receive { get; set; }
        // public int[] i_transmit { get;} 
        [JsonInclude]
        public int[] U_receive { get; set; }
        [JsonInclude]
        public string Action { get; set; }

    }
}
