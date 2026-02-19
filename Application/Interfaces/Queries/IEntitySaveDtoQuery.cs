using Application.DataTransferObjects;

namespace Application.Interfaces.Queries
{
    public interface IEntitySaveDtoQuery<TEntitySaveDto> where TEntitySaveDto : EntitySaveDto
    {
        Task<TEntitySaveDto?> GetSaveDto(long id, CancellationToken cancellationToken);
    }
}