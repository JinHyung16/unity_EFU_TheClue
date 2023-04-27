using HughEnumData;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC : InteractiveObject
{
    private Animator anim;

    [SerializeField] private Transform npcTransform;

    private Vector3 offset;
    private void Start()
    {
        anim = GetComponentInChildren<Animator>();
        offset = new Vector3(0, 4.8f, -0.3f);
    }

    #region InteractiveObject Override
    protected override void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            InteractiveManager.GetInstance.IsInteractive = true;
            Interacitve();
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
            NotInteractvie();
        }
    }

    protected override void Interacitve()
    {
        GameManager.GetInstance.VisibleInteractiveCanvas(npcTransform, offset, false);
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
        return InteractiveType.ThemeSecond_NPC;
    }

    #endregion
}
