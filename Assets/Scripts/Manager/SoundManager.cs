using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance { get; private set; }

    [Header("Audio Sources")]
    [SerializeField] private AudioSource m_bgmSource;
    [SerializeField] private AudioSource m_seSource;

    [Header("Audio Clips")]
    [SerializeField] private List<AudioClip> m_bgmClips;
    [SerializeField] private List<AudioClip> m_seClips;

    private Dictionary<string, AudioClip> m_bgmDictionary;
    private Dictionary<string, AudioClip> m_seDictionary;

    [Header("Volume Settings")]
    [Range(0, 1)] private float m_masterVolume = 1f;
    [Range(0, 1)] private float m_bgmVolume = 1f;
    [Range(0, 1)] private float m_seVolume = 1f;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        InitializeDictionaries();
    }

    private void LateUpdate()
    {
        float newMasterVolume = Mathf.Clamp01(SettingDataManager.Instance.CurrentSettingData.Get<float>("MasterVolume"));
        float newBgmVolume = Mathf.Clamp01(SettingDataManager.Instance.CurrentSettingData.Get<float>("BGMVolume"));
        float newSeVolume = Mathf.Clamp01(SettingDataManager.Instance.CurrentSettingData.Get<float>("SEVolume"));

        if (newMasterVolume != m_masterVolume)
        {
            SetVolume("Master", newMasterVolume);
        }
        if (newBgmVolume != m_bgmVolume)
        {
            SetVolume("BGM", newBgmVolume);
        }
        if (newSeVolume != m_seVolume)
        {
            SetVolume("SE", newSeVolume);
        }
    }

    /// <summary>
    /// アタッチされているAudioClipをclipの名前をキーにしてDictionaryに格納します。
    /// </summary>
    private void InitializeDictionaries()
    {
        m_bgmDictionary = new Dictionary<string, AudioClip>();
        m_seDictionary = new Dictionary<string, AudioClip>();

        foreach (var clip in m_bgmClips) m_bgmDictionary[clip.name] = clip;
        foreach (var clip in m_seClips) m_seDictionary[clip.name] = clip;

        m_masterVolume = SettingDataManager.Instance.CurrentSettingData.Get<float>("MasterVolume");
        m_bgmVolume = SettingDataManager.Instance.CurrentSettingData.Get<float>("BGMVolume");
        m_seVolume = SettingDataManager.Instance.CurrentSettingData.Get<float>("SEVolume");
    }

    /// <summary>
    /// 指定したBGMを再生します。
    /// </summary>
    /// <param name="clipName"></param>
    public void PlayBGM(string clipName)
    {
        if (m_bgmDictionary.TryGetValue(clipName, out var clip))
        {
            // 現在再生中のBGMと同じBGMを再生しようとしているならreturnする。
            if (m_bgmSource.clip == clip) return;

            m_bgmSource.clip = clip;
            m_bgmSource.volume = m_bgmVolume * m_masterVolume;
            m_bgmSource.loop = true;
            m_bgmSource.Play();
        }
        else
        {
            Debug.LogWarning($"BGM '{clipName}' not found!");
        }
    }

    /// <summary>
    /// 指定したSEを再生します。
    /// </summary>
    /// <param name="clipName"></param>
    public void PlaySE(string clipName)
    {
        if (m_seDictionary.TryGetValue(clipName, out var clip))
        {
            m_seSource.PlayOneShot(clip, m_seVolume * m_masterVolume);
        }
        else
        {
            Debug.LogWarning($"SFX '{clipName}' not found!");
        }
    }

    /// <summary>
    /// 指定した場所で指定したSEを再生します。
    /// </summary>
    /// <param name="audioSource"></param>
    /// <param name="clipName"></param>
    public void PlaySE(AudioSource audioSource, string clipName)
    {
        if (m_seDictionary.TryGetValue(clipName, out var clip))
        {
            audioSource.PlayOneShot(clip, m_seVolume * m_masterVolume);
        }
        else
        {
            Debug.LogWarning($"SFX '{clipName}' not found!");
        }
    }

    /// <summary>
    /// 指定したタイプの音量を変更します。
    /// </summary>
    /// <param name="type"></param>
    /// <param name="volume"></param>
    public void SetVolume(string type, float volume)
    {
        switch (type)
        {
            case "Master":
                m_masterVolume = Mathf.Clamp01(volume);
                m_bgmSource.volume = m_bgmVolume * m_masterVolume;
                break;
            case "BGM":
                m_bgmVolume = Mathf.Clamp01(volume);
                m_bgmSource.volume = m_bgmVolume * m_masterVolume;
                break;
            case "SE":
                m_seVolume = Mathf.Clamp01(volume);
                break;
        }
    }

    /// <summary>
    /// 全ての音を止めます。
    /// </summary>
    public void StopAllSounds()
    {
        m_bgmSource.Stop();
        m_seSource.Stop();
    }
}