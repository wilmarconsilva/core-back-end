using Application.Documents.Budgets.DataTransferObjects;
using AutoMapper;
using Domain.Documents.Budgets.Entities;

namespace Application.Documents.Budgets.Mappings
{
    public class BudgetProfile : Profile
    {
        public BudgetProfile()
        {
            CreateMap<BudgetSaveDto, Budget>();
        }
    }
}
