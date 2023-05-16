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
    [SerializeField] private Animator cameraInterAnim;
    [SerializeField] private Transform camInterPos;

    [Header("Enemy Positions 관련 데이터들")]
    [SerializeField] private GameObject regionKeys;
    [SerializeField] private List<Transform> regionTransList = new List<Transform>();
    [SerializeField] private List<Transform> enemyAnimTransList = new List<Transform>();

    [Header("Enemies 데이터들")]
    [SerializeField] private GameObject enemyProfessorObj;
    [SerializeField] private GameObject enemyGradStudentObj;
    [SerializeField] private Professor enemyProfessor;
    [SerializeField] private GradStudent enemyGradStudent;

    [Header("Prision Escape Keys")]
    [SerializeField] private List<GameObject> escapeKeyList = new List<GameObject>();
    public List<Transform> RegionTargetTransList { get { return this.regionTransList; } }

    public bool IsCallEnemyAnimation { get; private set; } = false;

    private bool isCallRegion02Anim = false;
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

        TimerManager.GetInstance.ThemeClearTime = 900.0f;

        if (tokenSource != null)
        {
            tokenSource.Cancel();
            tokenSource.Dispose();
        }
        tokenSource = new CancellationTokenSource();


        //CamInteractiveSet(camAnimPosList[0].transform, true);
        themeThirdViewer.DialogueStart();
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
        CamInteractiveSet(camInterPos.transform, false);
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


    public void DropTheKeyByButton()
    {
        string text = "주위에 무언가 떨어진 소리가 들렸다!!!";
        themeThirdViewer.NarrativeCanvas(text);
    }

    /// <summary>
    /// Region Second에서 교수 캐릭터 먼저 옮겨서 
    /// </summary>
    public void DropTheKeyByNPC()
    {
        if (isCallRegion02Anim)
        {
            string text = "잠잠하다...";
            themeThirdViewer.NarrativeCanvas(text);
        }
        else
        {
            if (tokenSource != null)
            {
                tokenSource.Cancel();
                tokenSource.Dispose();
            }
            tokenSource = new CancellationTokenSource();

            isCallRegion02Anim = true;
            IsCallEnemyAnimation = true;

            enemyGradStudent.ChangeState(EnemyIdleState.GetInstance);

            string text = "누가 다가오는거 같다!!!";
            themeThirdViewer.NarrativeCanvas(text);

            CamInteractiveSet(camInterPos, true);
            //조교 캐릭터의 state를 idle로 바꾸고 시작
            enemyProfessorObj.transform.position = new Vector3(enemyAnimTransList[0].position.x, enemyProfessor.transform.position.y, enemyAnimTransList[0].position.z);
            enemyProfessorObj.transform.rotation = enemyAnimTransList[0].rotation;
            enemyProfessorObj.SetActive(true);

            cameraInterAnim.SetInteger("IsCamAnim", 1);
            DropTheKeyByNPCAnimation().Forget();
        }
    }

    private async UniTaskVoid DropTheKeyByNPCAnimation()
    {
        while (true)
        {
            if (1.0f <= cameraInterAnim.GetCurrentAnimatorStateInfo(0).normalizedTime)
            {
                //귀신 캐릭터를 교수님 뒤로 이동시킨다.
                enemyGradStudentObj.transform.position = new Vector3(enemyAnimTransList[2].position.x, enemyGradStudentObj.transform.position.y, enemyAnimTransList[2].position.z);
                enemyGradStudentObj.transform.rotation = enemyAnimTransList[2].rotation;
                enemyGradStudentObj.SetActive(true);

                //교수님이 뒤를 돌아보고 놀란다.
                enemyProfessor.PlayAnimation(0);
                await enemyProfessorObj.transform.DORotate(new Vector3(0, 90, 0), 1.5f).WithCancellation(tokenSource.Token);
                enemyProfessor.PlayAnimation(2);

                Debug.Log("애니메이션 시간 지남");
            }

            //교수님이 도망간다.
            enemyProfessor.PlayAnimation(1);
            await enemyProfessorObj.transform.DOMoveZ(-4.0f, 3.0f).WithCancellation(tokenSource.Token);

            await UniTask.Delay(TimeSpan.FromSeconds(1.0f), cancellationToken: tokenSource.Token);
            cameraInterAnim.SetInteger("IsCamAnim", 0);
            CamInteractiveSet(camInterPos, false);
            enemyProfessorObj.transform.position = new Vector3(enemyAnimTransList[1].position.x, enemyProfessor.transform.position.y, enemyAnimTransList[1].position.z);
            enemyProfessorObj.transform.rotation = enemyAnimTransList[1].rotation;

            await UniTask.Delay(TimeSpan.FromSeconds(5.0f), cancellationToken: tokenSource.Token);
            CamInteractiveSet(camInterPos, true);
            enemyProfessor.PlayAnimation(3);
            /*
            if (1.0f <= enemyProfessorObj.GetComponentInChildren<Animator>().GetCurrentAnimatorStateInfo(0).normalizedTime)
            {
                tokenSource.Cancel();
                IsCallEnemyAnimation = false;
            }
            */
            await UniTask.Delay(TimeSpan.FromSeconds(8.0f), cancellationToken: tokenSource.Token);
            IsCallEnemyAnimation = false;
            enemyGradStudent.ChangeState(EnemyMoveState.GetInstance);
            CamInteractiveSet(camInterPos, false);
            tokenSource.Cancel();
            await UniTask.Yield();
        }
    }

    public void CallNPCByButton(int regionNum)
    {
        enemyGradStudent.ChangeState(EnemyIdleState.GetInstance);
        string text = "누가 다가오는거 같다!!!";
        themeThirdViewer.NarrativeCanvas(text);

        enemyGradStudent.OnChaseTarget = regionNum;
        enemyGradStudent.ChangeState(EnemyMoveState.GetInstance);
    }


}
