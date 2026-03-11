using Application.DataTransferObjects.Commercial.Documents.Payments;
using Domain.ValueObjects.Commercial.Documents.Payments;
using Domain.ValueObjects.Commercial.Documents.Payments.Installments;

namespace Application.Mappings.Commercial.Documents.Budgets
{
    public class PaymentProfile : CrudProfile<PaymentSaveDto, Payment>
    {
        public PaymentProfile()
        {
            CreateMap<PaymentSaveDto, Payment>().ConstructUsing((src, ctx) =>
                new Payment(
                        src.Method,
                        src.DiscountValue,
                        ctx.Mapper.Map<Installment>(src.Installment)
                    )
            );
        }
    }
}