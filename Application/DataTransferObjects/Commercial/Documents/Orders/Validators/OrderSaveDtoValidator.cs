using Application.DataTransferObjects.Commercial.Documents.Budgets.Validators;
using Application.DataTransferObjects.Commercial.Documents.Payments;
using FluentValidation;

namespace Application.DataTransferObjects.Commercial.Documents.Orders.Validators
{
    public class OrderSaveDtoValidator : DocumentSaveDtoValidator<OrderSaveDto, OrderProductSaveDto>
    {
        public OrderSaveDtoValidator(OrderProductSaveDtoValidator documentProductValidator, IValidator<PaymentSaveDto> paymentSaveDtoValidator) : base(documentProductValidator, paymentSaveDtoValidator)
        {

        }
    }
}
