using HughGenerics;
using UnityEngine;
using UnityEngine.UI;
using System;
using UnityEngine.Rendering.Universal;
using HughPathFinding;
using UnityEditor;

public class GameManager : Singleton<GameManager>, IDisposable
{
    [Header("GameOption Canvas")]
    [SerializeField] private Canvas gameOptionCanvas;
    [SerializeField] private Button gameExitBtn;
    [SerializeField] private Button mainBtn;
    [SerializeField] private Button helpBtn;

    [Header("Panel Under the GameOption Canvas")]
    [SerializeField] private GameObject helpPanel;

    [Header("Interactive Canvas")]
    [SerializeField] private Canvas interactiveCanvs;
    [SerializeField] private Transform interactiveTrans;

    [Header("Player Prefab")]
    [SerializeField] private GameObject playerPrefab;
    private GameObject player;
    private PlayerManager playerManager;

    [SerializeField] private Texture2D cursorSprite;

    private bool isOptionKeyDown;
    public bool IsDialogueStart { get; set; } = false; //대화 시스템이 실행중인지 확인한다.
    public bool IsUIOpen { get; set; } = false; //게임에서 퍼즐을 풀기위한 UI가 열려있을 경우 true
    public bool IsInputStop { get; set; } = false; //게임중 esc키를 눌러 option을 누를경우 true
    public bool IsEndTheme { private get; set; } = false; //현재 테마가 마지막 테마이면 true
    public bool IsGameClear { get; set; } = false; //테마별로 현재 테마를 클리어했으면 true
    public bool IsFirstGame { get; set; } = false;

    public Camera CameraTheme { get; set; } = null; //테마별 카메라
    public Camera CameraInteractive { get; set; } = null; //interactive 전용 카메라

    public GameObject Player { get { return this.player; } }

    protected override void OnAwake()
    {
        IsFirstGame = true;
    }

    private void Start()
    {
        gameOptionCanvas.enabled = false;
        interactiveCanvs.enabled = false;

        isOptionKeyDown = false;
        IsInputStop = false;

        gameExitBtn.onClick.AddListener(QuitGameAndSaveData);
        mainBtn.onClick.AddListener(GoToMain);
        helpBtn.onClick.AddListener(HelpPanelOpen);

        helpPanel.SetActive(false);
        CursorSet(true);
    }

