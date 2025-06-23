using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �{�X�̉E�r�̍U���p�R���C�_�[���Ǘ����܂��B
/// </summary>
public class AttackRCollider : MonoBehaviour
{
    private BossAttack m_bossAttack;    // BossAttack��ۊǂ���ׂ̕ϐ��ł��B
    private float m_attackDamage;       // �U���͂�ۊǂ��邽�߂̕ϐ��ł��B

    /// <summary>
    /// �{�X�̉E�r�̍U���p�R���C�_�[�ł��B
    /// ���̃v���p�e�B�͓ǂݎ���p�ł��B
    /// </summary>
    public Collider Coll { get; private set; }

    public void Awake()
    {
        // ���
        m_bossAttack = GetComponentInParent<BossAttack>();
        Coll = GetComponent<Collider>();
    }

    /// <summary>
    /// �U���͂�ݒ肵�܂��B
    /// </summary>
    /// <param name="ad">�U����</param>
    public void SetAttackDamage(float ad)
    {
        m_attackDamage = ad;
    }

    // ���̃R���C�_�[���I�u�W�F�N�g�ɓ�����������s����܂��B
    private void OnTriggerEnter(Collider other)
    {
        // ���������I�u�W�F�N�g��HitBox��ϐ��Ɋi�[���܂��B
        var hitBox = other.GetComponent<HitBox>();

        // ���������I�u�W�F�N�g����s�K�؂ȃI�u�W�F�N�g��r�����܂��B
        if (hitBox == null) return;                                                 // HitBox�X�N���v�g���A�^�b�`����Ă��Ȃ�
        if (m_bossAttack.GetInstanceIDList.Contains(hitBox.InstanceID)) return;     // ���ɍU���ς݂̃I�u�W�F�N�g�ł���
        if (!other.gameObject.CompareTag("Player")) return;                         // �U����̃I�u�W�F�N�g�^�O��Player�ł͂Ȃ�

        // �{�X�A�^�b�N�̃C���X�^���XID�ɂ����œ���HitBox�̃C���X�^���XID��ݒ肵�܂��B
        // ����ɂ����������U���ŉ��x���q�b�g���肪�s��ꂽ�ꍇ�ł���Ŕ����s�K�؂��Ƃ݂Ȃ��r�����邱�Ƃ��ł��܂��B
        m_bossAttack.SetInstanceID(hitBox.InstanceID);

        // IDamageable�C���^�[�t�F�C�X�������Ă�����̂Ƀ_���[�W��^����B
        IDamageable damageable = other.gameObject.GetComponentInParent<IDamageable>();

        // IDamageable�C���^�[�t�F�C�X�������Ă�����
        if (damageable != null)
        {
            // �擾�����I�u�W�F�N�g�Ƀ_���[�W��^���܂��B
            damageable.Damage(m_attackDamage);
        }
        // IDamageable�C���^�[�t�F�C�X�������Ă��Ȃ�������
        else
        {
            Debug.Log("�擾�ł��Ȃ�����");
        }
    }
}
