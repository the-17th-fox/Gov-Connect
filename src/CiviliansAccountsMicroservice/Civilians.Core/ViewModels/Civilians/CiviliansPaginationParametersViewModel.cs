using Civilians.Core.ViewModels.Pagination;

namespace Civilians.Core.ViewModels.Civilians
{
    public class CiviliansPaginationParametersViewModel : PageParametersViewModel
    {
        public bool ShowDeleted { get; set; } = false;
        public bool ShowBlocked { get; set; } = false;
    }
}
