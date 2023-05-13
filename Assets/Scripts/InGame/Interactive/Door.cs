using Cysharp.Threading.Tasks;
using HughEnumData;
using System;
using System.Collections;
using System.Threading;
using TMPro;
using UnityEngine;

public class Door : InteractiveObject
{
    [SerializeField] private Transform doorTransform;

    private Vector3 offset;

    //UniTask 토큰
    private CancellationTokenSource tokenSource;

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
        GameManager.GetInstance.VisibleInteractiveCanvas(doorTransform, offset);
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
        return InteractiveType.Door;
    }

    #endregion
    private void Start()
    {
        if (tokenSource != null)
        {
            tokenSource.Dispose();
        }
        tokenSource = new CancellationTokenSource();

        offset = new Vector3(0, 0.2f, 0);
    }
}
