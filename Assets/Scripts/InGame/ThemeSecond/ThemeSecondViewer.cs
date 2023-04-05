using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThemeSecondViewer : MonoBehaviour
{
    [SerializeField] private List<Canvas> canvasList = new List<Canvas>();

    [Header("NoteCanvas의 Panel")]
    [SerializeField] private List<GameObject> notePanelList = new List<GameObject>();
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

    /// <summary>
    /// NPC와 첫 대화시 Note를 선택하는 Canvas를 연다
    /// </summary>
    public void NPCSelectNoteCanvasOpen()
    {
        GameManager.GetInstance.IsUIOpen = true;
        UIManager.GetInstance.ShowCanvas("NoteSelect Canvas");
    }

    public void NoteCanvasOpen(int index)
    {
        GameManager.GetInstance.IsUIOpen = true;
        UIManager.GetInstance.ShowCanvas("Note Canvas");
        //notePanelList[index].SetActive(true);
    }

    public void DoorCanvasOpen()
    {
        GameManager.GetInstance.IsUIOpen = true;
        UIManager.GetInstance.ShowCanvas("Door Canvas");
    }

    /// <summary>
    /// ThemeFirst Scene에서 Canvas들에게 붙어있는 Close버튼을 누르면 사용하는 공용함수
    /// </summary>
    public void CloseCanvas()
    {
        GameManager.GetInstance.IsUIOpen = false;
        UIManager.GetInstance.HideCanvas();
    }
}
