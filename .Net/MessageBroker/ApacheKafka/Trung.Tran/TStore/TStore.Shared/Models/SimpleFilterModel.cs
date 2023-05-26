namespace TStore.Shared.Models
{
    public class SimpleFilterModel
    {
        public string Terms { get; set; }
        public int? Page { get; set; }
        public int? PageSize { get; set; }
    }
}
