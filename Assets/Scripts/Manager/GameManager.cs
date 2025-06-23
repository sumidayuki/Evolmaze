using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;
using System.Linq;
using UnityEngine.EventSystems;

/// <summary>
/// ゲームプレイの全体を管理します。
/// </summary>
public class GameManager : MonoBehaviour
{
    [SerializeField] private DungeonManager dungeonManager;
    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private GameObject gameClearPanel;
    [SerializeField] private GameObject loadingPanel;
    [SerializeField] private TextMeshProUGUI playTimeText;
    [SerializeField] private GameObject m_firstGameClearButton;

    private FloorData m_floorData;

    private float playTime;

    private bool isPause = false;

    private PlayerInput m_playerInput;

    public static GameManager Instance { get; private set; }

    /// <summary>
    /// 現在のゲームの状態を表します。
    /// </summary>
    private enum STATE
    {
        LOADING,
        GAMEPLAY,
        PAUSE,
        GAMEOVER,
        GAMECLEAR,
    }

    private STATE m_state;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        m_state = STATE.LOADING;
        playTime = 0;
        m_floorData = SceneChenger.Instance.FloorData;
    }

    private void Start()
    {
        ClearPanel();
        
        Time.timeScale = 1.0f;
        StartCoroutine(dungeonManager.CreateDungeon(m_floorData));
    }

    private void Update()
    {
        // 現在のゲームの状態に合わせて分岐します。
        switch (m_state)
        {
            case STATE.LOADING:
                Time.timeScale = 1;
                Cursor.lockState = CursorLockMode.None;
                loadingPanel.SetActive(true);
                if(dungeonManager.IsDungeonGenerated)
                {
                    m_state = STATE.GAMEPLAY;
                    m_playerInput = dungeonManager.Player.GetComponent<PlayerInput>();
                    m_playerInput.enabled = false;
                    loadingPanel.SetActive(false);
                }
                break;

            case STATE.GAMEPLAY:
                Time.timeScale = 1;
                UIManager.Instance.UIInput.enabled = false;
                m_playerInput.enabled = true;
                Cursor.lockState = CursorLockMode.Locked;
                playTime += Time.unscaledDeltaTime;
                break;

            case STATE.PAUSE:
                Time.timeScale = 0;
                m_playerInput.enabled = false;
                UIManager.Instance.UIInput.enabled = true;
                Cursor.lockState = CursorLockMode.None;
                break;

            case STATE.GAMECLEAR:
                Time.timeScale = 0;
                m_playerInput.enabled = false;
                UIManager.Instance.UIInput.enabled = true;
                Cursor.lockState = CursorLockMode.None;
                break;

            case STATE.GAMEOVER:
                Time.timeScale = 0;
                m_playerInput.enabled = false;
                UIManager.Instance.UIInput.enabled = true;
                Cursor.lockState = CursorLockMode.None;
                gameOverPanel.SetActive(true);

                if (Keyboard.current.anyKey.wasPressedThisFrame || Gamepad.current != null && Gamepad.current.allControls.Any(c => c.IsPressed()))
                {
                    Player player = dungeonManager.Player.GetComponent<Player>();

                    SaveData data = SaveDataManager.Instance.CurrentSaveData;
                    data.Set("PlayTime", playTime + data.Get<float>("PlayTime"));
                    data.Set("AttackDamage", player.AttackDamage);
                    data.Set("MaxHP", player.MaxHP);

                    SaveDataManager.Instance.SaveToFile(data, data.Get<int>("SlotIndex"));

                    SceneChenger.Instance.ToGame(m_floorData);
                }

                break;
        }
    }

    /// <summary>
    /// パネルを全て消します。
    /// </summary>
    private void ClearPanel()
    {
        gameOverPanel.SetActive(false);
        gameClearPanel.SetActive(false);
    }

    /// <summary>
    /// ポーズ処理を行います。
    /// </summary>
    public void Pause()
    {
        if (m_state == STATE.GAMEPLAY)
        {
            m_state = STATE.PAUSE;
            isPause = true;
            UIManager.Instance.ShowPausePanel();
        }
        else if (m_state == STATE.PAUSE)
        {
            m_state = STATE.GAMEPLAY;
            isPause = false;
            UIManager.Instance.ClosePausePanel();
        }
    }

    /// <summary>
    /// ゲームオーバー時の処理を行います。
    /// </summary>
    public void GameOver()
    {
        m_state = STATE.GAMEOVER;
    }

    /// <summary>
    /// タイトルへ戻ります。
    /// </summary>
    public void ToTitle()
    {
        Player player = dungeonManager.Player.GetComponent<Player>();

        SaveData data = SaveDataManager.Instance.CurrentSaveData;
        data.Set("PlayTime", playTime + data.Get<float>("PlayTime"));
        data.Set("AttackDamage", player.AttackDamage);
        data.Set("MaxHP", player.MaxHP);

        if (m_state == STATE.GAMECLEAR)
        {
            if (data.Get<int>("FloorNumber") == m_floorData.floorNumber)
            {
                data.Set("FloorNumber", m_floorData.floorNumber + 1);
            }

            SaveDataManager.Instance.SaveToFile(data, data.Get<int>("SlotIndex"));
            SceneChenger.Instance.LoadTo("SelectStage");
            return;
        }

        SaveDataManager.Instance.SaveToFile(data, data.Get<int>("SlotIndex"));
        SceneChenger.Instance.LoadTo("Title");
    }

    /// <summary>
    /// ゲームクリア時の処理を行います。
    /// </summary>
    public void GameClear()
    {
        m_state = STATE.GAMECLEAR;
        gameClearPanel.SetActive(true);

        int minutes = (int)playTime / 60;
        int seconds = (int)playTime % 60;

        playTimeText.text = $"プレイ時間 {minutes:D2}分{seconds:D2}秒";

        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(m_firstGameClearButton);
    }

    /// <summary>
    /// ゲーム終了ボタンが押されたときの処理を行います。
    /// </summary>
    public void EndGame()
    {
        Player player = dungeonManager.Player.GetComponent<Player>();

        SaveData data = SaveDataManager.Instance.CurrentSaveData;

        data.Set("PlayTime", playTime + data.Get<float>("PlayTime", 0.0f));
        data.Set("AttackDamage", player.AttackDamage);
        data.Set("MaxHP", player.MaxHP);

        SaveDataManager.Instance.SaveToFile(data, data.Get<int>("SlotIndex"));

        Application.Quit();
    }
}