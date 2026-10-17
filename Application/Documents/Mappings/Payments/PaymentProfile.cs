using Application.Documents.DataTransferObjects.Payments;
using Application.Shared.Mappings;
using Domain.Documents.ValueObjects.Payments;

namespace Application.Documents.Mappings.Payments
{
    public class PaymentProfile : CrudProfile<PaymentSaveDto, Payment>
    {
    }
}
