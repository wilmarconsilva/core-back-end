using Application.DataTransferObjects.Commercial.Documents.Payments;
using Domain.ValueObjects.Commercial.Documents.Payments.Installments;

namespace Application.Mappings.Commercial.Documents.Budgets
{
    public class DownPaymentProfile : CrudProfile<DownPaymentSaveDto, DownPayment>
    {
        public DownPaymentProfile()
        {
            CreateMap<DownPaymentSaveDto, DownPayment>().ConstructUsing((src, ctx) =>
                new DownPayment(src.Value, src.Percentage, src.Method)
            );
        }
    }
}