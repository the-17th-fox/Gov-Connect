namespace Communications.Application.AutoMapper.CiviliansInfoConsistency;

public class CivilianInfoViewModel
{
    public Guid CivilianId { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string Patronymic { get; set; } = string.Empty;
}