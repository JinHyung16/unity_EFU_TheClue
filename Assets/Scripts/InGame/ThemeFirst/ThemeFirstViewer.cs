using HughEnumData;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ThemeFirstViewer : MonoBehaviour
{
    [Header("하위의 있는 Canvas 담을 List")]
    [SerializeField] private List<Canvas> canvasList = new List<Canvas>();

    [Header("Timer Text")]
    [SerializeField] private TMP_Text resultTimerText;

    private void Start()
    {
        foreach (var canvas in canvasList)
        {
            UIManager.GetInstance.AddCanvasInDictionary(canvas.name, canvas);
        }
    }

    private void OnDisable()
    {
        if (UIManager.GetInstance != null)
        {
            UIManager.GetInstance.ClearAllCanvas();
        }
    }

    /// <summary>
    /// 도어락 캔버스를 열 때 호출
    /// </summary>
    public void OpenDoorLock()
    {
        GameManager.GetInstance.IsUIOpen = true;
        UIManager.GetInstance.ShowCanvas("DoorLock Canvas");
    }

    /// <summary>
    /// 바닥에 타일을 배치하려 할 때 해당 문양이 맞는지 확인하는 UI가 뜬다.
    /// </summary>
    public void TilePatternCanvasOpen(GameObject patternObj)
    {
        GameManager.GetInstance.IsUIOpen = true;
        TileManager.GetInstance.VisibleTilePattern(patternObj);
        UIManager.GetInstance.ShowCanvas("TilePattern Canvas");
    }

    public void OpenResultCanvas(bool isClear)
    {
        GameManager.GetInstance.IsUIOpen = true;
        if (isClear)
        {
            resultTimerText.text = TimerManager.GetInstance.CurTimeString.ToString();
            UIManager.GetInstance.ShowCanvas("GameClearResult Canvas");
        }
        else
        {
            PlayerAnimationController.GetInstance.PlayerAnimationControl(AnimationType.P_Died);
            UIManager.GetInstance.ShowCanvas("GameFailedResult Canvas");
        }
    }

    public void NPCMissionCanvasOpen()
    {
        GameManager.GetInstance.IsUIOpen = true;
        UIManager.GetInstance.ShowCanvas("NPCMission Canvas");
    }

    #region Game Result Canvas하위 Button 기능
    public void GoToMain()
    {
        SceneController.GetInstance.LoadScene("Main");
        GameManager.GetInstance.GameClear();
    }

    public void NextStage()
    {
        SceneController.GetInstance.LoadScene("ThemeSecond");
        GameManager.GetInstance.GameClear();
    }
    public void RetryGame()
    {
        SceneController.GetInstance.LoadScene("ThemeFirst");
        GameManager.GetInstance.GameClear();
    }

    public void QuitGame()
    {
        GameManager.GetInstance.OnApplicationQuit();
    }
    #endregion

    /// <summary>
    /// ThemeFirst Scene에서 Canvas들에게 붙어있는 Close버튼을 누르면 사용하는 공용함수
    /// </summary>
    public void CloseCanvas()
    {
        GameManager.GetInstance.IsUIOpen = false;
        UIManager.GetInstance.HideCanvas();
    }
}
