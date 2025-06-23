using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDeathState : StateBase<Enemy>
{
    float deathTimer;

    public override void Enter(Enemy enemy)
    {
        enemy.Anime.Animator.SetTrigger("Death");
        DungeonManager.Instance.AddEnemyDefeatedCount();
        deathTimer = 5f;
    }

    public override void Execute(Enemy enemy)
    {
        deathTimer -= 1 * Time.deltaTime;
        if(0 > deathTimer)
        {
            Exit(enemy);
        }
    }

    public override void Exit(Enemy enemy)
    {
        enemy.gameObject.SetActive(false);
    }
}
