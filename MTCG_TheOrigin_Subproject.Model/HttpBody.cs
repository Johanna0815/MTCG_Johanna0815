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
        public int[] deck { get; set; }
        [JsonInclude]
        public int tradeofferID { get;set; }
        [JsonInclude]
        public int receiverUID { get; set; }
        [JsonInclude]
        public int[] i_receive { get; set; }
        // public int[] i_transmit { get;} 
        [JsonInclude]
        public int[] u_receive { get; set; }
        [JsonInclude]
        public string action { get; set; }

    }
}
