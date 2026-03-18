using Application.DataTransferObjects.Commercial.Documents;

namespace Application.Interfaces.Queries.Commercial.Documents
{
    public interface IDocumentReportQuery<TDocumentReport, TDocumentProductReport>
        : IReportQuery<TDocumentReport, DocumentReportFilter>
        where TDocumentReport : DocumentReport<TDocumentProductReport>
        where TDocumentProductReport : DocumentProductReport
    {
    }
}
