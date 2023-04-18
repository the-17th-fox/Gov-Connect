namespace Communications.Core.Models;

public class Classification
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;

    public List<Notification> Notifications { get; set; } = new();
    public List<Report> Reports { get; set; } = new();
}