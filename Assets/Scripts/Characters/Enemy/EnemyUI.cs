using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyUI : MonoBehaviour
{
    [SerializeField] Slider hpBarPrefab;

    public void ShowHPBar()
    {
        hpBarPrefab.gameObject.SetActive(true);
    }

    public void HideHPBar()
    {
        hpBarPrefab.gameObject.SetActive(false);
    }

    public void RotateCanvas()
    {
        this.transform.LookAt(Camera.main.transform.position);
    }

    public void TakeDamage(float amount)
    {
        HPBar hpBarScript = hpBarPrefab.GetComponent<HPBar>();
        hpBarScript.UpdateHPBar(amount);
    }
}
