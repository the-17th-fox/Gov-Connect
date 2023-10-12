using Communications.Application.ViewModels.ElasticSearch;

namespace Communications.Api.ViewModels.ElasticSearch;

public class SearchResponseViewModel : IndexedMessageViewModel
{
    public string Type { get; set; } = string.Empty;

    public SearchResponseViewModel(Guid id, string header, DateTime date) : base(id, header, date)
    {
    }
}