using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using UnityEngine;

public class Sword : MonoBehaviour
{
    private float m_attackDamage;

    private List<int> m_instanceIDList = new List<int>();
    private List<SwordColl> m_swordColls = new List<SwordColl>();

    private Player m_player;

    private GameObject m_previousHitObj;

    private void Start()
    {
        m_previousHitObj = null;
        foreach (Transform child in this.transform)
        {
            SwordColl sword = child.gameObject.GetComponent<SwordColl>();
            m_swordColls.Add(sword);
        }

        m_player = GetComponentInParent<Player>();

        Init();
    }

    public void HitStart()
    {
        foreach (SwordColl sword in m_swordColls)
        {
            sword.Coll.enabled = true;
        }
    }

    public void HitEnd()
    {
        foreach (SwordColl sword in m_swordColls)
        {
            if (sword.Coll.enabled)
            {
                Debug.Log("コライダーを無効化します");
                sword.Coll.enabled = false;
            }
        }
    }

    public void Init()
    {
        foreach (SwordColl sword in m_swordColls)
        {
            if (sword.Coll.enabled)
            {
                sword.Coll.enabled = false;
            }
        }
        m_instanceIDList.Clear();
    }

    public void SetAttackDamage(float ad)
    {
        m_attackDamage = ad;
    }

    public void Hit(GameObject other, GameObject coll)
    {
        var hitBox = other.GetComponent<HitBox>();

        if (hitBox == null) return;
        if (m_instanceIDList.Contains(hitBox.InstanceID)) return;
        
        if (other.CompareTag("Wall"))
        {
            m_instanceIDList.Add(hitBox.InstanceID);
            m_player.Sound.PlaySE("SwordWallHit");
            EffectManager.Instance.ObjectHitEffect(coll.transform.position, 1.5f);
            m_player.Damage(0);
        }

        if (!other.CompareTag("Enemy")) return;

        m_instanceIDList.Add(hitBox.InstanceID);

        // Damageableインターフェイスを持っているものにダメージを与える。
        IDamageable damageable = other.GetComponentInParent<IDamageable>();

        if (damageable != null)
        {
            m_player.Sound.PlaySE("SwordHit");
            EffectManager.Instance.CharacterHitEffect(coll.transform.position, 1.5f);
            damageable.Damage(m_attackDamage);
        }
        else
        {
            Debug.Log("取得できなかった");
        }
    }
}
