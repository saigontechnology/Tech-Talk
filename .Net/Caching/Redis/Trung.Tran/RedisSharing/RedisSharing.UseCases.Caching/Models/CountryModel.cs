namespace RedisSharing.UseCases.Caching.Models
{
    public class CountryObject
    {
        public IEnumerable<CountryModel> Countries { get; set; }
    }

    public class CountryModel
    {
        public string Country { get; set; }
        public IEnumerable<string> States { get; set; }
    }
}