    /// <summary>
    /// UI창을 열었을 때 마우스 커서를 보여준다.
    /// </summary>
    /// <param name="isCursorVisible"> 마우스 커서를 보여줄지 말지 결정할 파라메터 </param>
    public void CursorSet(bool isCursorVisible)
    {
        if (isCursorVisible)
        {
            Cursor.SetCursor(cursorSprite, Vector2.zero, CursorMode.Auto);
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        else
        {
            Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }

    /// <summary>
    /// ThemeFirst의 진입하면 Player를 생성시킨다.
    /// 최초 1회만 생성시킨다.
    /// </summary>
    public void SpawnPlayer()
    {
        if (player != null)
        {
            Dispose();
        }

        player = Instantiate(playerPrefab);
        playerManager = player.GetComponent<PlayerManager>();
        player.SetActive(true);

        if (Time.timeScale == 0)
        {
            Time.timeScale = 1;
        }

        if (PathManager.GetInstance != null)
        {
            PathManager.GetInstance.SetTargetPosition(player.transform);
        }
    }

    /// <summary>
    /// 테마를 진행하다 게임을 끌 때 호출
    /// 테마를 다 클리어했을때 호출한다.
    /// </summary>
    public void Dispose()
    {
        Destroy(player);
        GC.SuppressFinalize(player);
    }

    public void PlayerCameraStack(Camera cam)
    {
        playerManager.SetParentCamera(cam.transform);
        playerManager.PlayerCamera().GetUniversalAdditionalCameraData().cameraStack.Add(cam);
    }

    public void PlayerCameraControl(bool active)
    {
        playerManager.PlayerCamera().enabled = active;
    }
    
    #region Option Canvas and Interactive Canvas Control
    /// <summary>
    /// Player가 esc버튼을 누르면 OptionCanvas를 연다
    /// </summary>
    /// <param name="isOption"> esc버튼 눌렀는지 안눌렀는지 </param>
    public void OptionCanvasOpen(bool isOption)
    {
        if (isOption)
        {
            if (!isOptionKeyDown)
            {
                gameOptionCanvas.enabled = true;
                isOptionKeyDown = true;
                IsInputStop = true;

                Time.timeScale = 0;
                CursorSet(true);
            }
            else
            {
                helpPanel.SetActive(false);
                gameOptionCanvas.enabled = false;
                isOptionKeyDown = false;
                IsInputStop = false;

                Time.timeScale = 1;
               CursorSet(false);
            }
        }
    }


    /// <summary>
    /// 게임을 진행하다 그만하려고 나가고자 Exit Button을 누르면 호출
    /// 자동으로 게임 진행상황을 저장해준다.
    /// </summary>
    private void QuitGameAndSaveData()
    {
        AudioManager.GetInstance.PlaySFX(AudioManager.SFX.UIClick);

        gameOptionCanvas.enabled = false;
        interactiveCanvs.enabled = false;

        isOptionKeyDown = false;
        IsInputStop = false;
        IsUIOpen = false;


        Dispose();

        ProgramQuit();
    }

    public void GoToMain()
    {
        AudioManager.GetInstance.PlaySFX(AudioManager.SFX.UIClick);
        gameOptionCanvas.enabled = false;
        interactiveCanvs.enabled = false;

        isOptionKeyDown = false;
        IsInputStop = false;
        IsUIOpen = false;

        Dispose();

        SceneController.GetInstance.LoadScene("Main");
    }

    public void HelpPanelOpen()
    {
        AudioManager.GetInstance.PlaySFX(AudioManager.SFX.UIClick);
        helpPanel.SetActive(true);
    }

    public void ClosePanel()
    {
        AudioManager.GetInstance.PlaySFX(AudioManager.SFX.UIClick);
        helpPanel.SetActive(false);
    }

    public void VisibleInteractiveCanvas(Transform target, Vector3 offset, bool isGround = false)
    {
        interactiveCanvs.renderMode = RenderMode.WorldSpace;
        if (interactiveCanvs.worldCamera == null)
        {
            interactiveCanvs.worldCamera = CameraTheme;
        }

        interactiveCanvs.enabled = true;

        interactiveTrans.position = target.position + offset;
        if (!isGround)
        {
            interactiveTrans.rotation = Quaternion.Euler(0, 0, 0);
        }
        else
        {
            interactiveTrans.rotation = Quaternion.Euler(90, 0, 0);
        }

        //interactiveTrans.LookAt(interactiveTrans.position + playerManager.PlayerCamera().transform.rotation * Vector3.back, playerManager.PlayerCamera().transform.rotation * Vector3.up);
        interactiveTrans.LookAt(interactiveTrans.position + playerManager.PlayerCamera().transform.rotation * Vector3.forward, playerManager.PlayerCamera().transform.rotation * Vector3.up);
    }

    public void InvisibleInteractiveCanvas()
    {
        interactiveCanvs.enabled = false;
    }
    #endregion

    /// <summary>
    /// 게임 클리어 후 메인화면으로 갈 떄 호출
    /// </summary>
    public void GameDataClear()
    {
        Dispose();
    }

    public void ProgramQuit()
    {
        //테스트 버전에선 무조건 3개다 열어두기
        AudioManager.GetInstance.PlaySFX(AudioManager.SFX.UIClick);
#if UNITY_EDITOR
        EditorApplication.isPlaying = false;
#else
    Application.Quit();
#endif
    }

    private void OnApplicationQuit()
    {
        IsFirstGame = false;
        DataManager.GetInstance.SaveData(DataManager.GetInstance.SaveThemeIndex);
    }
}
