using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ボスのジャンプアタックを管理します。
/// </summary>
public class BossAttackJampState : StateBase<Boss>
{
    BossJampAttackSMB m_jampAttackSMB;

    public override void Enter(Boss boss)
    {
        // 初期化
        boss.Anime.Animator.SetTrigger("AttackC");

        boss.Attack.Init();

        boss.Attack.SetAttackDamage(boss.BossData.attackDamage * 2);

        m_jampAttackSMB = boss.GetBehaviour<BossJampAttackSMB>();
    }

    public override void Execute(Boss boss)
    {
        // ターゲットの方向を向きます。
        boss.AI.LookTarget(boss);

        // このアニメーションが終了していたら
        if (m_jampAttackSMB.StateEnd)
        {
            boss.ChangeState(new BossMoveState());
        }

        if(m_jampAttackSMB.HitStart)
        {
            boss.Attack.AttackJampHitStart();
        }
        else if(m_jampAttackSMB.HitEnd)
        {
            boss.Attack.AttackJampHitEnd();
        }

        // ジャンプがスタートしたら
        if(m_jampAttackSMB.JampStart)
        {
            boss.AI.ToTarget();
        }
        // ジャンプが終了したら
        else if (m_jampAttackSMB.JampEnd)
        {
            boss.AI.Agent.ResetPath();
        }
    }

    public override void Exit(Boss boss)
    {
        boss.Anime.Animator.ResetTrigger("AttackC");
    }
}
