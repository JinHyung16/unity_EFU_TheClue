using Cysharp.Threading.Tasks;
using HughEnumData;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class ThemeThirdViewer : MonoBehaviour
{
    [SerializeField] private List<Canvas> canvasList = new List<Canvas>();

    [Header("Dialogue UI들")]
    [SerializeField] private Canvas dialogueCanvas;
    [SerializeField] private Button nextDialogueBtn;
    [SerializeField] private TMP_Text dialgoueText;
    private int dialgoueIndex = 0;

    [Header("Narrative UI들")]
    [SerializeField] private Canvas narrativeCanvas;
    [SerializeField] private TMP_Text narrativeText;

    [Header("Timer Text")]
    [SerializeField] private TMP_Text resultTimerText;

    private CancellationTokenSource tokenSource;

    private void Awake()
    {
        dialogueCanvas.enabled = false;
        nextDialogueBtn.onClick.AddListener(NextDialogueBtn);
    }
    private void Start()
    {
        foreach (var canvas in canvasList)
        {
            UIManager.GetInstance.AddCanvasInDictionary(canvas.name, canvas);
        }

        narrativeCanvas.enabled = false;

        if (tokenSource != null)
        {
            tokenSource.Cancel();
            tokenSource.Dispose();
        }
        tokenSource = new CancellationTokenSource();
    }
    private void OnDisable()
    {
        nextDialogueBtn.onClick.RemoveAllListeners();
        UIManager.GetInstance.ClearAllCanvas();
    }

    public void OpenResultCanvas(bool isClear)
    {
        Time.timeScale = 0;
        GameManager.GetInstance.IsUIOpen = true;
        if (isClear)
        {
            resultTimerText.text = TimerManager.GetInstance.CurTimeString.ToString();
            AudioManager.GetInstance.PlaySFX(AudioManager.SFX.GameResult_Celar);
            UIManager.GetInstance.ShowCanvas("GameClearResult Canvas");
        }
        else
        {
            PlayerAnimationController.GetInstance.PlayerAnimationControl(AnimationType.P_Died);
            AudioManager.GetInstance.PlaySFX(AudioManager.SFX.GAmeResult_Fail);
            UIManager.GetInstance.ShowCanvas("GameFailedResult Canvas");
        }
    }

    public void DialogueStart()
    {
        GameManager.GetInstance.IsUIOpen = true;
        dialgoueIndex = 0;
        dialgoueText.text = DataManager.GetInstance.ThemeSecondContent[0];
        dialogueCanvas.enabled = true;
        Time.timeScale = 0;
    }

    private void NextDialogueBtn()
    {
        AudioManager.GetInstance.PlaySFX(AudioManager.SFX.DialogueBtn);
        dialgoueIndex += 1;
        if (DataManager.GetInstance.ThemeThirdContent.Count <= dialgoueIndex)
        {
            GameManager.GetInstance.IsUIOpen = false;
            dialogueCanvas.enabled = false;
            Time.timeScale = 1;
            ThemeThirdPresenter.GetInstance.DoneDialogue();
            return;
        }
        dialgoueText.text = DataManager.GetInstance.ThemeThirdContent[dialgoueIndex];
    }

    public void NarrativeCanvas(string context)
    {
        narrativeText.text = context;
        NarrativeUI().Forget();
    }

    private async UniTaskVoid NarrativeUI()
    {
        narrativeCanvas.enabled = true;
        await UniTask.Delay(TimeSpan.FromSeconds(1.5f), cancellationToken: this.GetCancellationTokenOnDestroy());
        narrativeCanvas.enabled = false;
    }

    #region Game Result Canvas하위 Button 기능
    public void GoToMain()
    {
        GameManager.GetInstance.GameDataClear();
        SceneController.GetInstance.LoadScene("Main");
    }

    public void NextStage()
    {
        GameManager.GetInstance.GameDataClear();
        SceneController.GetInstance.LoadScene("Main");
    }
    public void RetryGame()
    {
        GameManager.GetInstance.GameDataClear();
        SceneController.GetInstance.LoadScene("ThemeThird");
    }

    public void QuitGame()
    {
        GameManager.GetInstance.ProgramQuit();
    }
    #endregion

    /// <summary>
    /// ThemeFirst Scene에서 Canvas들에게 붙어있는 Close버튼을 누르면 사용하는 공용함수
    /// </summary>
    public void CloseCanvas()
    {
        GameManager.GetInstance.IsUIOpen = false;
        AudioManager.GetInstance.PlaySFX(AudioManager.SFX.UIClick);
        UIManager.GetInstance.HideCanvas();
    }
}
