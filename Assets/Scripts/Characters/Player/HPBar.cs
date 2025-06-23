using UnityEngine;
using UnityEngine.UI;

public class HPBar : MonoBehaviour
{
    private float animationSpeed = 2f;

    private float targetValue;

    public Slider Slider { get; private set; }

    private void Awake()
    {
        Slider = gameObject.GetComponent<Slider>();
    }

    public void UpdateHPBar(float fillAmount)
    {
        targetValue = fillAmount;
        StopAllCoroutines();
        if (!gameObject.activeSelf) return;
        StartCoroutine(AnimateHPBar());
    }

    private System.Collections.IEnumerator AnimateHPBar()
    {
        while (Mathf.Abs(Slider.value - targetValue) > 0.01f)
        {
            Slider.value = Mathf.Lerp(Slider.value, targetValue, animationSpeed * Time.deltaTime);
            yield return null;
        }
        Slider.value = targetValue; // ç≈èIí≤êÆ
    }
}