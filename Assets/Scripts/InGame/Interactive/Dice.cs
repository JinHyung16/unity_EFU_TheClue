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

    private void OnEnable()
    {
        this.gameObject.transform.position = diceTransform.position;
    }
    private void OnDisable()
    {
        this.gameObject.transform.position = Vector3.zero;
        InteracitveOrNot(false);
    }
    private void Start()
    {
        offset = new Vector3(0, 0.8f, 0);
        dicePatternSpriteArray = diceData.patternSpriteArray;
        dicePatternNameArray = diceData.patternNameArray;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            GameManager.GetInstance.VisibleInteractiveCanvas(diceTransform, offset);
            InteracitveOrNot(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            GameManager.GetInstance.InvisibleInteractiveCanvas();
            InteracitveOrNot(false);
        }
    }

    public override void InteracitveOrNot(bool interactive)
    {
        if (interactive)
        {
            InteractiveManager.GetInstance.SetInteractiveObject(this, true);
            InteractiveManager.GetInstance.SetInventoryObject(this.gameObject);
        }
        else
        {
            InteractiveManager.GetInstance.SetInteractiveObject(this, false);
            InteractiveManager.GetInstance.SetInventoryObject(null);
        }
    }

    public override InteractiveType GetInteractiveType()
    {
        this.myInteractiveType = InteractiveType.ThemeFirst_Dice;
        return this.myInteractiveType;
    }

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
