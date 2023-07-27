using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AppFramerate : MonoBehaviour
{
    private void Awake()
    {
        int screenHeight = ConfigManager.Instance.Config.ScreenHeight;
        Screen.SetResolution(screenHeight, GetScreenWidth(screenHeight), false);
        Application.targetFrameRate = ConfigManager.Instance.Config.TargetFrameRate;
    }

    private int GetScreenWidth(int screenHeight)
    {
        return Mathf.RoundToInt((screenHeight * 9f) / 16f);
    }
}
