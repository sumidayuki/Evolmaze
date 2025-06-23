using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class StateBase<T> where T : Character
{
    /// <summary>
    /// �X�e�[�g�̊J�n���Ɏ��s���܂��B
    /// </summary>
    public virtual void Enter(T character) { }

    /// <summary>
    /// �X�e�[�g�̖��t���[���X�V���Ɏ��s���܂��B
    /// </summary>
    public virtual void Execute(T character) { }

    /// <summary>
    /// �X�e�[�g�̖��t���[���X�V���Ɏ��s���܂��B
    /// </summary>
    /// <param name="input">�f�o�C�X������͂��ꂽ�l���g�p�ł��܂��B</param>
    public virtual void Execute(T character, InputInfo input) { }

    /// <summary>
    /// �X�e�[�g�̕������Z�p�X�V���Ɏ��s���܂��B
    /// </summary>
    public virtual void FixedExecute(T character) { }

    /// <summary>
    /// �X�e�[�g�̕������Z�p�X�V���Ɏ��s���܂��B
    /// </summary>
    /// <param name="input">�f�o�C�X������͂��ꂽ�l���g�p�ł��܂��B</param>
    public virtual void FixedExecute(T character, InputInfo input) { }

    /// <summary>
    /// �X�e�[�g�̖��t���[���X�V�I�����Ɏ��s���܂��B
    /// </summary>
    public virtual void LateExecute(T character) { }

    /// <summary>
    /// �X�e�[�g�̖��t���[���X�V�I�����Ɏ��s���܂��B
    /// </summary>
    /// <param name="input">�f�o�C�X������͂��ꂽ�l���g�p�ł��܂��B</param>
    public virtual void LateExecute(T character, InputInfo input) { }

    /// <summary>
    /// �X�e�[�g�̏I�����Ɏ��s���܂��B
    /// </summary>
    public virtual void Exit(T character) { }

    public virtual void Damaged(T character) { }
}
