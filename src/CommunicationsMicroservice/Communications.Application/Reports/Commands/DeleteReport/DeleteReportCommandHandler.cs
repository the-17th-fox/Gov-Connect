using Communications.Application.Utilities;
using Communications.Core.Interfaces;
using MediatR;
using Microsoft.Extensions.Options;
using Nest;
using SharedLib.ElasticSearch.Extensions;
using SharedLib.ExceptionsHandler.CustomExceptions;

namespace Communications.Application.Reports.Commands;

public class DeleteReportCommandHandler : ReportsHandlerBase, IRequestHandler<DeleteReportCommand>
{
    private readonly IElasticClient _elasticSearchClient;
    private readonly string _reportsIndexName;

    public DeleteReportCommandHandler(
        IUnitOfWork unitOfWork, 
        IElasticClient elasticSearchClient,
        IOptions<ElasticSearchIndexesOptions> elasticSearchIndexesOptions) : base(unitOfWork)
    {
        _elasticSearchClient = elasticSearchClient ?? throw new ArgumentNullException(nameof(elasticSearchClient));
        _reportsIndexName = elasticSearchIndexesOptions.Value.ReportsIndexName;
    }

    public async Task Handle(DeleteReportCommand request, CancellationToken cancellationToken)
    {
        var report = await GetIfExistsAsync(request.Id);

        if (!report.CanBeEdited)
        {
            throw new BadRequestException("Report can not be edited anymore.");
        }

        UnitOfWork.ReportsRepository.Delete(report);

        await UnitOfWork.SaveChangesAsync();
        await _elasticSearchClient.DeleteIndexedDataAsync(_reportsIndexName, report.Id);
    }
}