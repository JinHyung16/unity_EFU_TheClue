using Cysharp.Threading.Tasks;
using HughGenerics;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Threading;
using Unity.Mathematics;
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
    [SerializeField] private TileData tileData;
    [SerializeField] private List<GameObject> tileObjectList; //생성한 9개 타일 오브젝트 담아두기.

    [Header("타일 배치할 위치 9곳 list")]
    [SerializeField] private List<Transform> tileSpawnPosList;
    [SerializeField] List<int> remainTileSpawnList; //탈출 키인 3가지 타일 배치한 위치를 제외한 나머지 구역

    private string doorLockKeyPad;
    private int switchOnCount = 1; //switch와 상호작용 한 횟수 저장

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
        switchSpotLight.color = colorList[switchOnCount];
        switchSpotLight.enabled = true;
        AutoTileColorChange(colorList[switchOnCount]);
        switchOnCount++;
        await UniTask.Delay(TimeSpan.FromSeconds(7.0f), cancellationToken: tokenSource.Token);
        switchSpotLight.enabled = false;
        AutoTilePosChange();
        switchSpotLight.color = colorList[0];
        AutoTileColorChange(colorList[0]);
        await UniTask.Delay(TimeSpan.FromSeconds(1.0f), cancellationToken: tokenSource.Token);
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
        remainTileSpawnList = new List<int>() { 0, 1, 2, 3, 4, 5, 6, 7, 8 };
        tileObjectList = new List<GameObject>();
        
        List<int> tileRandomList = new List<int>() { 0, 1, 2, 3, 4, 5 };
        List<int> tileRandomPos = new List<int>();

        //타일 랜덤으로 3개 선택
        for (int i = 0; i < 3; i++)
        {
            int random = UnityEngine.Random.Range(0, tileRandomList.Count);
            var tile = Instantiate(tileData.tileObjArray[random]);
            tile.name = tileData.tileObjArray[tileRandomList[random]].name;
            tile.GetComponent<Tile>().IsEscapeKey = true;
            tile.SetActive(false);
            tileObjectList.Add(tile);
            tileRandomList.RemoveAt(random);
        }

        //EscapeKey인 타일을 고정적으로 배열할 위치찾기
        //추가로 remainTileSpawnList에서 남은값들의 위치에는 계속 타일들이 바뀌어 위치된다.
        for (int i = 0; i < 3; i++)
        {
            int randomPos = UnityEngine.Random.Range(0, remainTileSpawnList.Count);
            tileRandomPos.Add(remainTileSpawnList[randomPos]);
            remainTileSpawnList.RemoveAt(randomPos);
        }

        foreach (var list in remainTileSpawnList)
        {
            Debug.Log("남은 위치: " + list);
        }

        //나머지 escape key가 아닌 tile도 tileObjectList에 추가한다.
        for (int i = 0; i < 3; i++)
        {
            var tile = Instantiate(tileData.tileObjArray[tileRandomList[i]]);
            tile.SetActive(false);
            tile.name = tileData.tileObjArray[tileRandomList[i]].name;
            tileObjectList.Add(tile);
        }

        //9개 타일중 6개가 선택됐으니, 나머지 3개에 대해서 랜덤으로 만든다.
        for (int i = 0; i < 3; i++)
        {
            int random = UnityEngine.Random.Range(0, 5);
            var tile = Instantiate(tileData.tileObjArray[random]);
            tile.name = tileData.tileObjArray[random].name + i.ToString();
            tile.SetActive(false);
            tile.GetComponent<Tile>().IsEscapeKey = false;
            tileObjectList.Add(tile);
        }

        int index1 = 0;
        int index2 = 0;
        foreach (var tile in tileObjectList)
        {
            if (tile.GetComponent<Tile>().IsEscapeKey)
            {
                tile.GetComponent<Tile>().InitialTileSetting(tileSpawnPosList[tileRandomPos[index1]].transform, colorList[(index1 + 1)]);
                tile.transform.position = tileSpawnPosList[tileRandomPos[index1]].position;
                tile.SetActive(true);
                index1++;
            }
            else
            {
                tile.GetComponent<Tile>().InitialTileSetting(tileSpawnPosList[remainTileSpawnList[index2]].transform, UnityEngine.Color.black);
                tile.transform.position = tileSpawnPosList[remainTileSpawnList[index2]].position;
                tile.SetActive(true);
                index2++;
            }

        }
    }

    private void AutoTileColorChange(UnityEngine.Color color)
    {
        foreach (var tile in tileObjectList)
        {
            if (color == UnityEngine.Color.white)
            {
                tile.GetComponent<Tile>().ChangeEmissionColor(UnityEngine.Color.black);
            }
            else
            {
                tile.GetComponent<Tile>().ChangeEmissionColor(color);
            }
        }
    }
    private void AutoTilePosChange()
    {
        List<int> suffleList = remainTileSpawnList;
        //기존에 탈출용 타일을 배치한 곳을 제외한 위치를 저장한 리스트를 셔플한다.
        int rand;
        int temp;
        for (int i = 0; i < remainTileSpawnList.Count - 1; i++)
        {
            rand = UnityEngine.Random.Range(0, i + 1);
            temp = suffleList[rand];
            suffleList[i] = suffleList[rand];
            suffleList[rand] = temp;
        }
        remainTileSpawnList = suffleList;

        foreach (var tile in tileObjectList)
        {
            tile.SetActive(false);
        }

        int j = 0;
        foreach (var tile in tileObjectList)
        {
            if (!tile.GetComponent<Tile>().IsEscapeKey)
            {
                tile.GetComponent<Tile>().ChangeTilePosition(tileSpawnPosList[remainTileSpawnList[j]]);
                tile.transform.position = tileSpawnPosList[remainTileSpawnList[j]].position;
                j++;
            }
        }

        foreach (var tile in tileObjectList)
        {
            tile.SetActive(true);
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
