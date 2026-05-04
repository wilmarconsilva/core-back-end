using Application.DataTransferObjects.Commercial.Documents.Budgets;
using Application.DataTransferObjects.Persons.Customers;
using Application.DataTransferObjects.Products;
using AutoMapper;
using Domain.Entities.Commercial.Documents.Budgets;

namespace Application.Mappings.Commercial.Documents.Budgets
{
    public class BudgetDetailProfile : Profile
    {
        public BudgetDetailProfile()
        {
            CreateMap<Budget, BudgetDetailDto>()
                .ForMember(d => d.Customer, opt => opt.MapFrom(s =>
                    s.Customer != null
                        ? new CustomerReferenceDto
                        {
                            Id = s.Customer.Id,
                            Name = s.Customer.Name,
                            Document = s.Customer.Document,
                        }
                        : new CustomerReferenceDto { Id = s.CustomerId }));

            CreateMap<BudgetProduct, BudgetProductDetailDto>()
                .ForMember(d => d.Product, opt => opt.MapFrom(s =>
                    new ProductReferenceDto
                    {
                        Id = s.Product != null ? s.Product.Id : s.ProductId,
                        Description = s.Product != null ? s.Product.Description : null,
                    }));
        }
    }
}
