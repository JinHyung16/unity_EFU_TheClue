using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HughEnumData;

public class Dice : InteractiveObject
{
    [SerializeField] private Transform diceTransform;
    [SerializeField] private Sprite dicePatternSprite;
    [SerializeField] private Color dicePatternColor;

    private Vector3 offset = Vector3.zero;

    public Sprite GetDicePatternSprite { get { return this.dicePatternSprite; } }
    public Color GetDicePatternColor { get { return this.dicePatternColor; } }
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
            InteractiveManager.GetInstance.SetInventoryObject(this.gameObject, true);
        }
        else
        {
            InteractiveManager.GetInstance.SetInteractiveObject(this, false);
            InteractiveManager.GetInstance.SetInventoryObject(this.gameObject, false); ;
        }
    }

    public override InteractiveType GetInteractiveType()
    {
        this.myInteractiveType = InteractiveType.ThemeFirst_Dice;
        return this.myInteractiveType;
    }
}
