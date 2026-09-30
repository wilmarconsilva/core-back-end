using Domain.Documents.Entities;
using Domain.Documents.ValueObjects.Payments;
using Infrastructure.Shared;
using Infrastructure.Shared.Persistance;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Documents
{
    public abstract class DocumentoRepository<IDocument, IDocumentProduct, IDocumentPayment> : RepositoryCrud<IDocument>
        where IDocument : Document<IDocumentProduct, IDocumentPayment>
        where IDocumentProduct : DocumentProduct
        where IDocumentPayment : Payment
    {
        protected DocumentoRepository(AppDbContext context) : base(context)
        {
        }


        protected override IQueryable<IDocument> ApplyIncludesForGetByIds(IQueryable<IDocument> query)
        {
            return query.Include(d => d.Products).ThenInclude(p => p.Product);
        }
    }
}
