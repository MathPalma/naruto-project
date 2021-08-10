using Microsoft.AspNetCore.Http;

namespace Domain.Models
{
    public class NinjaModel
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Rank { get; set; }
        public string Village { get; set; }
        public bool Renegade { get; set; }
        public IFormFile ImageFile { get; set; }
        public string ImageName { get; set; }
        public string ImageSrc { get; set; }
    }
}
