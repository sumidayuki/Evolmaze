using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputInfo
{
    public Vector3 Move { get; set; }

    public bool Dash { get; set; }

    public bool AutoDash { get; set; }

    public Vector2 Look { get; set; }

    public bool AttackA { get; set; }

    public bool AttackB { get; set; }

    public bool DiveRoll { get; set; }

    public bool Pause { get; set; }

    public bool LockOn { get; set; }
}
