using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectManager : MonoBehaviour
{
    [SerializeField] GameObject[] hitEffects;

    public static EffectManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject); // すでにインスタンスが存在する場合は破棄
        }
    }

    public void CharacterHitEffect(Vector3 pos, float time)
    {
        GameObject effect = Instantiate(hitEffects[0], pos, Quaternion.identity);
        Destroy(effect, time);
    }

    public void ObjectHitEffect(Vector3 pos, float time)
    {
        GameObject effect = Instantiate(hitEffects[1], pos, Quaternion.identity);
        Destroy(effect, time);
    }

    public void SlashHitEffect(Vector3 pos, float time)
    {
        GameObject effect = Instantiate(hitEffects[2], pos, Quaternion.identity);
        Destroy(effect, time);
    }
}
