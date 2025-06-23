using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyChaceState : StateBase<Enemy>
{
    private float m_coolDown;
    private float m_coolDownTimer;

    public override void Enter(Enemy enemy)
    {
        enemy.Anime.Animator.SetTrigger("Move");
        enemy.AI.Agent.updateRotation = false;
        enemy.UI.ShowHPBar();

        m_coolDownTimer = 0;
        m_coolDown = 1.5f;
    }

    public override void Execute(Enemy enemy)
    {
        m_coolDownTimer += 1 * Time.deltaTime;

        Vector3 direction = (enemy.transform.position - enemy.Camera.GetPlayerPos().position).normalized;
        LookTarget(enemy, direction);

        enemy.AI.SetNewDestination(enemy.Camera.GetPlayerPos());

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

        float distance = Vector3.Distance(enemy.gameObject.transform.position, enemy.Camera.GetPlayerPos().position);

        if (distance > 30)
        {
            enemy.ChangeState(new EnemyPatrolState());
        }
        else if(distance >= 1.5 && distance <= 2.0)
        {
            enemy.AI.Agent.ResetPath();

            if(m_coolDownTimer >= m_coolDown)
            {
                enemy.ChangeState(new EnemyAttackState());
            }
        }
        else if(distance < 1.5)
        {
            Vector3 target = enemy.transform.position + direction * enemy.AI.Agent.angularSpeed * Time.deltaTime;
            enemy.AI.Agent.SetDestination(target);
        }

        enemy.Anime.Animator.SetFloat("Speed", enemy.AI.Agent.velocity.magnitude);
    }

    public override void Exit(Enemy enemy)
    {
        enemy.Anime.Animator.ResetTrigger("Move");
        enemy.AI.Agent.ResetPath();
    }

    private void LookTarget(Enemy enemy, Vector3 direction)
    {
        direction.y = 0f;
        if(direction != Vector3.zero)
        {
            Quaternion rotation = Quaternion.LookRotation(-direction);
            enemy.transform.rotation = Quaternion.Slerp(enemy.transform.rotation, rotation, Time.deltaTime * 10);
        }
    }
}
