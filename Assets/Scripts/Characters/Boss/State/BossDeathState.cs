using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossDeathState : StateBase<Boss>
{
    public override void Enter(Boss boss)
    {
        boss.Anime.Animator.SetTrigger("Death");
    }

    public override void Execute(Boss boss)
    {
        
    }

    public override void Exit(Boss boss)
    {
        
    }
}
