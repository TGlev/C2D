using C2D.Shared.Models.POST;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace C2D.Shared.Models.Responses
{
    public class GetContactsResponse
    {
        public List<POSTContact> records { get; set; }
        public List<Contact> parsedRecords { get; set; }

        public void Convert()
        {
            parsedRecords = new List<Contact>();
            foreach(var i in records)
            {
                parsedRecords.Add(i);
            }
        }
    }
}
