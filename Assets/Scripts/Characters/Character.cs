using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

/// <summary>
/// このクラスを継承することでStateBase<T>を使用することができます。
/// </summary>
public abstract class Character : MonoBehaviour
{
    public CharacterAnime Anime { get; set; }

    /// <summary>
    /// AnimatorについているSMBを取得します。
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public T GetBehaviour<T>() where T : StateMachineBehaviour
    {
        return Anime.Animator.GetBehaviour<T>();
    }
}
