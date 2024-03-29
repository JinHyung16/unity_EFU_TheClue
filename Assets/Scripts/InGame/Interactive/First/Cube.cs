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

    public Sprite GetCubeUISprite { get { return this.cubeUISprite; } }
    private Sprite[] cubeSpriteArray;

    private Vector3 offset;

    private void OnDisable()
    {
        if (GameManager.GetInstance != null)
        {
            GameManager.GetInstance.InvisibleInteractiveCanvas();
        }
        NotInteractvie();
        this.gameObject.transform.position = cubeTransform.position;
    }
    private void Start()
    {
        cubeSpriteArray = diceData.patternSpriteArray;
        offset = new Vector3(0, 0.5f, 0);
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
        GameManager.GetInstance.VisibleInteractiveCanvas(cubeTransform, offset);
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
        return InteractiveType.ThemeFirst_Cube;
    }

    #endregion

    public Sprite GetCubeSprite(int index)
    {
        return this.cubeSpriteArray[index];
    }
}
