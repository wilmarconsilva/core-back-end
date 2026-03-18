using Application.DataTransferObjects.Commercial.Documents;
using Application.Interfaces.Queries.Commercial.Documents;
using Domain.Entities.Commercial.Documents;
using Domain.ValueObjects.Enums;
using Infrastructure.Persistece;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Queries.Commercial.Documents
{
    public abstract class DocumentReportQuery<TDocument, TDocumentProduct, TDocumentReport, TDocumentProductReport>
        : ReportQuery<TDocument, TDocumentReport, DocumentReportFilter>, IDocumentReportQuery<TDocumentReport, TDocumentProductReport>
        where TDocument : Document<TDocumentProduct>
        where TDocumentProduct : DocumentProduct
        where TDocumentReport : DocumentReport<TDocumentProductReport>, new()
        where TDocumentProductReport : DocumentProductReport
    {
        protected DocumentReportQuery(AppDbContext context) : base(context)
        {
        }

        public override async Task<IEnumerable<TDocumentReport>> Get(DocumentReportFilter filter, CancellationToken cancellationToken)
        {
            IQueryable<TDocument> query = ApplyFilter(filter);

            List<TDocument> entities = await ApplyIncludes(query).ToListAsync(cancellationToken);

            return entities.Select(d =>
            {
                TDocumentReport report = MapCommonProperties(d);
                MapExtraProperties(report, d);
                return report;
            });
        }

        protected virtual IQueryable<TDocument> ApplyIncludes(IQueryable<TDocument> query)
        {
            return query
                .Include(d => d.User)
                .Include(d => d.Customer)
                    .ThenInclude(c => c!.Addresses)
                    .ThenInclude(a => a.Locality)
                    .ThenInclude(l => l!.State);
        }

        protected virtual void MapExtraProperties(TDocumentReport report, TDocument document) { }

        private IQueryable<TDocument> ApplyFilter(DocumentReportFilter filter)
        {
            IQueryable<TDocument> query = _query;

            if (filter.CreationDateTimeStart.HasValue)
                query = query.Where(d => d.CreationDateTime >= filter.CreationDateTimeStart.Value);

            if (filter.CreationDateTimeEnd.HasValue)
                query = query.Where(d => d.CreationDateTime <= filter.CreationDateTimeEnd.Value);

            if (filter.LastUpdateDateTimeStart.HasValue)
                query = query.Where(d => d.LastUpdateDateTime >= filter.LastUpdateDateTimeStart.Value);

            if (filter.LastUpdateDateTimeEnd.HasValue)
                query = query.Where(d => d.LastUpdateDateTime <= filter.LastUpdateDateTimeEnd.Value);

            return query;
        }

        private TDocumentReport MapCommonProperties(TDocument d)
        {
            return new TDocumentReport
            {
                CreationDateTimeDescription = d.CreationDateTime.ToShortDateString(),
                LastUpdateDateTimeDescription = d.LastUpdateDateTime.ToShortDateString(),
                ValueSubtotal = d.ValueSubtotal,
                ValueTotal = d.ValueTotal,
                PaymentMethodDescription = d.Payment.Method.GetDescription(),
                PaymentDiscountValue = d.Payment.DiscountValue,
                PaymentInstallmentValue = d.Payment.Installment?.Value,
                PaymentInstallmentQuantity = d.Payment.Installment?.Quantity,
                PaymentInstallmentDownPaymentValue = d.Payment.Installment?.DownPayment?.Value,
                PaymentInstallmentDownPaymentPercentage = d.Payment.Installment?.DownPayment?.Percentage,
                PaymentInstallmentDownPaymentMethodDescription = d.Payment.Installment?.DownPayment?.Method.GetDescription(),
                Observations = d.Observations,
                UserName = d.User!.Name,
                CustomerName = d.Customer!.Description,
                CustomerAddressCityState = $"{d.Customer.Addresses.First().Locality!.Description} - {d.Customer.Addresses.First().Locality!.State!.Acronym}"
            };
        }
    }
}
