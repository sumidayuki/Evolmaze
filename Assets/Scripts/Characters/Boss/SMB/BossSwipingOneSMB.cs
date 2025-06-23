using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossSwipingOneSMB : StateMachineBehaviour
{
    [SerializeField] private float END_START_TIME;  // ステートの終了時間

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

    public bool HitStart { get; private set; }  // 当たり判定開始を伝えるプロパティ
    public bool HitEnd { get; private set; }    // 当たり判定終了を伝えるプロパティ

    public bool StateEnd { get; private set; }  // ステートの終了を伝えるプロパティ

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
            // ステート内の時間を 0.0 ~ 1.0 に変換した時間
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
