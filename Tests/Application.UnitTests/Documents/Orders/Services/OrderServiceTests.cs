using Application.Documents.Orders.DataTransferObjects;
using Application.Documents.Orders.Queries;
using Application.Documents.Orders.Services;
using Application.Email.DataTransferObjects;
using Application.UnitTests.Builders;
using Application.UnitTests.Documents.Services;
using Application.UnitTests.Fixtures;
using Domain.Documents.Orders.Entities;
using Domain.Documents.Orders.Repositories.Interfaces;
using Domain.Documents.ValueObjects.Payments.Enums;
using Domain.Shared.Entities;
using Domain.Shared.Exceptions;
using Domain.UnitTests.Builders;
using Microsoft.Extensions.Logging.Abstractions;

namespace Application.UnitTests.Documents.Orders.Services
{
    public class OrderServiceTests : DocumentServiceTestsBase, IClassFixture<MapperFixture>
    {
        private readonly IOrderRepository _repository = Substitute.For<IOrderRepository>();
        private readonly OrderService _service;

        public OrderServiceTests(MapperFixture fixture)
        {
            _service = new OrderService(_repository, Substitute.For<IOrderQuery>(), _unitOfWork, fixture.Mapper, _customerRepository,
                _serviceScopeFactory, _currentUser, NullLogger<OrderService>.Instance);

            _repository
                .When(r => r.SaveAsync(Arg.Any<IEnumerable<Order>>(), Arg.Any<CancellationToken>()))
                .Do(call => { foreach (Order order in call.Arg<IEnumerable<Order>>()) order.Id = PersistedId; });

            ArrangeCustomer(email: null);
        }

        private Order ArrangeExistingOrder()
        {
            Order existing = DocumentBuilder.Order();
            existing.Id = PersistedId;
            existing.Products.Single().Id = 1;
            _repository.GetByIdsAsync(Arg.Any<IEnumerable<long>>(), Arg.Any<CancellationToken>()).Returns([existing]);
            return existing;
        }

        [Fact]
        public async Task CreateAsync_FromBudget_CreatesOrderWithBudgetId()
        {
            OrderSaveDto dto = DocumentSaveDtoBuilder.Order(
                payment: new OrderPaymentSaveDto { Method = PaymentMethod.CreditCard, Installment = DocumentSaveDtoBuilder.Installment(2) },
                budgetId: 9);

            IEnumerable<Entity> entities = await _service.CreateAsync([dto], TestContext.Current.CancellationToken);

            Order order = entities.ShouldHaveSingleItem().ShouldBeOfType<Order>();
            order.SellerId.ShouldBe(CurrentUserId);
            order.BudgetId.ShouldBe(9);
            order.Payment.Method.ShouldBe(PaymentMethod.CreditCard);
            order.Payment.Installment!.Value.ShouldBe(50);
        }

        [Fact]
        public async Task CreateAsync_InvalidPaymentMethod_ThrowsBusinessRuleException()
        {
            OrderSaveDto dto = DocumentSaveDtoBuilder.Order(payment: new OrderPaymentSaveDto { Method = (PaymentMethod)0 });

            await Should.ThrowAsync<BusinessRuleException>(() => _service.CreateAsync([dto], TestContext.Current.CancellationToken));
            await _unitOfWork.DidNotReceive().BeginTransactionAsync(Arg.Any<CancellationToken>());
        }

        [Fact]
        public async Task UpdateAsync_ExistingOrder_FillsLoadedEntity()
        {
            Order existing = ArrangeExistingOrder();

            await _service.UpdateAsync(PersistedId, DocumentSaveDtoBuilder.Order(products: [DocumentSaveDtoBuilder.OrderProduct(quantity: 3, valueUnit: 10)]), TestContext.Current.CancellationToken);

            existing.SellerId.ShouldBe(CurrentUserId);
            existing.ValueTotal.ShouldBe(30);
            await _repository.Received(1).SaveAsync(Arg.Is<IEnumerable<Order>>(o => o.Single() == existing), Arg.Any<CancellationToken>());
        }

        [Fact]
        public async Task UpdateAsync_CustomerWithEmailAndPdf_SendsUpdatedEmailWithAttachment()
        {
            ArrangeExistingOrder();
            ArrangeCustomer(email: "joao@email.com");
            Task<EmailMessageDto> sentEmail = CaptureSentEmailAsync();

            await _service.UpdateAsync(PersistedId, DocumentSaveDtoBuilder.Order(pdf: Pdf), TestContext.Current.CancellationToken);

            EmailMessageDto email = await sentEmail;
            email.Subject.ShouldBe("Pedido atualizado com sucesso!");
            email.Body.ShouldBe("Seu pedido de código: 15 foi atualizado com sucesso e encontra-se em anexo.");
            email.Attachments.ShouldHaveSingleItem().FileName.ShouldBe("pedido-15.pdf");
        }
    }
}
