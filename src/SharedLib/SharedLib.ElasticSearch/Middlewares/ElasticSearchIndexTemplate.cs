namespace SharedLib.ElasticSearch.Middlewares;

public class ElasticSearchIndexTemplate
{
    public string IndexName { get; set; }
    public Type MessageType { get; set; }

    public ElasticSearchIndexTemplate(string indexName, Type messageType)
    {
        IndexName = indexName;
        MessageType = messageType;
    }
}