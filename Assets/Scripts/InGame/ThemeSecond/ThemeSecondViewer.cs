using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ThemeSecondViewer : MonoBehaviour
{
    [SerializeField] private List<Canvas> canvasList = new List<Canvas>();

    [Header("Timer Text")]
    [SerializeField] private TMP_Text resultTimerText;
    private void Start()
    {
        for (int i = 0; i < canvasList.Count; i++)
        {
            UIManager.GetInstance.AddCanvasInDictionary(canvasList[i].name, canvasList[i]);
        }
    }

    public void DoorLockCanvasOpen()
    {
        GameManager.GetInstance.IsUIOpen = true;
        UIManager.GetInstance.ShowCanvas("DoorLock Canvas");
    }

    public void InteractiveDoorCanvas()
    {
        GameManager.GetInstance.IsUIOpen = true;
        UIManager.GetInstance.ShowCanvas("IDoor Canvas");
    }
    public void InteractiveShowcanseCanvasOpen()
    {
        GameManager.GetInstance.IsUIOpen = true;
        UIManager.GetInstance.ShowCanvas("IShowcase Canvas");
    }

    public void NoteCanvaseOpen()
    {
        GameManager.GetInstance.IsUIOpen = true;
        UIManager.GetInstance.ShowCanvas("Note Canvas");
    }

    /// <summary>
    /// NPC와 첫 대화시 Note를 선택하는 Canvas를 연다
    /// </summary>
    public void NPCSelectNoteCanvasOpen()
    {
        GameManager.GetInstance.IsUIOpen = true;
        UIManager.GetInstance.ShowCanvas("NPCNoteSelect Canvas");
    }

    public void NPCMissionCanvasOpen()
    {
        GameManager.GetInstance.IsUIOpen = true;
        UIManager.GetInstance.ShowCanvas("NPCMission Canvas");
    }


    public void OpenResultCanvas(bool isClear)
    {
        if (isClear)
        {
            resultTimerText.text = TimerManager.GetInstance.CurTimeString.ToString();
            UIManager.GetInstance.ShowCanvas("GameClearResult Canvas");
        }
        else
        {
            UIManager.GetInstance.ShowCanvas("GameFailedResult Canvas");
        }
        GameManager.GetInstance.IsUIOpen = true;
    }

    #region Game Result Canvas하위 Button 기능
    public void GoToMain()
    {
        SceneController.GetInstance.LoadScene("Main");
        GameManager.GetInstance.GameClear();
    }

    public void NextStage()
    {
        SceneController.GetInstance.LoadScene("ThemeThird");
        GameManager.GetInstance.GameClear();
    }
    public void RetryGame()
    {
        SceneController.GetInstance.LoadScene("ThemeSecond");
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
