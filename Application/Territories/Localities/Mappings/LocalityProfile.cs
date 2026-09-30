using Application.Shared.Mappings;
using Application.Territories.Localities.DataTransferObjects;
using Domain.Territories.Localities.Entities;

namespace Application.Territories.Localities.Mappings
{
    public class LocalityProfile : CrudProfile<LocalitySaveDto, Locality>
    {
    }
}
