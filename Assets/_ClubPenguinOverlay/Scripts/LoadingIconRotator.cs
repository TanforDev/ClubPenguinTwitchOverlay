using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadingIconRotator : MonoBehaviour
{
    private void Start()
    {
        transform.DORotate(new Vector3(0, 0, -180), 0.9f, RotateMode.FastBeyond360).SetEase(Ease.OutSine).OnComplete(() =>
        {
            transform.DORotate(new Vector3(0, 0, -360), 0.9f, RotateMode.FastBeyond360).SetEase(Ease.OutSine).OnComplete(() =>
            {
                transform.localEulerAngles = Vector3.zero;
            });
        }).SetLoops(-1);
    }
}
