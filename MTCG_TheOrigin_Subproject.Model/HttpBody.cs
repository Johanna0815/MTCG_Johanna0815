using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json.Serialization;

namespace MTCG_TheOrigin_SubProject.Model
{
    public class HttpBody
    {
        [JsonInclude]
        public int[] deck { get; set; }
        [JsonInclude]
        public int tradeoffer_id { get; set; }
        [JsonInclude]
        public int receiver_uid { get; set; }
        [JsonInclude]
        public int[] a_receive { get; set; }
        [JsonInclude]
        public int[] b_receive { get; set; }
        [JsonInclude]
        public string action { get; set; }
    }
}
