using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class StateBase<T> where T : Character
{
    /// <summary>
    /// ステートの開始時に実行します。
    /// </summary>
    public virtual void Enter(T character) { }

    /// <summary>
    /// ステートの毎フレーム更新時に実行します。
    /// </summary>
    public virtual void Execute(T character) { }

    /// <summary>
    /// ステートの毎フレーム更新時に実行します。
    /// </summary>
    /// <param name="input">デバイスから入力された値を使用できます。</param>
    public virtual void Execute(T character, InputInfo input) { }

    /// <summary>
    /// ステートの物理演算用更新時に実行します。
    /// </summary>
    public virtual void FixedExecute(T character) { }

    /// <summary>
    /// ステートの物理演算用更新時に実行します。
    /// </summary>
    /// <param name="input">デバイスから入力された値を使用できます。</param>
    public virtual void FixedExecute(T character, InputInfo input) { }

    /// <summary>
    /// ステートの毎フレーム更新終了時に実行します。
    /// </summary>
    public virtual void LateExecute(T character) { }

    /// <summary>
    /// ステートの毎フレーム更新終了時に実行します。
    /// </summary>
    /// <param name="input">デバイスから入力された値を使用できます。</param>
    public virtual void LateExecute(T character, InputInfo input) { }

    /// <summary>
    /// ステートの終了時に実行します。
    /// </summary>
    public virtual void Exit(T character) { }

    public virtual void Damaged(T character) { }
}
