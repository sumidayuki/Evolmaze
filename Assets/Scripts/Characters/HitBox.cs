using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ダメージを受ける用のコライダーを管理します。
/// </summary>
public class HitBox : MonoBehaviour
{
    [SerializeField] private string m_hitBoxName;

    /// <summary>
    /// このヒットボックスがついているオブジェクトの名前
    /// </summary>
    public string HitBoxName { get { return m_hitBoxName; } }

    public Collider Coll { get; private set; }

    /// <summary>
    /// このオブジェクトのID
    /// </summary>
    public int InstanceID { get; private set; }

    private void Start()
    {
        Coll = GetComponent<Collider>();
        InstanceID = GetComponentInParent<HitMaster>().GetInstanceID();
    }
}
