using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour
{
    [SerializeField] private Image uiImage;

    public int priority = 0;
    public bool IsSetObject { get; set; } = false;

    private void Start()
    {
        uiImage.sprite = null;
        uiImage.color = Color.white;
    }

    public void SetObject(Sprite objSprite, Color objColor)
    {
        IsSetObject = true;
        uiImage.sprite = objSprite;
        uiImage.color = objColor;
    }

    public void GetObject()
    {
        IsSetObject = false;
        uiImage.sprite = null;
        uiImage.material = null;
    }
}
