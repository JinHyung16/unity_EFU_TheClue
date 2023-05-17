using HughEnumData;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EscapeKey : InteractiveObject
{
    [SerializeField] private Transform escapeKeyTrans;

    [Header("DoorKey UI sprite")]
    [SerializeField] private Sprite escapeKeyImg;

    public string escapeKeyName;

    private Vector3 offset;

    public Sprite GetEscapeKeyUISprite { get { return this.escapeKeyImg; } }

    private void Start()
    {
        offset = new Vector3(0, 0.3f, 0);
        this.gameObject.SetActive(false);
    }
    private void OnDisable()
    {
        if (GameManager.GetInstance != null)
        {
            GameManager.GetInstance.InvisibleInteractiveCanvas();
            this.NotInteractvie();
        }
    }

    #region InteractiveObject Override
    protected override void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (ThemeSecondPresenter.GetInstance != null && ThemeSecondPresenter.GetInstance.IsInteractiveNum != 1)
            {
                InteractiveManager.GetInstance.IsInteractive = true;
                this.Interacitve();
            }
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
        GameManager.GetInstance.VisibleInteractiveCanvas(escapeKeyTrans, offset);
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
        return InteractiveType.ThemeThird_EscapeKey;
    }
    #endregion
}
