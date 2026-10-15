
namespace Application.Shared.Helps
{
    public static class ToDicionary
    {
        public static Dictionary<string, object> ConvertDicionary(object obj)
        {
            return obj.GetType()
                .GetProperties()
                .Where(prop => prop.GetValue(obj) != null && prop.Name != "SqlCode") 
                .ToDictionary(prop => prop.Name, prop => prop.GetValue(obj) ?? string.Empty);
        }
    }
}
