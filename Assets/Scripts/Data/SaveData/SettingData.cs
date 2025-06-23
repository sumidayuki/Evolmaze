using System;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SettingData
{
    public Dictionary<string, object> Data = new Dictionary<string, object>();

    public void Set(string key, object data)
    {
        if (Data.ContainsKey(key))
        {
            Data[key] = data;
        }
        else
        {
            Data.Add(key, data);
        }
    }

    public T Get<T>(string key, T defaultValue = default)
    {
        if (Data.TryGetValue(key, out object value))
        {
            return (T)Convert.ChangeType(value, typeof(T));
        }
        return defaultValue;
    }
}
