namespace Communications.Core.Models;

public class Notification : MessageBase
{
    public Guid Id { get; set; }
    public Guid AuthorityId { get; set; }

    public List<Classification> Classifications { get; set; } = new();
    
    public string Organization { get; set; } = string.Empty;
}