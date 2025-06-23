using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class MoveSMB : StateMachineBehaviour
{
    private bool m_canStateChangeA;
    private bool m_canStateChangeB;
    private bool m_canStateChangeRoll;
    private bool m_canStateChangeHit;

    public bool StateChangeA { get; private set; }
    public bool StateChangeB { get; private set; }
    public bool StateChangeRoll { get; private set; }
    public bool StateChangeHit { get; private set; }

    /// <summary>
    /// �X�e�[�g���J�n���ꂽ�Ƃ��ɌĂяo����܂��B
    /// </summary>
    /// <param name="animator"></param>
    /// <param name="stateInfo"></param>
    /// <param name="layerIndex"></param>
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        m_canStateChangeA = false;
        m_canStateChangeB = false;
        m_canStateChangeRoll = false;
        m_canStateChangeHit = false;
        StateChangeA = false;
        StateChangeB = false;
        StateChangeRoll = false;
        StateChangeHit = false;
    }

    /// <summary>
    /// �X�e�[�g���L���ȊԁA���t���[���Ăяo����܂��B
    /// </summary>
    /// <param name="animator"></param>
    /// <param name="stateInfo"></param>
    /// <param name="layerIndex"></param>
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if(!animator.IsInTransition(layerIndex))
        {
            if(m_canStateChangeA)
            {
                StateChangeA = true;
            }

            if(m_canStateChangeB)
            {
                StateChangeB = true;
            }

            if(m_canStateChangeRoll)
            {
                StateChangeRoll = true;
            }

            if(m_canStateChangeHit)
            {
                StateChangeHit = true;
            }
        }
    }

    /// <summary>
    /// �U��A�����͂��ꂽ���ɌĂт�����܂��B
    /// </summary>
    public void AttackAInput()
    {
        m_canStateChangeA = true;
    }

    /// <summary>
    /// �U��B�����͂��ꂽ���ɌĂт�����܂��B
    /// </summary>
    public void AttackBInput()
    {
        m_canStateChangeB = true;
    }

    /// <summary>
    /// DiveRoll�����͂��ꂽ���ɌĂт�����܂��B
    /// </summary>
    public void DiveRollInput()
    {
        m_canStateChangeRoll = true;
    }

    /// <summary>
    /// Hit�����o���ꂽ�Ƃ��ɌĂяo����܂��B
    /// </summary>
    public void HitInput()
    {
        m_canStateChangeHit = true;
    }
}
