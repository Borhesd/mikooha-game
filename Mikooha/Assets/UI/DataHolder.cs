using UnityEngine;
using System.Collections.Generic;

public static class DataHolder
{
    private static Dictionary<string, object> values = new Dictionary<string, object>();

    public static void AddFloat(string key, float value)
    {
        if (values.ContainsKey(key))
            values.Remove(key);

        values.Add(key, value);
    }

    public static float GetFloat(string key)
    {
        if (values.TryGetValue(key, out var value))
            return (float)value;
        else
            return 0f;
    }
}
