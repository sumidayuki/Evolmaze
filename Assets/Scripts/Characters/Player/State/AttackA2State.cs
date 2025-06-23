using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackA2State : StateBase<Player>
{
    private AttackATwoSMB m_attackATwoSMB;

    private bool m_playedSE;
    private bool m_isHit;

    public override void Enter(Player player)
    {
        player.Anime.Animator.applyRootMotion = true;

        player.Anime.Animator.SetTrigger("AttackA");

        player.Weapon.Init();
        player.Weapon.SetAttackDamage(player.AttackDamage * 1.2f);

        m_attackATwoSMB = player.GetBehaviour<AttackATwoSMB>();

        m_playedSE = false;
        m_isHit = false;
    }

    public override void Execute(Player player, InputInfo input)
    {
        if (input.AttackA)
        {
            m_attackATwoSMB.AttackInput();
        }

        if(m_attackATwoSMB.StateChange)
        {
            player.ChangeState(new AttackA3State());
        }
        else if(m_attackATwoSMB.StateEnd)
        {
            player.ChangeState(new PlayerMoveState());
        }
        else if (m_attackATwoSMB.StateChangeHit)
        {
            player.ChangeState(new PlayerHitState());
        }

        if (m_attackATwoSMB.HitStart)
        {
            if (m_isHit) return;
            player.Weapon.HitStart();
            m_isHit = true;

            if (m_playedSE) return;
            player.Sound.PlaySE("SwordSwing");
            m_playedSE = true;
        }
        else if(m_attackATwoSMB.HitEnd)
        {
            player.Weapon.HitEnd(); 
        }
    }

    public override void Damaged(Player player)
    {
        m_attackATwoSMB.HitInput();
    }

    public override void LateExecute(Player player, InputInfo input)
    {
        player.Camera.NormalCamera(player, input);
    }

    public override void Exit(Player player)
    {
        player.Anime.Animator.applyRootMotion = false;
        player.Anime.Animator.ResetTrigger("AttackA");

        if (!m_attackATwoSMB.HitEnd)
        {
            player.Weapon.HitEnd();
        }
    }
}
