using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDiveRollState : StateBase<Player>
{
    private PlayerDiveRollSMB m_playerDiveRollSMB;

    public override void Enter(Player player)
    {
        player.MyHitBox.Coll.enabled = false;

        m_playerDiveRollSMB = player.GetBehaviour<PlayerDiveRollSMB>();

        player.Anime.Animator.applyRootMotion = true;
        player.Anime.Animator.SetTrigger("DiveRoll");
    }

    public override void Execute(Player player, InputInfo input)
    {
        if (m_playerDiveRollSMB.StateEnd)
        {
            player.ChangeState(new PlayerMoveState());
        }
    }

    public override void LateExecute(Player player, InputInfo input)
    {
        player.Camera.NormalCamera(player, input);
    }

    public override void Exit(Player player)
    {
        player.Anime.Animator.applyRootMotion = false;
        player.MyHitBox.Coll.enabled = true;
        player.Anime.Animator.ResetTrigger("DiveRoll");
    }
}
