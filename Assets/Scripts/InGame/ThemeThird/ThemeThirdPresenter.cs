using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.CompilerServices;
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
    //[SerializeField] private Animator cameraInterAnim;
    [SerializeField] private List<Transform> camAnimPosList = new List<Transform>();

    [Header("Enemy Positions 관련 데이터들")]
    [SerializeField] private List<Transform> regionTransList = new List<Transform>();
    [SerializeField] private List<Transform> enemyAnimTransList = new List<Transform>();

    [Header("Enemies 데이터들")]
    [SerializeField] private GameObject enemyProfessorObj;
    [SerializeField] private GameObject enemyGradStudentObj;
    [SerializeField] private Professor enemyProfessor;
    [SerializeField] private GradStudent enemyGradStudent;

    [Header("Prision Escape Keys")]
    [SerializeField] private List<GameObject> escapeKeyRegion01List = new List<GameObject>();
    [SerializeField] private GameObject escapeKeyRegion02;

    [Header("MiddleDoor Data")]
    [SerializeField] private GameObject middleDoorObj;
    [SerializeField] private Transform middleDoorOpenTrans;
    [SerializeField] private Transform middleDoorCloseTrans;

    public List<Transform> RegionTargetTransList { get { return this.regionTransList; } }

    public bool IsCallEnemyAnimation { get; private set; } = false;
    public bool IsOpenMiddleDoor { get; private set; } = false;
    private bool isCallRegion02Anim = false;
    private bool isDropKeyRegion01 = false;
    private GameObject invenObj;

    private CancellationTokenSource tokenSource;

    private void Start()
    {
        DataManager.GetInstance.SaveThemeIndex = 3;

        GameManager.GetInstance.SpawnPlayer();
        GameManager.GetInstance.CameraTheme = this.cameraMain;
        GameManager.GetInstance.CameraInteractive = this.cameraInteractive;
        GameManager.GetInstance.PlayerCameraStack(this.cameraMain);

        this.cameraInteractive.enabled = false;

        GameManager.GetInstance.IsUIOpen = false;
        GameManager.GetInstance.IsInputStop = false;
        GameManager.GetInstance.IsGameClear = false;

        TimerManager.GetInstance.ThemeClearTime = 900.0f;
        IsCallEnemyAnimation = false;
        IsOpenMiddleDoor  = false;
        isCallRegion02Anim = false;
        isDropKeyRegion01 = false;

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
        tokenSource.Dispose();
    }

    public void DoneDialogue()
    {
        CamInteractiveSet(camAnimPosList[0].transform, false);
        GameManager.GetInstance.CursorSet(false);
    }

    private void CamInteractiveSet(Transform transform, bool isActive)
    {
        this.cameraInteractive.transform.position = transform.position;
        this.cameraInteractive.transform.rotation = transform.rotation;

        if (isActive)
        {
            this.cameraInteractive.depth = 1;
            this.cameraInteractive.enabled = true;
            GameManager.GetInstance.PlayerCameraControl(false);
        }
        else
        {
            this.cameraInteractive.depth = 0;
            GameManager.GetInstance.PlayerCameraControl(true);
            this.cameraInteractive.enabled = false;
        }
    }


    public void EscapeKeyPutInInventory(GameObject obj)
    {
        var escapeKey = obj.GetComponent<EscapeKey>();
        InventoryManager.GetInstance.PutInInventory(obj, escapeKey.GetEscapeKeyUISprite, UnityEngine.Color.white); ;
    }

    public void EscapeKeySelect()
    {
        invenObj = InventoryManager.GetInstance.GetInvenObject();
    }

    public void HeadStoneNarrative()
    {
        string context = "대학생이라는 죄로\n 캠퍼스라는 교도소에서\n 강의실이라는 감옥에 갇혀...";
        NarrativeSet(context);
    }

    public void EnemyHitToPlayer(int hitCount)
    {
        string context = (4 - hitCount).ToString() + "대 더 맞으면 정신을 잃을 거 같다.";
        NarrativeSet(context);
    }
    /// <summary>
    /// 최종 나가는 문을 열 때 호출
    /// </summary>
    public void DoorOpen()
    {
        if (invenObj != null)
        {
            if (invenObj.GetComponent<EscapeKey>() != null && invenObj.GetComponent<EscapeKey>().escapeKeyName == "key06")
            {
                string context = "문이 열렸다";
                NarrativeSet(context);
                InventoryManager.GetInstance.InvenObjectUse();
                GameClear(true);
            }
            else
            {
                string context = "다른 열쇠가 필요하다! 시간이 없어 서둘러!!";
                NarrativeSet(context);
            }
        }
        else
        {
            string context = "문을 열 만한걸 찾아보자!! 서둘러!!!";
            NarrativeSet(context);
        }
    }

    /// <summary>
    /// 중간문 열 때 호출
    /// </summary>
    public void MiddleDoorOpen()
    {
        if (invenObj != null)
        {
            if (invenObj.GetComponent<EscapeKey>() != null && invenObj.GetComponent<EscapeKey>().escapeKeyName == "key03")
            {
                string context = "문이 열렸다";
                NarrativeSet(context);
                IsOpenMiddleDoor = true;
                InventoryManager.GetInstance.InvenObjectUse();
                middleDoorObj.transform.DOMove(middleDoorOpenTrans.position, 0.8f);
            }
            else
            {
                string context = "열쇠가 잘못된 거 같다.";
                NarrativeSet(context);
                IsOpenMiddleDoor = false;
            }
        }
        else
        {
            string context = "문을 열 만한걸 찾아보자";
            NarrativeSet(context);
            IsOpenMiddleDoor = false;
        }
    }

    public void DropTheKeyByButton()
    {
        if (!isDropKeyRegion01)
        {
            string context = "주위에 무언가 떨어진 소리가 들렸다!!";
            NarrativeSet(context);
            AudioManager.GetInstance.PlaySFX(AudioManager.SFX.ThemeThird_DropKey);
            for (int i = 0; i < escapeKeyRegion01List.Count; i++)
            {
                escapeKeyRegion01List[i].SetActive(true);
            }
            isDropKeyRegion01 = true;
        }
        else
        {
            string context = "...?";
            NarrativeSet(context);
        }
    }

    /// <summary>
    /// Region Second에서 교수 캐릭터 먼저 옮겨서 
    /// </summary>
    public void DropTheKeyByNPC()
    {
        AudioManager.GetInstance.PlaySFX(AudioManager.SFX.ThemeThird_RegionButton);
        if (isCallRegion02Anim)
        {
            string context = "탁자 위에 뭔가 놓고 간 거 같다. 확인해보자!!";
            NarrativeSet(context);
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

            enemyGradStudent.ChangeState(GradStudentIdleState.GetInstance);

            string context = "누가 다가오는 소리가 들린다. 문 앞을 조심하자";
            NarrativeSet(context);

            //Interactive Camera로 전환시키고
            CamInteractiveSet(camAnimPosList[0], true);

            //조교 캐릭터의 state를 idle로 바꾸고 시작
            enemyProfessor.PlayAnimation(0);
            enemyProfessorObj.transform.position = new Vector3(enemyAnimTransList[0].position.x, enemyProfessor.transform.position.y, enemyAnimTransList[0].position.z);
            enemyProfessorObj.transform.rotation = enemyAnimTransList[0].rotation;
            enemyProfessorObj.SetActive(true);
            DropKeyByProfessor().Forget();
        }
    }

    private async UniTaskVoid DropKeyByProfessor()
    {
        //귀신 캐릭터를 교수님 뒤로 이동시킨다.
        enemyGradStudentObj.transform.position = new Vector3(enemyAnimTransList[2].position.x, enemyGradStudentObj.transform.position.y, enemyAnimTransList[2].position.z);
        enemyGradStudentObj.transform.rotation = enemyAnimTransList[2].rotation;
        enemyGradStudentObj.SetActive(true);
        AudioManager.GetInstance.PlaySFX(AudioManager.SFX.ThemeThird_GradStudent_Tired);

        string context = "조교: 교수님~~~~~";
        NarrativeSet(context);
        enemyProfessor.PlayAnimation(2);

        //교수님이 뒤를 돌아보고 놀란다.
        await enemyProfessorObj.transform.DORotate(new Vector3(0, 90, 0), 0.8f).WithCancellation(tokenSource.Token);
        enemyProfessor.PlayAnimation(0);
        escapeKeyRegion02.SetActive(true);
        await enemyProfessorObj.transform.DORotate(new Vector3(0, -180, 0), 0.8f).WithCancellation(tokenSource.Token);
        CamInteractiveSet(camAnimPosList[1], true);
        //await UniTask.Delay(TimeSpan.FromSeconds(0.5f), cancellationToken: tokenSource.Token);

        //교수님의 놀라는 애니메이션이 끝나면 교수님이 도망간다.
        enemyProfessor.PlayAnimation(1);
        await enemyProfessorObj.transform.DOMoveZ(-4.0f, 2.0f).WithCancellation(tokenSource.Token);
        enemyProfessor.PlayAnimation(0);

        //중간 철창 닫는 위치로 이동시키고 철창 닫는 애니메이션 재생
        middleDoorObj.transform.position = middleDoorCloseTrans.position;
        enemyProfessorObj.transform.position = new Vector3(enemyAnimTransList[1].position.x, enemyProfessor.transform.position.y, enemyAnimTransList[1].position.z);
        enemyProfessorObj.transform.rotation = enemyAnimTransList[1].rotation;
        enemyProfessor.PlayAnimation(3);
        //철창 닫는 애니메이션이 끝났다면, 교수님이 맵에서 나가는 듯하게 사라진다.
        CamInteractiveSet(camAnimPosList[2], true);
        enemyProfessor.PlayAnimation(0);
        await enemyProfessorObj.transform.DORotate(new Vector3(0, 90, 0), 0.8f);

        await UniTask.Delay(TimeSpan.FromSeconds(1.3f), cancellationToken: tokenSource.Token);
        enemyProfessor.PlayAnimation(1);

        await enemyProfessorObj.transform.DOMoveX(4.0f, 2.0f);
        enemyProfessor.PlayAnimation(0);
        enemyProfessorObj.SetActive(false);
        IsCallEnemyAnimation = false;
        CamInteractiveSet(camAnimPosList[2], false);
        enemyGradStudent.ChangeState(GradStudentIdleState.GetInstance);
        tokenSource.Cancel();
    }

    public void CallNPCByButton(int regionNum)
    {
        string context = "누군가 다가오는거 같다!!!";
        NarrativeSet(context);
        AudioManager.GetInstance.PlaySFX(AudioManager.SFX.ThemeThird_RegionButton);
        enemyGradStudent.OnChaseTarget = regionNum;
        enemyGradStudent.ChangeState(GradStudentMoveState.GetInstance);
    }

    public void NarrativeSet(string text)
    {
        themeThirdViewer.NarrativeCanvas(text);
    }

    public void GameClear(bool isClear)
    {
        themeThirdViewer.OpenResultCanvas(isClear);
        GameManager.GetInstance.IsGameClear = true;
    }
}
