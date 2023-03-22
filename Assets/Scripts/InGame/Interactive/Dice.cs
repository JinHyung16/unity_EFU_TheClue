using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HughEnumData;

public class Dice : InteractiveObject
{
    [Header("Dice의 원래 위치")]
    [SerializeField] private Transform diceTransform;

    [Header("Dice의 pattern sprites")]
    [SerializeField] private Sprite[] dicePatternSprites;
    [Header("Dice의 색깔")]
    [SerializeField] private Color dicePatternColor;

    [Header("Dice UI sprite")]
    [SerializeField] private Sprite dicePatternSprite;

    public string DicePatternName { get; set; }

    private Vector3 offset = Vector3.zero;

    public Sprite GetDicePatternUISprite { get { return this.dicePatternSprite; } }
    public Color GetDicePatternColor { get { return this.dicePatternColor; } }

    private void OnEnable()
    {
        this.gameObject.transform.position = diceTransform.position;
    }
    private void OnDisable()
    {
        this.gameObject.transform.position = Vector3.zero;
        GameManager.GetInstance.InvisibleInteractiveCanvas();
        InteracitveOrNot(false);
    }
    private void Start()
    {
        offset = new Vector3(0, 0.8f, 0);
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

    public Sprite GetDicePattern(int index)
    {
        return this.dicePatternSprites[index];
    }
}
