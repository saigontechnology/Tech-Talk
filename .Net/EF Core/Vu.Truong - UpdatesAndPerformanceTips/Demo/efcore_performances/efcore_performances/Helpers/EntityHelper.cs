namespace efcore_performances.Helpers;
internal static class EntityHelper
{
    public static AddressEntity GenerateAddress()
    {
        return new AddressEntity
        {
            City = Faker.Address.City(),
            Country = Faker.Address.Country(),
            Postcode = Faker.Address.UkPostCode(),
            Street = Faker.Address.StreetAddress()
        };
    }

    public static DeliveryDetailEntity GenerateDeliveryDetail()
    {
        return new DeliveryDetailEntity
        {
            Note = Faker.Lorem.Sentence(),
            Tips = Faker.RandomNumber.Next(1000, 5000),
            Address = GenerateAddress()
        };
    }
}
