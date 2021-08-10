using Application.Interfaces;
using Domain.Models;
using Domain.Repositories;
using System;
using System.Threading.Tasks;

namespace Application.Services
{
    public class NinjaService : INinjaService
    {
        private readonly INinjaRepository _narutoRepository;
        
        public NinjaService(INinjaRepository narutoRepository)
        {
            _narutoRepository = narutoRepository;
        }

        public async Task AddNinja(NinjaModel ninja)
        {
            try
            {
                await _narutoRepository.AddNinja(ninja);
            }
            catch (Exception ex)
            {
                throw;
            }
            
        }

        public async Task UpdateNinja(NinjaModel ninja)
        {
            try
            {
                await _narutoRepository.UpdateNinja(ninja);
            }
            catch (Exception ex)
            {

                throw;
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

                return new PagedSearchModel
                {
                    CurrentPage = page,
                    List = ninjas,
                    PageSize = size,
                    SortDirections = sort,
                    TotalResults = totalResults
                };
            }
            catch (Exception)
            {

                throw;
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

        public async Task DeleteNinja(int id)
        {
            try
            {
                await _narutoRepository.Delete(id);
            }
            catch (Exception ex)
            {

                throw;
            }
        }

    }
}
