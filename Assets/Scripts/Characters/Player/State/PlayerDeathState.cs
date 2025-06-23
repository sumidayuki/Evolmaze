using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerDeathState : StateBase<Player>
{
    public override void Enter(Player player)
    {
        player.Anime.Animator.SetTrigger("Death");
        GameManager.Instance.GameOver();
    }

    public override void Execute(Player player)
    {

    }

    public override void Exit(Player player)
    {
    }
}
