using Application.DataTransferObjects.Commercial.Documents;

namespace Application.Interfaces.Queries.Commercial.Documents
{
    public interface IDocumentReportQuery<TDocumentReport>
        : IReportQuery<TDocumentReport, DocumentReportFilter> 
        where TDocumentReport : DocumentReport
    {
    }
}
