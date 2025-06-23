using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChenger : MonoBehaviour
{
    public static SceneChenger Instance { get; private set; }         // ���g���V���O���g�������܂��B

    public FloorData FloorData { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject); // ���łɃC���X�^���X�����݂���ꍇ�͔j��
        }
    }

    public void LoadTo(string sceneName)
    {
        SoundManager.Instance.PlaySE("ButtonClick");
        SceneManager.LoadScene(sceneName);
    }

    public void ToGame(FloorData floorData)
    {
        FloorData = floorData;
        LoadTo("Main");
    }
}
