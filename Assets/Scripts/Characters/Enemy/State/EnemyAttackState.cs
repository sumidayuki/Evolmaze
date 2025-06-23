using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyAttackState : StateBase<Enemy>
{
    EnemyAttackSMB m_attackSMB;

    public override void Enter(Enemy enemy)
    {
        enemy.Anime.Animator.SetTrigger("Attack");

        enemy.Weapon.Init();

        m_attackSMB = enemy.GetBehaviour<EnemyAttackSMB>();
    }

    public override void Execute(Enemy enemy)
    {
        if (enemy.IsHit)
        {
            if (enemy.CurrentHP <= 0)
            {
                enemy.Death();
            }
            else
            {
                enemy.ChangeState(new EnemyHitState());
            }
            return;
        }

        if (m_attackSMB.StateEnd)
        {
            enemy.ChangeState(new EnemyChaceState());
        }

        if (m_attackSMB.IsAttack)
        {
            enemy.Weapon.HitStart();
        }
        else
        {
            enemy.Weapon.HitEnd();
        }
    }

    public override void Exit(Enemy enemy)
    {
        enemy.Anime.Animator.ResetTrigger("Attack");
    }
}
