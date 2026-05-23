using System.Reflection;

public static class PropertyLogger
{
    public static List<string> GetLog(object obj)
    {
        if (obj == null)
            return new List<string>();

        var result = new List<string>();
        var properties = obj.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);

        foreach (var prop in properties)
        {
            var value = prop.GetValue(obj);
            if (value != null)
            {
                result.Add($"[{prop.Name}]: [{value}]");
            }
        }

        return result;
    }
}
