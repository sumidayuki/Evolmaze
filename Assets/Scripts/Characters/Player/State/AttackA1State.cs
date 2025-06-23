using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackA1State : StateBase<Player>
{
    private AttackAOneSMB m_attackAOneSMB;

    private bool m_playedSE;
    private bool m_isHit;

    public override void Enter(Player player)
    {
        player.Anime.Animator.applyRootMotion = true;

        player.Anime.Animator.SetTrigger("AttackA");

        player.Weapon.Init();
        player.Weapon.SetAttackDamage(player.AttackDamage);

        m_playedSE = false;
        m_isHit = false;

        m_attackAOneSMB = player.GetBehaviour<AttackAOneSMB>();
    }

    public override void Execute(Player player, InputInfo input)
    {
        if(input.AttackA)
        {
            m_attackAOneSMB.AttackInput();
        }

        if(m_attackAOneSMB.StateChange)
        {
            player.ChangeState(new AttackA2State());
        }
        else if(m_attackAOneSMB.StateEnd)
        {
            player.ChangeState(new PlayerMoveState());
        }
        else if(m_attackAOneSMB.StateChangeHit)
        {
            player.ChangeState(new PlayerHitState());
        }

        if (m_attackAOneSMB.HitStart)
        {
            if (m_isHit) return;
            player.Weapon.HitStart();
            m_isHit = true;

            if (m_playedSE) return;
            player.Sound.PlaySE("SwordSwing");
            m_playedSE = true;
        }
        else if (m_attackAOneSMB.HitEnd)
        {
            player.Weapon.HitEnd();
        }
    }

    public override void Damaged(Player player)
    {
        m_attackAOneSMB.HitInput();
    }

    public override void LateExecute(Player player, InputInfo input)
    {
        player.Camera.NormalCamera(player, input);
    }

    public override void Exit(Player player)
    {
        Debug.Log("Exit AttackA1State");
        player.Anime.Animator.applyRootMotion = false;
        player.Anime.Animator.ResetTrigger("AttackA");

        if (!m_attackAOneSMB.HitEnd)
        {
            player.Weapon.HitEnd();
        }
    }
}
