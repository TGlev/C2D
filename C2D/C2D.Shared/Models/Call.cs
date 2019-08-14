using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace C2D.Shared.Models
{
    public class Call
    {
        public string CallId { get; set; }
        public string ClientId { get; set; }
        public string ExtensionNumber { get; set; }
        public string ExtensionId { get; set; }
        public string CallerId { get; set; }
        public string ToPhonebookId { get; set; }
        public string ToNumber { get; set; }
        public string Direction { get; set; }
        public string Duration { get; set; }
        public string Status { get; set; }
    }
}
