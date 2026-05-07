using Application.DataTransferObjects.Commercial.Documents.Orders;
using AutoMapper;
using Domain.Entities.Commercial.Documents.Orders;

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
