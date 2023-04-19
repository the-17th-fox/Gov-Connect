using Communications.Application.BaseMethods;
using Communications.Core.Models;

namespace Communications.Application.Reports.Queries;

public class GetAllReportsByCivilianQuery : BaseGetAllQuery<Report>
{
    public Guid CivilianId { get; set; }
}