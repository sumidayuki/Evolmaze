using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ボスの近接攻撃を管理します。
/// </summary>
public class BossAttackMeleeState : StateBase<Boss>
{
    private BossPunchOneSMB m_bossPunchOneSMB;  // 近接攻撃の一回目（右パンチ）のアニメーション
    private BossPunchTwoSMB m_bossPunchTwoSMB;  // 近接攻撃の二回目（左パンチ）のアニメーション

    // ボスの現在の状態を管理します。
    private enum STATE
    {
        ONE,    // 一回目の攻撃
        TWO,    // 二回目の攻撃
    }

    private STATE m_state;

    public override void Enter(Boss boss)
    {
        // SMBを取得します。
        m_bossPunchOneSMB = boss.GetBehaviour<BossPunchOneSMB>();
        m_bossPunchTwoSMB = boss.GetBehaviour<BossPunchTwoSMB>();

        // 初期化
        boss.Attack.Init();

        // この攻撃のダメージを設定します。
        boss.Attack.SetAttackDamage(boss.BossData.attackDamage);

        // この攻撃のアニメーションを再生します。
        boss.Anime.Animator.SetTrigger("AttackA");
    }

    public override void Execute(Boss boss)
    {
        // ボスの状態が ONE なら
        if(m_state == STATE.ONE)
        {
            // 一回目の攻撃が終了したら
            if (m_bossPunchOneSMB.StateEnd)
            {
                // ボスの攻撃を初期化します。（攻撃用のコライダーオフや既に攻撃している判定になっているので解除）
                boss.Attack.Init();

                // 二回目の攻撃のアニメーションを再生します。
                boss.Anime.Animator.SetTrigger("AttackA");

                // ボスの状態を二回目の攻撃に変更します。  
                m_state = STATE.TWO;
            }

            // 一回目の攻撃のヒットスタートが発生したら
            if (m_bossPunchOneSMB.HitStart)
            {
                // ボスの右攻撃の当たり判定を有効にします。
                boss.Attack.PunchHitStart(true);
            }
            // 一回目の攻撃のヒットが終了したら
            else if (m_bossPunchOneSMB.HitEnd)
            {
                // プレイヤーの方向を向きます。
                boss.AI.LookTarget(boss);

                // ボスの右攻撃の当たり判定を無効にします。
                boss.Attack.PunchHitEnd(true);
            }
        }
        // ボスの状態が TWO なら
        else if (m_state == STATE.TWO)
        {
            // 二回目の攻撃が終了したら
            if (m_bossPunchTwoSMB.StateEnd)
            {
                // プレイヤーとの距離が 3 以下なら
                if(boss.AI.GetTargetDistance(boss.gameObject.transform.position) <= 3)
                {
                    // 回避ステートに変更します。
                    boss.ChangeState(new BossRetreatState());
                }
                // プレイヤーとの距離が離れているなら
                else
                {
                    // 移動ステートに変更します。
                    boss.ChangeState(new BossMoveState());
                }
            }

            // 二回目の攻撃のヒットスタートが発生したら
            if (m_bossPunchTwoSMB.HitStart)
            {
                // ボスの左攻撃の当たり判定を有効にします。
                boss.Attack.PunchHitStart(false);
            }
            // 二回目の攻撃のヒットが終了したら
            else if (m_bossPunchTwoSMB.HitEnd)
            {
                // プレイヤーの方向を向きます。
                boss.Attack.PunchHitEnd(false);
            }
        }
    }

    public override void Exit(Boss boss)
    {
        // アニメーションをリセットします。
        boss.Anime.Animator.ResetTrigger("AttackA");
    }
}
