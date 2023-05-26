namespace RedisSharing.UseCases.Caching.Models
{
    public class CountryState
    {
        public string Name { get; set; }
        public string CountryName { get; set; }

        public virtual CountryItem Country { get; set; }
    }
}
