using Application.Authentication.Interfaces;
using Application.Email.DataTransferObjects;
using Application.Email.Services.Interfaces;
using Application.UnitTests.Builders;
using Domain.Persons.Customers.Entities;
using Domain.Persons.Customers.Repositories.Interfaces;
using Domain.Shared.Persistance.Interfaces;
using Domain.UnitTests.Builders;
using Microsoft.Extensions.DependencyInjection;

namespace Application.UnitTests.Documents.Services
{
    /// <summary>
    /// Dependências comuns aos services de documento (orçamento e pedido), incluindo o envio do PDF por e-mail,
    /// que roda em background (fire-and-forget) em um escopo de DI próprio.
    /// </summary>
    public abstract class DocumentServiceTestsBase
    {
        protected const long CurrentUserId = 3;
        protected const long PersistedId = 15;

        protected static readonly TimeSpan EmailTimeout = TimeSpan.FromSeconds(5);
        protected static readonly byte[] Pdf = [37, 80, 68, 70];

        protected readonly IUnitOfWork _unitOfWork = Substitute.For<IUnitOfWork>();
        protected readonly ICustomerRepository _customerRepository = Substitute.For<ICustomerRepository>();
        protected readonly IServiceScopeFactory _serviceScopeFactory = Substitute.For<IServiceScopeFactory>();
        protected readonly IHttpContextCurrentUser _currentUser = Substitute.For<IHttpContextCurrentUser>();
        protected readonly IEmailService _emailService = Substitute.For<IEmailService>();

        protected DocumentServiceTestsBase()
        {
            _currentUser.UserId.Returns(CurrentUserId);

            IServiceScope scope = Substitute.For<IServiceScope>();
            scope.ServiceProvider.GetService(typeof(IEmailService)).Returns(_emailService);
            _serviceScopeFactory.CreateScope().Returns(scope);
        }

        protected void ArrangeCustomer(string? email)
        {
            Customer customer = CustomerBuilder.Customer(email: email);
            customer.Id = DocumentSaveDtoBuilder.CustomerId;
            _customerRepository.GetByIdsAsync(Arg.Any<IEnumerable<long>>(), Arg.Any<CancellationToken>()).Returns([customer]);
        }

        protected Task<EmailMessageDto> CaptureSentEmailAsync()
        {
            TaskCompletionSource<EmailMessageDto> sent = new(TaskCreationOptions.RunContinuationsAsynchronously);

            _emailService
                .When(e => e.SendAsync(Arg.Any<EmailMessageDto>(), Arg.Any<CancellationToken>()))
                .Do(call => sent.TrySetResult(call.Arg<EmailMessageDto>()));

            return sent.Task.WaitAsync(EmailTimeout, TestContext.Current.CancellationToken);
        }
    }
}
