using Domain.Entities;

namespace Application.DataTransferObjects
{
    public class EntityDto<TEntity, TDto> where TEntity : Entity where TDto : class
    {
        public required TEntity Entity { get; set; }

        public required TDto Dto { get; set; }
    }
}