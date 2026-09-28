using Application.Mappings;
using Application.Territories.Localities.DataTransferObjects;
using Domain.Territories.Localities.Entities;

namespace Application.Mappings.Territories
{
    public class LocalityProfile : CrudProfile<LocalitySaveDto, Locality>
    {
    }
}
