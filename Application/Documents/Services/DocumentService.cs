using Application.Authentication.Interfaces;
using Application.Documents.DataTransferObjects;
using Application.Documents.DataTransferObjects.DocumentsProducts;
using Application.Documents.DataTransferObjects.Payments.Installments;
using Application.Email.DataTransferObjects;
using Application.Email.Services.Interfaces;
using Application.Shared.DataTransferObjects.Entities;
using Application.Shared.DataTransferObjects.Listings;
using Application.Shared.Queries.Interfaces;
using Application.Shared.Services;
using Domain.Documents.Entities;
using Domain.Documents.Interfaces;
using Domain.Documents.ValueObjects.Payments.Installments;
using Domain.Persons.Customers.Entities;
using Domain.Persons.Customers.Repositories.Interfaces;
using Domain.Shared.Entities;
using Domain.Shared.Persistance.Interfaces;
using Domain.Shared.Repositories.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Application.Documents.Services
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

        protected static Installment? CreateInstallment(InstallmentSaveDto? dto)
        {
            if (dto == null)
                return null;

            return new Installment(dto.Quantity, CreateDownPayment(dto.DownPayment));
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

        private static DownPayment? CreateDownPayment(DownPaymentSaveDto? dto)
        {
            if (dto == null)
                return null;

            return new DownPayment(dto.Value, dto.Percentage, dto.Method);
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
