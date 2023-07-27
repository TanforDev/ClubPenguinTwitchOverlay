using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PenguinFlip : MonoBehaviour
{
    private bool flipX;
    private RectTransform rectTransform;

    [SerializeField] private Penguin penguin;

    private void OnEnable()
    {
        rectTransform = GetComponent<RectTransform>();
        if(flipX == false)
        {
            UnsetFlipX();
        }

        penguin.EventChangeState += ResetFlip;
    }

    private void OnDisable()
    {
        penguin.EventChangeState += ResetFlip;
    }

    private void ResetFlip(PenguinState dummy)
    {
        UnsetFlipX();
    }

    public void SetFlipX()
    {
        Vector3 initialScale = rectTransform.localScale;
        int dir = -1;
        rectTransform.localScale = new Vector3(dir, initialScale.y, initialScale.z);

        flipX = true;
    }

    public void UnsetFlipX()
    {
        Vector3 initialScale = rectTransform.localScale;
        int dir = 1;
        rectTransform.localScale = new Vector3(dir, initialScale.y, initialScale.z);

        flipX = false;
    }
}
