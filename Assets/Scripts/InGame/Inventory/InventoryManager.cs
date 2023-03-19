using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    #region Static
    public static InventoryManager GetInstance;
    private void Awake()
    {
        GetInstance = this;
    }
    #endregion

    [SerializeField] private List<InventoryUI> inventoryObjects = new List<InventoryUI>();
    private InventoryUI emptyInventory;
    private void Start()
    {
        foreach (var obj in inventoryObjects)
        {
            obj.IsSetObject = false;
        }
    }

    private void InventoryCheck()
    {
        int minPriority = 987654321;

        foreach (var obj in inventoryObjects)
        {
            if (!obj.IsSetObject)
            {
                if (obj.priority < minPriority)
                {
                    minPriority = obj.priority;
                    emptyInventory = obj;
                    Debug.Log(emptyInventory.IsSetObject + " " + emptyInventory.priority);
                }
            }
        }
    }

    public void PutInInventory(Sprite sprite, Color color)
    {
        InventoryCheck();
        emptyInventory.SetObject(sprite, color);
        emptyInventory = null;
    }

    public void RemoveInventory()
    {
        emptyInventory.GetObject();
    }
}
