using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class DoorLockManager : MonoBehaviour
{
    #region static
    public static DoorLockManager GetInstance;
    private void Awake()
    {
        GetInstance = this;
    }
    #endregion

    [Header("DoorLock Key Pad 관련")]
    [SerializeField] private List<Button> buttonList = new List<Button>();
    [SerializeField] private List<Image> displayImageList = new List<Image>();
    [SerializeField] private List<Image> btnImageList = new List<Image>();

    [Header("DoorLock 버튼의 pattern image")]
    [SerializeField] private List<Sprite> doorLockImageList;
    private List<int> doorLockRandomIndex; //doorlock의 7,8,9번은 랜덤이미지 배치로 input시 이 값에서 이미지 가져온다.

    private int displayIndex = 0;

    private void Start()
    {
        ClearDoorLockDisplay();

        for (int i = 0; i < buttonList.Count; i++)
        {
            buttonList[i].onClick.AddListener(PressDoorLockKeyPad);
        }

        for (int i = 0; i < displayImageList.Count; i++)
        {
            displayImageList[i].sprite = null;
        }

        doorLockRandomIndex = new List<int>();
        for (int i = 0; i < 3; i++)
        {
            int index = Random.Range(0, 6);
            doorLockRandomIndex.Add(index);
            btnImageList[i].sprite = doorLockImageList[index];
        }
    }

    /// <summary>
    /// 도어락에 있는 버튼 누를때 호출되는 함수
    /// </summary>
    private void PressDoorLockKeyPad()
    {
        int index = int.Parse(EventSystem.current.currentSelectedGameObject.name);
        UpdateDoorLockDisplay(index);
    }

    public void UpdateDoorLockDisplay(int index)
    {
        if (displayImageList.Count <= displayIndex)
        {
            displayIndex = 0;
        }

        if (index < 7)
        {
            displayImageList[displayIndex].sprite = doorLockImageList[(index - 1)];
        }
        else
        {
            int temp = index - 7;
            displayImageList[displayIndex].sprite = doorLockImageList[doorLockRandomIndex[temp]];
        }
        displayIndex += 1;
    }

    /// <summary>
    /// 도어락에 입력한 번호를 보여주는 Text UI 지우기
    /// </summary>
    public void ClearDoorLockDisplay()
    {
        for (int i = 0; i < displayImageList.Count; i++)
        {
            displayImageList[i].sprite = null;
        }
    }

    /// <summary>
    /// 도어락의 번호를 다 입력한 경우
    /// </summary>
    public void DoneDoorLockInput()
    {
        //원래 비번이랑 비교해서
        //같으면 탈출
        //다르면 실패처리로 씬 다시 시작

        GameManager.GetInstance.IsUIOpen = false;
        ThemeFirstPresenter.GetInstance.DoneDoorLock();
        ClearDoorLockDisplay();
    }
}
