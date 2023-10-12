namespace Authorities.Application.ViewModels.Pagination
{
    public class PaginationParametersViewModel
    {
        public short PageNumber { get; set; } = 1;
        public byte PageSize { get; set; } = 20;
    }
}
