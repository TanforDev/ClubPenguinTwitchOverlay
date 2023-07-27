using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnowBallTest : MonoBehaviour
{
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Snowball snow = SnowballManager.Instance.GetSnowball();
            snow.RectTransform.anchoredPosition = Vector2.zero;

            Vector2 target = new Vector2(Random.Range(0, 1920.0f) - 1920 * 0.5f, Random.Range(0, 1080.0f) - 1080 * 0.5f);
            snow.Shoot(target);
        }
    }
}
