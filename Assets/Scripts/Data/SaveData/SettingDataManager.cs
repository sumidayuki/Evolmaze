using UnityEngine;
using System.IO;
using Newtonsoft.Json;

/// <summary>
/// ゲームの設定データを管理します。
/// </summary>
public class SettingDataManager : MonoBehaviour
{
    public static SettingDataManager Instance { get; private set; }

    public SettingData CurrentSettingData { get; private set; }

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

        m_savePath = Application.persistentDataPath + "/setting.json";
    }

    /// <summary>
    /// セッティングデータがあるかを確認します。
    /// </summary>
    /// <returns></returns>
    public bool IsSettingDataExists()
    {
        return File.Exists(m_savePath);
    }

    /// <summary>
    /// セッティングデータをロードします。
    /// </summary>
    public void LoadSattingData()
    {
        // データをロードする処理
        SettingData data = LoadFromFile();

        if (IsSettingDataExists())
        {
            CurrentSettingData = data;
        }
        else
        {
            CurrentSettingData = data;
            DefaultSetting();
            SaveToFile();
        }
    }

    /// <summary>
    /// ファイルからセッティングデータをロードします。
    /// </summary>
    /// <returns></returns>
    private SettingData LoadFromFile()
    {
        if (IsSettingDataExists())
        {
            string json = File.ReadAllText(m_savePath);
            return JsonConvert.DeserializeObject<SettingData>(json);
        }
        return new SettingData(); // データがない場合は新規作成
    }

    /// <summary>
    /// セッティングデータを保存します。
    /// </summary>
    /// <param name="data"></param>
    public void SaveToFile()
    {
        string json = JsonConvert.SerializeObject(CurrentSettingData, Formatting.Indented);
        File.WriteAllText(m_savePath, json);
    }

    /// <summary>
    /// デフォルト設定を行います。
    /// </summary>
    /// <param name="data"></param>
    public void DefaultSetting()
    {
        CurrentSettingData.Set("MouseSensitivity", 2.0f);
        CurrentSettingData.Set("ControllerSensitivity", 20.0f);
        CurrentSettingData.Set("AutoDash", false);
        CurrentSettingData.Set("MasterVolume", 50);
        CurrentSettingData.Set("BGMVolume", 50);
        CurrentSettingData.Set("SEVolume", 50);
    }
}
