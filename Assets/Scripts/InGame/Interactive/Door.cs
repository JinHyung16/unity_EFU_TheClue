using Cysharp.Threading.Tasks;
using HughEnumData;
using System;
using System.Collections;
using System.Threading;
using UnityEngine;

public class Door : InteractiveObject
{
    [SerializeField] private Transform doorTransform;
    [SerializeField] private Canvas doorCanvas;

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

        doorCanvas.enabled = false;
        offset = new Vector3(0, 1.0f, -0.3f);
    }
    /// <summary>
    /// 테마1에서 문과 상호작용시 사용할 함수
    /// </summary>
    public void DoorOpenToEscape()
    {
        if (tokenSource != null)
        {
            tokenSource.Cancel();
            tokenSource.Dispose();
        }
        tokenSource = new CancellationTokenSource();

        if (GameManager.GetInstance.IsGameClear)
        {
            if (ThemeFirstPresenter.GetInstance != null)
            {
                ThemeFirstPresenter.GetInstance.GameResultOpen(true);
            }
        }
        else
        {
            doorCanvas.enabled = true;
            AutoDoorOpenAndClose().Forget();
        }
    }

    private async UniTaskVoid AutoDoorOpenAndClose()
    {
        await UniTask.Delay(TimeSpan.FromSeconds(1.5f), cancellationToken: tokenSource.Token);
        doorCanvas.enabled = false;
    }
}
