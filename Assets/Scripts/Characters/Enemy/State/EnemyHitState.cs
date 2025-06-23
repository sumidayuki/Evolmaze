using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHitState : StateBase<Enemy>
{
    EnemyHitSMB m_hitSMB;

    public override void Enter(Enemy enemy)
    {
        enemy.Anime.Animator.SetTrigger("Hit");
        m_hitSMB = enemy.GetBehaviour<EnemyHitSMB>();
    }

    public override void Execute(Enemy enemy)
    {
        if (m_hitSMB.StateEnd)
        {
            enemy.ChangeState(new EnemyChaceState());
        }
    }

    public override void Exit(Enemy enemy)
    {
        enemy.Anime.Animator.ResetTrigger("Hit");
    }
}
