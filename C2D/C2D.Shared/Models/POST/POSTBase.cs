using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace C2D.Shared.Models.POST
{
    public interface POSTBase
    {
        string client_id { get; set; }
        string access_token { get; set; }
        string action { get; set; }
    }
}
