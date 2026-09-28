using Application.Documents.Budgets.DataTransferObjects;
using AutoMapper;
using Domain.Documents.Budgets.Entities;

namespace Application.Mappings.Commercial.Documents.Budgets
{
    public class BudgetProductProfile : Profile
    {
        public BudgetProductProfile()
        {
            CreateMap<BudgetProductSaveDto, BudgetProduct>();
        }
    }
}
