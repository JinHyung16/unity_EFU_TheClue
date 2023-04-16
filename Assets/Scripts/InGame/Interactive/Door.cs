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

    [SerializeField] private MeshRenderer doorRenderer;

    private Vector3 offset;

    private bool isColorChange = false;
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
        offset = new Vector3(1.0f, 1.5f, -0.3f);

        if (ThemeSecondPresenter.GetInstance != null)
        {
            DoorColorChange();
        }
    }

    public void DoorColorChange()
    {
        if (ThemeSecondPresenter.GetInstance != null && !isColorChange)
        {
            if (TimerManager.GetInstance.CurMinTime <= 10)
            {
                //doorRenderer.material.SetColor("_EmissionColor", Color.blue * Mathf.LinearToGammaSpace(2.0f));
            }
            else
            {
                //doorRenderer.material.SetColor("_EmissionColor", Color.red * Mathf.LinearToGammaSpace(2.0f));
            }
        }
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
                ThemeFirstPresenter.GetInstance.GameClear(true);
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
