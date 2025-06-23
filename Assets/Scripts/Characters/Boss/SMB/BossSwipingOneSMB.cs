using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossSwipingOneSMB : StateMachineBehaviour
{
    [SerializeField] private float END_START_TIME;  // �X�e�[�g�̏I������

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

    public bool HitStart { get; private set; }  // �����蔻��J�n��`����v���p�e�B
    public bool HitEnd { get; private set; }    // �����蔻��I����`����v���p�e�B

    public bool StateEnd { get; private set; }  // �X�e�[�g�̏I����`����v���p�e�B

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        StateEnd = false;
        HitStart = false;
        HitEnd = false;
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

            if (m_hit == HIT.START)
            {
                if (normalizedTime > ATTACK_START)
                {
                    m_hit = HIT.ON;
                    HitStart = true;
                }
            }
            else if (m_hit == HIT.ON)
            {
                if (normalizedTime > ATTACK_END)
                {
                    m_hit = HIT.OFF;
                    HitStart = false;
                    HitEnd = true;
                }
            }
        }
    }
}
