using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �{�X�̍U���������Ǘ����܂��B
/// </summary>
public class BossAttack : MonoBehaviour
{
    [SerializeField] GameObject slash;          // Slash�I�u�W�F�N�g���擾

    private AttackLJampCollider m_jampLColl;    // �W�����v�A�^�b�N���̍����̃R���C�_�[
    private AttackRJampCollider m_jampRColl;    // �W�����v�A�^�b�N���̉E���̃R���C�_�[
    private Slash m_slash;                      // Slash�X�N���v�g��Slash�I�u�W�F�N�g����擾���܂�

    private AttackLCollider m_attackLColl;      // �ʏ�U�����̍����̃R���C�_�[
    private AttackRCollider m_attackRColl;      // �ʏ�U�����̉E���̃R���C�_�[

    private List<int> m_instanceIDList = new List<int>();                   // �q�b�g�����I�u�W�F�N�g�̃C���X�^���XID��ۊǂ��܂��B

    /// <summary>
    /// �C���X�^���XID���擾���邽�߂̊֐��ł��B
    /// </summary>
    public List<int> GetInstanceIDList { get { return m_instanceIDList; } }

    private void Awake()    
    {
        // ���
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
    /// ���������s���܂��B
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
    /// �C���X�^���XID��ݒ肵�܂��B
    /// </summary>
    /// <param name="id"></param>
    public void SetInstanceID(int id)
    {
        m_instanceIDList.Add(id);
    }

    /// <summary>
    /// �U���͂�ݒ肵�܂��B
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
    /// �W�����v�A�^�b�N�̓����蔻�肪�L���ɂȂ�܂��B
    /// </summary>
    public void AttackJampHitStart()
    {
        m_jampLColl.Coll.enabled = true;
        m_jampRColl.Coll.enabled = true;
        m_attackLColl.Coll.enabled = true;
        m_attackRColl.Coll.enabled = true;
    }

    /// <summary>
    /// �W�����v�A�^�b�N�̓����蔻�肪�����ɂȂ�܂��B
    /// </summary>
    public void AttackJampHitEnd()
    {
        m_jampLColl.Coll.enabled = false;
        m_jampRColl.Coll.enabled = false;
        m_attackLColl.Coll.enabled = false;
        m_attackRColl.Coll.enabled = false;
    }

    /// <summary>
    /// �p���`�U���̓����蔻�肪�L���ɂȂ�܂��B
    /// </summary>
    /// <param name="right">���E�𔻕ʂ��܂�</param>
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
    /// �p���`�U���̓����蔻�肪�����ɂȂ�܂��B
    /// </summary>
    /// <param name="right">���E�𔻕ʂ��܂�</param>
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
    /// �X���b�V���U�����s���܂��B
    /// </summary>
    /// <param name="right">���E�𔻕ʂ��܂�</param>
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
