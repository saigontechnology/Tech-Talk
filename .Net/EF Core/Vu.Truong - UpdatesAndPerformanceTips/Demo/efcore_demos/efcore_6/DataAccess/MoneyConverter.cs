using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System.Text.Json;

namespace efcore_demos.DataAccess;
public class MoneyConverter : ValueConverter<Money, string>
{
    public MoneyConverter()
        : base(
            v => JsonSerializer.Serialize(v, (JsonSerializerOptions)null),
            v => JsonSerializer.Deserialize<Money>(v, (JsonSerializerOptions)null))
    {
    }
}