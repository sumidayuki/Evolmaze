using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDiveRollSMB : StateMachineBehaviour
{
    // ���݂̏�Ԃ��I������^�C�~���O
    [SerializeField] private float END_START_TIME;

    /// <summary>
    /// �X�e�[�g���I�����Ă��邩���Ǘ����܂��B
    /// </summary>
    public bool StateEnd { get; private set; }

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // ������
        StateEnd = false;
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (!animator.IsInTransition(layerIndex))
        {
            var normalizedTime = stateInfo.normalizedTime;

            // ���̃X�e�[�g�̌o�ߎ��Ԃ��X�e�[�g�̏I�����ԂȂ�
            if (normalizedTime >= END_START_TIME)
            {
                // �X�e�[�g�̏I����`����
                StateEnd = true;
            }
        }
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.ResetTrigger("DiveRoll");
    }
}
