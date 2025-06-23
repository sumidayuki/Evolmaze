using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class EnemyWeapon : MonoBehaviour
{
    private BoxCollider weaponCollider;

    private int attackDamage;

    private List<int> m_instanceIDList = new List<int>();

    private void Awake()
    {
        weaponCollider = GetComponent<BoxCollider>();
        weaponCollider.enabled = false;
    }

    public void HitStart()
    {
        weaponCollider.enabled = true;
    }

    public void HitEnd()
    {
        weaponCollider.enabled = false;
    }

    public void Init()
    {
        weaponCollider.enabled = false;
        m_instanceIDList.Clear();
    }

    public void SetAttackDamage(int ad)
    {
        attackDamage = ad;
    }

    private void OnTriggerEnter(Collider other)
    {
        var hitBox = other.GetComponent<HitBox>();

        if (hitBox == null) return;
        if (m_instanceIDList.Contains(hitBox.InstanceID)) return;
        if (!other.gameObject.CompareTag("Player")) return;

        m_instanceIDList.Add(hitBox.InstanceID);

        // Damageableインターフェイスを持っているものにダメージを与える。
        IDamageable damageable = other.gameObject.GetComponentInParent<IDamageable>();

        if (damageable != null)
        {
            SoundManager.Instance.PlaySE("TakeDamage");
            damageable.Damage(attackDamage);
        }
        else
        {
            Debug.Log("取得できなかった");
        }
    }
}
