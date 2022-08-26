using Microsoft.AspNetCore.Http;

namespace Domain.Models
{
    public class NinjaModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Rank { get; set; }
        public string Village { get; set; }
        public bool Renegade { get; set; }
        public IFormFile ImageFile { get; set; }
        public string ImageName { get; set; }
        public string ImageSrc { get; set; }
        public NinjaModel() { }
        public NinjaModel(int id, string name, string rank, string village, bool renegade, IFormFile imageFile)
        {
            Id = id;
            Name = name;
            Rank = rank;
            Village = village;
            Renegade = renegade;
            ImageFile = imageFile;
        }
        public NinjaModel(int id, string name, string rank, string village, bool renegade, IFormFile imageFile, string imageName)
        {
            Id = id;
            Name = name;
            Rank = rank;
            Village = village;
            Renegade = renegade;
            ImageFile = imageFile;
            ImageName = imageName;
        }
    }
}
