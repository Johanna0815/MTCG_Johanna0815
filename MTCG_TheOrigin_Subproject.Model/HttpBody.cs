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
        public int[]? Deck { get; set; }
        [JsonInclude]
        public int TradeofferID { get;set; }
        [JsonInclude]
        public int ReceiverUID { get; set; }
        [JsonInclude]
        public int[]? A_receive { get; set; }
        [JsonInclude]
        public int[]? B_receive { get; set; }
        [JsonInclude]
        public string? Action { get; set; }

    }
}
