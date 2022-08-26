using Microsoft.AspNetCore.Http;

namespace Application.ViewModels
{
    public class AddNinjaViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Rank { get; set; }
        public string Village { get; set; }
        public bool Renegade { get; set; }
        public IFormFile ImageFile { get; set; }
    }
}
