namespace Communications.Api.ViewModels.Reports;

public class PublicReportViewModel : ShortReportViewModel
{
    public string FirstName { get; set; } = string.Empty; 
    public string Patronymic { get; set; } = string.Empty;

    public string Body { get; set; } = string.Empty;
    
    public Guid? ReplyId {get; set; }
    public DateTime CreatedAt { get; set; }
}