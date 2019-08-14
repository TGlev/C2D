using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace C2D.Shared.Models
{
    public class User
    {
        [PrimaryKey]
        public int Id { get; set; }

        public string ClientID { get; set; }
        public string ClientSecret { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Extension { get; set; }
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
    }
}
