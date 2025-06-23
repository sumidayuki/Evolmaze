using UnityEngine;

/// <summary>
/// �X�e�[�g�̏�Ԃ��Ǘ�����N���X�ł��B
/// </summary>
/// <typeparam name="T"></typeparam>
public class StateManager<T> where T : Character 
{
    /// <summary>
    /// ���݂̏��
    /// </summary>
    public StateBase<T> CurrentState { get; private set; }

    /// <summary>
    /// �X�e�[�g��ύX���܂��B
    /// </summary>
    /// <param name="newState">�ύX��̃X�e�[�g</param>
    /// <param name="character">�w���̃L�����N�^�[</param>
    public void ChangeState(StateBase<T> newState, T character)
    {
        // ���݂̃X�e�[�g�ƕύX�������X�e�[�g��������������
        if (CurrentState == newState)
        {
            Debug.Log("�����X�e�[�g�ł�");
            return;
        }

        // ���݂̃X�e�[�g���I�����܂��B
        CurrentState.Exit(character);

        // ���݂̃X�e�[�g�ɐV�����X�e�[�g�������܂��B
        CurrentState = newState;

        // �V�����X�e�[�g�����s���܂��B
        newState.Enter(character);
    }

    /// <summary>
    /// �X�e�[�g�̏����������܂��B
    /// </summary>
    /// <param name="newState">��������̃X�e�[�g</param>
    /// <param name="character">�w���̃L�����N�^�[</param>
    public void Init(StateBase<T> newState, T character)
    {
        // ���݂̃X�e�[�g�ɐV�����X�e�[�g�������܂��B
        CurrentState = newState;

        // ���݂̃X�e�[�g�����s���܂��B
        CurrentState.Enter(character);

        // �A�j���[�^�[�̏��������s���܂��B
        character.Anime.Animator.ResetTrigger("Move");
    }
}
