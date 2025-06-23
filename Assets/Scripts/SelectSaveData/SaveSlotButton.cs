using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SaveSlotButton : MonoBehaviour
{
    [SerializeField] int m_slotIndex;   // 現在のスロット番号

    [SerializeField] TextMeshProUGUI m_slotText;           // スロット番号を表示するテキスト
    [SerializeField] TextMeshProUGUI m_playerNameText;     // スロット番号を表示するテキスト
    [SerializeField] TextMeshProUGUI m_floorText;          // スロット番号を表示するテキスト
    [SerializeField] TextMeshProUGUI m_playTimeText;       // スロット番号を表示するテキスト

    private void Start()
    {
        UpdateSlotData();
    }

    /// <summary>
    /// スロットのデータを更新します。
    /// </summary>
    private void UpdateSlotData()
    { 
        if(SaveDataManager.Instance.IsSaveDataExists(m_slotIndex))
        {
            SaveData data = SaveDataManager.Instance.LoadFromFile(m_slotIndex);

            // セーブデータがある場合の表示
            m_slotText.text = $"セーブデータ {m_slotIndex + 1}".ToString();
            object a = "";
            m_playerNameText.text = data.Get<string>("PlayerName");
            m_floorText.text    = $"階層　　　　{data.Get<int>("FloorNumber")}".ToString();

            int hours = (int)data.Get<float>("PlayTime") / 3600;
            int minutes = (int)(data.Get<float>("PlayTime") % 3600) / 60;
            int seconds = (int)data.Get<float>("PlayTime") % 60;

            m_playTimeText.text = $"プレイ時間　{hours:D2}:{minutes:D2}:{seconds:D2}".ToString();
        }
        else
        {
            m_slotText.text = $"セーブデータ {m_slotIndex + 1}";
            m_playerNameText.text = "新しいセーブデータ";
            m_floorText.text = "";
            m_playTimeText.text = "";
        }
    }
}
