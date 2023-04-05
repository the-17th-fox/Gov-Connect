using SharedLib.Pagination.ViewModels;

namespace Civilians.Core.ViewModels.Civilians
{
    public class CiviliansPaginationParametersViewModel : PageParametersViewModel
    {
        public bool ShowDeleted { get; set; } = false;
        public bool ShowBlocked { get; set; } = false;
    }
}
