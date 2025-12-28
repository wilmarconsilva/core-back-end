using System.Diagnostics.CodeAnalysis;

namespace Domain.Entities.Territories
{
    public class Locality : Entity
    {
        public State? State { get; set; }
    }
}