using HughEnumData;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThemeThirdButton : InteractiveObject
{
    [SerializeField] private Transform teleportBtnTransform;

    public int btnTagNum = 0;

    private Vector3 offset;

    private void Start()
    {
        offset = new Vector3(0, 0.3f, 0);
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
    protected override void OnTriggerStay(Collider other)
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
        GameManager.GetInstance.VisibleInteractiveCanvas(teleportBtnTransform, offset);
        InteractiveManager.GetInstance.SetInteractiving(this);
        InteractiveManager.GetInstance.SetInteractvieObjToInventory(this.gameObject);
    }

    protected override void NotInteractvie()
    {
        GameManager.GetInstance.InvisibleInteractiveCanvas();
        InteractiveManager.GetInstance.SetInteractvieObjToInventory(null);
    }

    public override InteractiveType GetInteractiveType()
    {
        InteractiveType myType = InteractiveType.None;

        switch (btnTagNum)
        {
            case 0:
                myType = InteractiveType.ThemeThird_Btn_GetKey;
                break;
            case 1:
                myType = InteractiveType.ThemeThird_Btn_Destroy;
                break;
            case 2:
                myType = InteractiveType.ThemeThird_Btn_GetKey;
                break;
        }
        return myType;
    }

    #endregion
}
