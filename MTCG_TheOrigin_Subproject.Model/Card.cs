﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTCG_TheOrigin_SubProject.Model
{
    public class Card
    {
        public int cid { get; set; }
        public string card_type { get; set; }
        public string card_name { get; set; }
        public string element_type { get; set; }
        public int damage { get; set; }
    }
}
