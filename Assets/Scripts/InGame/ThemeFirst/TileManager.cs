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
    [Header("인벤에 있는 아이템의 Pattern Image")]
    [SerializeField] private Image objPatternImage;

    [Header("Tile Pattern들을 담을 리스트")]
    [SerializeField] private List<Tile> tilePatterns = new List<Tile>();

    private Color curTileColor;
    private string curTilePatternName;
    private bool curTileIsEscapeKey = false;
    private GameObject invenObj = null;
    private GameObject patternObject = null; //해당 pattern의 object

    private Dice diceScript = null;
    private Cube cubeScript = null;

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
        if (obj != null)
        {
            if (invenObj != null)
            {
                cubeScript = null;
                diceScript = null;
                invenObj.SetActive(false);
                invenObj = null;
            }

            invenObj = obj;
            invenObj.SetActive(false);
            curDicePatternIndex = 0;
            var name = invenObj.name.Substring(0, 4);
            if (name == "Dice")
            {
                diceScript = invenObj.GetComponent<Dice>();
                objPatternImage.sprite = diceScript.GetDicePattern(curDicePatternIndex);
                cubeScript = null;
            }
            if (name == "Cube")
            {
                cubeScript = invenObj.GetComponent<Cube>();
                objPatternImage.sprite = cubeScript.GetCubeSprite(curDicePatternIndex);
                diceScript = null;
            }
        }
    }

    /// <summary>
    /// Tile Canvas가 오픈되어있지 않을 때, 플레이어가 인벤틀 클릭하여
    /// TilePattern에서 사용할 object를 꺼내려할때, 데이터 값만 이동시켜준다.
    /// 
    /// Tile Canvas가 오픈되어 있을 때, 플레이어가 인벤을 클릭하여 가져온
    /// 해당 오브젝트로 변경시킨다.
    /// </summary>
    public void SetInventorySync()
    {
        GameObject invenObj = InventoryManager.GetInstance.GetObjectInventory();
        SetDiceOnTileCanvas(invenObj);
    }

    /// <summary>
    /// SelectButton을 눌렀을 때 호출
    /// </summary>
    public void SelectDiceInTilePattern()
    {
        if (invenObj == null) { return; }

        if (diceScript != null && curTileIsEscapeKey && (curTilePatternName == diceScript.DicePatternName) && 
            (curTileColor == diceScript.GetDicePatternColor))
        {
            foreach (var curPattern in tilePatterns)
            {
                if (curPattern.TilePatternName == curTilePatternName)
                {
                    curPattern.IsSetDice = true;
                }
            }
            invenObj.SetActive(true);
            //invenObj object를 tile위에 올려두기
            invenObj.transform.position = patternObject.transform.position + new Vector3(0, 0.8f, 0);
            ThemeFirstPresenter.GetInstance.DicePutOnTileCheck(true);
            InventoryManager.GetInstance.InvenObjectPutOnTile();
            invenObj = null;
            patternObject = null;
            diceScript = null;
            objPatternImage.sprite = null;
            themeFirstViewer.CloseCanvas();
        }
        else
        {
            themeFirstViewer.CloseCanvas();
            ThemeFirstPresenter.GetInstance.DicePutOnTileCheck(false);
        }
    }


    /// <summary>
    /// Dice Rotation Button을 눌렀을 때 실행
    /// </summary>
    public void RotationDice()
    {
        curDicePatternIndex++;
        if (5 < curDicePatternIndex)
        {
            curDicePatternIndex = 0;
        }

        if (diceScript != null)
        {
            objPatternImage.sprite = diceScript.GetDicePattern(curDicePatternIndex);
            diceScript.SetCurDicePatternName(curDicePatternIndex);
        }
        else if (cubeScript != null)
        {
            objPatternImage.sprite = cubeScript.GetCubeSprite(curDicePatternIndex);
        }
    }
}
