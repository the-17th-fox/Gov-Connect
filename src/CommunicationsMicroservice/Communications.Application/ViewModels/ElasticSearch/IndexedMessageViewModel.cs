using SharedLib.ElasticSearch.Interfaces;

namespace Communications.Application.ViewModels.ElasticSearch;

public class IndexedMessageViewModel : IIndexedMessage
{
    /// <summary></summary>
    /// <param name="id"></param>
    /// <param name="header"></param>
    /// <param name="date">Creating or updating date. Will be used for sorting.</param>
    public IndexedMessageViewModel(Guid id, string header, DateTime date)
    {
        Id = id;
        Header = header;
        Date = date;
    }

    public Guid Id { get; set; }
    public string Header { get; set; }

    /// <summary>
    /// Creating or updating date. Will be used for sorting.
    /// </summary>
    public DateTime Date { get; set; }
}