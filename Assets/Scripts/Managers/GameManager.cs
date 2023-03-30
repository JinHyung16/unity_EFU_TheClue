using HughGenerics;
using UnityEngine;
using UnityEngine.UI;
using System;
using DG.Tweening;
using UnityEngine.Rendering.Universal;

public class GameManager : Singleton<GameManager>, IDisposable
{
    [Header("Game Option Canvas")]
    [SerializeField] private Canvas gameOptionCanvas;
    [SerializeField] private Button gameExitBtn;

    [Header("Interactive Canvas")]
    [SerializeField] private Canvas interactiveCanvs;
    [SerializeField] private Transform interactiveTrans;

    [Header("Player Prefab")]
    [SerializeField] private GameObject playerPrefab;
    private GameObject player;
    private PlayerManager playerManager;

    private bool isOptionKeyDown;

    public bool IsUIOpen { get; set; } //게임에서 퍼즐을 풀기위한 UI가 열려있을 경우 true
    public bool IsInputStop { get; set; } = false; //게임중 esc키를 눌러 option을 누를경우 true
    public bool IsEndTheme { private get; set; } = false; //현재 테마가 마지막 테마이면 true
    public bool IsGameClear { get; set; } = false; //테마별로 현재 테마를 클리어했으면 true

    public Camera themeCamera { get; set; } = null;
    private void Start()
    {
        gameOptionCanvas.enabled = false;
        interactiveCanvs.enabled = false;

        isOptionKeyDown = false;
        IsInputStop = false;

        gameExitBtn.onClick.AddListener(QuitGameAndSaveData);
    }


    /// <summary>
    /// 게임을 진행하다 그만하려고 나가고자 Exit Button을 누르면 호출
    /// 자동으로 게임 진행상황을 저장해준다.
    /// </summary>
    private void QuitGameAndSaveData()
    {
        int saveIndex = 0;
        Debug.Log("SceneController.GetInstance.CurSceneName");
        switch (SceneController.GetInstance.CurSceneName)
        {
            case "ThemeFirst":
                saveIndex = 1;
                break;
            case "ThemeSecond":
                saveIndex = 2;
                break;
            case "ThemeThird":
                saveIndex = 3;
                break;
        }
        Debug.Log("saveIndex: " + saveIndex);
        DataManager.GetInstance.SaveData(saveIndex);

        gameOptionCanvas.enabled = false;
        interactiveCanvs.enabled = false;

        isOptionKeyDown = false;
        IsInputStop = false;
        IsUIOpen = false;

        Dispose();
        SceneController.GetInstance.LoadScene("Main");
    }

    /// <summary>
    /// ThemeFirst의 진입하면 Player를 생성시킨다.
    /// 최초 1회만 생성시킨다.
    /// </summary>
    public void SpawnPlayer()
    {
        if (player == null)
        {
            player = Instantiate(playerPrefab, this.transform);
            playerManager = player.GetComponent<PlayerManager>();
        }
        player.SetActive(true);
    }

    /// <summary>
    /// Theme를 이동할때마다 호출한다.
    /// </summary>
    public void DespawnPlayer()
    {
        if (player != null)
        {
            player.SetActive(false);
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
            }
            else
            {
                gameOptionCanvas.enabled = false;
                isOptionKeyDown = false;
                IsInputStop = false;

                Time.timeScale = 1;
            }
        }
    }


    public void VisibleInteractiveCanvas(Transform target, Vector3 offset)
    {
        interactiveCanvs.renderMode = RenderMode.WorldSpace;
        if (interactiveCanvs.worldCamera == null)
        {
            interactiveCanvs.worldCamera = themeCamera;
        }

        interactiveCanvs.enabled = true;

        interactiveTrans.position = target.position + offset;
        //interactiveTrans.LookAt((interactiveTrans.position + playerManager.PlayerCamera().transform.rotation * Vector3.back), (playerManager.PlayerCamera().transform.rotation * Vector3.up));
    }

    public void InvisibleInteractiveCanvas()
    {
        interactiveCanvs.enabled = false;
    }
    #endregion

    /// <summary>
    /// 게임 클리어 후 메인화면으로 갈 떄 호출
    /// </summary>
    public void GameClear()
    {
        Dispose();
    }

    public void OnApplicationQuit()
    {
        int saveIndex = 0;
        switch (SceneController.GetInstance.CurSceneName)
        {
            case "ThemeFirst":
                saveIndex = 1;
                break;
            case "ThemeSecond":
                saveIndex = 2;
                break;
            case "ThemeThird":
                saveIndex = 3;
                break;
        }
        DataManager.GetInstance.SaveData(saveIndex);
    }
}
