using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// プレイヤーのサウンドを管理します。
/// </summary>
public class PlayerSound : MonoBehaviour
{
    [SerializeField] AudioSource m_footAS;
    [SerializeField] AudioSource m_swordAS;

    /// <summary>
    /// プレイヤーのSEを再生します。
    /// </summary>
    /// <param name="name">再生したいSE名</param>
    public void PlaySE(string name)
    {
        switch(name)
        {
            case "Walk":
                SoundManager.Instance.PlaySE(m_footAS, "WalkSE");
                break;

            case "Run":
                SoundManager.Instance.PlaySE(m_footAS, "RunSE");
                break;

            case "SwordSwing":
                SoundManager.Instance.PlaySE(m_swordAS, "SwordSwingSE");
                break;

            case "SwordHit":
                SoundManager.Instance.PlaySE(m_swordAS, "SwordHitSE");
                break;

            case "SwordWallHit":
                SoundManager.Instance.PlaySE(m_swordAS, "SwordWallHitSE");
                break;
        }
    }
}
