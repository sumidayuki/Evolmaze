using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMoveState : StateBase<Player>
{
    private MoveSMB m_moveSMB;

    public override void Enter(Player player)
    {
        // ‰Šú‰»
        player.Anime.Animator.SetTrigger("Move");

        m_moveSMB = player.GetBehaviour<MoveSMB>();
    }

    public override void Execute(Player player ,InputInfo input)
    {
        // ‚à‚µAttackA‚Ì“ü—Í‚ª‚ ‚Á‚½‚ç
        if (input.AttackA)
        {
            m_moveSMB.AttackAInput();
        }
        // ‚à‚µAttackB‚Ì“ü—Í‚ª‚ ‚Á‚½‚ç
        else if (input.AttackB)
        {
            m_moveSMB.AttackBInput();
        }
        // ‚à‚µDiveRoll‚Ì“ü—Í‚ª‚ ‚Á‚½‚ç
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
