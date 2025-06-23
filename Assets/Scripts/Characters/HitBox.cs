using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �_���[�W���󂯂�p�̃R���C�_�[���Ǘ����܂��B
/// </summary>
public class HitBox : MonoBehaviour
{
    [SerializeField] private string m_hitBoxName;

    /// <summary>
    /// ���̃q�b�g�{�b�N�X�����Ă���I�u�W�F�N�g�̖��O
    /// </summary>
    public string HitBoxName { get { return m_hitBoxName; } }

    public Collider Coll { get; private set; }

    /// <summary>
    /// ���̃I�u�W�F�N�g��ID
    /// </summary>
    public int InstanceID { get; private set; }

    private void Start()
    {
        Coll = GetComponent<Collider>();
        InstanceID = GetComponentInParent<HitMaster>().GetInstanceID();
    }
}
