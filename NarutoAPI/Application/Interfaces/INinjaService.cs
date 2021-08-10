using Domain.Models;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface INinjaService
    {
        Task AddNinja(NinjaModel ninja);
        Task UpdateNinja(NinjaModel ninja);
        Task<PagedSearchModel> FindWithPagedSearch(string name, string sortDirection, int pageSize, int page);
        Task<NinjaModel> FindByID(int id);
        Task DeleteNinja(int id);
    }
}
