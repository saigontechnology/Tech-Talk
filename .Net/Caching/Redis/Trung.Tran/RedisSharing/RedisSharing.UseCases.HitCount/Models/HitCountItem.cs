namespace RedisSharing.UseCases.HitCount.Models
{
    public class HitCountItem
    {
        public string Key { get; set; }
        public long Count { get; set; }
    }
}
