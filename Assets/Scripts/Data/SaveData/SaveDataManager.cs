using UnityEngine;
using System.IO;
using Newtonsoft.Json;

/// <summary>
/// �X���b�g���Ƃ̃Z�[�u�f�[�^���Ǘ����܂��B
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
    /// �w�肵���X���b�g�ɃZ�[�u�f�[�^�����邩���m�F���܂��B
    /// </summary>
    /// <param name="slotIndex"></param>
    /// <returns></returns>
    public bool IsSaveDataExists(int slotIndex)
    {
        return File.Exists(GetFilePath(slotIndex));
    }

    /// <summary>
    /// �w�肵���X���b�g�̃Z�[�u�f�[�^��ǂݍ��݂܂��B
    /// </summary>
    /// <param name="slotIndex"></param>
    public void LoadSaveData(int slotIndex)
    {
        if (IsSaveDataExists(slotIndex))
        {
            // �f�[�^�����[�h���鏈���iJSON�ǂݍ��݂Ȃǁj
            SaveData data = LoadFromFile(slotIndex);
            data.Set("SlotIndex", slotIndex);
            CurrentSaveData = data;
            SaveToFile(CurrentSaveData, slotIndex);
            SceneChenger.Instance.LoadTo("SelectStage");
        }
    }

    /// <summary>
    /// �w�肵���X���b�g�̃t�@�C���p�X���擾���܂��B
    /// </summary>
    /// <param name="slotIndex"></param>
    /// <returns></returns>
    private string GetFilePath(int slotIndex)
    {
        return m_savePath + slotIndex + ".json";
    }

    /// <summary>
    /// �t�@�C������w�肵���X���b�g�̃Z�[�u�f�[�^���擾���܂��B
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
        return new SaveData(); // �f�[�^���Ȃ��ꍇ�͐V�K�쐬
    }

    /// <summary>
    /// �w�肵���X���b�g�̃Z�[�u�f�[�^���������܂��B
    /// </summary>
    /// <param name="slotIndex"></param>
    public void DeleteSaveData(int slotIndex)
    {
        string path = GetFilePath(slotIndex);
        if (File.Exists(path))
        {
            File.Delete(path);
            Debug.Log($"�Z�[�u�f�[�^ {slotIndex} ���폜���܂����B");
        }
    }

    /// <summary>
    /// �w�肵���X���b�g�̃Z�[�u�f�[�^�Ƀf�[�^���Z�[�u���܂��B
    /// </summary>
    /// <param name="data"></param>
    /// <param name="slotIndex"></param>
    public void SaveToFile(SaveData data, int slotIndex)
    {
        string json = JsonConvert.SerializeObject(data, Formatting.Indented);
        File.WriteAllText(GetFilePath(slotIndex), json);
    }
}