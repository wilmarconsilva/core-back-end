using Application.DataTransferObjects.Commercial.Documents.Budgets;
using AutoMapper;
using Domain.Entities.Commercial.Documents.Budgets;

namespace Application.Mappings.Commercial.Documents.Budgets
{
    public class BudgetProfile : Profile
    {
        public BudgetProfile()
        {
            CreateMap<BudgetSaveDto, Budget>();
        }
    }
}
