using Application.Interfaces;
using Application.Mappers;
using Application.ViewModels;
using Azure.Storage.Blobs;
using Domain.Models;
using Domain.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Application.Services
{
    public class NinjaService : INinjaService
    {
        private readonly INinjaRepository _narutoRepository;
        private readonly ILogger<NinjaService> _logger;
        private readonly string _connectionBlobStorage;
        private readonly string _blobUrl;

        public NinjaService(INinjaRepository narutoRepository, ILogger<NinjaService> logger, IConfiguration configuration)
        {
            _narutoRepository = narutoRepository;
            _logger = logger;
            _connectionBlobStorage = configuration.GetConnectionString("BLOB_STORAGE");
            _blobUrl = configuration["BlobUrl"];
        }

        public async Task AddNinja(AddNinjaViewModel AddNinjaViewModel)
        {
            try
            {
                NinjaModel ninjaModel = AddNinjaViewModel.ParaModel();
                ninjaModel.ImageName = await SendImageToBlob(ninjaModel.ImageFile);
                await _narutoRepository.AddNinja(ninjaModel);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"NinjaService - Add - Error while adding ninja.");
            }

        }

        public async Task UpdateNinja(AddNinjaViewModel upsertNinjaViewModel)
        {
            try
            {
                NinjaModel ninjaModel = upsertNinjaViewModel.ParaModel();
                await DeleteBlobImage(ninjaModel.ImageName);
                ninjaModel.ImageName = await SendImageToBlob(ninjaModel.ImageFile);
                await _narutoRepository.UpdateNinja(ninjaModel);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"NinjaService - Update - Error while updating ninja.");
            }
        }

        public async Task<PagedSearchModel> FindWithPagedSearch(
            string name, string sortDirection, int pageSize, int page)
        {
            try
            {
                var sort = (!string.IsNullOrWhiteSpace(sortDirection)) && !sortDirection.Equals("desc") ? "asc" : "desc";
                var size = (pageSize < 1) ? 10 : pageSize;
                var offset = page > 0 ? (page - 1) * size : 0;

                string query = $"select * from Ninjas n where 1 = 1 ";
                if (!string.IsNullOrWhiteSpace(name)) query = query + $" and n.Name like '%{name}%' ";
                query += $" order by n.ID {sort} offset {offset} rows FETCH NEXT {size} ROWS ONLY";

                string countQuery = @"select count(*) from Ninjas n where 1 = 1 ";
                if (!string.IsNullOrWhiteSpace(name)) countQuery = countQuery + $" and n.Name like '%{name}%' ";

                var ninjas = await _narutoRepository.FindWithPagedSearch(query);
                var totalResults = await _narutoRepository.GetCountNinjas(countQuery);

                foreach (NinjaModel ninja in ninjas)
                {
                    ninja.ImageSrc = _blobUrl + ninja.ImageName;
                }

                return new PagedSearchModel
                {
                    CurrentPage = page,
                    List = ninjas,
                    PageSize = size,
                    SortDirections = sort,
                    TotalResults = totalResults
                };
            }
            catch (Exception ex)
            {

                _logger.LogError(ex, $"NinjaService - FindWithPagedSearch - Error while get ninjas by paged search.");
                return null;
            }

        }

        public async Task<NinjaModel> FindByID(int id)
        {
            try
            {
                return await _narutoRepository.FindByID(id);
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public async Task DeleteNinja(int id, string imageName)
        {
            try
            {
                await _narutoRepository.Delete(id);
                await DeleteBlobImage(imageName);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"NinjaService - Delete - Error while deleting ninja.");
            }
        }

        private async Task<string> SendImageToBlob(IFormFile imageFile)
        {
            string imageName = new String(Path.GetFileNameWithoutExtension(imageFile.FileName).Take(10).ToArray()).Replace(' ', '-');
            imageName = imageName + DateTime.Now.ToString("yymmssfff") + Path.GetExtension(imageFile.FileName);
            BlobClient blobClient = new BlobClient(_connectionBlobStorage, "naruto-project-photos", imageName);

            using (Stream file = imageFile.OpenReadStream())
            {
                await blobClient.UploadAsync(file);
            }

            return imageName;
        }

        private async Task DeleteBlobImage(string imageName)
        {
            BlobClient blobClient = new BlobClient(_connectionBlobStorage, "naruto-project-photos", imageName);
            await blobClient.DeleteAsync();
        }
    }
}
