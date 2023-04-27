using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HughEnumData;

public class Dice : InteractiveObject
{
    [Header("Dice의 원래 위치")]
    [SerializeField] private Transform diceTransform;

    [Header("DiceData ScriptableObject")]
    [SerializeField] private DiceData diceData;

    [Header("Dice의 색깔")]
    [SerializeField] private Color dicePatternColor;

    [Header("Dice UI sprite")]
    [SerializeField] private Sprite dicePatternSprite;

    private Vector3 offset = Vector3.zero;
    private Sprite[] dicePatternSpriteArray;
    private string[] dicePatternNameArray;


    public string DicePatternName { get; private set; }
    public Sprite GetDicePatternUISprite { get { return this.dicePatternSprite; } }
    public Color GetDicePatternColor { get { return this.dicePatternColor; } }


    private void OnDisable()
    {
        if (GameManager.GetInstance != null)
        {
            GameManager.GetInstance.InvisibleInteractiveCanvas();
        }
        NotInteractvie();
        this.gameObject.transform.position = diceTransform.position;
    }
    private void Start()
    {
        dicePatternSpriteArray = diceData.patternSpriteArray;
        dicePatternNameArray = diceData.patternNameArray;
        offset = new Vector3(0, 0.3f, 0);
    }
    #region InteractiveObject Override
    protected override void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            InteractiveManager.GetInstance.IsInteractive = true;
            this.Interacitve();
        }
    }
    protected override void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            InteractiveManager.GetInstance.IsInteractive = true;
            this.Interacitve();
        }
    }

    protected override void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            InteractiveManager.GetInstance.IsInteractive = false;
            this.NotInteractvie();
        }
    }

    protected override void Interacitve()
    {
        GameManager.GetInstance.VisibleInteractiveCanvas(diceTransform, offset);
        InteractiveManager.GetInstance.SetInteractiving(this);
        InteractiveManager.GetInstance.SetInteractvieObjToInventory(this.gameObject);
    }

    protected override void NotInteractvie()
    {
        GameManager.GetInstance.InvisibleInteractiveCanvas();
        InteractiveManager.GetInstance.SetInteractvieObjToInventory(null);
    }

    public override InteractiveType GetInteractiveType()
    {
        return InteractiveType.ThemeFirst_Dice;
    }

    #endregion
    /// <summary>
    /// Tile Pattern Canvas에서 현재 위로 올라온 주사위 문양
    /// </summary>
    /// <param name="index"> 현재 맨 위로 올라온 index </param>
    /// <returns> 주사위 문양 전달 </returns>
    public Sprite GetDicePattern(int index)
    {
        return this.dicePatternSpriteArray[index];
    }

    /// <summary>
    /// Tile Pattern Canvas에서 현재 위로 올라온 주사위 문양의 이름
    /// </summary>
    /// <param name="index"> 현재 맨 위로 올라온 index</param>

    public void SetCurDicePatternName(int index)
    {
        DicePatternName = this.dicePatternNameArray[index];
    }
}
