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

    private void PressDoorLockKeyPad()
    {
        ThemeFirstPresenter.GetInstance.InputDoorLockKeyPad(EventSystem.current.currentSelectedGameObject.name);
    }

    public void OpenDoorLock()
    {
        GameManager.GetInstance.IsUIOpen = true;
        UIManager.GetInstance.ShowCanvas("DoorLock Canvas");
    }

    public void UpdateDoorLockInput(string key)
    {
        doorLockInputText.text = key;
    }

    public void ClearDoorLockInput()
    {
        doorLockInputText.text = "";
        ThemeFirstPresenter.GetInstance.ClearDoorLockKeyPad();
    }

    public void DoneDoorLockLinput()
    {
        //원래 비번이랑 비교해서
        //같으면 탈출
        //다르면 실패처리로 씬 다시 시작

        GameManager.GetInstance.IsUIOpen = false;
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
