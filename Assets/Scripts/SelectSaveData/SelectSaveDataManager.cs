using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class SelectSaveDataManager : MonoBehaviour
{
    [Header("パネル")]
    [SerializeField] GameObject m_newGamePanel;
    [SerializeField] GameObject m_loadGamePanel;
    [SerializeField] GameObject m_newNamePanel;
    [SerializeField] TMP_InputField m_inputField;

    [Header("最初に選択されるボタン")]
    [SerializeField] GameObject m_firstSelectSlotButton;
    [SerializeField] GameObject m_firstNewGameButton;
    [SerializeField] GameObject m_firstLoadGameButton;
    [SerializeField] GameObject m_firstNewNameButton;

    private int m_slotIndex;

    private void Awake()
    {
        ClosePanel();
    }

    public void ClosePanel()
    {
        m_loadGamePanel.SetActive(false);
        m_newGamePanel.SetActive(false);
        m_newNamePanel.SetActive(false);

        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(m_firstSelectSlotButton);
        EventSystem.current.firstSelectedGameObject = m_firstSelectSlotButton;
    }

    /// <summary>
    /// スロットを選択します。
    /// </summary>
    public void SelectSlot(int slotIndex)
    {
        m_slotIndex = slotIndex;

        if (SaveDataManager.Instance.IsSaveDataExists(m_slotIndex))
        {
            m_loadGamePanel.SetActive(true);
            EventSystem.current.SetSelectedGameObject(null);
            EventSystem.current.SetSelectedGameObject(m_firstLoadGameButton);
            EventSystem.current.firstSelectedGameObject = m_firstLoadGameButton;
        }
        else
        {
            m_newGamePanel.SetActive(true);
            EventSystem.current.SetSelectedGameObject(null);
            EventSystem.current.SetSelectedGameObject(m_firstNewGameButton);
            EventSystem.current.firstSelectedGameObject = m_firstNewGameButton;
        }
    }

    public void LoadGame()
    {
        if (!SaveDataManager.Instance.IsSaveDataExists(m_slotIndex))
        {
            m_newNamePanel.SetActive(true);
            EventSystem.current.SetSelectedGameObject(null);
            EventSystem.current.SetSelectedGameObject(m_firstNewNameButton);
            EventSystem.current.firstSelectedGameObject = m_firstNewNameButton;
        }
        else
        {
            SaveDataManager.Instance.LoadSaveData(m_slotIndex);
        }
    }

    public void NewGame()
    {
        string playerName = m_inputField.text;

        if (string.IsNullOrEmpty(playerName))
        {
            Debug.LogWarning("プレイヤー名を入力してください！");
            return;
        }

        SaveData data = new SaveData();

        DefaultSetting(data);

        SaveDataManager.Instance.SaveToFile(data, m_slotIndex);

        SaveDataManager.Instance.LoadSaveData(m_slotIndex);
    }

    /// <summary>
    /// スロットのデータを削除します。
    /// </summary>
    public void DeleteSaveData()
    {
        SaveDataManager.Instance.DeleteSaveData(m_slotIndex);
        SceneChenger.Instance.LoadTo("SelectSaveData");
    }

    private void DefaultSetting(SaveData data)
    {
        data.Set("PlayerName", m_inputField.text);
        data.Set("FloorNumber", 1);
        data.Set("PlayTime", 0.0f);
        data.Set("MaxHP", 100.0f);
        data.Set("AttackDamage", 10.0f);
        data.Set("RunSpeed", 5.0f);
    }
}
