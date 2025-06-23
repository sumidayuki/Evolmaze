using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �{�X�̃X���b�V���U���i�������U���j���Ǘ����܂��B
/// </summary>
public class Slash : MonoBehaviour
{
    private Boss m_boss;            // Boss��ۊǂ��邽�߂̕ϐ��ł��B
    private float m_attackDamage;   // �U���͂�ۊǂ��邽�߂̕ϐ��ł��B

    private Rigidbody m_rb;         // ���W�b�h�{�f�B(�����G���W��)��ۊǂ��邽�߂̕ϐ��ł��B

    /// <summary>
    /// �{�X�̃X���b�V���U���i�������U���p�j�R���C�_�[�ł��B
    /// ���̃v���p�e�B�͓ǂݎ���p�ł��B
    /// </summary>
    public Collider Coll { get; private set; }

    public void Awake()
    {
        // ���
        m_rb = GetComponent<Rigidbody>();
        m_boss = FindObjectOfType<Boss>();
        Coll = GetComponent<Collider>();

        // ���̃I�u�W�F�N�g��3�b��ɔj�󂵂܂��B
        Destroy(this.gameObject, 3);
    }

    private void Update()
    {
        // ���W�b�h�{�f�B�������łȂ��Ȃ�
        if (m_rb != null)
        {
            m_rb.velocity = m_boss.transform.forward * 10; // �{�X�̐��ʕ����ɐi��
        }
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
        if (m_boss.Attack.GetInstanceIDList.Contains(hitBox.InstanceID)) return;    //���ɍU���ς݂̃I�u�W�F�N�g�ł���

        // �{�X�A�^�b�N�̃C���X�^���XID�ɂ����œ���HitBox�̃C���X�^���XID��ݒ肵�܂��B
        // ����ɂ����������U���ŉ��x���q�b�g���肪�s��ꂽ�ꍇ�ł���Ŕ����s�K�؂��Ƃ݂Ȃ��r�����邱�Ƃ��ł��܂��B
        m_boss.Attack.SetInstanceID(hitBox.InstanceID);

        // �U����̃I�u�W�F�N�g���^�O��Wall��������
        if(other.gameObject.CompareTag("Wall"))
        {
            // �q�b�g�G�t�F�N�g���o���B
            EffectManager.Instance.SlashHitEffect(this.transform.position, 1);

            // ���̃I�u�W�F�N�g��j�󂷂�B
            Destroy(this.gameObject);
        }

        if (!other.gameObject.CompareTag("Player")) return;                         // �U����̃I�u�W�F�N�g�^�O��Player�ł͂Ȃ�

        // IDamageable�C���^�[�t�F�C�X�������Ă�����̂Ƀ_���[�W��^����B
        IDamageable damageable = other.gameObject.GetComponentInParent<IDamageable>();

        // IDamageable�C���^�[�t�F�C�X�������Ă�����
        if (damageable != null)
        {
            // �擾�����I�u�W�F�N�g�Ƀ_���[�W��^���܂��B
            damageable.Damage(m_attackDamage);

            // �q�b�g�G�t�F�N�g���o���B
            EffectManager.Instance.SlashHitEffect(this.transform.position, 1);

            // ���̃I�u�W�F�N�g��j�󂷂�B
            Destroy(this.gameObject);
        }
        // IDmageable�C���^�[�t�F�C�X�������Ă��Ȃ�������
        else
        {
            Debug.Log("�擾�ł��Ȃ�����");
        }
    }
}
