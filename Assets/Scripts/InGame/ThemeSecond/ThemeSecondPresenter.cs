using DG.Tweening;
using HughGenerics;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThemeSecondPresenter : PresenterSingleton<ThemeSecondPresenter>
{
    [SerializeField] private ThemeSecondViewer themeSecondViewer;

    [SerializeField] private Camera cameraMain;

    [Header("Camera와 Interactive Transfomr들 관련")]
    [SerializeField] private Camera cameraInteractive;

    [Header("Interactive Camera가 움직일 위치 List")]
    //0==doorkey, 1==showcanse, 2==npc(빈 벽)
    [SerializeField] private List<Transform> interactiveCamMovePosList = new List<Transform>();

    [Header("Switch SpotLight")]
    [SerializeField] private Light switchSpotLight;


    //0==상호작용 X, 1==문과 상호작용, 2==showcase와 상호작용, 3==npc와 상호작용해서 노트 획득
    public int IsInteractiveNum { get; private set; } = 0;

    //NPC와 대화한게 처음인지 아닌지
    public bool IsNPCFirstTalk { get; set; } = false;

    private int numOfDoorLockAttempsCnt = 0;

    private string doorLockSuccessCode = "8282";

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

        this.cameraMain.cullingMask = 0;
        this.cameraInteractive.cullingMask = 0;

        GameManager.GetInstance.IsUIOpen = false;
        GameManager.GetInstance.IsInputStop = false;

        TimerManager.GetInstance.ThemeTime = 900.0f;

        numOfDoorLockAttempsCnt = 0;
    }

    public void OpenDoorLockUI()
    {
        themeSecondViewer.DoorLockCanvasOpen();
    }

    public void DoneDoorLock(string code)
    {
        if (code == doorLockSuccessCode)
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
            GameManager.GetInstance.PlayerCameraControl(false);
        }
        else
        {
            this.cameraInteractive.cullingMask = 0;
            cameraInteractive.depth = 0;
            GameManager.GetInstance.PlayerCameraControl(true);
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
        GameObject obj = InventoryManager.GetInstance.GetInvenObject();
        if (obj != null)
        {
            obj.transform.position = cameraInteractive.transform.position + new Vector3(0, 0, 1.0f);
            obj.transform.LookAt(cameraInteractive.transform);
            obj.SetActive(true);
        }
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
            themeSecondViewer.InteractiveCanvasOpen();
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
            themeSecondViewer.InteractiveCanvasOpen();
        }
        else
        {
            GameManager.GetInstance.IsUIOpen = false;
            IsInteractiveNum = 0;
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
            NoteManager.GetInstance.NoteVisibleToSelect();
            themeSecondViewer.NPCSelectNoteCanvasOpen();
        }
        else
        {
            GameManager.GetInstance.IsUIOpen = false;
            IsInteractiveNum = 0;
            CmaInteractiveSet(interactiveCamMovePosList[2], false);
            NoteManager.GetInstance.NoteInvisible();
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

    public void NoteSelectInInven(GameObject obj)
    {
        var note = obj.GetComponent<Note>();
        themeSecondViewer.NoteCanvasOpen(note.noteIndex);
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
            GameResultOpen(false);
        }
    }

    public void GameResultOpen(bool isClear)
    {
        themeSecondViewer.OpenResultCanvas(isClear);
    }
}
