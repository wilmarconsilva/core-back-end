namespace Application.DataTransferObjects
{
    public abstract class EntitySaveDto
    {
        [System.Text.Json.Serialization.JsonIgnore]
        public long Id { get; set; }
    }
}