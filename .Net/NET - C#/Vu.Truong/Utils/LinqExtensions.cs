namespace Utils;
public static class LinqExtensions
{
    public static List<T> InitIfNull<T>(this List<T> list)
    {
        return list ?? Enumerable.Empty<T>().ToList();
    }

    public static IEnumerable<T> InitIfNull<T>(this IEnumerable<T> list)
    {
        return list ?? Enumerable.Empty<T>();
    }

    public static T[] InitIfNull<T>(this T[] array)
    {
        return array ?? Array.Empty<T>();
    }

    public static bool IsNullOrEmpty<T>(this IEnumerable<T> enumerable)
    {
        return enumerable is null || !enumerable.Any();
    }

    public static bool ContainsDuplicates<T>(this IEnumerable<T> enumerable)
    {
        HashSet<T> set = new();

        foreach (var element in enumerable)
        {
            if (!set.Add(element))
            {
                return true;
            }
        }

        return false;
    }

    public static Dictionary<string, string> ToFormData(this object obj)
    {
        var result = new Dictionary<string, string>();

        if (obj is not null)
        {
            var properties = obj.GetType().GetProperties();

            foreach (var property in properties)
            {
                try
                {
                    var name = System.Text.Json.JsonNamingPolicy.CamelCase.ConvertName(property.Name);
                    result.TryAdd(name, property.GetValue(obj)?.ToString());
                }
                catch { }
            }
        }

        return result;
    }

    public static (List<string>, List<string>) ExceptData(this List<string> t1, List<string> t2)
    {
        t1 = t1.InitIfNull().Except(t2.InitIfNull()).ToList();
        t2 = t2.InitIfNull().Except(t1.InitIfNull()).ToList();

        return (t1, t2);
    }
}
