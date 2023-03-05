using HughCanvas;
using HughUIType;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThemeFirstCanvas : CanvasManager
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
