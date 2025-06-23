using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// キャラクターのアニメーターを管理します。
/// </summary>
public class CharacterAnime : MonoBehaviour
{
    public Animator Animator { get; private set; }

    private void Awake()
    {
        Animator = GetComponent<Animator>();
    }
}
