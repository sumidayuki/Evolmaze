using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossRetreatState : StateBase<Boss>
{
    private BossRetreatSMB m_bossRetreatSMB;

    public override void Enter(Boss boss)
    {
        m_bossRetreatSMB = boss.GetBehaviour<BossRetreatSMB>();

        boss.Anime.Animator.SetTrigger("Retreat");

        boss.AI.Retreat(boss);
    }

    public override void Execute(Boss boss)
    {
        boss.AI.LookTarget(boss);

        if (m_bossRetreatSMB.StateEnd)
        {
            boss.ChangeState(new BossMoveState());
        }

        if(m_bossRetreatSMB.JampEnd)
        {
            boss.AI.Agent.ResetPath();
        }
    }

    public override void Exit(Boss boss)
    {
        boss.Anime.Animator.ResetTrigger("Retreat");
    }
}
