using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HughEnumData;
public class DoorLock : InteractiveObject
{
    [SerializeField] private Transform doorLockObjTransform;

    private Vector3 offset = Vector3.zero;
    private void Start()
    {
        offset = new Vector3(0, 0.8f, 0);
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            GameManager.GetInstance.VisibleInteractiveCanvas(doorLockObjTransform, offset);
            InteracitveOrNot(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            GameManager.GetInstance.InvisibleInteractiveCanvas();
            InteracitveOrNot(false);
        }
    }

    public override void InteracitveOrNot(bool interactive)
    {
        if (interactive)
        {
            InteractiveManager.GetInstance.SetInteractiveObject(this, true);
        }
        else
        {
            InteractiveManager.GetInstance.SetInteractiveObject(this, false);
        }
    }

    public override InteractiveType GetInteractiveType()
    {
        this.myInteractiveType = InteractiveType.ThemeFirst_DoorLock;
        return this.myInteractiveType;
    }
}
