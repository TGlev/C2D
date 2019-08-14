using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace C2D.Shared.Models.POST
{
    public class PostCallExtra : POSTCall
    {
        public string call_id { get; set; }

        public PostCallExtra(string client_id, string access_token, string action, string extension, string call_id) : base(client_id, access_token, action, extension)
        {
            this.call_id = call_id;
        }
    }
}
