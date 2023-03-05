using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HughCanvas;
using HughUIType;

public class MainCanvas : CanvasManager
{
    private void Start()
    {
        SceneController.GetInstance.SetCurScene();
        UIManager.GetInstance.AddCanvasInDictionary(CanvasType.FixedCanvas, this);
    }
    private void OnDestroy()
    {
        UIManager.GetInstance.ClearAllCanvas();
    }

    /// <summary>
    /// 새로운 게임 시작할 때 호출
    /// </summary>
    public void StartNewGame()
    {
        GameManager.GetInstance.StartNewGame();
    }


    /// <summary>
    /// 이어서 하기 누르면 호출
    /// </summary>
    public void StartLoadGame()
    {
        GameManager.GetInstance.StartSavedGame();
    }
}
