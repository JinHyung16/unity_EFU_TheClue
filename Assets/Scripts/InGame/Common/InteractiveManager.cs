using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HughEnumData;

public class InteractiveManager : MonoBehaviour
{
    #region static
    public static InteractiveManager GetInstance;
    private void Awake()
    {
        GetInstance = this;
    }
    #endregion

    private List<InteractiveObject> interactiveObjects = new List<InteractiveObject>();
    private GameObject inventoryObj; //inventory에 들어갈 object

    private GameObject puzzleObj; //퍼즐같이 상호작용하는 object
    public bool IsInteractive { get; set; } = false; //상호작용 가능하다는 문구가 나온경우 true

    /// <summary>
    /// Player가 E키를 누르면 호출된다.
    /// </summary>
    public void Interactive()
    {
        if (0 < interactiveObjects.Count)
        {
            Notify(interactiveObjects[0].GetInteractiveType());
        }
    }

    /// <summary>
    /// 상호작용을 할 InteractiveObject를 상속하는 것을 받는다.
    /// </summary>
    /// <param name="obj">InteractiveObject를 상속하고 있는 obj</param>
    public void SetInteractiving(InteractiveObject obj)
    {
        if (0 < interactiveObjects.Count)
        {
            interactiveObjects.Clear();
        }
        interactiveObjects.Add(obj);
    }

    /// <summary>
    /// 테마1에서 tile 오브젝트를 받는다.
    /// 상호작용할 오브젝트로 G를 눌러 퍼즐 공간을 열고
    /// 실제로 데이터는 tile object를 TileManager에게 보낸다.
    /// </summary>
    /// <param name="itvObj">상호작용할 오브젝트</param>
    /// <param name="obj">실제 타일 오브젝트</param>
    public void SetPuzzleInteractive(InteractiveObject itvObj, GameObject obj)
    {
        if (0 < interactiveObjects.Count)
        {
            interactiveObjects.Clear();
        }
        interactiveObjects.Add(itvObj);
        this.puzzleObj = obj;
    }

    /// <summary>
    /// Invenotry에 들어갈 게임 오브젝트를 받는다.
    /// 또는 개별적으로 해당 오브젝트를 불러올 경우도 사용한다.
    /// </summary>
    /// <param name="obj">인벤토리에 들어갈 실제 게임오브젝트</param>
    public void SetInteractvieObjToInventory(GameObject obj)
    {
        this.inventoryObj = obj;
    }

    private void Notify(InteractiveType type)
    {
        switch (type)
        {
            case InteractiveType.None:
                break;
            case InteractiveType.DoorLock:
                PlayerAnimationController.GetInstance.PlayerAnimationControl(AnimationType.P_EnterCode);
                if (ThemeFirstPresenter.GetInstance != null)
                {
                    ThemeFirstPresenter.GetInstance.OpenDoorLockUI();
                }
                else if (ThemeSecondPresenter.GetInstance != null)
                {
                    ThemeSecondPresenter.GetInstance.OpenDoorLockUI();
                }
                break;
            case InteractiveType.Switch:
                if (ThemeFirstPresenter.GetInstance != null)
                {
                    ThemeFirstPresenter.GetInstance.SwitchOffAndAutoOn();
                    inventoryObj.GetComponent<Switch>().SwitchButtonRotate();
                }
                else if (ThemeSecondPresenter.GetInstance != null)
                {
                    ThemeSecondPresenter.GetInstance.SwitchOnOff();
                }
                break;
            case InteractiveType.Door:
                PlayerAnimationController.GetInstance.PlayerAnimationControl(AnimationType.P_OpenDoor);
                if (ThemeFirstPresenter.GetInstance != null)
                {
                    ThemeFirstPresenter.GetInstance.DoorOpen();
                }
                else if (ThemeSecondPresenter.GetInstance != null)
                {
                    ThemeSecondPresenter.GetInstance.DoorKeyHoleInteractive(true);
                }
                break;
            case InteractiveType.ThemeFirst_Dice:
                PlayerAnimationController.GetInstance.PlayerAnimationControl(AnimationType.P_PickUp);
                if (inventoryObj != null)
                {
                    ThemeFirstPresenter.GetInstance.DicePutInInveotry(inventoryObj);
                }
                break;
            case InteractiveType.ThemeFirst_Tile_Pattern:
                ThemeFirstPresenter.GetInstance.TileInteractiveOpen(puzzleObj);
                break;
            case InteractiveType.ThemeFirst_Cube:
                PlayerAnimationController.GetInstance.PlayerAnimationControl(AnimationType.P_PickUp);
                if (inventoryObj != null)
                {
                    ThemeFirstPresenter.GetInstance.CubePutInInveotry(inventoryObj);
                }
                break;
            case InteractiveType.ThemeSecond_Key:
                PlayerAnimationController.GetInstance.PlayerAnimationControl(AnimationType.P_PickUp);
                if (inventoryObj != null)
                {
                    ThemeSecondPresenter.GetInstance.DoorKeyInventory(inventoryObj);
                }
                break;
            case InteractiveType.ThemeSecond_ShowCase:
                ThemeSecondPresenter.GetInstance.ShowCaseInteractive(true);
                break;
            case InteractiveType.ThemeSecond_NPC:
                if (ThemeFirstPresenter.GetInstance != null)
                {
                    ThemeFirstPresenter.GetInstance.NPCInteractiveShowMission();
                }
                else if (ThemeSecondPresenter.GetInstance != null)
                {
                    if (!ThemeSecondPresenter.GetInstance.IsNPCFirstTalk)
                    {
                        ThemeSecondPresenter.GetInstance.NPCInteractiveSelectNote(true);
                    }
                    else
                    {
                        //ThemeSecondPresenter.GetInstance.NPCInteractiveShowMission();
                    }
                }
                break;
            case InteractiveType.ThemeThird_Btn_GetKey:
                ThemeThirdPresenter.GetInstance.DropTheKeyByButton();
                break;
            case InteractiveType.ThemeThird_Btn_CallNPC:
                ThemeThirdPresenter.GetInstance.CallNPCByButton();
                break;
            default:
                break;
        }
    }
}
