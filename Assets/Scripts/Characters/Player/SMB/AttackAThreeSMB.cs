using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackAThreeSMB : StateMachineBehaviour
{
    [SerializeField] private float END_START_TIME;

    [SerializeField] private float ATTACK_START;
    [SerializeField] private float ATTACK_END;

    private enum HIT
    {
        START,
        ON,
        OFF,
    }

    private HIT m_hit;

    private bool m_canStateChangeHit;

    public bool HitStart { get; private set; }
    public bool HitEnd { get; private set; }

    public bool StateChangeHit { get; private set; }

    public bool StateEnd { get; private set; }

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        m_canStateChangeHit = false;
        StateEnd = false;
        StateChangeHit = false;
        HitStart = false;
        HitEnd = false;

        m_hit = HIT.START;
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if(!animator.IsInTransition(layerIndex))
        {
            var normalizedTime = stateInfo.normalizedTime;

            if (normalizedTime >= END_START_TIME)
            {
                StateEnd = true;
            }

            if(m_hit == HIT.START)
            {
                if(normalizedTime > ATTACK_START)
                {
                    HitStart = true;
                    m_hit = HIT.ON;
                }
            }
            else if(m_hit == HIT.ON)
            {
                if (normalizedTime > ATTACK_END)
                {
                    HitStart = false;
                    HitEnd = true;
                    m_hit = HIT.OFF;
                }
            }

            if(m_canStateChangeHit)
            {
                StateChangeHit = true;
            }
        }
    }

    /// <summary>
    /// Hit‚ªŒŸo‚³‚ê‚½‚Æ‚«‚ÉŒÄ‚Ño‚³‚ê‚Ü‚·B
    /// </summary>
    public void HitInput()
    {
        m_canStateChangeHit = true;
    }
}
