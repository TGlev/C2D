using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace C2D.Shared.Models.POST
{
    public class PostTerminateCall
    {
        public string call_id { get; set; }
        public string event_type => "terminated";
        public string time => "2019-04-15T16:07:01.075Z";
    }
}
