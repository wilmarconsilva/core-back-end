using Application.Shared.DataTransferObjects.Entities;

namespace Application.Territories.Localities.DataTransferObjects.Synchronizations
{
    /// <summary>
    /// Localidade para o snapshot offline do front-end (inclui a sigla do estado).
    /// </summary>
    public class LocalitySynchronizationDto : EntityListing
    {
        public required string Description { get; init; }

        public required long StateId { get; init; }

        public required string StateAcronym { get; init; }
    }
}
