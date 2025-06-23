using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ボスの右腕の攻撃用コライダーを管理します。
/// </summary>
public class AttackRCollider : MonoBehaviour
{
    private BossAttack m_bossAttack;    // BossAttackを保管する為の変数です。
    private float m_attackDamage;       // 攻撃力を保管するための変数です。

    /// <summary>
    /// ボスの右腕の攻撃用コライダーです。
    /// このプロパティは読み取り専用です。
    /// </summary>
    public Collider Coll { get; private set; }

    public void Awake()
    {
        // 代入
        m_bossAttack = GetComponentInParent<BossAttack>();
        Coll = GetComponent<Collider>();
    }

    /// <summary>
    /// 攻撃力を設定します。
    /// </summary>
    /// <param name="ad">攻撃力</param>
    public void SetAttackDamage(float ad)
    {
        m_attackDamage = ad;
    }

    // このコライダーがオブジェクトに当たったら実行されます。
    private void OnTriggerEnter(Collider other)
    {
        // 当たったオブジェクトのHitBoxを変数に格納します。
        var hitBox = other.GetComponent<HitBox>();

        // 当たったオブジェクトから不適切なオブジェクトを排除します。
        if (hitBox == null) return;                                                 // HitBoxスクリプトがアタッチされていない
        if (m_bossAttack.GetInstanceIDList.Contains(hitBox.InstanceID)) return;     // 既に攻撃済みのオブジェクトである
        if (!other.gameObject.CompareTag("Player")) return;                         // 攻撃先のオブジェクトタグがPlayerではない

        // ボスアタックのインスタンスIDにここで得たHitBoxのインスタンスIDを設定します。
        // これによりもし同じ攻撃で何度もヒット判定が行われた場合でも上で判定を不適切だとみなし排除することができます。
        m_bossAttack.SetInstanceID(hitBox.InstanceID);

        // IDamageableインターフェイスを持っているものにダメージを与える。
        IDamageable damageable = other.gameObject.GetComponentInParent<IDamageable>();

        // IDamageableインターフェイスを持っていたら
        if (damageable != null)
        {
            // 取得したオブジェクトにダメージを与えます。
            damageable.Damage(m_attackDamage);
        }
        // IDamageableインターフェイスを持っていなかったら
        else
        {
            Debug.Log("取得できなかった");
        }
    }
}
