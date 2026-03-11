using Application.DataTransferObjects.Commercial.Documents.Payments.Installments;
using Domain.ValueObjects.Commercial.Documents.Payments.Installments;

namespace Application.Mappings.Commercial.Documents.Budgets
{
    public class InstallmentProfile : CrudProfile<InstallmentSaveDto, Installment>
    {
        public InstallmentProfile()
        {
            CreateMap<InstallmentSaveDto, Installment>().ConstructUsing((src, ctx) =>
                new Installment(src.Value, src.Quantity, ctx.Mapper.Map<DownPayment>(src.DownPayment))
            );
        }
    }
}