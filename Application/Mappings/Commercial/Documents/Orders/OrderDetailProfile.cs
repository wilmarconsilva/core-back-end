using Application.DataTransferObjects.Commercial.Documents.Orders;
using Application.DataTransferObjects.Persons.Customers;
using Application.DataTransferObjects.Products;
using AutoMapper;
using Domain.Entities.Commercial.Documents.Orders;

namespace Application.Mappings.Commercial.Documents.Orders
{
    public class OrderDetailProfile : Profile
    {
        public OrderDetailProfile()
        {
            CreateMap<Order, OrderDetailDto>()
                .ForMember(d => d.Customer, opt => opt.MapFrom(s =>
                    s.Customer != null
                        ? new CustomerReferenceDto
                        {
                            Id = s.Customer.Id,
                            Name = s.Customer.Name,
                            Document = s.Customer.Document,
                        }
                        : new CustomerReferenceDto { Id = s.CustomerId }));

            CreateMap<OrderProduct, OrderProductDetailDto>()
                .ForMember(d => d.Product, opt => opt.MapFrom(s =>
                    new ProductReferenceDto
                    {
                        Id = s.Product != null ? s.Product.Id : s.ProductId,
                        Description = s.Product != null ? s.Product.Description : null,
                    }));
        }
    }
}
