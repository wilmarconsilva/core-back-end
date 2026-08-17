using Application.DataTransferObjects.Commercial.Documents;
using Application.DataTransferObjects.Commercial.Documents.DocumentsProducts;
using Application.DataTransferObjects.Email;
using Application.DataTransferObjects.Entities;
using Application.DataTransferObjects.Listings;
using Application.Interfaces.Queries;
using Application.Interfaces.Security;
using Application.Interfaces.Services;
using Domain.Entities;
using Domain.Entities.Commercial.Documents;
using Domain.Entities.Persons.Customers;
using Domain.Interfaces.Data;
using Domain.Interfaces.Entities.Commercial;
using Domain.Interfaces.Repositories;
using Domain.ValueObjects.Commercial.Documents.Payments;
using Domain.ValueObjects.Commercial.Documents.Payments.Installments;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Application.Services.Commercial.Documents
{
    public abstract class DocumentService<TEntity, TDocumentSaveDto, TDocumentDetailDto, TDocumentProductSaveDto, TListing, TFilter> : CrudService<TEntity, TDocumentSaveDto, TDocumentDetailDto, TListing, TFilter>
        where TEntity : Entity, IDocument
        where TDocumentSaveDto : DocumentSaveDto<TDocumentProductSaveDto>
        where TDocumentDetailDto : class
        where TDocumentProductSaveDto : DocumentProductSaveDto
        where TListing : EntityListing
        where TFilter : ListingFilter
    {
        protected readonly IHttpContextCurrentUser _currentUser;

        private readonly ICustomerRepository _customerRepository;
        private readonly DocumentTypeInfo _documentTypeInfo;
        private readonly IServiceScopeFactory _serviceScopeFactory;
        private readonly ILogger _logger;

        protected DocumentService(IRepositoryCrud<TEntity> repository,
            IEntityCrudQuery<TDocumentDetailDto, TListing, TFilter> query,
            IUnitOfWork unitOfWork,
            ICustomerRepository customerRepository,
            IServiceScopeFactory serviceScopeFactory,
            DocumentTypeInfo documentTypeInfo,
            IHttpContextCurrentUser httpContextCurrentUser,
            ILogger logger)
            : base(repository, query, unitOfWork)
        {
            _customerRepository = customerRepository;
            _documentTypeInfo = documentTypeInfo;
            _currentUser = httpContextCurrentUser;
            _serviceScopeFactory = serviceScopeFactory;
            _logger = logger;
        }

        protected record DocumentTypeInfo(string Title, string FileName);

        protected abstract IEnumerable<DocumentProduct> GetProducts(TEntity entity);

        protected string GetDocumentFileName(TEntity entity) => $"{_documentTypeInfo.FileName}-{entity.Id}.pdf";

        protected static Payment CreatePayment(TDocumentSaveDto dto)
        {
            DownPayment? downPayment = null;
            if (dto.Payment.Installment?.DownPayment != null)
                downPayment = new DownPayment(dto.Payment.Installment.DownPayment.Value, dto.Payment.Installment.DownPayment.Percentage, dto.Payment.Installment.DownPayment.Method);

            Installment? installment = null;
            if (dto.Payment.Installment != null)
                installment = new Installment(dto.Payment.Installment.Quantity, downPayment);

            Payment payment = new Payment(dto.Payment.Method, dto.Payment.DiscountValue, installment);

            return payment;
        }

        protected override async Task SaveAsync(List<TEntity> entities, CancellationToken cancellationToken)
        {
            IEnumerable<long> customerIds = entities.Select(e => e.CustomerId).Distinct();
            List<Customer> customers = await _customerRepository.GetByIdsAsync(customerIds, cancellationToken);

            bool existsCustomerWithEmail = customers.Exists(c => c.Email != null);

            if (existsCustomerWithEmail)
            {
                await SaveAsync(entities, customers, cancellationToken);
            }
            else
                await base.SaveAsync(entities, cancellationToken);
        }

        private async Task SaveAsync(List<TEntity> entities, List<Customer> customers, CancellationToken cancellationToken)
        {
            long[] entitiesIdsToBeUpdated = entities.Where(e => e.Id > 0).Select(e => e.Id).ToArray();

            await base.SaveAsync(entities, cancellationToken);

            foreach (TEntity entity in entities)
            {
                Customer? customer = customers.Find(c => c.Id == entity.CustomerId);

                if (customer?.Email is null || entity.PDF is null)
                    continue;

                _ = Task.Run(async () =>
                {
                    try
                    {
                        await SendPdfByEmailToCustomerAsync(
                            entitiesIdsToBeUpdated,
                            entity,
                            customer);
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(
                            ex,
                            $"Eror while sending PDF by e-mail to customer of {nameof(Customer.Id)}: {{CustomerId}}",
                            customer.Id);
                    }
                });
            }
        }

        private async Task SendPdfByEmailToCustomerAsync(long[] entitiesIdsToBeUpdated, TEntity entity, Customer customer)
        {
            await using AsyncServiceScope scope = _serviceScopeFactory.CreateAsyncScope();
            IEmailService emailService = scope.ServiceProvider.GetRequiredService<IEmailService>();

            EmailAttachmentDto attachment = new(GetDocumentFileName(entity), entity.PDF!, "application/pdf");
            string action = entitiesIdsToBeUpdated.Contains(entity.Id) ? "atualizado" : "criado";
            EmailMessageDto email = new(customer.Email!, $"{_documentTypeInfo.Title} {action} com sucesso!", $"Seu {_documentTypeInfo.Title.ToLower()} de código: {entity.Id} foi {action} com sucesso e encontra-se em anexo.", [attachment]);
            await emailService.SendAsync(email, CancellationToken.None);
        }
    }
}
