using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtTest : MonoBehaviour
{
    [SerializeField] int horizontalDir = 0;
    [SerializeField] int verticalDir = 0;

    [SerializeField] private RectTransform lookAtTarget;
    private RectTransform rectTransform;

    public bool thrshld = true;

    private void OnEnable()
    {
        rectTransform = GetComponent<RectTransform>();
    }
    private void LookAtDir(Vector2 dir, bool allDirs)
    {
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;

        float snappedAngle;
        int roundedX, roundedY;

        if (allDirs)
        {
            // Snap to 8 directions
            snappedAngle = Mathf.Round(angle / 45f) * 45f;
            roundedX = Mathf.RoundToInt(Mathf.Cos(snappedAngle * Mathf.Deg2Rad));
            roundedY = Mathf.RoundToInt(Mathf.Sin(snappedAngle * Mathf.Deg2Rad));
        }
        else
        {
            // Snap to 4 diagonal directions
            float[] snapAngles = { 45f, 135f, -135f, -45f };
            float minAngleDifference = Mathf.Abs(angle - snapAngles[0]);
            snappedAngle = snapAngles[0];

            foreach (float snapAngle in snapAngles)
            {
                float angleDifference = Mathf.Abs(angle - snapAngle);
                if (angleDifference < minAngleDifference)
                {
                    snappedAngle = snapAngle;
                    minAngleDifference = angleDifference;
                }
            }

            roundedX = Mathf.RoundToInt(Mathf.Cos(snappedAngle * Mathf.Deg2Rad));
            roundedY = Mathf.RoundToInt(Mathf.Sin(snappedAngle * Mathf.Deg2Rad));
        }

        horizontalDir = roundedX;
        verticalDir = roundedY;
    }


    private void LookAtPoint(Vector2 point)
    {
        Vector2 dir = (point - rectTransform.anchoredPosition).normalized;

        LookAtDir(dir, thrshld);
    }




    private void Update()
    {
        LookAtPoint(lookAtTarget.anchoredPosition);

        Debug.DrawRay(rectTransform.position, new Vector2(horizontalDir, verticalDir) * 100);

    }
}
