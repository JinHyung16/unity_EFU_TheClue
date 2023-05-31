using HughEnumData;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class DoorKey : InteractiveObject
{
    [SerializeField] private Transform doorKeyTransform;

    [Header("책상위 위치")]
    [SerializeField] private Transform originPosition;

    [Header("DoorKey UI sprite")]
    [SerializeField] private Sprite doorKeyImage;

    [SerializeField] private int doorKeyNum = 0;

    private Vector3 offset;

    public Sprite GetDoorKeyUISprite { get { return this.doorKeyImage; } }
    public int GetDoorKeyNum { get { return this.doorKeyNum; } }
    private void Start()
    {
        offset = new Vector3(0, 0.3f, 0);
        this.gameObject.transform.position = originPosition.transform.position;
        this.gameObject.transform.rotation = originPosition.transform.rotation;
    }
    private void OnDisable()
    {
        if (GameManager.GetInstance != null)
        {
            GameManager.GetInstance.InvisibleInteractiveCanvas();
            this.NotInteractvie();
        }
        if (originPosition != null)
        {
            this.gameObject.transform.position = originPosition.position;
        }
    }

    #region InteractiveObject Override
    protected override void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (ThemeSecondPresenter.GetInstance != null && ThemeSecondPresenter.GetInstance.InteractiveTypeNum != 1)
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
        GameManager.GetInstance.VisibleInteractiveCanvas(doorKeyTransform, offset);
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
        return InteractiveType.ThemeSecond_Key;
    }

    #endregion
}
