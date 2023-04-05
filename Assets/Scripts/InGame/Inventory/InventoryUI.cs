using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour
{
    [SerializeField] private Image uiImage;

    public int priority = 0;
    public bool IsSetObject { get; private set; } = false;
    public GameObject InventoryObject { get; private set; } = null;

    private void Start()
    {
        IsSetObject = false;
        uiImage.sprite = null;
        uiImage.color = Color.white;
    }

    public void SetObject(GameObject obj, Sprite objSprite, Color objColor)
    {
        if (!IsSetObject)
        {
            IsSetObject = true;
            InventoryObject = obj;
            InventoryObject.name = obj.name;
            uiImage.sprite = objSprite;
            uiImage.color = objColor;
        }
    }

    public void GetObject()
    {
        IsSetObject = false;
        uiImage.sprite = null;
        uiImage.color = Color.white;
        InventoryObject = null;
        InventoryObject.name = null;
    }
}
