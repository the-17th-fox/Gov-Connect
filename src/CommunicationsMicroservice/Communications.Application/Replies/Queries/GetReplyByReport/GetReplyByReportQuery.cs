﻿using Communications.Core.Models;
using MediatR;

namespace Communications.Application.Replies.Queries.GetReplyByReport;

public class GetReplyByReportQuery : IRequest<Reply>
{
    public Guid ReportId { get; set; }
}