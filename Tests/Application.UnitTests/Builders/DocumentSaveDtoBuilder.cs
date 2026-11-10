using Application.Documents.Budgets.DataTransferObjects;
using Application.Documents.DataTransferObjects.Payments;
using Application.Documents.DataTransferObjects.Payments.Installments;
using Application.Documents.Orders.DataTransferObjects;
using Domain.Documents.ValueObjects.Payments.Enums;

namespace Application.UnitTests.Builders
{
    internal static class DocumentSaveDtoBuilder
    {
        public const long CustomerId = 2;

        public static readonly DateTime CreationDateTime = new(2026, 1, 10, 12, 0, 0, DateTimeKind.Utc);

        public static BudgetProductSaveDto BudgetProduct(long productId = 10, decimal quantity = 2, decimal valueUnit = 50, long id = 0)
        {
            return new BudgetProductSaveDto { Id = id, ProductId = productId, Quantity = quantity, ValueUnit = valueUnit };
        }

        public static OrderProductSaveDto OrderProduct(long productId = 10, decimal quantity = 2, decimal valueUnit = 50, long id = 0)
        {
            return new OrderProductSaveDto { Id = id, ProductId = productId, Quantity = quantity, ValueUnit = valueUnit };
        }

        public static BudgetSaveDto Budget(PaymentSaveDto? payment = null, List<BudgetProductSaveDto>? products = null, byte[]? pdf = null)
        {
            return new BudgetSaveDto
            {
                CreationDateTime = CreationDateTime,
                CustomerId = CustomerId,
                Observations = "Observação",
                PDF = pdf,
                Payment = payment ?? new PaymentSaveDto { Method = PaymentMethod.PIX },
                Products = products ?? [BudgetProduct()]
            };
        }

        public static OrderSaveDto Order(PaymentSaveDto? payment = null, List<OrderProductSaveDto>? products = null, long? budgetId = null, byte[]? pdf = null)
        {
            return new OrderSaveDto
            {
                CreationDateTime = CreationDateTime,
                CustomerId = CustomerId,
                BudgetId = budgetId,
                PDF = pdf,
                Payment = payment ?? new PaymentSaveDto { Method = PaymentMethod.PIX },
                Products = products ?? [OrderProduct()]
            };
        }

        public static InstallmentSaveDto Installment(int quantity, decimal? downPaymentValue = null)
        {
            DownPaymentSaveDto? downPayment = downPaymentValue.HasValue
                ? new DownPaymentSaveDto { Value = downPaymentValue.Value, Method = PaymentMethod.Money }
                : null;

            return new InstallmentSaveDto { Quantity = quantity, DownPayment = downPayment };
        }
    }
}
