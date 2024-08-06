namespace SharedDomains;
public static class DataGenerator
{
    public static IEnumerable<T> Generate<T>(this int amount) where T : BaseModel, new()
    {
        return Enumerable.Range(0, amount)
            .Select(x =>
            {
                var instance = new T();
                instance.Generate();
                return instance;
            });
    }
}
