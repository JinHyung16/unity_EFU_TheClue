using Cysharp.Threading.Tasks;
using HughGenerics;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class ThemeFirstPresenter : PresenterSingleton<ThemeFirstPresenter>
{
    [Header("Theme Frist의 있는 카메라")]
    [SerializeField] private Camera mainCamera;
    public Camera GetMainCamera { get { return mainCamera; } }

    [SerializeField] private ThemeFirstViewer themeFirstViewer;

    [Header("Switch SpotLight")]
    [SerializeField] private Light switchSpotLight;
    [SerializeField] private List<Color> switchLightColorList;

    [Header("TilePattern List들")]
    [SerializeField] private List<TilePattern> tilePatternList;


    private string doorLockKeyPad;
    private int switchOnCount;

    private int themeFirstClearCount = 0;

    //Game끝낼 때 GameProgress에 넘겨줄 현재 테마 이름 정보로 각 ThemePresenter들이 이 정보를 갖고 있는다.
    private string themeName = "ThemeFirst";

    //UniTask 관련
    private CancellationTokenSource tokenSource;

    protected override void OnAwake()
    {
        switchSpotLight.enabled = true;
        if (tokenSource != null)
        {
            tokenSource.Dispose();
        }
        tokenSource = new CancellationTokenSource();
    }

    private void Start()
    {
        SceneController.GetInstance.CurSceneName = themeName;

        GameManager.GetInstance.SpawnPlayer();
        GameManager.GetInstance.themeCamera = this.mainCamera;

        switchOnCount = 1;

        foreach (var tile in tilePatternList)
        {
        }
    }

    private void OnDestroy()
    {
        tokenSource.Cancel();
        tokenSource.Dispose();
    }

    public void OpenDoorLockUI()
    {
        themeFirstViewer.OpenDoorLock();
    }

    public void InputDoorLockKeyPad(string key)
    {
        doorLockKeyPad += key;
        themeFirstViewer.UpdateDoorLockInput(doorLockKeyPad);
    }

    public void ClearDoorLockKeyPad()
    {
        doorLockKeyPad = "";
    }

    public void DicePutInInveotry(GameObject obj)
    {
        var dice = obj.GetComponent<Dice>();
        InventoryManager.GetInstance.PutInInventory(obj, dice.GetDicePatternUISprite, dice.GetDicePatternColor);
    }

    public void SwitchOffAndAutoOn()
    {
        if (tokenSource != null)
        {
            tokenSource.Dispose();
        }
        tokenSource = new CancellationTokenSource();
        switchSpotLight.enabled = false;

        if (switchLightColorList.Count <= switchOnCount)
        {
            switchOnCount = 1;
        }
        AutoSwitchChange().Forget();
    }

    private async UniTaskVoid AutoSwitchChange()
    {
        //5초 뒤 빛 켜지게 (R -> G -> B 순서로)
        await UniTask.Delay(TimeSpan.FromSeconds(5.0f), cancellationToken: tokenSource.Token);
        switchSpotLight.color = switchLightColorList[switchOnCount];
        switchOnCount++;
        switchSpotLight.enabled = true;
        await UniTask.Delay(TimeSpan.FromSeconds(7.0f), cancellationToken: tokenSource.Token);
        switchSpotLight.enabled = false;
        switchSpotLight.color = switchLightColorList[0];
        await UniTask.Delay(TimeSpan.FromSeconds(1.0f), cancellationToken: tokenSource.Token);
        switchSpotLight.enabled = true;
        tokenSource.Cancel();
    }

    public void TilePatternOpen(GameObject tile)
    {
        GameObject dice = InventoryManager.GetInstance.GetObjectInventory();
        themeFirstViewer.TilePatternCanvasOpen(dice, tile);
    }

    public void DicePutOnTileCheck(bool isDone)
    {
        if (isDone)
        {
            themeFirstClearCount++;
        }
        else
        {
            themeFirstClearCount--;
        }

        switch (themeFirstClearCount)
        {
            case 3:
                //UI창 나옴
                themeFirstViewer.OpenResultCanvas(true);
                break;
            case -3:
                themeFirstViewer.OpenResultCanvas(false);
                break;
        }

    }
}
