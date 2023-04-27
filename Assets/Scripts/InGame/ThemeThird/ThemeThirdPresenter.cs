using Cysharp.Threading.Tasks;
using DG.Tweening;
using HughGenerics;
using System;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class ThemeThirdPresenter : PresenterSingleton<ThemeThirdPresenter>
{
    [SerializeField] private ThemeThirdViewer themeThirdViewer;

    [Header("Camera들")]
    [SerializeField] private Camera cameraMain;
    [SerializeField] private Camera cameraInteractive;
    [SerializeField] private List<Transform> camInterPosList = new List<Transform>();

    [SerializeField] private Transform oceanTransform;
    private float risingOceanTime = 0.0f;

    private CancellationTokenSource tokenSource;

    private string themeName = "ThemeThird";

    private void Start()
    {
        themeName = "ThemeThird";
        SceneController.GetInstance.CurSceneName = themeName;

        GameManager.GetInstance.SpawnPlayer();
        GameManager.GetInstance.CameraTheme = this.cameraMain;
        GameManager.GetInstance.CameraInteractive = this.cameraInteractive;
        GameManager.GetInstance.PlayerCameraStack(this.cameraMain);

        this.cameraInteractive.cullingMask = 0;
        this.cameraInteractive.enabled = false;

        GameManager.GetInstance.IsUIOpen = false;
        GameManager.GetInstance.IsInputStop = false;

        TimerManager.GetInstance.ThemeTime = 120.0f;

        risingOceanTime = 15.0f;

        if (tokenSource != null)
        {
            tokenSource.Cancel();
            tokenSource.Dispose();
        }
        tokenSource = new CancellationTokenSource();
    }

    private void CmaInteractiveSet(Transform transform, bool isActive)
    {
        this.cameraInteractive.transform.position = transform.position;
        this.cameraInteractive.transform.rotation = transform.rotation;

        if (isActive)
        {
            this.cameraInteractive.cullingMask = -1;
            cameraInteractive.depth = 1;
            this.cameraInteractive.enabled = true;
            GameManager.GetInstance.PlayerCameraControl(false);
        }
        else
        {
            this.cameraInteractive.cullingMask = 0;
            cameraInteractive.depth = 0;
            GameManager.GetInstance.PlayerCameraControl(true);
            this.cameraInteractive.enabled = false;
        }
    }
    

    public void TeleportationStart()
    {
        CameraAnimation().Forget();
        RisingOcean().Forget();
    }

    private async UniTaskVoid CameraAnimation()
    {
        CmaInteractiveSet(camInterPosList[0], true);
        cameraInteractive.transform.DOMove(new Vector3(0, 150.0f, 0), 3.0f);
        cameraInteractive.DOShakePosition(3.0f, 7.0f);
        await UniTask.Delay(TimeSpan.FromSeconds(3.0f), cancellationToken: tokenSource.Token);
        cameraInteractive.transform.position = new Vector3(0, 0, 0);
        cameraInteractive.transform.rotation = Quaternion.Euler(0, 0, 0);
        await UniTask.Delay(TimeSpan.FromSeconds(0.2f), cancellationToken: tokenSource.Token);
        CmaInteractiveSet(camInterPosList[1], true);
        await UniTask.Delay(TimeSpan.FromSeconds(1.0f), cancellationToken: tokenSource.Token);
        CmaInteractiveSet(camInterPosList[1], false);
        oceanTransform.DOMoveY(1.0f, risingOceanTime, false);
    }

    private async UniTaskVoid RisingOcean()
    {
        await UniTask.Delay(TimeSpan.FromSeconds(risingOceanTime), cancellationToken: tokenSource.Token);
        themeThirdViewer.GoToChaseMap();
    }
}
