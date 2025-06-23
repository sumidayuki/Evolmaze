using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackA3State : StateBase<Player>
{
    AttackAThreeSMB m_attackAThreeSMB;

    private bool m_playedSE;
    private bool m_isHit;

    public override void Enter(Player player)
    {
        player.Anime.Animator.applyRootMotion = true;

        player.Anime.Animator.SetTrigger("AttackA");

        player.Weapon.Init();
        player.Weapon.SetAttackDamage(player.AttackDamage * 1.5f);

        m_attackAThreeSMB = player.GetBehaviour<AttackAThreeSMB>();

        m_playedSE = false;
        m_isHit = false;
    }

    public override void Execute(Player player, InputInfo input)
    {
        if (m_attackAThreeSMB.StateEnd)
        {
            player.ChangeState(new PlayerMoveState());
        }
        else if(m_attackAThreeSMB.StateChangeHit)
        {
            player.ChangeState(new PlayerHitState());
        }

        if(m_attackAThreeSMB.HitStart)
        {
            if (m_isHit) return;
            player.Weapon.HitStart();
            m_isHit = true;

            if (m_playedSE) return;
            player.Sound.PlaySE("SwordSwing");
            m_playedSE = true;
        }
        else if(m_attackAThreeSMB.HitEnd)
        {
            player.Weapon.HitEnd();
        }
    }

    public override void Damaged(Player player)
    {
        m_attackAThreeSMB.HitInput();
    }

    public override void LateExecute(Player player, InputInfo input)
    {
        player.Camera.NormalCamera(player, input);
    }

    public override void Exit(Player player)
    {
        player.Anime.Animator.applyRootMotion = false;
        player.Anime.Animator.ResetTrigger("AttackA");

        if (!m_attackAThreeSMB.HitEnd)
        {
            player.Weapon.HitEnd();
        }
    }
}
