using Application.DataTransferObjects;
using AutoMapper;
using Domain.Entities;

namespace Application.Mappings
{
    public abstract class CrudProfile<TDto, TEntity> : Profile
        where TDto : EntitySaveDto
        where TEntity : Entity
    {
        protected CrudProfile()
        {
            CreateMap<TDto, TEntity>();
            CreateMap<TEntity, TDto>();
        }
    }
}
