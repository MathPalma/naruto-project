using Domain.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Domain.Repositories
{
    public interface INinjaRepository
    {
        Task AddNinja(NinjaModel ninja);
        Task UpdateNinja(NinjaModel ninja);
        Task<List<NinjaModel>> FindWithPagedSearch(string query);
        Task<NinjaModel> FindByID(int id);
        Task<int> GetCountNinjas(string query);
        Task Delete(int id);
    }
}
