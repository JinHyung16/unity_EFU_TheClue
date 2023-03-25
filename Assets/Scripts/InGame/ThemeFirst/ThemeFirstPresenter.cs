using Cysharp.Threading.Tasks;
using HughGenerics;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Threading;
using UnityEngine;
using UnityEngine.UIElements;

public class ThemeFirstPresenter : PresenterSingleton<ThemeFirstPresenter>
{
    [Header("Theme Frist의 있는 카메라")]
    [SerializeField] private Camera mainCamera;
    public Camera GetMainCamera { get { return mainCamera; } }

    [SerializeField] private ThemeFirstViewer themeFirstViewer;

    [Header("Switch SpotLight")]
    [SerializeField] private Light switchSpotLight;
    [SerializeField] private List<UnityEngine.Color> colorList;

    [Header("서로 다른 6가지 문양의 Tile")]
    [SerializeField] private List<Tile> tilePatternList;

    [Header("타일 배치할 위치 9곳 list")]
    [SerializeField] private List<Transform> tileSpawnPosList;
    List<int> notEscapeTilePosList = new List<int>(); //탈출 키인 3가지 타일 배치한 위치를 제외한 나머지 구역

    private string doorLockKeyPad;
    private int switchOnCount; //switch와 상호작용 한 횟수 저장

