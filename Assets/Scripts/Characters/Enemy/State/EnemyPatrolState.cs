using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPatrolState : StateBase<Enemy>
{
    public override void Enter(Enemy enemy)
    {
        // 初期化
        enemy.Anime.Animator.SetTrigger("Move");

        enemy.AI.Agent.speed = enemy.MoveSpeed;

        enemy.AI.Agent.updateRotation = true;

        enemy.AI.SetNewRamdomDestination(enemy);

        enemy.UI.HideHPBar();
    }

    public override void Execute(Enemy enemy)
    {
        // パトロール開始
        enemy.AI.Patrol(enemy);

        // もしダメージを食らったら
        if (enemy.IsHit)
        {
            enemy.ChangeState(new EnemyHitState());
        }

        if (enemy.GetEnemyType == EnemyType.Aggressive)
        {
            if(enemy.Camera.isChasing)
            {
                enemy.ChangeState(new EnemyChaceState());
            }
        }

        enemy.Camera.SearchPlayer();

        enemy.Anime.Animator.SetFloat("Speed", enemy.AI.Agent.velocity.magnitude);
    }

    public override void Exit(Enemy enemy)
    {
        enemy.AI.Initialize();
        enemy.Anime.Animator.ResetTrigger("Move");
    }
}
