using Cysharp.Threading.Tasks;
using DG.Tweening;
using HughGenerics;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;

public class ThemeSecondPresenter : PresenterSingleton<ThemeSecondPresenter>
{
    [SerializeField] private ThemeSecondViewer themeSecondViewer;

    [Header("Camera들")]
    [SerializeField] private Camera cameraMain;
    [SerializeField] private Camera cameraInteractive;

    [Header("Interactive Camera가 움직일 위치 List")]
    //0==doorkey, 1==showcanse, 2==npc(빈 벽)
    [SerializeField] private List<Transform> interactiveCamMovePosList = new List<Transform>();
    [SerializeField] private Transform doorKeyPutInDoorCamPos;

    [SerializeField] private Transform keyPutInPos; //열쇠 넣기 진행시 시작 위치

    [Header("Switch SpotLight")]
    [SerializeField] private Light switchSpotLight;

    [Header("ShowCaseTop Transform")]
    [SerializeField] private Transform showcaseTopTransform;

    [Header("WristWatches")]
    [SerializeField] private List<WristWatch> wristWatches = new List<WristWatch>();

    //0==상호작용 X, 1==문과 상호작용, 2==showcase와 상호작용, 3==npc와 상호작용해서 노트 획득, 4==note 오픈중
    public int IsInteractiveNum { get; private set; } = 0;

    //NPC와 대화한게 처음인지 아닌지
    public bool IsNPCFirstTalk { get; set; } = false;

    private int numOfDoorLockAttempsCnt = 0; //도어락 시도횟수
    private string doorLockSuccessCode = "8282"; //도어락 히든 비번
    private bool isDoorKeyPutIn = false;

    private CancellationTokenSource tokenSource;

    private string themeName = "ThemeSecond";
    protected override void OnAwake()
    {
        switchSpotLight.color = Color.white;
        switchSpotLight.enabled = true;
    }

    private void Start()
    {
        themeName = "ThemeSecond";
        SceneController.GetInstance.CurSceneName = themeName;

        GameManager.GetInstance.SpawnPlayer();
        GameManager.GetInstance.CameraTheme = this.cameraMain;
        GameManager.GetInstance.CameraInteractive = this.cameraInteractive;
        GameManager.GetInstance.PlayerCameraStack(this.cameraMain);

        this.cameraInteractive.cullingMask = 0;
        this.cameraInteractive.enabled = false;

        GameManager.GetInstance.IsUIOpen = false;
        GameManager.GetInstance.IsInputStop = false;

        TimerManager.GetInstance.ThemeTime = 900.0f;

        numOfDoorLockAttempsCnt = 0;

        if (tokenSource != null)
        {
            tokenSource.Dispose();
        }
        tokenSource = new CancellationTokenSource();
    }

    public void OpenDoorLockUI()
    {
        themeSecondViewer.DoorLockCanvasOpen();
    }

    public void DoneDoorLock(string code)
    {
        themeSecondViewer.CloseCanvas();

        if (code.Equals(doorLockSuccessCode))
        {
            GameClear(true);
        }
        else
        {
            numOfDoorLockAttempsCnt++;
            if (3 <= numOfDoorLockAttempsCnt)
            {
                numOfDoorLockAttempsCnt = 0;
                GameClear(false);
            }
        }
    }

