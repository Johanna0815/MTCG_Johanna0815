using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace MTCG_TheOrigin_SubProject.Model
{
    public class ResponseJson
    {
        [JsonInclude]
        public string? MSG { get; set; }
        [JsonInclude]
        public bool Success { get; set;}
        [JsonInclude]
        public int UID { get; set; }
        [JsonInclude]
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. 
        public string AccessToken { get; set; }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. 

        //  public int status { get; set; }

    }
}
