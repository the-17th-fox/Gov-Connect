using AutoMapper;
using Communications.Application.Utilities;
using Communications.Application.ViewModels.ElasticSearch;
using Communications.Core.Interfaces;
using Communications.Core.Models;
using MediatR;
using Microsoft.Extensions.Options;
using Nest;
using SharedLib.ElasticSearch.Extensions;

namespace Communications.Application.Reports.Commands;

public class CreateReportCommandHandler : ReportsHandlerBase, IRequestHandler<CreateReportCommand>
{
    private readonly IMapper _mapper;
    private readonly IElasticClient _elasticSearchClient;
    private readonly string _reportsIndexName;

    public CreateReportCommandHandler(
        IUnitOfWork unitOfWork, 
        IMapper mapper,
        IElasticClient elasticSearchClient,
        IOptions<ElasticSearchIndexesOptions> elasticSearchIndexesOptions) : base(unitOfWork)
    {
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        _elasticSearchClient = elasticSearchClient ?? throw new ArgumentNullException(nameof(elasticSearchClient));
        _reportsIndexName = elasticSearchIndexesOptions.Value.ReportsIndexName;
    }

    public async Task Handle(CreateReportCommand request, CancellationToken cancellationToken)
    {
        var report = _mapper.Map<Report>(request);

        foreach (var classificationId in request.ClassificationsIds)
        {
            var classification = await UnitOfWork.ClassificationsRepository.GetByIdAsync(classificationId);
            if (classification != null)
            {
                report.Classifications.Add(classification);
            }
        }

        UnitOfWork.ReportsRepository.Create(report);

        await UnitOfWork.SaveChangesAsync();

        await IndexReportAsync(report.Id, report.Header, report.UpdatedAt);
    }

    private async Task IndexReportAsync(Guid id, string header, DateTime updatedAt)
    {
        var indexedMessageViewModel = new IndexedMessageViewModel(id, header, updatedAt);
        await _elasticSearchClient.IndexDataAsync(_reportsIndexName, indexedMessageViewModel);
    }
}