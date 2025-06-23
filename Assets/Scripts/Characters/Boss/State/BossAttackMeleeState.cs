using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �{�X�̋ߐڍU�����Ǘ����܂��B
/// </summary>
public class BossAttackMeleeState : StateBase<Boss>
{
    private BossPunchOneSMB m_bossPunchOneSMB;  // �ߐڍU���̈��ځi�E�p���`�j�̃A�j���[�V����
    private BossPunchTwoSMB m_bossPunchTwoSMB;  // �ߐڍU���̓��ځi���p���`�j�̃A�j���[�V����

    // �{�X�̌��݂̏�Ԃ��Ǘ����܂��B
    private enum STATE
    {
        ONE,    // ���ڂ̍U��
        TWO,    // ���ڂ̍U��
    }

    private STATE m_state;

    public override void Enter(Boss boss)
    {
        // SMB���擾���܂��B
        m_bossPunchOneSMB = boss.GetBehaviour<BossPunchOneSMB>();
        m_bossPunchTwoSMB = boss.GetBehaviour<BossPunchTwoSMB>();

        // ������
        boss.Attack.Init();

        // ���̍U���̃_���[�W��ݒ肵�܂��B
        boss.Attack.SetAttackDamage(boss.BossData.attackDamage);

        // ���̍U���̃A�j���[�V�������Đ����܂��B
        boss.Anime.Animator.SetTrigger("AttackA");
    }

    public override void Execute(Boss boss)
    {
        // �{�X�̏�Ԃ� ONE �Ȃ�
        if(m_state == STATE.ONE)
        {
            // ���ڂ̍U�����I��������
            if (m_bossPunchOneSMB.StateEnd)
            {
                // �{�X�̍U�������������܂��B�i�U���p�̃R���C�_�[�I�t����ɍU�����Ă��锻��ɂȂ��Ă���̂ŉ����j
                boss.Attack.Init();

                // ���ڂ̍U���̃A�j���[�V�������Đ����܂��B
                boss.Anime.Animator.SetTrigger("AttackA");

                // �{�X�̏�Ԃ���ڂ̍U���ɕύX���܂��B  
                m_state = STATE.TWO;
            }

            // ���ڂ̍U���̃q�b�g�X�^�[�g������������
            if (m_bossPunchOneSMB.HitStart)
            {
                // �{�X�̉E�U���̓����蔻���L���ɂ��܂��B
                boss.Attack.PunchHitStart(true);
            }
            // ���ڂ̍U���̃q�b�g���I��������
            else if (m_bossPunchOneSMB.HitEnd)
            {
                // �v���C���[�̕����������܂��B
                boss.AI.LookTarget(boss);

                // �{�X�̉E�U���̓����蔻��𖳌��ɂ��܂��B
                boss.Attack.PunchHitEnd(true);
            }
        }
        // �{�X�̏�Ԃ� TWO �Ȃ�
        else if (m_state == STATE.TWO)
        {
            // ���ڂ̍U�����I��������
            if (m_bossPunchTwoSMB.StateEnd)
            {
                // �v���C���[�Ƃ̋����� 3 �ȉ��Ȃ�
                if(boss.AI.GetTargetDistance(boss.gameObject.transform.position) <= 3)
                {
                    // ����X�e�[�g�ɕύX���܂��B
                    boss.ChangeState(new BossRetreatState());
                }
                // �v���C���[�Ƃ̋���������Ă���Ȃ�
                else
                {
                    // �ړ��X�e�[�g�ɕύX���܂��B
                    boss.ChangeState(new BossMoveState());
                }
            }

            // ���ڂ̍U���̃q�b�g�X�^�[�g������������
            if (m_bossPunchTwoSMB.HitStart)
            {
                // �{�X�̍��U���̓����蔻���L���ɂ��܂��B
                boss.Attack.PunchHitStart(false);
            }
            // ���ڂ̍U���̃q�b�g���I��������
            else if (m_bossPunchTwoSMB.HitEnd)
            {
                // �v���C���[�̕����������܂��B
                boss.Attack.PunchHitEnd(false);
            }
        }
    }

    public override void Exit(Boss boss)
    {
        // �A�j���[�V���������Z�b�g���܂��B
        boss.Anime.Animator.ResetTrigger("AttackA");
    }
}
