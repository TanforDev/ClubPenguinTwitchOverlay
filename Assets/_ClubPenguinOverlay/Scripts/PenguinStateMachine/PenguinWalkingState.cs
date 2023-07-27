using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PenguinWalkingState : PenguinBaseState
{
    public override void EnterState(Penguin penguin)
    {

    }

    public override void UpdateState(Penguin penguin)
    {
        if (penguin.IsAI)
        {
            penguin.MoveToRandomPos();
        }
    }

    public override void ExitState(Penguin penguin)
    {

    }
}
