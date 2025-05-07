using System.Reflection;

namespace PlanningBook.Repository.EF.Extensions
{
    public static class RefectionExtensions
    {
        public static object? GetPropertyValue(object obj, string property)
        {
            if (!string.IsNullOrEmpty(property))
            {
                PropertyInfo? propertyInfo = obj.GetType().GetProperty(property, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
                if (propertyInfo != null)
                    return propertyInfo.GetValue(obj, null);
                return null;
            }
            return null;
        }
    }
}
