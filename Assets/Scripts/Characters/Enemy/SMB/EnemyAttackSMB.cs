using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttackSMB : StateMachineBehaviour
{
    [SerializeField] private float ATTACK_START_TIME;
    [SerializeField] private float ATTACK_END_TIME;
    [SerializeField] private float END_START_TIME;

    private enum STATE
    {
        ATTACKSTART,
        ATTACKEND,
        STATEEND,
    }
    private STATE m_state;

    public bool StateEnd { get; private set; }
    public bool IsAttack { get; private set; }

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        StateEnd = false;
        IsAttack = false;
        m_state = STATE.ATTACKSTART;
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (!animator.IsInTransition(layerIndex))
        {
            var normalizedTime = stateInfo.normalizedTime;
            if (m_state == STATE.ATTACKSTART)
            {
                if (normalizedTime >= ATTACK_START_TIME)
                {
                    IsAttack = true;
                    m_state = STATE.ATTACKEND;
                }
            }
            else if (m_state == STATE.ATTACKEND)
            {
                if (normalizedTime >= ATTACK_END_TIME)
                {
                    IsAttack = false;
                    m_state = STATE.STATEEND;
                }
            }
            else if (m_state == STATE.STATEEND)
            {
                if (normalizedTime >= END_START_TIME)
                {
                    StateEnd = true;
                }
            }
        }
    }
}
