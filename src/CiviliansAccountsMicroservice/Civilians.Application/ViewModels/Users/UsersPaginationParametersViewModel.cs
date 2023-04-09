namespace Civilians.Application.ViewModels.Civilians
{
    /// <summary>
    /// TODO: Add detailed validation
    /// </summary>

    public class UsersPaginationParametersViewModel
    {
        public short PageNumber { get; set; } = 1;
        public byte PageSize { get; set; } = 20;

        public bool ShowDeleted { get; set; } = false;
        public bool ShowBlocked { get; set; } = false;
    }
}
