using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �{�X�̃W�����v�A�^�b�N���Ǘ����܂��B
/// </summary>
public class BossAttackJampState : StateBase<Boss>
{
    BossJampAttackSMB m_jampAttackSMB;

    public override void Enter(Boss boss)
    {
        // ������
        boss.Anime.Animator.SetTrigger("AttackC");

        boss.Attack.Init();

        boss.Attack.SetAttackDamage(boss.BossData.attackDamage * 2);

        m_jampAttackSMB = boss.GetBehaviour<BossJampAttackSMB>();
    }

    public override void Execute(Boss boss)
    {
        // �^�[�Q�b�g�̕����������܂��B
        boss.AI.LookTarget(boss);

        // ���̃A�j���[�V�������I�����Ă�����
        if (m_jampAttackSMB.StateEnd)
        {
            boss.ChangeState(new BossMoveState());
        }

        if(m_jampAttackSMB.HitStart)
        {
            boss.Attack.AttackJampHitStart();
        }
        else if(m_jampAttackSMB.HitEnd)
        {
            boss.Attack.AttackJampHitEnd();
        }

        // �W�����v���X�^�[�g������
        if(m_jampAttackSMB.JampStart)
        {
            boss.AI.ToTarget();
        }
        // �W�����v���I��������
        else if (m_jampAttackSMB.JampEnd)
        {
            boss.AI.Agent.ResetPath();
        }
    }

    public override void Exit(Boss boss)
    {
        boss.Anime.Animator.ResetTrigger("AttackC");
    }
}
