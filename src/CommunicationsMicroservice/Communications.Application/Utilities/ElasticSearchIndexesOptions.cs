namespace Communications.Application.Utilities;

public class ElasticSearchIndexesOptions
{
    public string ReportsIndexName { get; set; } = string.Empty;
    public string NotificationsIndexName { get; set; } = string.Empty;
    public byte QueriedElementsAmount { get; set; }
}