    //Game Clear 또는 Fail이 되는 조건을 저장한 변수
    private int numOfTileAttemptsCnt = 0; //타일 퍼즐 풀기 시도 횟수
    private int numOfDoorLockAttempsCnt = 0; //도어락 퍼즐 풀기 시도 횟수

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
            tile.SetActiveTile(false);
        }


        SetTileRandom();
    }

    private void OnDestroy()
    {
        tokenSource.Cancel();
        tokenSource.Dispose();
    }

    #region DoorLock
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

    public void DoneDoorLock()
    {
        numOfDoorLockAttempsCnt++;
        if (3 <= numOfDoorLockAttempsCnt)
        {
            themeFirstViewer.OpenResultCanvas(false);
            numOfDoorLockAttempsCnt = 0;
        }
    }
    #endregion

    #region Switch
    public void SwitchOffAndAutoOn()
    {
        if (tokenSource != null)
        {
            tokenSource.Dispose();
        }
        tokenSource = new CancellationTokenSource();
        switchSpotLight.enabled = false;

        if (colorList.Count <= switchOnCount)
        {
            switchOnCount = 1;
        }
        AutoSwitchChange().Forget();
    }

    private async UniTaskVoid AutoSwitchChange()
    {
        //5초 뒤 빛 켜지게 (R -> G -> B 순서로)
        await UniTask.Delay(TimeSpan.FromSeconds(5.0f), cancellationToken: tokenSource.Token);
        AutoTileColorChange(colorList[switchOnCount]);
        switchSpotLight.color = colorList[switchOnCount];
        switchOnCount++;
        switchSpotLight.enabled = true;
        await UniTask.Delay(TimeSpan.FromSeconds(7.0f), cancellationToken: tokenSource.Token);
        switchSpotLight.enabled = false;
        switchSpotLight.color = colorList[0];
        AutoTileColorChange(colorList[0]);
        AutoTilePosChange();
        await UniTask.Delay(TimeSpan.FromSeconds(2.0f), cancellationToken: tokenSource.Token);
        switchSpotLight.enabled = true;
        tokenSource.Cancel();
    }
    #endregion

    #region Dice And Tile
    /// <summary>
    /// 스위치 불빛에 맞춰 
    /// </summary>
    private void SetTileRandom()
    {
        List<int> tileRandomList = new List<int>(); //탈출 키가 될 3가지 타일 선택하여 담을 리스트
        List<int> tileRandomPos = new List<int>(); //탈출 키인 3가지 타일의 위치를 선택하여 담을 리스트
        for (int t = 0; t < 9; t++)
        {
            notEscapeTilePosList.Add(t);
        }

        //탈출 키인 3가지 타일 랜덤 뽑기
        while (true)
        {
            int tileRandomSelect = UnityEngine.Random.Range(0, 5);
            if (!tileRandomList.Contains(tileRandomSelect))
            {
                tilePatternList[tileRandomSelect].IsEscapeKey = true;
                tileRandomList.Add(tileRandomSelect);
            }
            if (tileRandomList.Count == 3)
            {
                break;
            }
        }

        //탈출 키인 3가지 타일 위치 배정 -> 뽑은 값은 제거하면서
        while (true)
        {
            int randomPos = UnityEngine.Random.Range(0, 8);
            if (!tileRandomPos.Contains(randomPos))
            {
                tileRandomPos.Add(randomPos);
                notEscapeTilePosList.RemoveAt(randomPos);
            }
            if (tileRandomPos.Count == 3)
            {
                break;
            }
        }

        int i = 0;
        int j = 0;
        foreach (var tile in tilePatternList)
        {
            if (tile.IsEscapeKey)
            {
                tile.InitialTileSetting(tileSpawnPosList[tileRandomPos[i]].transform, colorList[(i + 1)]);
                tile.transform.position = tileSpawnPosList[tileRandomPos[i]].transform.position;
                tile.SetActiveTile(true);
                i++;
            }
            else
            {
                tile.InitialTileSetting(tileSpawnPosList[notEscapeTilePosList[j]].transform, colorList[0]);
                tile.transform.position = tileSpawnPosList[notEscapeTilePosList[j]].transform.position ;
                tile.SetActiveTile(true);
                j++;
            }
        }
    }

    private void AutoTileColorChange(UnityEngine.Color color)
    {
        foreach (var tile in tilePatternList)
        {
            tile.ChangeEmissionColor(color);
        }
    }
    private void AutoTilePosChange()
    {
        foreach (var tile in tilePatternList)
        {
            if (!tile.IsEscapeKey)
            {
                tile.SetActiveTile(false);
            }
        }

        //기존에 탈출용 타일을 배치한 곳을 제외한 위치를 저장한 리스트를 셔플한다.
        int rand1, rand2;
        int temp;
        for (int i = 0; i < notEscapeTilePosList.Count; i++)
        {
            rand1 = UnityEngine.Random.Range(0, notEscapeTilePosList.Count);
            rand2 = UnityEngine.Random.Range(0, notEscapeTilePosList.Count);
            temp = notEscapeTilePosList[rand1];
            notEscapeTilePosList[rand1] = notEscapeTilePosList[rand2];
            notEscapeTilePosList[rand2] = temp;

        }

        int j = 0;
        foreach (var tile in tilePatternList)
        {
            if (!tile.IsEscapeKey)
            {
                tile.SetActiveTile(false);
                tile.ChangeTilePosition(tileSpawnPosList[notEscapeTilePosList[j]]);
                j++;
            }
        }
    }

    /// <summary>
    /// 주사위를 인벤토리에 넣기
    /// </summary>
    /// <param name="obj"> 인벤토리에 들어갈 오브젝트 </param>
    public void DicePutInInveotry(GameObject obj)
    {
        var dice = obj.GetComponent<Dice>();
        InventoryManager.GetInstance.PutInInventory(obj, dice.GetDicePatternUISprite, dice.GetDicePatternColor);
    }

    public void TileInteractiveOpen(GameObject tile)
    {
        GameObject dice = InventoryManager.GetInstance.GetObjectInventory();
        themeFirstViewer.TilePatternCanvasOpen(dice, tile);
    }
    /// <summary>
    /// Tile위 같은 문양 찾기를 진행하여
    /// 최종적으로 문양 넣기를 버튼을 클릭하여 결과를 봤을때
    /// </summary>
    /// <param name="isDone">문양 배치 완료한 상태</param>
    public void DicePutOnTileCheck(bool isDone)
    {
        if (isDone)
        {
            numOfTileAttemptsCnt++;
        }
        else
        {
            numOfTileAttemptsCnt--;
        }

        switch (numOfTileAttemptsCnt)
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
    #endregion
}
