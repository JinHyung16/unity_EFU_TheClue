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
    private GameObject inventoryObj;

    private GameObject puzzleObj;

    public bool IsInteractive { get; set; } = false; //상호작용 가능하다는 문구가 나온경우 true

    /// <summary>
    /// Player가 G키를 누르면 호출된다.
    /// </summary>
    public void Interactive()
    {
        if (0 < interactiveObjects.Count)
        {
            Notify(interactiveObjects[0].GetInteractiveType());
        }
    }
    /// <summary>
    /// 테마별 미션을 확인할 때 호출
    /// </summary>
    public void MissionOpen()
    {
        if (ThemeFirstPresenter.GetInstance != null)
        {
        }
    }

    
    public void SetInteractiving(InteractiveObject obj)
    {
        if (0 < interactiveObjects.Count)
        {
            interactiveObjects.Clear();
        }
        interactiveObjects.Add(obj);
    }

    public void SetPuzzleInteractive(InteractiveObject itvObj, GameObject obj)
    {
        if (0 < interactiveObjects.Count)
        {
            interactiveObjects.Clear();
        }
        interactiveObjects.Add(itvObj);
        this.puzzleObj = obj;
    }

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
            case InteractiveType.ThemeFirst_DoorLock:
                ThemeFirstPresenter.GetInstance.OpenDoorLockUI();
                break;
            case InteractiveType.ThemeFirst_Dice:
                ThemeFirstPresenter.GetInstance.DicePutInInveotry(inventoryObj);
                break;
            case InteractiveType.ThemeFirst_Switch:
                ThemeFirstPresenter.GetInstance.SwitchOffAndAutoOn();
                inventoryObj.GetComponent<Switch>().SwitchButtonRotate();
                break;
            case InteractiveType.ThemeFirst_Tile_Pattern:
                ThemeFirstPresenter.GetInstance.TileInteractiveOpen(puzzleObj);
                break;
            case InteractiveType.ThemeFirst_Cube:
                ThemeFirstPresenter.GetInstance.CubePutInInveotry(inventoryObj);
                break;
            case InteractiveType.Door:
                inventoryObj.GetComponent<Door>().DoorUseToKey();
                break;
            default:
                break;
        }
    }
}
