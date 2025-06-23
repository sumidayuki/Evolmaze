using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHitState : StateBase<Player>
{
    private HitSMB m_hitSMB;

    public override void Enter(Player player)
    {
        player.Anime.Animator.SetTrigger("Hit");
        m_hitSMB = player.GetBehaviour<HitSMB>();
    }

    public override void Execute(Player player, InputInfo input)
    {
        if(m_hitSMB.StateEnd)
        {
            player.ChangeState(new PlayerMoveState());
        }
    }

    public override void Exit(Player player)
    {
        player.Anime.Animator.ResetTrigger("Hit");
    }
}
