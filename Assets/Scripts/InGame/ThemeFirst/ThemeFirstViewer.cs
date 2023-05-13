using Cysharp.Threading.Tasks;
using HughEnumData;
using System;
using System.Collections.Generic;
using System.Threading;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ThemeFirstViewer : MonoBehaviour
{
    [Header("하위의 있는 Canvas 담을 List")]
    [SerializeField] private List<Canvas> canvasList = new List<Canvas>();

    [Header("Dialogue UI들")]
    [SerializeField] private Canvas dialogueCanvas;
    [SerializeField] private Button nextDialogueBtn;
    [SerializeField] private TMP_Text dialogueText;
    private int dialgoueIndex = 0;

    [Header("Narrative UI들")]
    [SerializeField] private Canvas narrativeCanvas;
    [SerializeField] private TMP_Text narrativeText;

    [Header("NPC Mission UI들")]
    [SerializeField] private List<GameObject> missionList;
    private int missionIndex = 0;

    [Header("Timer Text")]
    [SerializeField] private TMP_Text resultTimerText;

    private CancellationTokenSource tokenSource;

    private void Awake()
    {
        nextDialogueBtn.onClick.AddListener(NextDialogueBtn);
        dialogueCanvas.enabled = false;
        narrativeCanvas.enabled = false;
    }
    private void Start()
    {
        foreach (var canvas in canvasList)
        {
            UIManager.GetInstance.AddCanvasInDictionary(canvas.name, canvas);
        }

        foreach (var mission in missionList)
        {
            mission.SetActive(false);
        }

        if (tokenSource != null)
        {
            tokenSource.Cancel();
            tokenSource.Dispose();
        }
        tokenSource = new CancellationTokenSource();
    }

    public void DialogueStart()
    {
        dialgoueIndex = 0;
        dialogueText.text = DataManager.GetInstance.ThemeFirstContent[0];
        dialogueCanvas.enabled = true;
        Time.timeScale = 0;
    }

    private void NextDialogueBtn()
    {
        dialgoueIndex += 1;
        if (DataManager.GetInstance.ThemeFirstContent.Count <= dialgoueIndex)
        {
            dialogueCanvas.enabled = false;
            Time.timeScale = 1;
            ThemeFirstPresenter.GetInstance.DoneDialogue();
            return;
        }
        dialogueText.text = DataManager.GetInstance.ThemeFirstContent[dialgoueIndex];
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

    public void NarrativeCanvase(string context)
    {
        narrativeText.text = context;
        NarrativeUI().Forget();
    }

    private async UniTaskVoid NarrativeUI()
    {
        narrativeCanvas.enabled = true;
        await UniTask.Delay(TimeSpan.FromSeconds(1.5f), cancellationToken: tokenSource.Token);
        narrativeCanvas.enabled = false;
    }

    public void NPCMissionCanvasOpen()
    {
        GameManager.GetInstance.IsUIOpen = true;
        if (missionList.Count <= missionIndex)
        {
            missionIndex = 0;
        }
        missionList[missionIndex].SetActive(true);
        missionIndex += 1;
        UIManager.GetInstance.ShowCanvas("NPCMission Canvas");
    }

    #region Game Result Canvas하위 Button 기능
    public void GoToMain()
    {
        GameManager.GetInstance.GameClear();
        SceneController.GetInstance.LoadScene("Main");
    }

    public void NextStage()
    {
        GameManager.GetInstance.GameClear();
        SceneController.GetInstance.LoadScene("ThemeSecond");
    }
    public void RetryGame()
    {
        GameManager.GetInstance.GameClear();
        SceneController.GetInstance.LoadScene("ThemeFirst");
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
