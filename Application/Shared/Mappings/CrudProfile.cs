using AutoMapper;

namespace Application.Shared.Mappings
{
    public abstract class CrudProfile<TDto, TEntity> : Profile
        where TDto : class
        where TEntity : class
    {
        protected CrudProfile()
        {
            CreateMap<TDto, TEntity>();
            CreateMap<TEntity, TDto>();
        }
    }
}
