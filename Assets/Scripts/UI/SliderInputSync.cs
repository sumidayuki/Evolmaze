using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SliderInputSync : MonoBehaviour
{
    private Slider m_slider;
    private TMP_InputField m_inputField;

    private string m_name;

    private void Awake()
    {
        m_name = gameObject.name;

        m_slider = GetComponentInChildren<Slider>();
        m_inputField = GetComponentInChildren<TMP_InputField>();

        m_slider.maxValue = 100;
        m_slider.minValue = 0;

        m_slider.value = SettingDataManager.Instance.CurrentSettingData.Get<int>(m_name);

        m_inputField.text = m_slider.value.ToString();

        m_slider.onValueChanged.AddListener(UpdateInputField);
        m_inputField.onEndEdit.AddListener(UpdateSlider);
    }

    private void UpdateInputField(float value)
    {
        m_inputField.text = value.ToString("0");
        SettingDataManager.Instance.CurrentSettingData.Set(m_name, value);
        SettingDataManager.Instance.SaveToFile();
    }

    private void UpdateSlider(string value)
    {
        if(float.TryParse(value, out float result))
        {
            result = Mathf.Clamp(result, 0, 100);
            m_slider.value = result;
            m_inputField.text = result.ToString("0");
            SettingDataManager.Instance.CurrentSettingData.Set(m_name, result);
            SettingDataManager.Instance.SaveToFile();
        }
    }
}
