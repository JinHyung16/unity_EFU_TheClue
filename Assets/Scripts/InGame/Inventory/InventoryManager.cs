using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Tilemaps;

public class InventoryManager : MonoBehaviour
{
    #region Static
    public static InventoryManager GetInstance;
    private void Awake()
    {
        GetInstance = this;
    }
    #endregion

    [SerializeField] private List<InventoryUI> inventoryUIList = new List<InventoryUI>();
    private InventoryUI emptyInventory;

    public int selectInvenIndex = 0;
    private void Start()
    {
        foreach (var obj in inventoryUIList)
        {
            obj.IsSetObject = false;
        }
    }

    private void InventoryCheck()
    {
        int minPriority = 987654321;

        foreach (var obj in inventoryUIList)
        {
            if (!obj.IsSetObject)
            {
                if (obj.priority < minPriority)
                {
                    minPriority = obj.priority;
                    emptyInventory = obj;
                }
            }
        }
    }

    public void PutInInventory(GameObject obj, Sprite sprite, Color color)
    {
        InventoryCheck();
        emptyInventory.SetObject(obj, sprite, color);
        emptyInventory = null;
        obj.SetActive(false);
    }

    /// <summary>
    /// 숫자 1,2,3 버튼을 눌러 Inventory를 선택해 담겨있는 아이템 정보를 가져온다.
    /// </summary>
    /// <param invenObjName="selectIdx"> 선택한 inventory 순서 </param>
    public void SelectInventory(int selectIdx)
    {
        selectInvenIndex = selectIdx;
        if (emptyInventory != null)
        {
            emptyInventory = null;
        }

        emptyInventory = inventoryUIList[selectInvenIndex];
        if (emptyInventory.InventoryObject != null)
        {
            var invenObjName = emptyInventory.InventoryObject.name.Substring(0, 4);
            if (invenObjName != "Dice")
            {
                Debug.Log("InventoryManager: " + invenObjName);
                //상호작용 UI오픈
            }
        }

        if (TilePatternManager.GetInstance != null)
        {
            //TilePatternManager가 있으면 테마1의 주사위 데이터 로드
            TilePatternManager.GetInstance.SetDiceSync();
        }
    }

    public GameObject GetObjectInventory()
    {
        if (emptyInventory == null)
        {
            return null;
        }

        emptyInventory = inventoryUIList[selectInvenIndex];
        if (!emptyInventory.IsSetObject)
        {
            return null;
        }

        GameObject invenObj = emptyInventory.InventoryObject;
        return invenObj;
    }

    /// <summary>
    /// 주사위를 Tile에 최종적으로 배치 완료할 때 호출하는 함수
    /// </summary>
    public void DicePutOnTile()
    {
        emptyInventory.GetObject();
        emptyInventory = null;
    }
}
