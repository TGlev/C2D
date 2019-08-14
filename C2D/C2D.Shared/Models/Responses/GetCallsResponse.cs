using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace C2D.Shared.Models.Responses
{
    public class GetCallsResponse
    {
        public string call_id { get; set; }
        public string client_id { get; set; }
        public From from { get; set; }
        public To to { get; set; }
        public string direction { get; set; }
        public string duration { get; set; }
        public string status { get; set; }

        public Call convert()
        {
            Call call = new Call
            {
                CallId = call_id,
                ClientId = client_id,
                ExtensionNumber = from.extension_number,
                ExtensionId = from.extension_id,
                CallerId = from.caller_id,
                ToPhonebookId = to.phonebook_id,
                ToNumber = to.number,
                Direction = direction,
                Duration = duration,
                Status = status
            };
            return call;
        }
    }

    public class From
    {
        public string extension_number { get; set; }
        public string extension_id { get; set; }
        public string caller_id { get; set; }
    }

    public class To
    {
        public string phonebook_id { get; set; }
        public string number { get; set; }
    }
}
