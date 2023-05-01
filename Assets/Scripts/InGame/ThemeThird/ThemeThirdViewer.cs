using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;


public class ThemeThirdViewer : MonoBehaviour
{
    [SerializeField] private List<Canvas> canvasList = new List<Canvas>();

    [Header("BtnInteractive Text")]
    [SerializeField] private TMP_Text btnInteractiveTxt;

    [Header("Timer Text")]
    [SerializeField] private TMP_Text resultTimerText;
    private void Start()
    {
        for (int i = 0; i < canvasList.Count; i++)
        {
            UIManager.GetInstance.AddCanvasInDictionary(canvasList[i].name, canvasList[i]);
        }
    }

    public void BtnInteractoveOn(string _text)
    {
        btnInteractiveTxt.text = _text;
        UIManager.GetInstance.ShowCanvas("BtnInteractive Canvas");
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
        SceneController.GetInstance.LoadScene("ThemeThird");
    }
    public void RetryGame()
    {
        GameManager.GetInstance.GameClear();
        SceneController.GetInstance.LoadScene("ThemeSecond");
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
