using System.Reflection;

namespace BuildingBlock.Domain.Shared.Extensions
{
    public static class ObjectExtensions
    {
        public static object? GetPropertyValue(object obj, string propertyName)
        {
            if (obj == null || string.IsNullOrWhiteSpace(propertyName))
                return null;

            PropertyInfo? propertyInfo = obj.GetType().GetProperty(propertyName, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
            if (propertyInfo != null)
                return propertyInfo.GetValue(obj, null);
            return null;
        }
    }
}
