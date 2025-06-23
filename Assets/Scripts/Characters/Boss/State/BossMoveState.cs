using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossMoveState : StateBase<Boss>
{
    /// <summary>
    /// ボスの行動をステートで管理します。
    /// </summary>
    private enum Move
    {
        Melee,
        Meddle,
        Renged,
        Move,
        Roaring,
        Retreat,
    }

    private Move m_move;

    float coolDown = 0f;
    float coolDownTimer;

    float moveCoolDown = 2f;
    float moveCoolDownTimer;

    public override void Enter(Boss boss)
    {
        if(boss.STATE == Boss.MYSTATE.NORMAL)
        {
            coolDown = 5f;
            boss.AI.Agent.speed = boss.BossData.moveSpeed;
        }
        else if(boss.STATE == Boss.MYSTATE.ROARING)
        {
            coolDown = 2f;
            boss.AI.Agent.speed = boss.BossData.moveSpeed * 1.5f;
        }
        m_move = Move.Move;

        coolDownTimer = 0;
        moveCoolDownTimer = 2;

        boss.AI.Agent.updateRotation = false;

        boss.Anime.Animator.SetTrigger("Move");
    }

    public override void Execute(Boss boss)
    {
        coolDownTimer += 1 * Time.deltaTime;

        // ボスとプレイヤーの距離を取得します。
        float distance = boss.AI.GetTargetDistance(boss.gameObject.transform.position);

        boss.AI.LookTarget(boss);

        if (boss.STATE == Boss.MYSTATE.NORMAL)
        {
            if (distance <= 2.5f && coolDownTimer >= coolDown)
            {
                // ステートを近接攻撃にします。
                m_move = Move.Melee;
            }
            else if(distance >= 5 && distance <= 8 && coolDownTimer >= coolDown)
            {
                m_move = Move.Meddle;
            }
            else if (distance > 10 && distance <= 15 && coolDownTimer >= coolDown)
            {
                // ステートを遠距離攻撃にします。
                m_move = Move.Renged;
            }
            else if (boss.CurrentHealth <= boss.MaxHealth * 0.5f && coolDownTimer >= coolDown)
            {
                // ステートをロアリングにします。
                m_move = Move.Roaring;
            }
            else
            {
                // ステートを移動にします。
                m_move = Move.Move;
            }

            // ステートによって分岐します。
            if (m_move == Move.Melee)
            {
                boss.ChangeState(new BossAttackMeleeState());
            }
            else if(m_move == Move.Meddle)
            {
                boss.ChangeState(new BossAttackJampState());
            }
            else if (m_move == Move.Renged)
            {
                boss.ChangeState(new BossAttackRengedState());
            }
            else if (m_move == Move.Move)
            {
                moveCoolDownTimer += 1 * Time.deltaTime;
                if (moveCoolDownTimer >= moveCoolDown)
                {
                    boss.AI.ToTarget();
                }
                boss.Anime.Animator.SetFloat("Speed", 0.5f);
            }
            else if (m_move == Move.Roaring)
            {
                boss.ChangeMyState(Boss.MYSTATE.ROARING);
                boss.ChangeState(new BossRoaringState());
            }
        }
        else if(boss.STATE == Boss.MYSTATE.ROARING)
        {
            // プレイヤーとの距離に応じて行動を分岐します。
            if (distance < 2f)
            {
                m_move = Move.Retreat;
            }

            else if (distance >= 2f && distance <= 2.5f && coolDownTimer >= coolDown)
            {
                // ステートを近接攻撃にします。
                m_move = Move.Melee;
            }
            else
            {
                // ステートを移動にします。
                m_move = Move.Move;
            }

            // ステートによって分岐します。
            if (m_move == Move.Melee)
            {
                boss.ChangeState(new BossAttackMeleeState());
            }
            else if (m_move == Move.Retreat)
            {
                boss.AI.Retreat(boss);
            }
            else if (m_move == Move.Move)
            {
                moveCoolDownTimer += 1 * Time.deltaTime;
                if (moveCoolDownTimer >= moveCoolDown)
                {
                    boss.AI.ToTarget();
                }
                boss.Anime.Animator.SetFloat("Speed", 1f);
            }
        }
    }

    public override void Exit(Boss boss)
    {
        boss.Anime.Animator.ResetTrigger("Move");
        boss.AI.Agent.ResetPath();
    }
}
