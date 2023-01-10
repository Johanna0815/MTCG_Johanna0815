using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace MTCG_TheOrigin_SubProject.Model
{
    [Serializable]
    public class Credentials
    {
        [JsonInclude]
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor.
        public string UserName { get; set; }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. 
        [JsonInclude]
        public string Password { get; set; }
        [JsonInclude]
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor.
        public string AccessToken { get; set; }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor.

    }
}
