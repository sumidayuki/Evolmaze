using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SelectStageManager : MonoBehaviour
{
    [Header("Quest")]
    [SerializeField] GameObject[] m_questButton;

    [Header("UI")]
    [SerializeField] GameObject m_confilmPanel;

    [Header("First Selected Objcet")]
    [SerializeField] GameObject m_firstQuestButton;
    [SerializeField] GameObject m_firstConfilmButton;

    private FloorData m_floorData;

    public void Awake()
    {
        m_floorData = null;

        m_confilmPanel.SetActive(false);

        foreach(var button in m_questButton)
        {
            button.SetActive(false);
        }

        for(int i = 0; i < SaveDataManager.Instance.CurrentSaveData.Get<float>("FloorNumber") ; i++)
        {
            m_questButton[i].SetActive(true);
        }
    }

    public void SelectQuest(FloorData floorData)
    {
        m_floorData = floorData;
        m_confilmPanel.SetActive(true);
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(m_firstConfilmButton);
        EventSystem.current.firstSelectedGameObject = m_firstConfilmButton;
    }

    public void ConfilmQuest()
    {
        if (m_floorData == null) return;

        SceneChenger.Instance.ToGame(m_floorData);
    }

    public void CancelQuest()
    {
        m_floorData = null;
        m_confilmPanel.SetActive(false);
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(m_firstQuestButton);
        EventSystem.current.firstSelectedGameObject = m_firstQuestButton;
    }
}
