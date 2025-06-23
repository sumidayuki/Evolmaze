using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordColl : MonoBehaviour
{
    private Sword m_sword;

    public Collider Coll { get; private set; }

    private void Awake()
    {
        Coll = GetComponent<Collider>();
        m_sword = GetComponentInParent<Sword>();
    }

    private void OnTriggerEnter(Collider other)
    {
        m_sword.Hit(other.gameObject, this.gameObject);
    }
}
