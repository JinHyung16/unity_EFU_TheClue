using HughGenerics;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using System;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;

public class GameManager : Singleton<GameManager>, IDisposable
{
    [Header("Player Prefab")]
    [SerializeField] private GameObject playerPrefab;
    private GameObject player;

    [Header("Game Option Canvas")]
    [SerializeField] private Canvas gameOptionCanvas;
    [SerializeField] private Button gameExitBtn;

    private KeyCode optionKeyCode = KeyCode.Escape;
    private bool isOptionKeyDown;

    public bool IsGamePause { get; private set; }
    public bool IsEndTheme { private get; set; }

    private void Start()
    {
        gameOptionCanvas.enabled = false;
        optionKeyCode = KeyCode.Escape;
        isOptionKeyDown = false;
        IsGamePause = false;

        gameExitBtn.onClick.AddListener(ExitGameAndSaveDataAsync);
    }

    private void Update()
    {
        OptionCanvasOpen();
    }

    private void OptionCanvasOpen()
    {
        if (Input.GetKeyDown(optionKeyCode))
        {
            if (!isOptionKeyDown)
            {
                Time.timeScale = 0;
                IsGamePause = true;

                gameOptionCanvas.enabled = true;
                isOptionKeyDown = true;
            }
            else
            {
                Time.timeScale = 1;
                IsGamePause = false;

                gameOptionCanvas.enabled = false;
                isOptionKeyDown = false;
            }
        }
    }

    /// <summary>
    /// 게임을 진행하다 그만하려고 나가고자 Exit Button을 누르면 호출
    /// 자동으로 게임 진행상황을 저장해준다.
    /// </summary>
    private void ExitGameAndSaveDataAsync()
    {
        Dispose();
        DataManager.GetInstance.SaveData(SceneController.GetInstance.CurSceneName);
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
        }
        player.SetActive(true);
        player.GetComponent<PlayerManager>().PlayerSetUp();
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

    #region Game 진행 플로우 관련 함수
    /// <summary>
    /// Main Scene에서 게임을 다시 시작할 경우 불러온다.
    /// </summary>
    public void StartNewGame()
    {
        if (!DataManager.GetInstance.GetUserLoginRecord())
        {
            //이전에 게임한 기록이 없다면 기록 해주기
            DataManager.GetInstance.SetUserLoginRecord();
        }
        SceneController.GetInstance.LoadScenario().Forget();
    }

    /// <summary>
    /// 이전의 기록된 게임을 불러와 시작한ㄷ.
    /// </summary>
    public void StartSavedGame()
    {
        var loadScene = DataManager.GetInstance.LoadData();
        SceneController.GetInstance.LoadScene(loadScene);
    }

    /// <summary>
    /// 게임을 Clear하지 못했을 경우, 현재 씬을 다시 불러와 처음부터 시작한다.
    /// </summary>
    public void FailedGameAndRestart()
    {
        DespawnPlayer();
        SceneController.GetInstance.LoadScene(SceneController.GetInstance.CurSceneName);
    }

    /// <summary>
    /// Scene을 Clear하면 다음 씬으로 보낸다.
    /// 이때, 마지막 Scene이면 Main으로 간다.
    /// </summary>
    /// <param name="nextScene">이동할 다음 Scene 이름</param>
    public void CelarGame()
    {
        if (IsEndTheme)
        {
            Dispose();
        }
        else
        {
            DespawnPlayer();
        }
        SceneController.GetInstance.LoadScene("Main");
    }
    #endregion
}
