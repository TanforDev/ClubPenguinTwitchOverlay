using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PenguinSittingState : PenguinBaseState
{
    public override void EnterState(Penguin penguin)
    {

    }

    public override void UpdateState(Penguin penguin)
    {
        if (penguin.IsAI)
        {
            PenguinState state = penguin.SitStateData.GetRandomState();

            if (state != PenguinState.None)
            {
                penguin.ChangePenguinStateByAi(state);
            }
        }
    }

    public override void ExitState(Penguin penguin)
    {

    }
}
