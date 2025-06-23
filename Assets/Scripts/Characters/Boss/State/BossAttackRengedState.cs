using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAttackRengedState : StateBase<Boss>
{
    BossSwipingOneSMB m_bossSwipingOneSMB;
    BossSwipingTwoSMB m_bossSwipingTwoSMB;

    private enum STATE
    {
        ONE,
        TWO,
    }

    private STATE m_state;

    private bool m_isSlash;

    public override void Enter(Boss boss)
    {
        m_bossSwipingOneSMB = boss.GetBehaviour<BossSwipingOneSMB>();
        m_bossSwipingTwoSMB = boss.GetBehaviour<BossSwipingTwoSMB>();

        m_state = STATE.ONE;

        m_isSlash = false;

        boss.Attack.Init();

        boss.Anime.Animator.SetTrigger("AttackB");
    }

    public override void Execute(Boss boss)
    {
        if(m_state == STATE.ONE)
        {
            if (m_bossSwipingOneSMB.StateEnd)
            {
                boss.Attack.Init();
                boss.Anime.Animator.SetTrigger("AttackB");
                m_state = STATE.TWO;
            }

            if(m_bossSwipingOneSMB.HitStart)
            {
                if (!m_isSlash)
                {
                    m_isSlash = true;
                    boss.Attack.SlashAttack(true, boss.transform, boss.BossData.attackDamage);
                }
            }
            else if(m_bossSwipingOneSMB.HitEnd)
            {
                m_isSlash = false;
                boss.AI.LookTarget(boss);
            }
        }
        else if(m_state == STATE.TWO)
        {
            if(m_bossSwipingTwoSMB.StateEnd)
            {
                boss.ChangeState(new BossMoveState());
            }

            if(m_bossSwipingTwoSMB.HitStart)
            {
                if (!m_isSlash)
                {
                    m_isSlash = true;
                    boss.Attack.SlashAttack(false, boss.transform, boss.BossData.attackDamage);
                }
            }
        }
    }

    public override void Exit(Boss boss)
    {
        boss.Anime.Animator.ResetTrigger("AttackB");
    }
}
