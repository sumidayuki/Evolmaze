using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMoveState : StateBase<Player>
{
    private MoveSMB m_moveSMB;

    public override void Enter(Player player)
    {
        // ������
        player.Anime.Animator.SetTrigger("Move");

        m_moveSMB = player.GetBehaviour<MoveSMB>();
    }

    public override void Execute(Player player ,InputInfo input)
    {
        // ����AttackA�̓��͂���������
        if (input.AttackA)
        {
            m_moveSMB.AttackAInput();
        }
        // ����AttackB�̓��͂���������
        else if (input.AttackB)
        {
            m_moveSMB.AttackBInput();
        }
        // ����DiveRoll�̓��͂���������
        else if (input.DiveRoll)
        {
            m_moveSMB.DiveRollInput();
        }

        if (m_moveSMB.StateChangeA)
        {
            player.ChangeState(new AttackA1State());
        }
        else if (m_moveSMB.StateChangeB)
        {
            player.ChangeState(new AttackBState());
        }
        else if(m_moveSMB.StateChangeRoll)
        {
            player.ChangeState(new PlayerDiveRollState());
        }
        else if(m_moveSMB.StateChangeHit)
        {
            player.ChangeState(new PlayerHitState());
        }
    }

    public override void Damaged(Player player)
    {
        m_moveSMB.HitInput();
    }

    public override void LateExecute(Player player, InputInfo input)
    {
        player.Camera.NormalCamera(player, input);
    }

    public override void FixedExecute(Player player, InputInfo input)
    {
        if (m_moveSMB.StateChangeA || m_moveSMB.StateChangeB || m_moveSMB.StateChangeRoll || m_moveSMB.StateChangeHit) return;
        player.Move.Movement(player, input);

        player.Anime.Animator.SetFloat("Speed", input.Move.magnitude * player.Move.CurrentSpeed);
    }

    public override void Exit(Player player)
    {
        player.Anime.Animator.ResetTrigger("Move");
    }
}
