using System;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SaveData
{
    /// <summary>
    /// セーブデータを管理します。
    /// </summary>
    public Dictionary<string, object> Data = new Dictionary<string, object>();

    /// <summary>
    /// セーブデータに新たな情報を追加します。
    /// </summary>
    /// <param name="key"></param>
    /// <param name="data"></param>
    public void Set(string key, object data)
    {
        // セーブデータにこの情報が存在するなら
        if(Data.ContainsKey(key))
        {
            // 情報を上書きします。
            Data[key] = data;
        }
        // セーブデータにこの情報が存在しないなら
        else
        {
            // セーブデータにこの情報を追加します。
            Data.Add(key, data);
        }
    }
        
    /// <summary>
    /// セーブデータから指定の情報を取得します。
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="key"></param>
    /// <param name="defaultValue"></param>
    /// <returns></returns>
    public T Get<T>(string key, T defaultValue = default)
    {
        // セーブデータからこの情報のデータが取得できたら
        if(Data.TryGetValue(key, out object value))
        {
            // 指定した型でデータの情報を変えします。
            return (T)Convert.ChangeType(value, typeof(T));
        }

        // デフォルト値を返します。
        return defaultValue;
    }
}
