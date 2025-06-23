using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossRoaringState : StateBase<Boss>
{
    BossRoaringSMB m_roaringSMB;

    public override void Enter(Boss boss)
    {
        boss.Anime.Animator.applyRootMotion = true;
        boss.Anime.Animator.SetTrigger("Roaring");

        m_roaringSMB = boss.GetBehaviour<BossRoaringSMB>();
    }

    public override void Execute(Boss boss)
    {
        if(m_roaringSMB.StateEnd)
        {
            boss.ChangeState(new BossMoveState());
        }
    }

    public override void Exit(Boss boss)
    {
        boss.Anime.Animator.applyRootMotion = false;
        boss.Anime.Animator.ResetTrigger("Roaring");
    }
}
