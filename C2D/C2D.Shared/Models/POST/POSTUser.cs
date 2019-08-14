using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace C2D.Shared.Models.POST
{
    [JsonObject(MemberSerialization.OptIn)]
    public class POSTUser : User
    {
        [JsonProperty]
        public string username
        {
            get
            {
                return Username;
            }
            set
            {
                Username = value;
            }
       }

        [JsonProperty]
        public string password
        {
            get
            {
                return Password;
            }
            set
            {
                Password = value;
            }
        }

        [JsonProperty]
        public string client_id
        {
            get
            {
                return ClientID;
            }
            set
            {
                ClientID = value;
            }
        }

        [JsonProperty]
        public string client_secret
        {
            get
            {
                return ClientSecret;
            }
            set
            {
                ClientSecret = value;
            }
        }

        [JsonProperty]
        public string extension
        {
            get
            {
                return Extension;
            }
            set
            {
                Extension = value;
            }
        }

        [JsonProperty]
        public string redirect_uri => "https://api.windoos.net/c2d/auth";
    }
}
