using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTCG_TheOrigin_SubProject.Model
{
    public class TradeOffer
    {
        public int tradeoffer_id { get; set; }
        public int sender_uid { get; set; }
        public int receiver_uid { get; set; }
        public int[] a_receive { get; set; }
        public int[] b_receive { get; set; }
        public string status { get; set; }
    }
}
