using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ボスの攻撃処理を管理します。
/// </summary>
public class BossAttack : MonoBehaviour
{
    [SerializeField] GameObject slash;          // Slashオブジェクトを取得

    private AttackLJampCollider m_jampLColl;    // ジャンプアタック時の左側のコライダー
    private AttackRJampCollider m_jampRColl;    // ジャンプアタック時の右側のコライダー
    private Slash m_slash;                      // SlashスクリプトをSlashオブジェクトから取得します

    private AttackLCollider m_attackLColl;      // 通常攻撃時の左側のコライダー
    private AttackRCollider m_attackRColl;      // 通常攻撃時の右側のコライダー

    private List<int> m_instanceIDList = new List<int>();                   // ヒットしたオブジェクトのインスタンスIDを保管します。

    /// <summary>
    /// インスタンスIDを取得するための関数です。
    /// </summary>
    public List<int> GetInstanceIDList { get { return m_instanceIDList; } }

    private void Awake()    
    {
        // 代入
        m_jampLColl = GetComponentInChildren<AttackLJampCollider>();
        m_jampRColl = GetComponentInChildren<AttackRJampCollider>();

        m_attackLColl = GetComponentInChildren<AttackLCollider>();
        m_attackRColl = GetComponentInChildren<AttackRCollider>();

        if (m_jampLColl != null && m_jampLColl.Coll.enabled)
        {
            m_jampLColl.Coll.enabled = false;
        }
        if (m_jampRColl != null && m_jampRColl.Coll.enabled)
        {
            m_jampRColl.Coll.enabled = false;
        }
        if (m_attackLColl != null && m_attackLColl.Coll.enabled)
        {
            m_attackLColl.Coll.enabled = false;
        }
        if (m_attackRColl != null && m_attackRColl.Coll.enabled)
        {
            m_attackRColl.Coll.enabled = false;
        }
    }

    /// <summary>
    /// 初期化を行います。
    /// </summary>
    public void Init()
    {
        m_jampLColl.Coll.enabled = false;
        m_jampRColl.Coll.enabled = false;
        m_attackLColl.Coll.enabled = false;
        m_attackRColl.Coll.enabled = false;

        m_instanceIDList.Clear();
    }

    /// <summary>
    /// インスタンスIDを設定します。
    /// </summary>
    /// <param name="id"></param>
    public void SetInstanceID(int id)
    {
        m_instanceIDList.Add(id);
    }

    /// <summary>
    /// 攻撃力を設定します。
    /// </summary>
    /// <param name="ad"></param>
    public void SetAttackDamage(float ad)
    {
        m_jampLColl.SetAttackDamage(ad);
        m_jampRColl.SetAttackDamage(ad);
        m_attackLColl.SetAttackDamage(ad);
        m_attackRColl.SetAttackDamage(ad);
    }

    /// <summary>
    /// ジャンプアタックの当たり判定が有効になります。
    /// </summary>
    public void AttackJampHitStart()
    {
        m_jampLColl.Coll.enabled = true;
        m_jampRColl.Coll.enabled = true;
        m_attackLColl.Coll.enabled = true;
        m_attackRColl.Coll.enabled = true;
    }

    /// <summary>
    /// ジャンプアタックの当たり判定が無効になります。
    /// </summary>
    public void AttackJampHitEnd()
    {
        m_jampLColl.Coll.enabled = false;
        m_jampRColl.Coll.enabled = false;
        m_attackLColl.Coll.enabled = false;
        m_attackRColl.Coll.enabled = false;
    }

    /// <summary>
    /// パンチ攻撃の当たり判定が有効になります。
    /// </summary>
    /// <param name="right">左右を判別します</param>
    public void PunchHitStart(bool right)
    {
        if(right)
        {
            m_attackRColl.Coll.enabled = true;
        }
        else
        {
            m_attackLColl.Coll.enabled = true;
        }
    }

    /// <summary>
    /// パンチ攻撃の当たり判定が無効になります。
    /// </summary>
    /// <param name="right">左右を判別します</param>
    public void PunchHitEnd(bool right)
    {
        if(right)
        {
            m_attackRColl.Coll.enabled = false;
        }
        else
        {
            m_attackLColl.Coll.enabled = false;
        }
    }

    /// <summary>
    /// スラッシュ攻撃を行います。
    /// </summary>
    /// <param name="right">左右を判別します</param>
    /// <param name="bossTransform"></param>
    /// <param name="ad"></param>
    public void SlashAttack(bool right, Transform bossTransform, float ad)
    {
        if (right)
        {
            GameObject obj = Instantiate(slash, m_attackRColl.Coll.transform.position, bossTransform.rotation);
            m_slash = obj.GetComponent<Slash>();
            m_slash.SetAttackDamage(ad);
        }
        else
        {
            GameObject obj = Instantiate(slash, m_attackLColl.Coll.transform.position, bossTransform.rotation);
            m_slash = obj.GetComponent<Slash>();
            m_slash.SetAttackDamage(ad);
        }
    }
}
