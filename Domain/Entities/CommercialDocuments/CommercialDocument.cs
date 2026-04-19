using Domain.Entities.Persons;

namespace Domain.Entities.CommercialDocuments
{
    
public abstract class CommercialDocument : Entity
{
    public required DateTime CreationDateTime { get; private set; }
    public DateTime? LastUpdateDateTime { get; private set; }
    public required long CustomerId { get; private set; }
    public Customer? Customer { get; private set; }
    public required long UserId { get; private set; }
    public User? User { get; private set; }
    public decimal ValuePayable { get; private set; }
    public decimal ValueTotal { get; private set; }
}

}