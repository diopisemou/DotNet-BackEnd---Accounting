using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BackEnd.Models {
    public class FileManagerModel {
        public string FilePath { get; set; }
        public IFormFile file { get; set; }
    }
}
