namespace TAuth.ResourceAPI.Entities
{
    public class ResourceEntity : IOwnedEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string OwnerId { get; set; }
    }
}
