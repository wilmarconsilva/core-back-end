using Application.Documents.Orders.DataTransferObjects;
using AutoMapper;
using Domain.Documents.Orders.Entities;

namespace Application.Mappings.Commercial.Documents.Orders
{
    public class OrderProfile : Profile
    {
        public OrderProfile()
        {
            CreateMap<OrderSaveDto, Order>();
        }
    }
}
