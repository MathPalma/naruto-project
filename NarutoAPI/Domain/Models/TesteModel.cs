using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Models
{
    public class TesteModel
    {
        public string ImageName { get; set; }
        public IFormFile ImageFile { get; set; }
    }
}
