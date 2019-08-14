using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace C2D.Shared.Models.POST
{
    public class POSTGetContacts
    {
        public string client_id { set; get; }
        public string access_token { set; get; }
        public string action { get; set; }

        public POSTGetContacts(string client_id, string access_token)
        {
            this.client_id = client_id;
            this.access_token = access_token;
            action = "read";
        }
    }
}
