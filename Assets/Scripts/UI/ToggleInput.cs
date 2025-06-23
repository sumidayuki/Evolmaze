using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ToggleInput : MonoBehaviour
{
    private Toggle m_toggle;

    private string m_name;

    private void Awake()
    {
        m_name = gameObject.name;

        m_toggle = GetComponentInChildren<Toggle>();

        m_toggle.isOn = SettingDataManager.Instance.CurrentSettingData.Get<bool>(m_name);

        m_toggle.onValueChanged.AddListener(UpdateToggle);
    }

    private void UpdateToggle(bool value)
    {
        SettingDataManager.Instance.CurrentSettingData.Set(m_name, value);
        SettingDataManager.Instance.SaveToFile();
    }
}
