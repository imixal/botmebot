using System.Threading.Tasks;
using Icogram.ViewModels.Layout;

namespace Icogram.ViewModelBuilder
{
    public interface IViewModelBuilder
    {
        Task<TViewModel> GetPageViewModelAsync<TViewModel>() where TViewModel : LayoutViewModel, new();
    }
}