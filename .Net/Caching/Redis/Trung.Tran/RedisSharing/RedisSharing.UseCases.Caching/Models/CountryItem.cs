namespace RedisSharing.UseCases.Caching.Models
{
    public class CountryItem
    {
        public string Name { get; set; }

        public virtual ICollection<CountryState> States { get; set; }
    }
}
