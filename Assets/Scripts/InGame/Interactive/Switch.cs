using HughEnumData;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;

public class Switch : InteractiveObject
{
    [SerializeField] private Transform switchTransform;
    [SerializeField] private Transform switchBtnTransform;

    private Vector3 offset;

    private void Start()
    {
        offset = new Vector3(0, 0.8f, -1.0f);
        switchBtnTransform.rotation = Quaternion.Euler(0.0f, 0.0f, 0.0f);
    }
    private void OnDestroy()
    {
        InteracitveOrNot(false);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            GameManager.GetInstance.VisibleInteractiveCanvas(this.switchTransform, offset);
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
        this.myInteractiveType = InteractiveType.ThemeFirst_Switch;
        return this.myInteractiveType;
    }

    public void SwitchButtonRotate()
    {
        if (ThemeFirstPresenter.GetInstance.IsSwitchOn)
        {
            switchBtnTransform.Rotate(-20.0f, 0, 0);
        }
        else
        {
            switchBtnTransform.Rotate(20.0f, 0, 0);
        }
    }
}
