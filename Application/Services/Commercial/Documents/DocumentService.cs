using Application.DataTransferObjects.Commercial.Documents;
using Application.DataTransferObjects.Email;
using Application.DataTransferObjects.Entities;
using Application.DataTransferObjects.Listings;
using Application.Interfaces.Queries;
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
        private readonly ICustomerRepository _customerRepository;
        private readonly DocumentTypeInfo _documentTypeInfo;
        private readonly IProductRepository _productRepository;
        private readonly IServiceScopeFactory _serviceScopeFactory;

        protected DocumentService(IRepositoryCrud<TEntity> repository,
            IEntityCrudQuery<TDocumentDetailDto, TListing, TFilter> query,
            IUnitOfWork unitOfWork,
            ICustomerRepository customerRepository,
            IProductRepository productRepository,
            IServiceScopeFactory serviceScopeFactory,
            DocumentTypeInfo documentTypeInfo)
            : base(repository, query, unitOfWork)
        {
            _customerRepository = customerRepository;
            _documentTypeInfo = documentTypeInfo;
            _productRepository = productRepository;
            _serviceScopeFactory = serviceScopeFactory;
        }

        protected record DocumentTypeInfo(string Title, string FileName);

        protected abstract IEnumerable<DocumentProduct> GetProducts(TEntity entity);

        protected virtual void AppendExtraTemplateFields(TEntity entity, Dictionary<string, object> model) { }

        protected string GetDocumentFileName(TEntity entity) => $"{_documentTypeInfo.FileName}-{entity.Id}.pdf";

        protected object GetTemplateModel(TEntity entity, Customer customer, List<Product> products)
        {
            DocumentTypeInfo info = _documentTypeInfo;

            Dictionary<string, object> model = new()
            {
                ["DocumentTitle"] = info.Title,
                ["Id"] = entity.Id,
                ["ValueSubtotal"] = entity.ValueSubtotal.ToString("N2"),
                ["ValueTotal"] = entity.ValueTotal.ToString("N2"),
                ["CreationDateTime"] = entity.CreationDateTime.ToString("dd/MM/yyyy HH:mm"),
                ["CustomerName"] = customer.Name,
                ["CustomerDocument"] = customer.Document,
                ["Products"] = GetProducts(entity).Select(documentProduct =>
                {
                    Product product = products.Find(pr => pr.Id == documentProduct.ProductId) ??
                    throw new NullReferenceException($"{nameof(Product)} with {nameof(Product.Id)}: {documentProduct.ProductId} not found in {nameof(products)}");

                    return (object)new
                    {
                        Name = product.Description,
                        documentProduct.Quantity,
                        ValueUnit = documentProduct.ValueUnit.ToString("N2"),
                        ValueTotal = documentProduct.ValueTotal.ToString("N2"),
                    };
                }).ToList()
            };

            if (entity.Observations is not null)
                model["Observations"] = entity.Observations;

            AppendExtraTemplateFields(entity, model);

            return model;
        }

        protected Payment CreatePayment(TDocumentSaveDto dto)
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

            IEnumerable<long> productsIds = entities.SelectMany(e => GetProducts(e).Select(p => p.ProductId)).Distinct();
            List<Product> products = await _productRepository.GetByIdsAsync(productsIds, cancellationToken);

            foreach (TEntity entity in entities)
            {
                Customer? customer = customers.Find(c => c.Id == entity.CustomerId);

                if (customer?.Email is null)
                    continue;

                _ = Task.Run(async () =>
                {
                    await SentDocumentEmailToCustomerAsync(entitiesIdsToBeUpdated, products, entity, customer);
                }, CancellationToken.None);
            }
        }

        private async Task SentDocumentEmailToCustomerAsync(long[] entitiesIdsToBeUpdated, List<Product> products, TEntity entity, Customer customer)
        {
            await using AsyncServiceScope scope = _serviceScopeFactory.CreateAsyncScope();
            IPdfService pdfService = scope.ServiceProvider.GetRequiredService<IPdfService>();
            ITemplateRenderer templateRenderer = scope.ServiceProvider.GetRequiredService<ITemplateRenderer>();
            IEmailService emailService = scope.ServiceProvider.GetRequiredService<IEmailService>();

            byte[] pdfBytes = await pdfService.GenerateFromHtmlAsync(templateRenderer.Render("document", GetTemplateModel(entity, customer, products)), CancellationToken.None);
            EmailAttachmentDto attachment = new(GetDocumentFileName(entity), pdfBytes, "application/pdf");
            string action = entitiesIdsToBeUpdated.Contains(entity.Id) ? "atualizado" : "criado";
            EmailMessageDto email = new(customer.Email!, $"{_documentTypeInfo.Title} {action} com sucesso!", $"Seu {_documentTypeInfo.Title.ToLower()} de código: {entity.Id} foi {action} com sucesso e encontra-se em anexo.", [attachment]);
            await emailService.SendAsync(email, CancellationToken.None);
        }
    }
}
