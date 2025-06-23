using UnityEngine;
using System.IO;
using Newtonsoft.Json;

/// <summary>
/// スロットごとのセーブデータを管理します。
/// </summary>
public class SaveDataManager : MonoBehaviour
{
    public static SaveDataManager Instance { get; private set; }

    public SaveData CurrentSaveData { get; private set; }

    private string m_savePath;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        m_savePath = Application.persistentDataPath + "/save_";
    }

    /// <summary>
    /// 指定したスロットにセーブデータがあるかを確認します。
    /// </summary>
    /// <param name="slotIndex"></param>
    /// <returns></returns>
    public bool IsSaveDataExists(int slotIndex)
    {
        return File.Exists(GetFilePath(slotIndex));
    }

    /// <summary>
    /// 指定したスロットのセーブデータを読み込みます。
    /// </summary>
    /// <param name="slotIndex"></param>
    public void LoadSaveData(int slotIndex)
    {
        if (IsSaveDataExists(slotIndex))
        {
            // データをロードする処理（JSON読み込みなど）
            SaveData data = LoadFromFile(slotIndex);
            data.Set("SlotIndex", slotIndex);
            CurrentSaveData = data;
            SaveToFile(CurrentSaveData, slotIndex);
            SceneChenger.Instance.LoadTo("SelectStage");
        }
    }

    /// <summary>
    /// 指定したスロットのファイルパスを取得します。
    /// </summary>
    /// <param name="slotIndex"></param>
    /// <returns></returns>
    private string GetFilePath(int slotIndex)
    {
        return m_savePath + slotIndex + ".json";
    }

    /// <summary>
    /// ファイルから指定したスロットのセーブデータを取得します。
    /// </summary>
    /// <param name="slotIndex"></param>
    /// <returns></returns>
    public SaveData LoadFromFile(int slotIndex)
    {
        string path = GetFilePath(slotIndex);
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            return JsonConvert.DeserializeObject<SaveData>(json);
        }
        return new SaveData(); // データがない場合は新規作成
    }

    /// <summary>
    /// 指定したスロットのセーブデータを消去します。
    /// </summary>
    /// <param name="slotIndex"></param>
    public void DeleteSaveData(int slotIndex)
    {
        string path = GetFilePath(slotIndex);
        if (File.Exists(path))
        {
            File.Delete(path);
            Debug.Log($"セーブデータ {slotIndex} を削除しました。");
        }
    }

    /// <summary>
    /// 指定したスロットのセーブデータにデータをセーブします。
    /// </summary>
    /// <param name="data"></param>
    /// <param name="slotIndex"></param>
    public void SaveToFile(SaveData data, int slotIndex)
    {
        string json = JsonConvert.SerializeObject(data, Formatting.Indented);
        File.WriteAllText(GetFilePath(slotIndex), json);
    }
}