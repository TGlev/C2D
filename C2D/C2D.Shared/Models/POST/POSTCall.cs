using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace C2D.Shared.Models.POST
{
    public class POSTCall : POSTBase
    {
        public string client_id { get; set; }
        public string access_token { get; set; }
        public string action { get; set; }
        public string extension { get; set; }
        public PostCallTo to { get; set; }

        public POSTCall(string client_id, string access_token, string action, string extension)
        {
            this.client_id = client_id;
            this.access_token = access_token;
            this.action = action;
            this.extension = extension;
            to = new PostCallTo();
        }
    }
}
