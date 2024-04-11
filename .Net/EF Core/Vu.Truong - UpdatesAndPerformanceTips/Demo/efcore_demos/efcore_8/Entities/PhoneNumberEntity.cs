using System.ComponentModel.DataAnnotations.Schema;

namespace efcore_demos.Entities;

internal record PhoneNumberEntity(int CountryCode, long Number);