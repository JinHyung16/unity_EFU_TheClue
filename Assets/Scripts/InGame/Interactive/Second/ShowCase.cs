using HughEnumData;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowCase : InteractiveObject
{
    [SerializeField] private Transform showCaseTransform;

    private Vector3 offset;

    private void Start()
    {
        offset = new Vector3(0, 1.2f, 0.0f);
    }
    #region InteractiveObject Override
    protected override void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            InteractiveManager.GetInstance.IsInteractive = true;
            this.Interacitve();
        }
    }

    protected override void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            InteractiveManager.GetInstance.IsInteractive = false;
            this.NotInteractvie();
        }
    }

    protected override void Interacitve()
    {
        GameManager.GetInstance.VisibleInteractiveCanvas(showCaseTransform, offset);
        InteractiveManager.GetInstance.SetInteractiving(this);
    }

    protected override void NotInteractvie()
    {
        GameManager.GetInstance.InvisibleInteractiveCanvas();
    }

    public override InteractiveType GetInteractiveType()
    {
        return InteractiveType.ThemeSecond_ShowCase;
    }

    #endregion
}
