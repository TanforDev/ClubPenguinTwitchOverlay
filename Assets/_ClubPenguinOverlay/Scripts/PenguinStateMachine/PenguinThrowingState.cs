using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PenguinThrowingState : PenguinBaseState
{

    private Tween waitTween;
    public override void EnterState(Penguin penguin)
    {
        //Super ugly but it works...
        float currentTime = 0;
        if (waitTween != null)
        {
            waitTween.Kill();
        }
        waitTween = DOTween.To(() => currentTime, x => currentTime = x, 1, 1).SetEase(Ease.Linear)
        .OnComplete(() =>
        {
            penguin.ChangePenguinState(PenguinState.Idle);
        });
    }

    public override void UpdateState(Penguin penguin)
    {

    }

    public override void ExitState(Penguin penguin)
    {

    }
}
