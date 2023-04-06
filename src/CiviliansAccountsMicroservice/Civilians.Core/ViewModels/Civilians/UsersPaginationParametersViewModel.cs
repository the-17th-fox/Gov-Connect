using Civilians.Core.ViewModels.Pagination;

namespace Civilians.Core.ViewModels.Civilians
{
    public class UsersPaginationParametersViewModel : PageParametersViewModel
    {
        public bool ShowDeleted { get; set; } = false;
        public bool ShowBlocked { get; set; } = false;
    }
}
