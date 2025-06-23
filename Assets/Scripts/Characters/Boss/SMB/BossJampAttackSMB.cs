using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ボスのジャンプアタックアニメーションを管理します。
/// </summary>
public class BossJampAttackSMB : StateMachineBehaviour
{
    [SerializeField] private float END_START_TIME;  // ステートの終了時間

    [SerializeField] private float JAMP_START;      // ジャンプ攻撃の開始時間
    [SerializeField] private float JAMP_END;        // ジャンプ攻撃の終了時間

    [SerializeField] private float ATTACK_START;    // 攻撃の開始時間
    [SerializeField] private float ATTACK_END;      // 攻撃の終了時間

    // 当たり判定のステート
    private enum HIT    
    {
        START,  // ステート開始
        ON,     // 当たり判定オン
        OFF,    // 当たり判定オフ
    }

    private HIT m_hit;

    /// <summary>
    /// 当たり判定の開始を判断します。
    /// このプロパティは読み取り専用です。
    /// </summary>
    public bool HitStart { get; private set; }
    
    /// <summary>
    /// 当たり判定の終了を判断します。
    /// このプロパティは読み取り専用です。
    /// </summary>
    public bool HitEnd { get; private set; }

    /// <summary>
    /// ジャンプの開始を判断します。
    /// このプロパティは読み取り専用です。
    /// </summary>
    public bool JampStart { get; private set; }

    /// <summary>
    /// ジャンプの終了を判断します。
    /// このプロパティは読み取り専用です。
    /// </summary>
    public bool JampEnd { get; private set; }

    // ジャンプの状態を表します。
    private enum JAMP
    {
        START,  // ジャンプの開始時
        AIR,    // ジャンプの空中状態
        END,    // ジャンプの終了時
    }

    // ジャンプの状態を管理します。
    private JAMP m_jamp;

    /// <summary>
    /// このアニメーションの終了を表します。
    /// このプロパティは読み取り専用です。
    /// </summary>
    public bool StateEnd { get; private set; }

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // 初期化
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
            // ステート内の時間を 0.0 ~ 1.0 に変換した時間
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
