using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ActionState
{
    POI,
    Chase,
    LKP,
    Attack,
    Investigate,
    Hide,
    Patrol,
    Reinforcement,
    Alert,
    Pause,
    LookAround,
}

public enum AlertState
{
    Relaxed,
    Concerned,
    Alerted,
    Engaged
}
