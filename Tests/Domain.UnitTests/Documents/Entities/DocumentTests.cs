using Domain.Documents.Budgets.Entities;
using Domain.Documents.Orders.Entities;
using Domain.Documents.ValueObjects.Payments.Installments;
using Domain.Shared.Exceptions;
using Domain.UnitTests.Builders;

namespace Domain.UnitTests.Documents.Entities
{
    public class DocumentTests
    {
        [Fact]
        public void Constructor_ValidData_FillsProperties()
        {
            Order order = DocumentBuilder.Order(budgetId: 3);

            order.SellerId.ShouldBe(DocumentBuilder.SellerId);
            order.CustomerId.ShouldBe(DocumentBuilder.CustomerId);
            order.CreationDateTime.ShouldBe(DocumentBuilder.CreationDateTime);
            order.LastUpdateDateTime.ShouldBe(DateTime.UtcNow, TimeSpan.FromMinutes(1));
            order.Observations.ShouldBe("Observação");
            order.BudgetId.ShouldBe(3);
            order.Products.Count.ShouldBe(1);
        }

        [Fact]
        public void Constructor_WithoutProducts_ThrowsBusinessRuleException()
        {
            BusinessRuleException exception = Should.Throw<BusinessRuleException>(() => DocumentBuilder.Order(products: []));

            exception.Message.ShouldBe("O documento: 0 deve possuir ao menos um produto.");
        }

        [Fact]
        public void Constructor_ZeroSeller_ThrowsBusinessRuleException()
        {
            Should.Throw<BusinessRuleException>(() => DocumentBuilder.Order(sellerId: 0));
        }

        [Fact]
        public void Constructor_ZeroCustomer_ThrowsBusinessRuleException()
        {
            Should.Throw<BusinessRuleException>(() => DocumentBuilder.Budget(customerId: 0));
        }

        [Fact]
        public void Constructor_SeveralProducts_CalculatesSubtotalAndTotal()
        {
            Budget budget = DocumentBuilder.Budget(
                payment: DocumentBuilder.BudgetPayment(discountValue: 10),
                products: [DocumentBuilder.BudgetProduct(quantity: 2, valueUnit: 50), DocumentBuilder.BudgetProduct(productId: 11, quantity: 1, valueUnit: 30)]);

            budget.ValueSubtotal.ShouldBe(130);
            budget.ValueTotal.ShouldBe(120);
        }

        [Fact]
        public void Constructor_DiscountEqualToSubtotal_ThrowsBusinessRuleException()
        {
            Should.Throw<BusinessRuleException>(() => DocumentBuilder.Order(payment: DocumentBuilder.OrderPayment(discountValue: 100)));
        }

        [Fact]
        public void Constructor_DiscountGreaterThanSubtotal_ThrowsBusinessRuleException()
        {
            BusinessRuleException exception = Should.Throw<BusinessRuleException>(() => DocumentBuilder.Order(payment: DocumentBuilder.OrderPayment(discountValue: 150)));

            exception.Message.ShouldBe("O valor do desconto não pode ser maior que o subtotal do documento.");
        }

        [Fact]
        public void Constructor_NegativeProductValues_ThrowsBusinessRuleException()
        {
            // Quantidade e valor unitário negativos dariam um subtotal positivo (-2 × -50 = 100) se fossem aceitos.
            Should.Throw<BusinessRuleException>(() => DocumentBuilder.Order(products: [new OrderProduct(10, -2, -50, 100)]));
        }

        [Fact]
        public void Constructor_WithInstallment_CalculatesInstallmentValueOverTotal()
        {
            Order order = DocumentBuilder.Order(payment: DocumentBuilder.OrderPayment(discountValue: 20, installment: new Installment(2, null)));

            order.Payment.Installment!.Value.ShouldBe(40);
        }

        [Fact]
        public void Fill_ExistingDocument_SynchronizesProducts()
        {
            Order order = DocumentBuilder.Order(products: [DocumentBuilder.OrderProduct(productId: 10), DocumentBuilder.OrderProduct(productId: 11)]);
            SimulatePersisted(order);

            order.Fill(DocumentBuilder.SellerId, DocumentBuilder.CustomerId, DocumentBuilder.OrderPayment(), null, null, null,
                [DocumentBuilder.OrderProduct(productId: 10, quantity: 5, valueUnit: 10, id: 1), DocumentBuilder.OrderProduct(productId: 12, quantity: 1, valueUnit: 15)]);

            order.Products.Count.ShouldBe(2);
            order.Products.ShouldContain(p => p.Id == 1 && p.Quantity == 5 && p.ValueTotal == 50);
            order.Products.ShouldContain(p => p.Id == 0 && p.ProductId == 12 && p.DocumentId == 7);
            order.Products.ShouldNotContain(p => p.Id == 2);
            order.ValueSubtotal.ShouldBe(65);
            order.Observations.ShouldBeNull();
        }

        [Fact]
        public void Fill_ExistingDocument_KeepsCreationDateTime()
        {
            Budget budget = DocumentBuilder.Budget();

            budget.Fill(DocumentBuilder.SellerId, DocumentBuilder.CustomerId, DocumentBuilder.BudgetPayment(), null, null, null, [DocumentBuilder.BudgetProduct()]);

            budget.CreationDateTime.ShouldBe(DocumentBuilder.CreationDateTime);
        }

        /// <summary>
        /// Simula o agregado materializado pelo ORM: documento com Id 7 e produtos com Ids 1..N.
        /// </summary>
        private static void SimulatePersisted(Order order)
        {
            order.Id = 7;

            long productId = 1;
            foreach (OrderProduct product in order.Products)
                product.Id = productId++;
        }

        [Fact]
        public void Description_ReturnsId()
        {
            Order order = DocumentBuilder.Order();
            order.Id = 42;

            order.Description.ShouldBe("42");
        }
    }
}
