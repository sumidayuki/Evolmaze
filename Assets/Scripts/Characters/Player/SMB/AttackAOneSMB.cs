using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackAOneSMB : StateMachineBehaviour
{
    // ���͂��󂯕t���n�߂�^�C�~���O�i���K�����ꂽ�A�j���[�V�������ԁj
    [SerializeField] private float INPUT_START_TIME;

    // ���̏�Ԃւ̑J�ڂ��\�ɂȂ�^�C�~���O
    [SerializeField] private float CHANGE_START_TIME;

    // ���݂̏�Ԃ��I������^�C�~���O
    [SerializeField] private float END_START_TIME;

    // �U�����J�n����^�C�~���O
    [SerializeField] private float ATTACK_START;
    
    // �U�����I������^�C�~���O
    [SerializeField] private float ATTACK_END;

    // ���݂̍U���̃q�b�g��Ԃ��Ǘ�����X�e�[�g�ł��B
    private enum HIT
    {
        START,  // �X�e�[�g�J�n
        ON,     // �q�b�g������J�n
        OFF,    // �q�b�g������I��
    }

    private HIT m_hit;

    /// <summary>
    /// �����蔻��̊J�n���Ǘ����܂��B
    /// </summary>
    public bool HitStart { get; private set; }

    /// <summary>
    /// �����蔻��̏I�����Ǘ����܂��B
    /// </summary>
    public bool HitEnd { get; private set; }

    // ���݂̃A�j���[�V�����̏�Ԃ��Ǘ�����X�e�[�g�ł��B
    private enum STATE
    {
        START,      // �X�e�[�g�J�n
        BUFFERING,  // ���͎�t�J�n
        CHANGE,     // �X�e�[�g�ύX���\
    }
    private STATE m_state;

    // ���͂��\��
    private bool m_canInput;

    // �X�e�[�g�̕ύX���\��
    private bool m_canStateChange;
    private bool m_canStateChangeHit;

    /// <summary>
    /// �X�e�[�g�̕ύX���\�����Ǘ����܂��B
    /// </summary>
    public bool StateChange { get; private set; }
    public bool StateChangeHit { get; private set; }
    
    /// <summary>
    /// �X�e�[�g���I�����Ă��邩���Ǘ����܂��B
    /// </summary>
    public bool StateEnd { get; private set; }

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // ������
        m_canInput = false;
        m_canStateChange = false;
        m_canStateChangeHit = false;
        StateChange = false;
        StateChangeHit = false;
        StateEnd = false;
        HitStart = false;
        HitEnd = false;

        m_state = STATE.START;
        m_hit = HIT.START;
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (!animator.IsInTransition(layerIndex))
        {
            var normalizedTime = stateInfo.normalizedTime;

            // ���݂̃A�j���[�V�����̃X�e�[�g��START�Ȃ�
            if (m_state == STATE.START)
            {
                if (normalizedTime >= INPUT_START_TIME)
                {
                    m_canInput = true;
                    m_state = STATE.BUFFERING;
                }
            }
            // ���݂̃A�j���[�V�����̃X�e�[�g��BUFFERING�Ȃ�
            else if (m_state == STATE.BUFFERING)
            {
                if (normalizedTime >= CHANGE_START_TIME)
                {
                    m_state = STATE.CHANGE;
                }
            }
            // ���݂̃A�j���[�V�����̃X�e�[�g��CHANGE�Ȃ�
            else if (m_state == STATE.CHANGE)
            {
                if (m_canStateChange)
                {
                    StateChange = true;
                }

                if (normalizedTime >= END_START_TIME)
                {
                    m_canInput = false;
                    m_canStateChange = false;
                    StateEnd = true;
                }
            }

            // ���݂̃q�b�g�X�e�[�g��START�Ȃ�
            if(m_hit == HIT.START)
            {
                if(normalizedTime > ATTACK_START)
                {
                    m_hit = HIT.ON;
                    HitStart = true;
                }
            }
            // ���݂̃q�b�g�X�e�[�g��ON�Ȃ�
            else if(m_hit == HIT.ON)
            {
                if(normalizedTime > ATTACK_END)
                {
                    m_hit = HIT.OFF;
                    HitStart = false;
                    HitEnd = true;
                }
            }

            if(m_canStateChangeHit)
            {
                StateChangeHit = true;
            }
        }
    }

    /// <summary>
    /// �U���̓��͂��擾���܂��B
    /// </summary>
    public void AttackInput()
    {
        if (m_canInput)
        {
            m_canStateChange = true;
        }
    }

    /// <summary>
    /// Hit�����o���ꂽ�Ƃ��ɌĂяo����܂��B
    /// </summary>
    public void HitInput()
    {
        m_canStateChangeHit = true;
    }
}
