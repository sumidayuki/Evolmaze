using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackAOneSMB : StateMachineBehaviour
{
    // 入力を受け付け始めるタイミング（正規化されたアニメーション時間）
    [SerializeField] private float INPUT_START_TIME;

    // 次の状態への遷移が可能になるタイミング
    [SerializeField] private float CHANGE_START_TIME;

    // 現在の状態が終了するタイミング
    [SerializeField] private float END_START_TIME;

    // 攻撃が開始するタイミング
    [SerializeField] private float ATTACK_START;
    
    // 攻撃が終了するタイミング
    [SerializeField] private float ATTACK_END;

    // 現在の攻撃のヒット状態を管理するステートです。
    private enum HIT
    {
        START,  // ステート開始
        ON,     // ヒット判定を開始
        OFF,    // ヒット判定を終了
    }

    private HIT m_hit;

    /// <summary>
    /// 当たり判定の開始を管理します。
    /// </summary>
    public bool HitStart { get; private set; }

    /// <summary>
    /// 当たり判定の終了を管理します。
    /// </summary>
    public bool HitEnd { get; private set; }

    // 現在のアニメーションの状態を管理するステートです。
    private enum STATE
    {
        START,      // ステート開始
        BUFFERING,  // 入力受付開始
        CHANGE,     // ステート変更が可能
    }
    private STATE m_state;

    // 入力が可能か
    private bool m_canInput;

    // ステートの変更が可能か
    private bool m_canStateChange;
    private bool m_canStateChangeHit;

    /// <summary>
    /// ステートの変更が可能かを管理します。
    /// </summary>
    public bool StateChange { get; private set; }
    public bool StateChangeHit { get; private set; }
    
    /// <summary>
    /// ステートが終了しているかを管理します。
    /// </summary>
    public bool StateEnd { get; private set; }

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // 初期化
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

            // 現在のアニメーションのステートがSTARTなら
            if (m_state == STATE.START)
            {
                if (normalizedTime >= INPUT_START_TIME)
                {
                    m_canInput = true;
                    m_state = STATE.BUFFERING;
                }
            }
            // 現在のアニメーションのステートがBUFFERINGなら
            else if (m_state == STATE.BUFFERING)
            {
                if (normalizedTime >= CHANGE_START_TIME)
                {
                    m_state = STATE.CHANGE;
                }
            }
            // 現在のアニメーションのステートがCHANGEなら
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

            // 現在のヒットステートがSTARTなら
            if(m_hit == HIT.START)
            {
                if(normalizedTime > ATTACK_START)
                {
                    m_hit = HIT.ON;
                    HitStart = true;
                }
            }
            // 現在のヒットステートがONなら
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
    /// 攻撃の入力を取得します。
    /// </summary>
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
