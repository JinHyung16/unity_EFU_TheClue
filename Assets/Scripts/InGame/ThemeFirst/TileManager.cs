using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TileManager : MonoBehaviour
{
    #region static
    public static TileManager GetInstance;
    private void Awake()
    {
        GetInstance = this;
    }
    #endregion

    [SerializeField] private ThemeFirstViewer themeFirstViewer;

    [Header("Tile Pattern Image")]
    [SerializeField] private Image tilePatternImg;
    [Header("Dice Pattern Image")]
    [SerializeField] private Image dicePatternImg;

    [Header("Tile Pattern들을 담을 리스트")]
    [SerializeField] private List<Tile> tilePatterns = new List<Tile>();

    private Color curTileColor;
    private string curTilePatternName;
    private bool curTileIsEscapeKey = false;
    private GameObject diceObject = null; //layer가 OverUI인 object 받을 변수
    private GameObject patternObject = null;

    private Dice diceScript = null;

    private int curDicePatternIndex = 0; //현재 dice의 패턴을 보여줄 순서

    public void VisibleTilePattern(GameObject obj)
    {
        patternObject = obj;

        var tilepattern = obj.GetComponent<Tile>();
        tilePatternImg.sprite = tilepattern.TilePatternSprite;
        curTilePatternName = tilepattern.TilePatternName;
        curTileIsEscapeKey = tilepattern.IsEscapeKey;
        curTileColor = tilepattern.TileColor;
    }

    /// <summary>
    /// Tile과 상호작용키를 통해 Tile Canvas를 연 상태에서
    /// 주사위의 값을 전달받는 함수.
    /// </summary>
    /// <param name="obj"></param>
    public void SetDiceOnTileCanvas(GameObject obj)
    {
        if (obj == null) { return; }

        if (diceObject != null)
        {
            diceObject.SetActive(false);
            diceObject = null;
            diceScript = null;
        }
        diceObject = obj;
        diceObject.SetActive(false);
        diceScript = diceObject.GetComponent<Dice>();
        curDicePatternIndex = 0;
        dicePatternImg.sprite = diceScript.GetDicePattern(curDicePatternIndex);
    }

    /// <summary>
    /// Tile Canvas가 오픈되어있지 않을 때, 플레이어가 인벤틀 클릭하여
    /// TilePattern에서 사용할 object를 꺼내려할때, 데이터 값만 이동시켜준다.
    /// 
    /// Tile Canvas가 오픈되어 있을 때, 플레이어가 인벤을 클릭하여 가져온
    /// 해당 오브젝트로 변경시킨다.
    /// </summary>
    public void SetDiceSync()
    {
        GameObject dice = InventoryManager.GetInstance.GetObjectInventory();
        SetDiceOnTileCanvas(dice);
    }

    /// <summary>
    /// SelectButton을 눌렀을 때 호출
    /// </summary>
    public void SelectDiceInTilePattern()
    {
        if (diceObject == null) { return; }

        if (curTileIsEscapeKey && (curTilePatternName == diceScript.DicePatternName) && 
            (curTileColor == diceScript.GetDicePatternColor))
        {
            foreach (var curPattern in tilePatterns)
            {
                if (curPattern.TilePatternName == curTilePatternName)
                {
                    curPattern.IsDone = true;
                }
            }
            diceObject.SetActive(true);
            //dice object를 tile위에 올려두기
            diceObject.transform.position = patternObject.transform.position + new Vector3(0, 0.8f, 0);
            ThemeFirstPresenter.GetInstance.DicePutOnTileCheck(true);
            InventoryManager.GetInstance.DicePutOnTile();
            diceObject = null;
            patternObject = null;
            diceScript = null;
            dicePatternImg.sprite = null;
            themeFirstViewer.CloseCanvas();
        }
        else
        {
            ThemeFirstPresenter.GetInstance.DicePutOnTileCheck(false);
            themeFirstViewer.CloseCanvas();
        }
    }


    /// <summary>
    /// Dice Rotation Button을 눌렀을 때 실행
    /// </summary>
    public void RotationDice()
    {
        if(diceScript == null) { return; }

        dicePatternImg.sprite = diceScript.GetDicePattern(curDicePatternIndex);
        diceScript.SetCurDicePatternName(curDicePatternIndex);
        curDicePatternIndex++;
        if (5 < curDicePatternIndex)
        {
            curDicePatternIndex = 0;
        }
    }
}
