using Domain.Documents.Entities;
using Infrastructure.Shared;
using Infrastructure.Shared.Persistance;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Documents
{
    public abstract class DocumentRepository<IDocument, IDocumentProduct> : RepositoryCrud<IDocument>
        where IDocument : Document<IDocumentProduct>
        where IDocumentProduct : DocumentProduct
    {
        protected DocumentRepository(AppDbContext context) : base(context)
        {
        }


        protected override IQueryable<IDocument> ApplyIncludesForGetByIds(IQueryable<IDocument> query)
        {
            return query.Include(d => d.Products).ThenInclude(p => p.Product);
        }
    }
}
