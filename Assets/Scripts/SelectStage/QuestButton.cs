using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class QuestButton : MonoBehaviour
{
    [Header("Data")]
    [SerializeField] FloorData m_floorData;

    [Header("UI")]
    [SerializeField] TextMeshProUGUI m_floorNumText;
    [SerializeField] TextMeshProUGUI m_floorNameText;
    [SerializeField] GameObject m_clearMark;

    private void Awake()
    {
        m_floorNumText.text = $"äKëwÅ@{m_floorData.floorNumber}";
        m_floorNameText.text = m_floorData.floorName;

        if (SaveDataManager.Instance.CurrentSaveData.Get<float>("FloorNumber") > m_floorData.floorNumber)
        {
            m_clearMark.SetActive(true);
        }
        else
        {
            m_clearMark.SetActive(false);
        }
    }
}
