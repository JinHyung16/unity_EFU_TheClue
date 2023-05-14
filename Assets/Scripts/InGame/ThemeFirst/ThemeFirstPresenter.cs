using Cysharp.Threading.Tasks;
using HughGenerics;
using System;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class ThemeFirstPresenter : PresenterSingleton<ThemeFirstPresenter>
{
    [SerializeField] private ThemeFirstViewer themeFirstViewer;

    [Header("Camera Data 바인딩")]
    [SerializeField] private Camera cameraMain;
    [SerializeField] private Camera cameraInteractive;

    [Header("Global Light")]
    [SerializeField] private Light globalLight;

    [Header("Switch SpotLight")]
    [SerializeField] private Light switchSpotLight;
    [SerializeField] private List<UnityEngine.Color> colorList;

    [Header("서로 다른 6가지 문양의 Tile")]
    [SerializeField] private TileData tileData;

    [Header("타일 배치할 위치 9곳 list")]
    [SerializeField] private List<Transform> tileSpawnPosList;

    private List<GameObject> tileObjectList; //생성한 9개 타일 오브젝트 담아두기.
    private List<int> remainTileSpawnList; //탈출 키인 3가지 타일 배치한 위치를 제외한 나머지 구역

    public Camera GetMainCamera { get { return cameraMain; } }

    private int switchLightIndex; //switch의 불빛을 가져올 배열의 index값


    //Game Clear 또는 Fail이 되는 조건을 저장한 변수
    private int failedDiceSetTile = 0;
    private int successDiceSetTile = 0; //타일 올바르게 놓은 주사위 개수
    private int numOfDoorLockAttempsCnt = 0; //도어락 퍼즐 풀기 시도 횟수

    // 현재 상호작용 한 오브젝트 타입에 따라 door와 상호작용시 문구 다르게 보이게 하기
    // 0=상호작용 클리어 못함, 1=탈출조건 클리어함
    public int InteractiveTypeToDoor { get; private set; } = 0;

    //UniTask 관련
    private CancellationTokenSource tokenSource;

    private bool IsSwitchOn = false;

    //Game끝낼 때 GameProgress에 넘겨줄 현재 테마 이름 정보로 각 ThemePresenter들이 이 정보를 갖고 있는다.
    private string themeName = "ThemeFirst";

    protected override void OnAwake()
    {
        switchSpotLight.enabled = true;
        switchLightIndex = 1;
        successDiceSetTile = 0;
        failedDiceSetTile = 2;

        if (tokenSource != null)
        {
            tokenSource.Dispose();
        }
        tokenSource = new CancellationTokenSource();
    }

    private void Start()
    {
        themeName = "ThemeFirst";
        SceneController.GetInstance.CurSceneName = themeName;

        GameManager.GetInstance.SpawnPlayer();
        GameManager.GetInstance.CameraTheme = this.cameraMain;
        GameManager.GetInstance.CameraInteractive = this.cameraInteractive;
        GameManager.GetInstance.PlayerCameraStack(this.cameraMain);

        TimerManager.GetInstance.ThemeClearTime = 600.0f;
        GameManager.GetInstance.IsUIOpen = false;
        GameManager.GetInstance.IsInputStop = false;

        this.cameraInteractive.cullingMask = 0;
        this.cameraInteractive.enabled = false;

        globalLight.color = new Color(0.7f, 0.7f, 0.7f);

        SetTileRandom();
        
        CamInteractiveSet(cameraInteractive.transform, true);
        themeFirstViewer.DialogueStart();
    }

    private void OnDisable()
    {
        tokenSource.Cancel();
        tokenSource.Dispose();

        tileObjectList.Clear();
        remainTileSpawnList.Clear();

        if (GameManager.GetInstance != null)
        {
            GameManager.GetInstance.IsUIOpen = false;
            GameManager.GetInstance.IsInputStop = false;
        }
    }
    private void CamInteractiveSet(Transform transform, bool isActive)
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

    public void DoneDialogue()
    {
        CamInteractiveSet(cameraInteractive.transform, false);
    }

    #region DoorLock
    public void OpenDoorLockUI()
    {
        themeFirstViewer.OpenDoorLock();
    }

    public void DoneDoorLock()
    {
        themeFirstViewer.CloseCanvas();

        numOfDoorLockAttempsCnt++;
        if (3 <= numOfDoorLockAttempsCnt)
        {
            numOfDoorLockAttempsCnt = 0;
            themeFirstViewer.OpenResultCanvas(false);
        }

        string context = "좋지 않은 예감이 든다...";
        themeFirstViewer.NarrativeCanvase(context);
    }

    #endregion

    #region Switch
    public void SwitchOffAndAutoOn()
    {
        if (tokenSource != null)
        {
            tokenSource.Cancel();
            tokenSource.Dispose();
        }
        tokenSource = new CancellationTokenSource();

        if (!IsSwitchOn)
        {
            if (colorList.Count <= switchLightIndex)
            {
                switchLightIndex = 1;
            }
            switchSpotLight.enabled = false;
            globalLight.color = new Color(0, 0, 0);
            AutoSwitchChange().Forget();
        }
        else
        {
            AutoTileColorChange(UnityEngine.Color.black);
            switchSpotLight.color = colorList[0];
            globalLight.color = new Color(0.7f, 0.7f, 0.7f);
            switchSpotLight.enabled = true;
            IsSwitchOn = false;
        }
    }

    private async UniTaskVoid AutoSwitchChange()
    {
        IsSwitchOn = true;
        while (true)
        {
            switchSpotLight.color = colorList[switchLightIndex];
            switchSpotLight.enabled = true;
            AutoTileColorChange(colorList[switchLightIndex]);
            switchLightIndex++;
            //5초 뒤 빛 R -> Y -> B 순서로
            await UniTask.Delay(TimeSpan.FromSeconds(5.0f), cancellationToken: tokenSource.Token);
            AutoTilePosChange();
            switchSpotLight.enabled = false;
            AutoTileColorChange(UnityEngine.Color.black);
            await UniTask.Delay(TimeSpan.FromSeconds(2.0f), cancellationToken: tokenSource.Token);
            if (colorList.Count == switchLightIndex)
            {
                tokenSource.Cancel();
                break; 
            }
        }
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

        //나머지 escape key가 아닌 tile도 tileObjectList에 추가한다.
        for (int i = 0; i < 3; i++)
        {
            var tile = Instantiate(tileData.tileObjArray[tileRandomList[i]]);
            tile.SetActive(false);
            tile.name = tileData.tileObjArray[tileRandomList[i]].name;
            tile.GetComponent<Tile>().IsEscapeKey = false;
            tileObjectList.Add(tile);
        }

        //9개 타일중 6개의 모든 문양이 1번씩 선택됐으니, 나머지 3개에 대해선 랜덤으로 선택한다.
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
        for(int i = 0; i < tileObjectList.Count; i++)
        {
            if (tileObjectList[i].GetComponent<Tile>().IsEscapeKey)
            {
                tileObjectList[i].GetComponent<Tile>().InitialTileSetting(tileSpawnPosList[tileRandomPos[index1]].transform, 
                    colorList[(index1 + 1)]);
                tileObjectList[i].transform.position = tileSpawnPosList[tileRandomPos[index1]].position;
                tileObjectList[i].SetActive(true);
                index1++;
            }
            else
            {
                tileObjectList[i].GetComponent<Tile>().InitialTileSetting(tileSpawnPosList[remainTileSpawnList[index2]].transform, 
                    UnityEngine.Color.black);
                tileObjectList[i].transform.position = tileSpawnPosList[remainTileSpawnList[index2]].position;
                tileObjectList[i].SetActive(true);
                index2++;
            }

        }
    }

    private void AutoTileColorChange(UnityEngine.Color color)
    {
        for(int i =0; i < tileObjectList.Count; i++)
        {
            if (color == UnityEngine.Color.white)
            {
                tileObjectList[i].GetComponent<Tile>().ChangeEmissionColor(UnityEngine.Color.black);
            }
            else
            {
                tileObjectList[i].GetComponent<Tile>().ChangeEmissionColor(color);
            }
        }
    }
    private void AutoTilePosChange()
    {
        for (int i = 0; i < tileObjectList.Count; i++)
        {
            tileObjectList[i].SetActive(false);
        }

        List<int> suffleList = remainTileSpawnList;
        //기존에 탈출용 타일을 배치한 곳을 제외한 위치를 저장한 리스트를 셔플한다.
        int rand;
        int temp;
        for (int i = 0; i < remainTileSpawnList.Count; i++)
        {
            rand = UnityEngine.Random.Range(0, i + 1);
            temp = suffleList[i];
            suffleList[i] = suffleList[rand];
            suffleList[rand] = temp;
        }
        remainTileSpawnList = suffleList;

        int j = 0;
        for(int i = 0; i < tileObjectList.Count; i++)
        {
            if (!tileObjectList[i].GetComponent<Tile>().IsEscapeKey)
            {
                tileObjectList[i].GetComponent<Tile>().ChangeTilePosition(tileSpawnPosList[remainTileSpawnList[j]]);
                tileObjectList[i].transform.position = tileSpawnPosList[remainTileSpawnList[j]].position;
                j++;
            }
        }

        for (int i = 0; i < tileObjectList.Count; i++)
        {
            tileObjectList[i].SetActive(true);
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
        themeFirstViewer.TilePatternCanvasOpen(tile);
    }

    /// <summary>
    /// Tile위 같은 문양 찾기를 진행하여
    /// 최종적으로 문양 넣기를 버튼을 클릭하여 결과를 봤을때
    /// </summary>
    /// <param name="isDone">문양 배치 완료한 상태</param>
    public void DicePutOnTileCheck(bool isDone)
    {
        themeFirstViewer.CloseCanvas();

        if (isDone == true)
        {
            successDiceSetTile++;
            if (3 <= successDiceSetTile)
            {
                InteractiveTypeToDoor = 1;
                successDiceSetTile = 0;
                GameManager.GetInstance.IsGameClear = true;
            }
            string context = "타일에 배치됐다. 무언가 열리는 소리가 들렸다.";
            themeFirstViewer.NarrativeCanvase(context);
        }
        if(isDone == false)
        {
            failedDiceSetTile--;
            string context = "타일에 배치가 잘못된 거 같다... 무슨 소리지?";
            themeFirstViewer.NarrativeCanvase(context);
            if (failedDiceSetTile <= 0)
            {
                themeFirstViewer.OpenResultCanvas(false);
            }
        }
    }
    #endregion


    public void CubePutInInveotry(GameObject obj)
    {
        var cube = obj.GetComponent<Cube>();
        InventoryManager.GetInstance.PutInInventory(obj, cube.GetCubeUISprite, UnityEngine.Color.white); ;
    }

    public void DoorOpen()
    {
        string context = " ";
        if (GameManager.GetInstance.IsGameClear)
        {
            context = "문을 열었습니다! 어서 탈출하세요.";
            themeFirstViewer.NarrativeCanvase(context);
        }
        else
        {
            context = "문을 닫혀있습니다. 빨리 문을 여세요.";
            themeFirstViewer.NarrativeCanvase(context);
        }
    }
    /// <summary>
    /// NPC와 상호작용시 현재 미션을 볼 수 있다.
    /// </summary>
    public void NPCInteractiveShowMission()
    {
        GameManager.GetInstance.IsUIOpen = false;
        themeFirstViewer.NPCMissionCanvasOpen();
    }

    public void GameClear(bool isClear)
    {
        if (isClear)
        {
            GameManager.GetInstance.IsGameClear = true;
        }
        else
        {
            GameManager.GetInstance.IsGameClear = false;
        }
        themeFirstViewer.OpenResultCanvas(isClear);
    }
}
