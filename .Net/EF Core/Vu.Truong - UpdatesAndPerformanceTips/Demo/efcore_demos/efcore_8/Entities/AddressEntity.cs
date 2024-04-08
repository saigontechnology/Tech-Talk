using System.ComponentModel.DataAnnotations.Schema;

namespace efcore_demos.Entities;

internal class AddressEntity
{
    public string Street { get; set; }
    public string City { get; set; }
    public string Postcode { get; set; }
    public string Country { get; set; }
}