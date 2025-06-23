using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �v���C���[�̃T�E���h���Ǘ����܂��B
/// </summary>
public class PlayerSound : MonoBehaviour
{
    [SerializeField] AudioSource m_footAS;
    [SerializeField] AudioSource m_swordAS;

    /// <summary>
    /// �v���C���[��SE���Đ����܂��B
    /// </summary>
    /// <param name="name">�Đ�������SE��</param>
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
