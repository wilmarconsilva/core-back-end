using Application.DataTransferObjects.Commercial.Documents.Orders;
using AutoMapper;
using Domain.Entities.Commercial.Documents.Orders;

namespace Application.Mappings.Commercial.Documents.Orders
{
    public class OrderProductProfile : Profile
    {
        public OrderProductProfile()
        {
            CreateMap<OrderProductSaveDto, OrderProduct>();
        }
    }
}
