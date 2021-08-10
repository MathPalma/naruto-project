using Application.Interfaces;
using Domain.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace NarutoAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class NarutoController : ControllerBase
    {
        private readonly INinjaService _ninjaService;
        private readonly IWebHostEnvironment _hostEnvironment;

        public NarutoController(INinjaService ninjaService, IWebHostEnvironment hostEnvironment)
        {
            _ninjaService = ninjaService;
            _hostEnvironment = hostEnvironment;
        }

        [HttpGet("{sortDirection}/{pageSize}/{page}")]
        public async Task<IActionResult> Get(
            [FromQuery] string title,
            string sortDirection,
            int pageSize,
            int page)
        {
            var pagedSearch = await _ninjaService.FindWithPagedSearch(title, sortDirection, pageSize, page);
            foreach (NinjaModel ninja in pagedSearch.List)
            {
                ninja.ImageSrc = String.Format("{0}://{1}{2}/Images/{3}", Request.Scheme, Request.Host, Request.PathBase, ninja.ImageName);
            }
            return Ok(pagedSearch);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var ninja = await _ninjaService.FindByID(id);
            if (ninja == null) return NotFound();
            return Ok(ninja);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromForm] NinjaModel ninja)
        {
            if (_ninjaService == null) return BadRequest();
            ninja.ImageName = await SaveImage(ninja.ImageFile);
            await _ninjaService.AddNinja(ninja);
            return Ok();
        }

        [HttpPut]
        public async Task<IActionResult> Put([FromForm] NinjaModel ninja)
        {
            DeleteImage(ninja.ImageName);
            ninja.ImageName = await SaveImage(ninja.ImageFile);
            if (_ninjaService == null) return BadRequest();
            await _ninjaService.UpdateNinja(ninja);
            return Ok();
        }

        [HttpDelete("{id}/{imageName}")]
        public async Task<IActionResult> Delete(int id, string imageName)
        {
            await _ninjaService.DeleteNinja(id);
            DeleteImage(imageName);
            return NoContent();
        }

        [NonAction]
        public async Task<string> SaveImage(IFormFile imageFile)
        {
            try
            {
                string imageName = new String(Path.GetFileNameWithoutExtension(imageFile.FileName).Take(10).ToArray()).Replace(' ', '-');
                imageName = imageName + DateTime.Now.ToString("yymmssfff") + Path.GetExtension(imageFile.FileName);
                var imagePath = Path.Combine(_hostEnvironment.ContentRootPath, "Images", imageName);
                using (var fileStream = new FileStream(imagePath, FileMode.Create))
                {
                    await imageFile.CopyToAsync(fileStream);
                }
                return imageName;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                throw;
            }

        }

        [NonAction]
        public void DeleteImage(string imageName)
        {
            var imagePath = Path.Combine(_hostEnvironment.ContentRootPath, "Images", imageName);
            if (System.IO.File.Exists(imagePath))
                System.IO.File.Delete(imagePath);
        }


    }
}
