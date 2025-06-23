using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackATwoSMB : StateMachineBehaviour
{
    // 入力を受け付け始めるタイミング（正規化されたアニメーション時間）
    [SerializeField] private float INPUT_START_TIME;

    // 次の状態への遷移が可能になるタイミング
    [SerializeField] private float CHANGE_START_TIME;

    // 現在の状態が終了するタイミング
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

    public bool HitStart { get; private set; }
    public bool HitEnd { get; private set; }

    private enum STATE
    {
        START,
        BUFFERING,
        CHANGE,
    }
    private STATE m_state;

    private bool m_canInput;
    private bool m_canStateChange;
    private bool m_canStateChangeHit;

    public bool StateChange { get; private set; }
    public bool StateChangeHit { get; private set; }
    public bool StateEnd { get; private set; }

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
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

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (!animator.IsInTransition(layerIndex))
        {
            var normalizedTime = stateInfo.normalizedTime;

            if (m_state == STATE.START)
            {
                if (normalizedTime >= INPUT_START_TIME)
                {
                    m_canInput = true;
                    m_state = STATE.BUFFERING;
                }
            }
            else if (m_state == STATE.BUFFERING)
            {
                if (normalizedTime >= CHANGE_START_TIME)
                {
                    m_state = STATE.CHANGE;
                }
            }
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
                if(normalizedTime > ATTACK_END)
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

    public void AttackInput()
    {
        if (m_canInput)
        {
            m_canStateChange = true;
        }
    }

    /// <summary>
    /// Hitが検出されたときに呼び出されます。
    /// </summary>
    public void HitInput()
    {
        m_canStateChangeHit = true;
    }
}
