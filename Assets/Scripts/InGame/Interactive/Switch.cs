using HughEnumData;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Switch : InteractiveObject
{
    [SerializeField] private Transform switchTransform;
    private Vector3 offset;

    private void Start()
    {
        offset = new Vector3(0, 0.8f, 0);
    }
    private void OnDestroy()
    {
        GameManager.GetInstance.InvisibleInteractiveCanvas();
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
}
