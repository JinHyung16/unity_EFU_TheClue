using HughEnumData;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cube : InteractiveObject
{
    [Header("Cube의 원래 위치")]
    [SerializeField] private Transform cubeTransform;

    [Header("DiceData ScriptableObject")]
    [SerializeField] private DiceData diceData;

    [Header("Cube의 UI sprite")]
    [SerializeField] private Sprite cubeUISprite;

    private Vector3 offset = Vector3.zero;

    public Sprite GetCubeUISprite { get { return this.cubeUISprite; } }
    private Sprite[] cubeSpriteArray;

    private void OnEnable()
    {
        this.gameObject.transform.position = cubeTransform.position;
    }
    private void OnDisable()
    {
        this.gameObject.transform.position = Vector3.zero;
        InteracitveOrNot(false);
    }
    private void Start()
    {
        offset = new Vector3(0, 0.8f, 0);
        cubeSpriteArray = diceData.patternSpriteArray;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            GameManager.GetInstance.VisibleInteractiveCanvas(cubeTransform, offset);
            InteracitveOrNot(true);
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
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
        this.myInteractiveType = InteractiveType.ThemeFirst_Cube;
        return this.myInteractiveType;
    }

    public Sprite GetCubeSprite(int index)
    {
        return this.cubeSpriteArray[index];
    }
}
