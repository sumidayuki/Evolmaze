using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SaveSlotButton : MonoBehaviour
{
    [SerializeField] int m_slotIndex;   // ���݂̃X���b�g�ԍ�

    [SerializeField] TextMeshProUGUI m_slotText;           // �X���b�g�ԍ���\������e�L�X�g
    [SerializeField] TextMeshProUGUI m_playerNameText;     // �X���b�g�ԍ���\������e�L�X�g
    [SerializeField] TextMeshProUGUI m_floorText;          // �X���b�g�ԍ���\������e�L�X�g
    [SerializeField] TextMeshProUGUI m_playTimeText;       // �X���b�g�ԍ���\������e�L�X�g

    private void Start()
    {
        UpdateSlotData();
    }

    /// <summary>
    /// �X���b�g�̃f�[�^���X�V���܂��B
    /// </summary>
    private void UpdateSlotData()
    { 
        if(SaveDataManager.Instance.IsSaveDataExists(m_slotIndex))
        {
            SaveData data = SaveDataManager.Instance.LoadFromFile(m_slotIndex);

            // �Z�[�u�f�[�^������ꍇ�̕\��
            m_slotText.text = $"�Z�[�u�f�[�^ {m_slotIndex + 1}".ToString();
            object a = "";
            m_playerNameText.text = data.Get<string>("PlayerName");
            m_floorText.text    = $"�K�w�@�@�@�@{data.Get<int>("FloorNumber")}".ToString();

            int hours = (int)data.Get<float>("PlayTime") / 3600;
            int minutes = (int)(data.Get<float>("PlayTime") % 3600) / 60;
            int seconds = (int)data.Get<float>("PlayTime") % 60;

            m_playTimeText.text = $"�v���C���ԁ@{hours:D2}:{minutes:D2}:{seconds:D2}".ToString();
        }
        else
        {
            m_slotText.text = $"�Z�[�u�f�[�^ {m_slotIndex + 1}";
            m_playerNameText.text = "�V�����Z�[�u�f�[�^";
            m_floorText.text = "";
            m_playTimeText.text = "";
        }
    }
}
