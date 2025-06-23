using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour
{
    [SerializeField] Slider hpBar;
    [SerializeField] Camera miniMapCamera;
    [SerializeField] TextMeshProUGUI playerName;

    public void SetName()
    {
        playerName.text = SaveDataManager.Instance.CurrentSaveData.Get<string>("PlayerName");
    }

    public void UpdateHPBar(float fillAmount)
    {
        HPBar hpBarScript = hpBar.GetComponent<HPBar>();
        hpBarScript.UpdateHPBar(fillAmount);
    }

    public void UpdateMiniMapCamera(Vector3 playerPos)
    {
        miniMapCamera.transform.position = playerPos + Vector3.up * 20;
    }
}
