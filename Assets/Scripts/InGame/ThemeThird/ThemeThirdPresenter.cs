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

    [Header("Camera 관련 데이터들")]
    [SerializeField] private Camera cameraMain;
    [SerializeField] private Camera cameraInteractive;
    [SerializeField] private Animator cameraInteractiveAnimator;
    [SerializeField] private Transform camInterPos;

    [Header("Region Button 관련 데이터들")]
    [SerializeField] private GameObject regionKeys;
    [SerializeField] private GameObject regionCallNpc;

    [Header("Enemies 데이터들")]
    [SerializeField] private Professor enemyPrisonOfficer;
    [SerializeField] private GradStudent enemyGradStudent;

    public Transform IsCallingPositions { get { return this.regionCallNpc.transform; } }
    public bool IsCallEnemy { get; set; } = false;

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

        if (tokenSource != null)
        {
            tokenSource.Cancel();
            tokenSource.Dispose();
        }
        tokenSource = new CancellationTokenSource();


        CamInteractiveSet(cameraInteractive.transform, true);
        themeThirdViewer.DialogueStart();

        //CameraAnimation().Forget();
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

    public void DoneDialogue()
    {
        CamInteractiveSet(cameraInteractive.transform, false);
    }
    private void CamInteractiveSet(Transform transform, bool isActive)
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

    private async UniTaskVoid CameraAnimation()
    {
        CamInteractiveSet(camInterPos, true);
        cameraInteractiveAnimator.SetTrigger("onCamAnim");
        await UniTask.Delay(TimeSpan.FromSeconds(8.0f), cancellationToken: tokenSource.Token);
        CamInteractiveSet(camInterPos, false);
    }


    public void DropTheKeyByButton()
    {
        string text = "주위에 무언가 떨어진 소리가 들렸다!!!";
        themeThirdViewer.NarrativeCanvas(text);
    }

    public void CallNPCByButton()
    {
        IsCallEnemy = true;
        string text = "누가 다가오는거 같다!!!";
        themeThirdViewer.NarrativeCanvas(text);
    }
}
