using Application.Documents.Budgets.DataTransferObjects;
using Application.Documents.Budgets.Queries.Interfaces;
using Application.Documents.Budgets.Services;
using Application.Documents.DataTransferObjects.Payments;
using Application.Email.DataTransferObjects;
using Application.UnitTests.Builders;
using Application.UnitTests.Documents.Services;
using Application.UnitTests.Fixtures;
using Domain.Documents.Budgets.Entities;
using Domain.Documents.Budgets.Repositories.Interfaces;
using Domain.Documents.ValueObjects.Payments.Enums;
using Domain.Shared.Entities;
using Domain.UnitTests.Builders;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging.Abstractions;

namespace Application.UnitTests.Documents.Budgets.Services
{
    public class BudgetServiceTests : DocumentServiceTestsBase, IClassFixture<MapperFixture>
    {
        private readonly IBudgetRepository _repository = Substitute.For<IBudgetRepository>();
        private readonly BudgetService _service;

        public BudgetServiceTests(MapperFixture fixture)
        {
            _service = new BudgetService(_repository, Substitute.For<IBudgetQuery>(), _unitOfWork, fixture.Mapper, _customerRepository,
                _serviceScopeFactory, _currentUser, NullLogger<BudgetService>.Instance);

            _repository
                .When(r => r.SaveAsync(Arg.Any<IEnumerable<Budget>>(), Arg.Any<CancellationToken>()))
                .Do(call => { foreach (Budget budget in call.Arg<IEnumerable<Budget>>()) budget.Id = PersistedId; });

            ArrangeCustomer(email: null);
        }

        private Budget ArrangeExistingBudget()
        {
            Budget existing = DocumentBuilder.Budget();
            existing.Id = PersistedId;
            existing.Products.Single().Id = 1;
            _repository.GetByIdsAsync(Arg.Any<IEnumerable<long>>(), Arg.Any<CancellationToken>()).Returns([existing]);
            return existing;
        }

        [Fact]
        public async Task CreateAsync_ValidDto_CreatesBudgetForCurrentUser()
        {
            BudgetSaveDto dto = DocumentSaveDtoBuilder.Budget(
                payment: new PaymentSaveDto { Method = null, DiscountValue = 10, Installment = DocumentSaveDtoBuilder.Installment(3, downPaymentValue: 27) });

            IEnumerable<Entity> entities = await _service.CreateAsync([dto], TestContext.Current.CancellationToken);

            Budget budget = entities.ShouldHaveSingleItem().ShouldBeOfType<Budget>();
            budget.SellerId.ShouldBe(CurrentUserId);
            budget.CustomerId.ShouldBe(DocumentSaveDtoBuilder.CustomerId);
            budget.CreationDateTime.ShouldBe(DocumentSaveDtoBuilder.CreationDateTime);
            budget.ValueTotal.ShouldBe(90);
            budget.Payment.Method.ShouldBeNull();
            budget.Payment.Installment!.Quantity.ShouldBe(3);
            budget.Payment.Installment.Value.ShouldBe(21);
            budget.Payment.Installment.DownPayment!.Method.ShouldBe(PaymentMethod.Money);
            budget.Payment.Installment.DownPayment.Percentage.ShouldBe(30);
            budget.Products.ShouldHaveSingleItem().ProductId.ShouldBe(10);
        }

        [Fact]
        public async Task UpdateAsync_ExistingBudget_FillsLoadedEntity()
        {
            Budget existing = ArrangeExistingBudget();

            await _service.UpdateAsync(PersistedId, DocumentSaveDtoBuilder.Budget(products: [DocumentSaveDtoBuilder.BudgetProduct(quantity: 5, valueUnit: 10)]), TestContext.Current.CancellationToken);

            existing.SellerId.ShouldBe(CurrentUserId);
            existing.ValueTotal.ShouldBe(50);
            await _repository.Received(1).SaveAsync(Arg.Is<IEnumerable<Budget>>(b => b.Single() == existing), Arg.Any<CancellationToken>());
        }

        [Fact]
        public async Task CreateAsync_CustomerWithEmailAndPdf_SendsCreatedEmailWithAttachment()
        {
            ArrangeCustomer(email: "joao@email.com");
            Task<EmailMessageDto> sentEmail = CaptureSentEmailAsync();

            await _service.CreateAsync([DocumentSaveDtoBuilder.Budget(pdf: Pdf)], TestContext.Current.CancellationToken);

            EmailMessageDto email = await sentEmail;
            email.To.ShouldBe("joao@email.com");
            email.Subject.ShouldBe("Orçamento criado com sucesso!");
            email.Body.ShouldBe("Seu orçamento de código: 15 foi criado com sucesso e encontra-se em anexo.");
            EmailAttachmentDto attachment = email.Attachments.ShouldHaveSingleItem();
            attachment.FileName.ShouldBe("orcamento-15.pdf");
            attachment.Content.ShouldBe(Pdf);
            attachment.MediaType.ShouldBe("application/pdf");
        }

        [Fact]
        public async Task UpdateAsync_CustomerWithEmailAndPdf_SendsUpdatedEmail()
        {
            ArrangeExistingBudget();
            ArrangeCustomer(email: "joao@email.com");
            Task<EmailMessageDto> sentEmail = CaptureSentEmailAsync();

            await _service.UpdateAsync(PersistedId, DocumentSaveDtoBuilder.Budget(pdf: Pdf), TestContext.Current.CancellationToken);

            (await sentEmail).Subject.ShouldBe("Orçamento atualizado com sucesso!");
        }

        [Fact]
        public async Task CreateAsync_CustomerWithoutEmail_DoesNotSendEmail()
        {
            await _service.CreateAsync([DocumentSaveDtoBuilder.Budget(pdf: Pdf)], TestContext.Current.CancellationToken);

            _serviceScopeFactory.DidNotReceive().CreateScope();
            await _repository.Received(1).SaveAsync(Arg.Any<IEnumerable<Budget>>(), Arg.Any<CancellationToken>());
        }

        [Fact]
        public async Task CreateAsync_WithoutPdf_DoesNotSendEmail()
        {
            ArrangeCustomer(email: "joao@email.com");

            await _service.CreateAsync([DocumentSaveDtoBuilder.Budget(pdf: null)], TestContext.Current.CancellationToken);

            _serviceScopeFactory.DidNotReceive().CreateScope();
        }

        [Fact]
        public async Task CreateAsync_EmailFailure_DoesNotPropagate()
        {
            ArrangeCustomer(email: "joao@email.com");
            TaskCompletionSource attempted = new(TaskCreationOptions.RunContinuationsAsynchronously);
            _emailService
                .When(e => e.SendAsync(Arg.Any<EmailMessageDto>(), Arg.Any<CancellationToken>()))
                .Do(_ => { attempted.TrySetResult(); throw new InvalidOperationException("SMTP indisponível"); });

            await Should.NotThrowAsync(() => _service.CreateAsync([DocumentSaveDtoBuilder.Budget(pdf: Pdf)], TestContext.Current.CancellationToken));

            await attempted.Task.WaitAsync(EmailTimeout, TestContext.Current.CancellationToken);
            await _unitOfWork.Received(1).CommitAsync(Arg.Any<CancellationToken>());
        }
    }
}
