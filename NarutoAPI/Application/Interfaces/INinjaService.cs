using Application.ViewModels;
using Domain.Models;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface INinjaService
    {
        Task AddNinja(AddNinjaViewModel AddNinjaViewModel);
        Task UpdateNinja(AddNinjaViewModel upsertNinjaViewModel);
        Task<PagedSearchModel> FindWithPagedSearch(string name, string sortDirection, int pageSize, int page);
        Task<NinjaModel> FindByID(int id);
        Task DeleteNinja(int id, string imageName);
    }
}
