using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �{�X�̃W�����v�A�^�b�N�A�j���[�V�������Ǘ����܂��B
/// </summary>
public class BossJampAttackSMB : StateMachineBehaviour
{
    [SerializeField] private float END_START_TIME;  // �X�e�[�g�̏I������

    [SerializeField] private float JAMP_START;      // �W�����v�U���̊J�n����
    [SerializeField] private float JAMP_END;        // �W�����v�U���̏I������

    [SerializeField] private float ATTACK_START;    // �U���̊J�n����
    [SerializeField] private float ATTACK_END;      // �U���̏I������

    // �����蔻��̃X�e�[�g
    private enum HIT    
    {
        START,  // �X�e�[�g�J�n
        ON,     // �����蔻��I��
        OFF,    // �����蔻��I�t
    }

    private HIT m_hit;

    /// <summary>
    /// �����蔻��̊J�n�𔻒f���܂��B
    /// ���̃v���p�e�B�͓ǂݎ���p�ł��B
    /// </summary>
    public bool HitStart { get; private set; }
    
    /// <summary>
    /// �����蔻��̏I���𔻒f���܂��B
    /// ���̃v���p�e�B�͓ǂݎ���p�ł��B
    /// </summary>
    public bool HitEnd { get; private set; }

    /// <summary>
    /// �W�����v�̊J�n�𔻒f���܂��B
    /// ���̃v���p�e�B�͓ǂݎ���p�ł��B
    /// </summary>
    public bool JampStart { get; private set; }

    /// <summary>
    /// �W�����v�̏I���𔻒f���܂��B
    /// ���̃v���p�e�B�͓ǂݎ���p�ł��B
    /// </summary>
    public bool JampEnd { get; private set; }

    // �W�����v�̏�Ԃ�\���܂��B
    private enum JAMP
    {
        START,  // �W�����v�̊J�n��
        AIR,    // �W�����v�̋󒆏��
        END,    // �W�����v�̏I����
    }

    // �W�����v�̏�Ԃ��Ǘ����܂��B
    private JAMP m_jamp;

    /// <summary>
    /// ���̃A�j���[�V�����̏I����\���܂��B
    /// ���̃v���p�e�B�͓ǂݎ���p�ł��B
    /// </summary>
    public bool StateEnd { get; private set; }

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // ������
        JampStart = false;
        StateEnd = false;
        JampEnd = false;
        HitStart = false;
        HitEnd = false;
        m_jamp = JAMP.START;
        m_hit = HIT.START;
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (!animator.IsInTransition(layerIndex))
        {
            // �X�e�[�g���̎��Ԃ� 0.0 ~ 1.0 �ɕϊ���������
            var normalizedTime = stateInfo.normalizedTime;

            if (normalizedTime >= END_START_TIME)
            {
                StateEnd = true;
            }

            if (m_jamp == JAMP.START)
            {
                if (normalizedTime > JAMP_START)
                {
                    JampStart = true;
                    m_jamp = JAMP.AIR;
                }
            }
            if (m_jamp == JAMP.AIR)
            {
                if (normalizedTime > JAMP_END)
                {
                    JampStart = false;
                    JampEnd = true;
                    m_jamp = JAMP.END;
                }
            }

            if(m_hit == HIT.START)
            {
                if(normalizedTime > ATTACK_START)
                {
                    m_hit = HIT.ON;
                    HitStart = true;
                }
            }
            else if(m_hit == HIT.ON)
            {
                if(normalizedTime > ATTACK_END)
                {
                    m_hit = HIT.OFF;
                    HitStart = false;
                    HitEnd = true;
                }
            }
        }
    }
}
