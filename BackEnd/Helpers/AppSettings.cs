﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BackEnd.Helpers {
    public class AppSettings {
        public string Secret { get; set; }
        public string JwtIssuer { get; set; }
    }
}
