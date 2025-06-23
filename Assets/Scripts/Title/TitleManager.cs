using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine.Windows;

public class TitleManager : MonoBehaviour
{
    float m_coolDown = 1.0f;
    float m_coolDownTimer = 0;

    [SerializeField] Text m_text;

    const float m_blinkSpeed = 3.0f;

    void Start()
    {
        Time.timeScale = 1.0f;
        m_coolDownTimer = 0;
        SettingDataManager.Instance.LoadSattingData();
        // SoundManager.Instance.PlayBGM("Title");
    }

    private void Update()
    {
        m_coolDownTimer += 1 * Time.deltaTime;

        if (m_coolDown <= m_coolDownTimer)
        {
            if (Keyboard.current.anyKey.wasPressedThisFrame || Gamepad.current != null && Gamepad.current.allControls.Any(c => c.IsPressed()))
            {
                SceneChenger.Instance.LoadTo("SelectSaveData");
            }
        }

        Color color = m_text.color;
        color.a = Mathf.Abs(Mathf.Sin(m_blinkSpeed * Time.time));
        m_text.color = color;
    }
}
