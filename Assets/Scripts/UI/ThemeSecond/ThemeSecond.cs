using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HughCanvas;
using HughUIType;

public class ThemeSecond : CanvasManager
{
    [SerializeField] private Camera mainCamera;
    private void Start()
    {
        mainCamera.enabled = false;
        SceneController.GetInstance.SetCurScene();
        UIManager.GetInstance.AddCanvasInDictionary(CanvasType.FixedCanvas, this);
    }

    private void OnDestroy()
    {
        UIManager.GetInstance.ClearAllCanvas();
    }
}
