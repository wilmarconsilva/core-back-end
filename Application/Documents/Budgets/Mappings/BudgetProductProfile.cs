using Application.Documents.Budgets.DataTransferObjects;
using AutoMapper;
using Domain.Documents.Budgets.Entities;

namespace Application.Documents.Budgets.Mappings
{
    public class BudgetProductProfile : Profile
    {
        public BudgetProductProfile()
        {
            CreateMap<BudgetProductSaveDto, BudgetProduct>();
        }
    }
}
