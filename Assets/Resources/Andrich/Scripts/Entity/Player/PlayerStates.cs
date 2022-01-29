using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PlayerStates
{
    notSet, // 0
    idleForward, // 1
    idleBackwards, // 2
    runningForward, // 3
    runningBackwards, // 4
    duckingForward, // 5
    duckingBackwards, // 6
    onGround, // 7
    inAir, // 8
    jumping, // 9
    wallJumping, // 10
    falling, // 11
    climbing, // 12
    wallSliding, // 13
    dashing, // 14
    pushing, // 15
    climbingIdle, // 16
    climbingUp, // 17
    climbingDown, // 18
    tired, // 19
    dead, // 20
}
