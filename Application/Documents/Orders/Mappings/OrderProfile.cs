using Application.Documents.Orders.DataTransferObjects;
using AutoMapper;
using Domain.Documents.Orders.Entities;

namespace Application.Documents.Orders.Mappings
{
    public class OrderProfile : Profile
    {
        public OrderProfile()
        {
            CreateMap<OrderSaveDto, Order>();
        }
    }
}
