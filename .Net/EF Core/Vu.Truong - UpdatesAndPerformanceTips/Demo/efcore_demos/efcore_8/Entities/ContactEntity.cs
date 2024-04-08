using System.ComponentModel.DataAnnotations.Schema;

namespace efcore_demos.Entities;

internal class ContactEntity
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public AddressEntity Address { get; set; }

    public PhoneNumberEntity PhoneNumber { get; set; }
}
