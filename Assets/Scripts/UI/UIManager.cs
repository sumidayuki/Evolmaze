using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

/// <summary>
/// UIを管理します。
/// </summary>
public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        UIInput = GetComponent<PlayerInput>();
        m_inputInfo = new InputInfo();
        Init();
    }

    [Header("パネル")]
    [SerializeField] private GameObject m_pausePanel;
    [SerializeField] private GameObject m_optionPanel;
    [SerializeField] private GameObject m_generalPanel;
    [SerializeField] private GameObject m_graphicsPanel;
    [SerializeField] private GameObject m_audioPanel;

    [Header("最初に選択されるボタン")]
    [SerializeField] private GameObject m_firstPauseButton;
    [SerializeField] private GameObject m_firstGeneralButton;

    public PlayerInput UIInput { get; private set; }

    private InputInfo m_inputInfo;

    public void ShowPausePanel()
    {
        CloseOptionPanel();
        Time.timeScale = 0;
        m_pausePanel.SetActive(true);
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(m_firstPauseButton);
    }

    public void ClosePausePanel()
    { 
        m_pausePanel.SetActive(false);
        Time.timeScale = 1.0f;

        EventSystem.current.SetSelectedGameObject(EventSystem.current.firstSelectedGameObject);
    }

    public void ShowOptionPanel()
    {
        m_optionPanel.SetActive(true);
        ShowScrollView("General");
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(m_firstGeneralButton);
    }

    public void CloseOptionPanel()
    {
        m_optionPanel.SetActive(false);
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(m_firstPauseButton);
    }

    public void ShowScrollView(string panelName)
    {
        switch (panelName)
        {
            case "General":
                CloseScrollView();
                m_generalPanel.transform.GetChild(1).gameObject.SetActive(true);
                SelectButton(m_generalPanel.transform.GetChild(0).GetComponent<TextMeshProUGUI>());
                break;

            case "Graphics":
                CloseScrollView();
                m_graphicsPanel.transform.GetChild(1).gameObject.SetActive(true);
                SelectButton(m_graphicsPanel.transform.GetChild(0).GetComponent<TextMeshProUGUI>());
                break;

            case "Audio":
                CloseScrollView();
                m_audioPanel.transform.GetChild(1).gameObject.SetActive(true);
                SelectButton(m_audioPanel.transform.GetChild(0).GetComponent<TextMeshProUGUI>());
                break;
        }
    }

    public void SelectButton(TextMeshProUGUI text)
    {
        text.color = Color.yellow;
    }

    private void Init()
    {
        Debug.Log("呼ばれた");
        CloseScrollView();
        m_optionPanel.SetActive(false);
        ClosePausePanel();
    }

    public void CloseScrollView()
    {
        m_graphicsPanel.transform.GetChild(1).gameObject.SetActive(false);
        m_graphicsPanel.transform.GetChild(0).GetComponent<TextMeshProUGUI>().color = Color.white;
        m_generalPanel.transform.GetChild(1).gameObject.SetActive(false);
        m_generalPanel.transform.GetChild(0).GetComponent<TextMeshProUGUI>().color = Color.white;
        m_audioPanel.transform.GetChild(1).gameObject.SetActive(false);
        m_audioPanel.transform.GetChild(0).GetComponent<TextMeshProUGUI>().color = Color.white;
    }

    public void OnPause()
    {
        Debug.Log("Pauseが押された");

        if (m_optionPanel.activeSelf)
        {
            CloseOptionPanel();
            return;
        }

        if(GameManager.Instance != null)
        {
            GameManager.Instance.Pause();
        }
        else
        {
            m_inputInfo.Pause = !m_inputInfo.Pause;

            if (m_inputInfo.Pause)
            {
                ShowPausePanel();
            }
            else
            {
                ClosePausePanel();
            }
        }
    }

    public void OnToTitle()
    {
        if(GameManager.Instance != null)
        {
            GameManager.Instance.ToTitle();
            return;
        }

        SceneChenger.Instance.LoadTo("Title");
    }

    public void OnEnd()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.EndGame();
            return;
        }

        Application.Quit();
    }
}