    #region Interactive Camera Functions
    private void CmaInteractiveSet(Transform transform, bool isActive)
    {
        cameraInteractive.transform.position = transform.position;
        cameraInteractive.transform.rotation = transform.rotation;

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

    /// <summary>
    /// Switch On or Off를 통해 불빛 켜고 끄기
    /// </summary>
    public void SwitchOnOff()
    {
        if (switchSpotLight.enabled)
        {
            switchSpotLight.enabled = false;
        }
        else
        {
            switchSpotLight.enabled = true;
        }
    }


    public void ObjectSyncToDoorKeyHole()
    {
        var obj = InventoryManager.GetInstance.GetInvenObject();
        if (obj != null)
        {
            obj.transform.position = cameraInteractive.transform.position + new Vector3(0, 0, 0.6f);
            obj.transform.LookAt(cameraInteractive.transform);
            obj.SetActive(true);
        }
    }

    /// <summary>
    /// 문과 상호작용 중, 최종적으로 오브젝트를 문에 넣을때 호출하는 함수
    /// </summary>
    /// <param name="obj"> 문에 꽂으려는 오브젝트 </param>
    public void PutInTheDoor(GameObject obj)
    {
        if (tokenSource != null)
        {
            tokenSource.Cancel();
            tokenSource.Dispose();
        }
        tokenSource = new CancellationTokenSource();

        CmaInteractiveSet(doorKeyPutInDoorCamPos, true);
        if (obj.CompareTag("DoorKey"))
        {
            obj.transform.position = keyPutInPos.position;
            obj.transform.rotation = keyPutInPos.rotation;
            obj.transform.DOMove(keyPutInPos.position + new Vector3(0, 0, 0.2f), 3.0f, false).SetEase(Ease.Linear);
            isDoorKeyPutIn = true;
            DoorKeyAnimationDone().Forget();
        }
        else
        {
            obj.transform.position = keyPutInPos.position;
            obj.transform.DOMove(keyPutInPos.position + new Vector3(0, 0, 0.2f), 3.0f, false).SetEase(Ease.Linear);
            isDoorKeyPutIn = false;
            DoorKeyAnimationDone().Forget();
        }
    }

    private async UniTaskVoid DoorKeyAnimationDone()
    {
        await UniTask.Delay(TimeSpan.FromSeconds(1.5f), cancellationToken: tokenSource.Token);
        CmaInteractiveSet(doorKeyPutInDoorCamPos, false);
        themeSecondViewer.CloseCanvas();
        if (isDoorKeyPutIn)
        {
            GameClear(true);
        }
        else
        {
            GameClear(false);
        }
        tokenSource.Cancel();
    }

    public void DoorKeyHoleInteractive(bool active)
    {
        CmaInteractiveSet(interactiveCamMovePosList[0], true);
        GameObject obj = InventoryManager.GetInstance.GetInvenObject();
        if (active)
        {
            GameManager.GetInstance.Player.transform.position += new Vector3(0, 0, -2.0f);
            IsInteractiveNum = 1;
            if (obj != null)
            {
                obj.transform.position = cameraInteractive.transform.position + new Vector3(0, 0, 1.0f);
                obj.transform.LookAt(cameraInteractive.transform);
                obj.SetActive(true);
            }
            themeSecondViewer.InteractiveDoorCanvas();
        }
        else
        {
            IsInteractiveNum = 0;
            if (obj != null)
            {
                obj.SetActive(false);
            }
            CmaInteractiveSet(interactiveCamMovePosList[0], false);
            themeSecondViewer.CloseCanvas();
        }
    }

    public void ShowCaseInteractive(bool active)
    {
        if (active)
        {
            GameManager.GetInstance.Player.transform.position += new Vector3(-1.0f, 0, 0);
            GameManager.GetInstance.IsUIOpen = true;
            IsInteractiveNum = 2;
            CmaInteractiveSet(interactiveCamMovePosList[1], true);
            showcaseTopTransform.DOMove(showcaseTopTransform.position + new Vector3(0, 0, -3.3f), 0.8f, false);
            themeSecondViewer.InteractiveShowcanseCanvasOpen();
        }
        else
        {
            GameManager.GetInstance.IsUIOpen = false;
            IsInteractiveNum = 0;
            for (int i = 0; i < wristWatches.Count; i++)
            {
                wristWatches[i].PutDownWristWatch();
            }
            showcaseTopTransform.DOMove(showcaseTopTransform.position + new Vector3(0, 0, 3.3f), 0.8f, false);
            CmaInteractiveSet(interactiveCamMovePosList[1], false);
            themeSecondViewer.CloseCanvas();
        }
    }

    /// <summary>
    /// NPC와 첫 대화시 active가 true로 호출된다.
    /// </summary>
    /// <param name="active">호출 상태를 받는다</param>
    public void NPCInteractiveSelectNote(bool active)
    {
        if (active)
        {
            GameManager.GetInstance.Player.transform.position += new Vector3(0, 0, -1.0f);
            GameManager.GetInstance.IsUIOpen = true;
            IsInteractiveNum = 3;
            CmaInteractiveSet(interactiveCamMovePosList[2], true);
            NPCNoteSelectManager.GetInstance.NoteVisibleToSelect();
            themeSecondViewer.NPCSelectNoteCanvasOpen();
        }
        else
        {
            GameManager.GetInstance.IsUIOpen = false;
            IsInteractiveNum = 0;
            CmaInteractiveSet(interactiveCamMovePosList[2], false);
            NPCNoteSelectManager.GetInstance.NoteInvisible();
            themeSecondViewer.CloseCanvas();
        }
    }
    #endregion

    /// <summary>
    /// NPC와 상호작용시 현재 미션을 볼 수 있다.
    /// </summary>
    public void NPCInteractiveShowMission()
    {
        GameManager.GetInstance.IsUIOpen = false;
        themeSecondViewer.NPCMissionCanvasOpen();
    }

    public void NoteSelectInInven(GameObject obj, bool isOpen)
    {
        if (isOpen)
        {
            var note = obj.GetComponent<Note>();
            themeSecondViewer.NoteCanvaseOpen();
            NoteManager.GetInstance.NotePanelOpen(note.noteIndex);
            IsInteractiveNum = 4;
        }
        else
        {
            NoteManager.GetInstance.NotePanelClose();
            IsInteractiveNum = 0;
            themeSecondViewer.CloseCanvas();
        }
    }
    public void DoorKeyInventory(GameObject obj)
    {
        var doorKey = obj.GetComponent<DoorKey>();
        InventoryManager.GetInstance.PutInInventory(obj, doorKey.GetDoorKeyUISprite, UnityEngine.Color.white); ;
    }

    public void GameClear(bool isClear)
    {
        themeSecondViewer.CloseCanvas();

        if (isClear)
        {
            GameManager.GetInstance.IsGameClear = true;
        }
        else
        {
            GameManager.GetInstance.IsGameClear = false;
        }

        themeSecondViewer.OpenResultCanvas(isClear);
    }
}
