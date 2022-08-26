using Application.Interfaces;
using Application.ViewModels;
using Domain.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
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
        private readonly ILogger<NarutoController> _logger;
        private readonly IWebHostEnvironment _hostEnvironment;

        public NarutoController(INinjaService ninjaService, ILogger<NarutoController> logger, IWebHostEnvironment hostEnvironment)
        {
            _ninjaService = ninjaService;
            _logger = logger;
            _hostEnvironment = hostEnvironment;
        }

        [HttpGet("{sortDirection}/{pageSize}/{page}")]
        public async Task<IActionResult> Get(
            [FromQuery] string title,
            string sortDirection,
            int pageSize,
            int page)
        {
            _logger.LogInformation($"NinjaController - PagedSearch - BEGIN - Sort Direction: {sortDirection}. PageSize: {pageSize}. Page: {page}");
            var pagedSearch = await _ninjaService.FindWithPagedSearch(title, sortDirection, pageSize, page);
            _logger.LogInformation($"NinjaController - PagedSearch - END - Return: {pagedSearch.List}");
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
        public async Task<IActionResult> Post([FromForm] AddNinjaViewModel addNinjaViewModel)
        {
            _logger.LogInformation($"NinjaController - Add - BEGIN - ViewModel: {addNinjaViewModel}");
            if (_ninjaService == null) return BadRequest();
            await _ninjaService.AddNinja(addNinjaViewModel);
            _logger.LogInformation($"NinjaController - Add - END - ViewModel: {addNinjaViewModel}");
            return Ok();
        }

        [HttpPut]
        public async Task<IActionResult> Put([FromForm] UpdateNinjaViewModel updateNinjaViewModel)
        {
            _logger.LogInformation($"NinjaController - Update - BEGIN - ViewModel: {updateNinjaViewModel}");
            if (_ninjaService == null) return BadRequest();
            await _ninjaService.UpdateNinja(updateNinjaViewModel);
            _logger.LogInformation($"NinjaController - Update - END - ViewModel: {updateNinjaViewModel}");
            return Ok();
        }

        [HttpDelete("{id}/{imageName}")]
        public async Task<IActionResult> Delete(int id, string imageName)
        {
            _logger.LogInformation($"NinjaController - Delete - BEGIN - ID: {id}");
            if (_ninjaService == null) return BadRequest();
            await _ninjaService.DeleteNinja(id, imageName);
            _logger.LogInformation($"NinjaController - Delete - END - ID: {id}");
            return NoContent();
        }
    }
}
