using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour
{
    [SerializeField] private Image uiImage;

    public int priority = 0;

    private GameObject invenObj = null;
    private bool isSetObj = false;
    public bool IsSetObject { get { return this.isSetObj; } }
    public GameObject InventoryObject { get { return this.invenObj; } }

    private void Start()
    {
        this.isSetObj = false;
        this.uiImage.sprite = null;
        this.uiImage.color = Color.white;
    }

    public void SetObject(GameObject obj, Sprite objSprite, Color objColor)
    {
        if (!IsSetObject)
        {
            this.isSetObj = true;
            this.invenObj = obj;
            this.InventoryObject.name = obj.name;
            this.uiImage.sprite = objSprite;
            this.uiImage.color = objColor;
        }
    }

    public void GetObject()
    {
        this.isSetObj = false;
        this.invenObj = null;
        this.uiImage.sprite = null;
        this.uiImage.color = Color.white;
    }
}
