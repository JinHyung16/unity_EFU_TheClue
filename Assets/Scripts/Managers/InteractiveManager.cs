using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HughGenerics;
using HughEnumData;

public class InteractiveManager : Singleton<InteractiveManager>
{
    private List<InteractiveObject> interactiveObjects;
    private InteractiveType curInteractiveType;
    private Transform interactiveTrans;


    private void Start()
    {
        interactiveObjects = new List<InteractiveObject>();
    }

    public void Interactive()
    {
        if (interactiveObjects.Count <= 0)
        {
            return;
        }

        curInteractiveType = interactiveObjects[0].GetInteractiveType();
        Notify(curInteractiveType);
    }

    public void SetInteractiveObject(InteractiveObject obj, bool isCall)
    {
        if (isCall)
        {
            interactiveObjects.Add(obj);
        }
        else
        {
            if (interactiveObjects.Contains(obj))
            {
                interactiveObjects.Remove(obj);
            }
        }
    }

    public void SetInteractivePosition(Transform transform)
    {
        this.interactiveTrans = transform;
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
                ThemeFirstPresenter.GetInstance.DicePutInInveotry(this.interactiveTrans);
                break;
            default:
                break;
        }
    }
}
