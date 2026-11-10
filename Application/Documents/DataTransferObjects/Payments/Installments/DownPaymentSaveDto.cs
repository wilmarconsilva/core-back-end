using Domain.Documents.ValueObjects.Payments.Enums;

namespace Application.Documents.DataTransferObjects.Payments.Installments
{
    public class DownPaymentSaveDto
    {
        public required decimal Value { get; set; }

        /// <summary>
        /// Somente leitura: calculado pelo domínio a partir de <see cref="Value"/> e ignorado ao salvar.
        /// </summary>
        public decimal Percentage { get; set; }

        public required PaymentMethod Method { get; set; }
    }
}
