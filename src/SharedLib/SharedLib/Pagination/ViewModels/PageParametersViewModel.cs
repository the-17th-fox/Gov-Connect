using System.ComponentModel.DataAnnotations;

namespace SharedLib.Pagination.ViewModels
{
    /// <summary>
    /// TODO: Add detailed validation
    /// </summary>

    public class PageParametersViewModel
    {
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 20;
    }
}
