using Communications.Core.Models;
using MediatR;

namespace Communications.Application.Classifications.Queries;

public class GetClassificationByIdQuery : IRequest<Classification>
{
    public Guid Id { get; set; }
}