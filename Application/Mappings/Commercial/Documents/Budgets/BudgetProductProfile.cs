using Application.DataTransferObjects.Commercial.Documents.Budgets;
using AutoMapper;
using Domain.Entities.Commercial.Documents.Budgets;

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
