using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitSMB : StateMachineBehaviour
{
    [SerializeField] private float END_START_TIME;

    public bool StateEnd { get; private set; }

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        StateEnd = false;
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (!animator.IsInTransition(layerIndex))
        {
            var normalizedTime = stateInfo.normalizedTime;

            if (normalizedTime >= END_START_TIME)
            {
                StateEnd = true;
            }
        }
    }
}
