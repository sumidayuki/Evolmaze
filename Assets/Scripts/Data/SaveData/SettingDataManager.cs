using UnityEngine;
using System.IO;
using Newtonsoft.Json;

/// <summary>
/// �Q�[���̐ݒ�f�[�^���Ǘ����܂��B
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
    /// �Z�b�e�B���O�f�[�^�����邩���m�F���܂��B
    /// </summary>
    /// <returns></returns>
    public bool IsSettingDataExists()
    {
        return File.Exists(m_savePath);
    }

    /// <summary>
    /// �Z�b�e�B���O�f�[�^�����[�h���܂��B
    /// </summary>
    public void LoadSattingData()
    {
        // �f�[�^�����[�h���鏈��
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
    /// �t�@�C������Z�b�e�B���O�f�[�^�����[�h���܂��B
    /// </summary>
    /// <returns></returns>
    private SettingData LoadFromFile()
    {
        if (IsSettingDataExists())
        {
            string json = File.ReadAllText(m_savePath);
            return JsonConvert.DeserializeObject<SettingData>(json);
        }
        return new SettingData(); // �f�[�^���Ȃ��ꍇ�͐V�K�쐬
    }

    /// <summary>
    /// �Z�b�e�B���O�f�[�^��ۑ����܂��B
    /// </summary>
    /// <param name="data"></param>
    public void SaveToFile()
    {
        string json = JsonConvert.SerializeObject(CurrentSettingData, Formatting.Indented);
        File.WriteAllText(m_savePath, json);
    }

    /// <summary>
    /// �f�t�H���g�ݒ���s���܂��B
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
