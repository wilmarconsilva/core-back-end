using Application.DataTransferObjects.Commercial.Documents;
using Application.Interfaces.Queries.Commercial.Documents;
using Infrastructure.Persistece;
using Microsoft.EntityFrameworkCore;
using System.Data.Common;
using System.Text;

namespace Infrastructure.Queries.Commercial.Documents
{
    public abstract class DocumentReportQuery<TDocumentReport> : ReportQuery<TDocumentReport, DocumentReportFilter>, IDocumentReportQuery<TDocumentReport>
        where TDocumentReport : DocumentReport
    {
        protected DocumentReportQuery(AppDbContext context) : base(context)
        {
        }

        public override async Task<IEnumerable<TDocumentReport>> Get(DocumentReportFilter filter, CancellationToken cancellationToken)
        {
            List<DbParameter> parameters = [];
            string sql = GetSql(filter, parameters);

            return await _context.Database
                .SqlQueryRaw<TDocumentReport>(sql, GetParameters(parameters))
                .ToListAsync(cancellationToken);
        }

        protected virtual string GetExtraSelectColumns() => "";

        protected virtual string GetExtraJoins() => "";

        private string GetSql(DocumentReportFilter filter, List<DbParameter> parameters)
        {
            StringBuilder where = new();
            using DbCommand command = CreateCommand();

            if (filter.CreationDateTimeStart.HasValue)
            {
                string parameterName = "@creation_date_time_start";
                where.Append($" AND d.creation_date_time >= {parameterName}");
                AddParameter(command, parameters, parameterName, filter.CreationDateTimeStart.Value);
            }

            if (filter.CreationDateTimeEnd.HasValue)
            {
                string parameterName = "@creation_date_time_end";
                where.Append($" AND d.creation_date_time <= {parameterName}");
                AddParameter(command, parameters, parameterName, filter.CreationDateTimeEnd.Value);
            }

            if (filter.LastUpdateDateTimeStart.HasValue)
            {
                string parameterName = "@update_date_time_start";
                where.Append($" AND d.last_update_date_time >= {parameterName}");
                AddParameter(command, parameters, parameterName, filter.LastUpdateDateTimeStart.Value);
            }

            if (filter.LastUpdateDateTimeEnd.HasValue)
            {
                string parameterName = "@update_date_time_end";
                where.Append($" AND d.last_update_date_time <= {parameterName}");
                AddParameter(command, parameters, parameterName, filter.LastUpdateDateTimeEnd.Value);
            }

            string dateTimePattern = "'DD/MM/YYYY'";

            return $"""
                SELECT
                    to_char(d.creation_date_time, {dateTimePattern}) AS "{nameof(DocumentReport.CreationDateTimeDescription)}",
                    to_char(d.last_update_date_time, {dateTimePattern}) AS "{nameof(DocumentReport.LastUpdateDateTimeDescription)}",
                    d.value_subtotal AS "{nameof(DocumentReport.ValueSubtotal)}",
                    d.value_total AS "{nameof(DocumentReport.ValueTotal)}",
                    d.payment_method AS "{nameof(DocumentReport.PaymentMethod)}",
                    d.payment_discount_value AS "{nameof(DocumentReport.PaymentDiscountValue)}",
                    CASE
                        WHEN d.payment_installment_has_installment THEN d.payment_installment_quantity::numeric
                        ELSE NULL
                    END AS "{nameof(DocumentReport.PaymentInstallmentQuantity)}",
                    CASE
                        WHEN d.payment_installment_has_installment THEN d.payment_installment_value
                        ELSE NULL
                    END AS "{nameof(DocumentReport.PaymentInstallmentValue)}",
                    CASE
                        WHEN COALESCE(d.payment_installment_has_down_payment, false) THEN d.payment_installment_down_payment_value
                        ELSE NULL
                    END AS "{nameof(DocumentReport.PaymentInstallmentDownPaymentValue)}",
                    CASE
                        WHEN COALESCE(d.payment_installment_has_down_payment, false) THEN d.payment_installment_down_payment_percentage
                        ELSE NULL
                    END AS "{nameof(DocumentReport.PaymentInstallmentDownPaymentPercentage)}",
                    d.payment_installment_has_down_payment AS "{nameof(DocumentReport.PaymentInstallmentDownPaymentMethod)}",
                    d.observations AS "{nameof(DocumentReport.Observations)}",
                    u.name AS "{nameof(DocumentReport.UserName)}",
                    c.name AS "{nameof(DocumentReport.CustomerName)}",
                    COALESCE(addr.locality_description, '') || ' - ' || COALESCE(addr.state_acronym, '') AS "{nameof(DocumentReport.CustomerAddressCityState)}"
                    {GetExtraSelectColumns()}
                FROM {GetTableName()} d
                INNER JOIN users u ON u.id = d.user_id
                INNER JOIN customers c ON c.id = d.customer_id
                LEFT JOIN LATERAL (
                    SELECT l.description AS locality_description, s.acronym AS state_acronym
                    FROM customers_addresses ca
                    INNER JOIN localities l ON l.id = ca.locality_id
                    INNER JOIN states s ON s.id = l.state_id
                    WHERE ca.customer_id = d.customer_id
                    ORDER BY ca.id
                    LIMIT 1
                ) addr ON true
                {GetExtraJoins()}
                WHERE 1 = 1{where}
                """;
        }

    }
}
