using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

/// <summary>
/// ���̃N���X���p�����邱�Ƃ�StateBase<T>���g�p���邱�Ƃ��ł��܂��B
/// </summary>
public abstract class Character : MonoBehaviour
{
    public CharacterAnime Anime { get; set; }

    /// <summary>
    /// Animator�ɂ��Ă���SMB���擾���܂��B
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public T GetBehaviour<T>() where T : StateMachineBehaviour
    {
        return Anime.Animator.GetBehaviour<T>();
    }
}
