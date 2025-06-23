using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackBState : StateBase<Player>
{
    AttackBSMB m_attackBSMB;

    private bool m_playedSE;
    private bool m_isHit;

    public override void Enter(Player player)
    {
        player.Anime.Animator.applyRootMotion = true;

        player.Anime.Animator.SetTrigger("AttackB");

        player.Weapon.Init();
        player.Weapon.SetAttackDamage(player.AttackDamage * 1.3f);

        m_attackBSMB = player.GetBehaviour<AttackBSMB>();

        m_playedSE = false;
        m_isHit = false;
    }

    public override void Execute(Player player, InputInfo input)
    {   
        if (m_attackBSMB.StateEnd)
        {
            player.ChangeState(new PlayerMoveState());
        }
        else if (m_attackBSMB.StateChangeHit)
        {
            player.ChangeState(new PlayerHitState());
        }

        if (m_attackBSMB.HitStart)
        {
            if (m_isHit) return;
            player.Weapon.HitStart();
            m_isHit = true;

            if (m_playedSE) return;
            player.Sound.PlaySE("SwordSwing");
            m_playedSE = true;
        }
        else if(m_attackBSMB.HitEnd)
        {
            player.Weapon.HitEnd();
        }
    }

    public override void Damaged(Player player)
    {
        m_attackBSMB.HitInput();
    }

    public override void LateExecute(Player player, InputInfo input)
    {
        player.Camera.NormalCamera(player, input);
    }

    public override void Exit(Player player)
    {
        player.Anime.Animator.applyRootMotion = false;

        if(!m_attackBSMB.HitEnd)
        {
            player.Weapon.HitEnd();
        }
    }
}
