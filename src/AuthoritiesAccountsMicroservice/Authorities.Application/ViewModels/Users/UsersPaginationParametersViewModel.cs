using Authorities.Application.ViewModels.Pagination;

namespace Authorities.Application.ViewModels.Users
{
    /// <summary>
    /// TODO: Add detailed validation
    /// </summary>

    public class UsersPaginationParametersViewModel : PaginationParametersViewModel
    {
        public bool ShowDeleted { get; set; } = false;
        public bool ShowBlocked { get; set; } = false;
    }
}
