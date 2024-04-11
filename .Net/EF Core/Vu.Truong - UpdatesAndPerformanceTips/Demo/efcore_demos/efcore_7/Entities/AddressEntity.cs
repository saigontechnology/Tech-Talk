using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace efcore_demos.Entities;
internal class AddressEntity
{
    public AddressEntity()
    {
        Id = Guid.NewGuid().ToString();
    }

    public string Street { get; set; }
    public string City { get; set; }
    public string Postcode { get; set; }
    public string Country { get; set; }

    [JsonPropertyName("Id")]
    [JsonInclude]
    private string _identifier;

    [NotMapped]
    [JsonIgnore]
    public string Id
    {
        get => _identifier;
        set => _identifier = value;
    }
}