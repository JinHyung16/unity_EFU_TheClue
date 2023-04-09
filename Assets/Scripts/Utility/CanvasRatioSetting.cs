using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CanvasRatioSetting : MonoBehaviour
{
    private CanvasScaler cavasScaler;
    private float fixedAspectRatio = 16.0f / 9.0f;
    private float curAspectRatio = 0.0f;
    private void Start()
    {
        cavasScaler = GetComponent<CanvasScaler>();
        curAspectRatio = (float)Screen.width / (float)Screen.height;
        if (fixedAspectRatio < curAspectRatio)
        {
            this.cavasScaler.matchWidthOrHeight = 1.0f;
        }
        else
        {
            this.cavasScaler.matchWidthOrHeight = 0.0f;
        }
    }
}
