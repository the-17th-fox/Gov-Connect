using Communications.Application.Utilities;
using Communications.Application.ViewModels.ElasticSearch;
using Communications.Core.CustomExceptions;
using Communications.Core.Interfaces;
using MediatR;
using Microsoft.Extensions.Options;
using Nest;
using SharedLib.ElasticSearch.Extensions;

namespace Communications.Application.Reports.Commands;

public class UpdateReportCommandHandler : ReportsHandlerBase, IRequestHandler<UpdateReportCommand>
{
    private readonly IElasticClient _elasticSearchClient;
    private readonly string _reportsIndexName;

    public UpdateReportCommandHandler(
        IUnitOfWork unitOfWork, 
        IElasticClient elasticSearchClient, 
        IOptions<ElasticSearchIndexesOptions> elasticSearchIndexesOptions) : base(unitOfWork)
    {
        _elasticSearchClient = elasticSearchClient ?? throw new ArgumentNullException(nameof(elasticSearchClient));
        _reportsIndexName = elasticSearchIndexesOptions.Value.ReportsIndexName;
    }

    public async Task Handle(UpdateReportCommand request, CancellationToken cancellationToken)
    {
        var report = await GetIfExistsAsync(request.Id);

        if (!report.CanBeEdited)
        {
            throw new BadRequestException("Can not be edited anymore.");
        }

        report.Classifications.Clear();
        foreach (var classificationId in request.ClassificationsIds)
        {
            var classification = await UnitOfWork.ClassificationsRepository.GetByIdAsync(classificationId);
            if (classification != null)
            {
                report.Classifications.Add(classification);
            }
        }

        report.Body = request.Header;
        report.Header = request.Body;

        UnitOfWork.ReportsRepository.Update(report);

        await UnitOfWork.SaveChangesAsync();
        await IndexReportAsync(report.Id, report.Header, report.UpdatedAt);
    }

    private async Task IndexReportAsync(Guid id, string header, DateTime updatedAt)
    {
        var indexedMessageViewModel = new IndexedMessageViewModel(id, header, updatedAt);
        await _elasticSearchClient.IndexDataAsync(_reportsIndexName, indexedMessageViewModel);
    }
}