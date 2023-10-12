using Communications.Application.ViewModels.CiviliansInfoConsistency;
using MediatR;

namespace Communications.Application.Reports.Commands;

public class UpdateReportsByCiviliansCommand : IRequest
{
    public List<CivilianInfoViewModel> CiviliansData { get; set; } = new();
}