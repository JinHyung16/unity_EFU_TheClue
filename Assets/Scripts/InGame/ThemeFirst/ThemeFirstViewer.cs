using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ThemeFirstViewer : MonoBehaviour
{
    [Header("하위의 있는 Canvas 담을 List")]
    [SerializeField] private List<Canvas> canvasList = new List<Canvas>();

    [Header("Door Lock Key Pad 버튼 List")]
    [SerializeField] private List<Button> keypadButtonList = new List<Button>();

    [SerializeField] private TMP_Text doorLockInputText;

    //[SerializeField] private Canvas gameResultCanvas;
    private void Awake()
    {
        foreach (var canvas in canvasList)
        {
            UIManager.GetInstance.AddCanvasInDictionary(canvas.name, canvas);
        }

        foreach (var button in keypadButtonList)
        {
            button.onClick.AddListener(PressDoorLockKeyPad);
        }
    }

    private void Start()
    {
        doorLockInputText.text = "";
        ThemeFirstPresenter.GetInstance.ClearDoorLockKeyPad();
    }

    private void OnDestroy()
    {
        UIManager.GetInstance.ClearAllCanvas();
    }

    /// <summary>
    /// 도어락에 있는 버튼 누를때 호출되는 함수
    /// </summary>
    private void PressDoorLockKeyPad()
    {
        ThemeFirstPresenter.GetInstance.InputDoorLockKeyPad(EventSystem.current.currentSelectedGameObject.name);
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
    /// 도어락에 있는 버튼을 입력하면 입력한 버튼을 보여주는
    /// Text UI를 업데이트한다
    /// </summary>
    /// <param name="key"> 누른 버튼 번호 </param>
    public void UpdateDoorLockInput(string key)
    {
        doorLockInputText.text = key;
    }

    /// <summary>
    /// 도어락에 입력한 번호를 보여주는 Text UI 지우기
    /// </summary>
    public void ClearDoorLockInput()
    {
        doorLockInputText.text = "";
        ThemeFirstPresenter.GetInstance.ClearDoorLockKeyPad();
    }

    /// <summary>
    /// 도어락의 번호를 다 입력한 경우
    /// </summary>
    public void DoneDoorLockLinput()
    {
        //원래 비번이랑 비교해서
        //같으면 탈출
        //다르면 실패처리로 씬 다시 시작

        GameManager.GetInstance.IsUIOpen = false;
        ThemeFirstPresenter.GetInstance.DoneDoorLock();
    }

    /// <summary>
    /// 바닥에 타일을 배치하려 할 때 해당 문양이 맞는지 확인하는 UI가 뜬다.
    /// </summary>
    public void TilePatternCanvasOpen(GameObject diceObj, GameObject patternObj)
    {
        GameManager.GetInstance.IsUIOpen = true;
        UIManager.GetInstance.ShowCanvas("TilePattern Canvas");
        TileManager.GetInstance.VisibleTilePattern(patternObj);
        TileManager.GetInstance.SetDiceOnTileCanvas(diceObj);
    }

    public void OpenResultCanvas(bool isClear)
    {
        GameManager.GetInstance.IsUIOpen = true;
        if (isClear)
        {
            UIManager.GetInstance.ShowCanvas("GameClearResult Canvas");
        }
        else
        {
            UIManager.GetInstance.ShowCanvas("GameFaileResult Canvas");
        }
    }

    /// <summary>
    /// ThemeFirst Scene에서 Canvas들에게 붙어있는 Close버튼을 누르면 사용하는 공용함수
    /// </summary>
    public void CloseCanvas()
    {
        GameManager.GetInstance.IsUIOpen = false;
        ClearDoorLockInput();
        UIManager.GetInstance.HideCanvas();
    }
}
