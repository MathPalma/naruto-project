using Application.ViewModels;
using Domain.Models;

namespace Application.Mappers
{
    public static class NinjaMapper
    {
        public static NinjaModel ParaModel(this AddNinjaViewModel viewModel) => new NinjaModel(viewModel.Id, viewModel.Name, viewModel.Rank, viewModel.Village, viewModel.Renegade, viewModel.ImageFile);
        public static NinjaModel ParaModel(this UpdateNinjaViewModel viewModel) => new NinjaModel(viewModel.Id, viewModel.Name, viewModel.Rank, viewModel.Village, viewModel.Renegade, viewModel.ImageFile, viewModel.ImageName);

    }
}
