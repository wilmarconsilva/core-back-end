using Application.DataTransferObjects.Commercial.Documents;
using Application.DataTransferObjects.Email;
using Application.DataTransferObjects.Entities;
using Application.DataTransferObjects.Listings;
using Application.Interfaces.Queries;
using Application.Interfaces.Services;
using Domain.Entities;
using Domain.Entities.Persons.Customers;
using Domain.Interfaces.Data;
using Domain.Interfaces.Repositories;
using Domain.ValueObjects.Commercial.Documents.Payments;
using Domain.ValueObjects.Commercial.Documents.Payments.Installments;

namespace Application.Services.Commercial.Documents
{
    public abstract class DocumentService<TEntity, TDocumentSaveDto, TDocumentProductSaveDto, TListing, TFilter> : CrudService<TEntity, TDocumentSaveDto, TListing, TFilter>
        where TEntity : Entity
        where TDocumentSaveDto : DocumentSaveDto<TDocumentProductSaveDto>
        where TDocumentProductSaveDto : DocumentProductSaveDto
        where TListing : EntityListing
        where TFilter : ListingFilter
    {
        private readonly IEmailService _emailService;
        private readonly ICustomerRepository _customerRepository;
        private readonly IPdfService _pdfService;
        private readonly ITemplateRenderer _templateRenderer;

        protected DocumentService(IRepositoryCrud<TEntity> repository, IEntitySaveDtoQuery<TDocumentSaveDto> query, IUnitOfWork unitOfWork, IEmailService emailService, ICustomerRepository customerRepository, IPdfService pdfService, ITemplateRenderer templateRenderer)
            : base(repository, query, unitOfWork)
        {
            _emailService = emailService;
            _customerRepository = customerRepository;
            _pdfService = pdfService;
            _templateRenderer = templateRenderer;
        }

        protected abstract long GetCustomerId(TEntity entity);

        protected abstract string GetEmailSubject(TEntity entity);

        protected abstract string GetEmailBody(TEntity entity);

        protected abstract string GetTemplateName();

        protected abstract object GetTemplateModel(TEntity entity, Customer customer);

        protected abstract string GetDocumentFileName(TEntity entity);

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
            await base.SaveAsync(entities, cancellationToken);

            IEnumerable<long> customerIds = entities.Select(GetCustomerId);
            List<Customer> customers = await _customerRepository.GetByIdsAsync(customerIds, cancellationToken);

            List<Task> emailTasks = new(entities.Count);

            for (int i = 0; i < entities.Count; i++)
            {
                TEntity entity = entities[i];
                Customer? customer = customers.Find(c => c.Id == GetCustomerId(entity));

                if (customer?.Email is null)
                    continue;

                byte[] pdfBytes = await _pdfService.GenerateFromHtmlAsync(_templateRenderer.Render(GetTemplateName(), GetTemplateModel(entity, customer)), cancellationToken);
                EmailAttachmentDto attachment = new(GetDocumentFileName(entity), pdfBytes);
                EmailMessageDto email = new(customer.Email, GetEmailSubject(entity), GetEmailBody(entity), [attachment]);
                emailTasks.Add(_emailService.SendAsync(email, cancellationToken));
            }

            await Task.WhenAll(emailTasks);
        }
    }
}
