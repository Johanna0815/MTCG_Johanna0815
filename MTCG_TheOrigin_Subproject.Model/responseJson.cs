﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace MTCG_TheOrigin_SubProject.Model
{
    public class responseJson
    {
        [JsonInclude]
        public string msg { get; set; }
        [JsonInclude]
        public bool success { get; set;}
        [JsonInclude]
        public int uid { get; set; }
        [JsonInclude]
        public string accessToken { get; set; }

      //  public int status { get; set; }

    }
}