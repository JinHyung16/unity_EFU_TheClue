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
    [SerializeField] private Animator cameraInteractiveAnimator;
    [SerializeField] private Transform camInterPos;

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

        risingOceanTime = 12.0f;

        if (tokenSource != null)
        {
            tokenSource.Cancel();
            tokenSource.Dispose();
        }
        tokenSource = new CancellationTokenSource();
    }

    private void OnDisable()
    {
        tokenSource.Cancel();
    }

    private void OnDestroy()
    {
        tokenSource.Cancel();
        tokenSource.Dispose();
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
        cameraInteractive.transform.DOShakePosition(1.5f, 7.0f, fadeOut: true);
        CmaInteractiveSet(camInterPos, true);
        await UniTask.Delay(TimeSpan.FromSeconds(2.0f), cancellationToken: tokenSource.Token);
        cameraInteractiveAnimator.SetTrigger("onCamAnim");
        await UniTask.Delay(TimeSpan.FromSeconds(2.2f), cancellationToken: tokenSource.Token);
        CmaInteractiveSet(camInterPos, false);
    }

    private async UniTaskVoid RisingOcean()
    {
        await UniTask.Delay(TimeSpan.FromSeconds(risingOceanTime), cancellationToken: tokenSource.Token);
        themeThirdViewer.GoToChaseMap();
    }
}
