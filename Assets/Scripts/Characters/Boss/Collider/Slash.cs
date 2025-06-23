using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ボスのスラッシュ攻撃（遠距離攻撃）を管理します。
/// </summary>
public class Slash : MonoBehaviour
{
    private Boss m_boss;            // Bossを保管するための変数です。
    private float m_attackDamage;   // 攻撃力を保管するための変数です。

    private Rigidbody m_rb;         // リジッドボディ(物理エンジン)を保管するための変数です。

    /// <summary>
    /// ボスのスラッシュ攻撃（遠距離攻撃用）コライダーです。
    /// このプロパティは読み取り専用です。
    /// </summary>
    public Collider Coll { get; private set; }

    public void Awake()
    {
        // 代入
        m_rb = GetComponent<Rigidbody>();
        m_boss = FindObjectOfType<Boss>();
        Coll = GetComponent<Collider>();

        // このオブジェクトを3秒後に破壊します。
        Destroy(this.gameObject, 3);
    }

    private void Update()
    {
        // リジッドボディが無効でないなら
        if (m_rb != null)
        {
            m_rb.velocity = m_boss.transform.forward * 10; // ボスの正面方向に進む
        }
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
        if (m_boss.Attack.GetInstanceIDList.Contains(hitBox.InstanceID)) return;    //既に攻撃済みのオブジェクトである

        // ボスアタックのインスタンスIDにここで得たHitBoxのインスタンスIDを設定します。
        // これによりもし同じ攻撃で何度もヒット判定が行われた場合でも上で判定を不適切だとみなし排除することができます。
        m_boss.Attack.SetInstanceID(hitBox.InstanceID);

        // 攻撃先のオブジェクトがタグがWallだったら
        if(other.gameObject.CompareTag("Wall"))
        {
            // ヒットエフェクトを出す。
            EffectManager.Instance.SlashHitEffect(this.transform.position, 1);

            // このオブジェクトを破壊する。
            Destroy(this.gameObject);
        }

        if (!other.gameObject.CompareTag("Player")) return;                         // 攻撃先のオブジェクトタグがPlayerではない

        // IDamageableインターフェイスを持っているものにダメージを与える。
        IDamageable damageable = other.gameObject.GetComponentInParent<IDamageable>();

        // IDamageableインターフェイスを持っていたら
        if (damageable != null)
        {
            // 取得したオブジェクトにダメージを与えます。
            damageable.Damage(m_attackDamage);

            // ヒットエフェクトを出す。
            EffectManager.Instance.SlashHitEffect(this.transform.position, 1);

            // このオブジェクトを破壊する。
            Destroy(this.gameObject);
        }
        // IDmageableインターフェイスを持っていなかったら
        else
        {
            Debug.Log("取得できなかった");
        }
    }
}
