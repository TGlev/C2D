﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace C2D.Shared.Models.Responses
{
    public class AuthResponse
    {
        public string access_token { get; set; }
        public string refresh_token { get; set; }
        public long expires_in { get; set; }
        public string scope { get; set; }
        public string token_type { get; set; }
    }
}
