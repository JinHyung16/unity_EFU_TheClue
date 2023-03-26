using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using Cysharp.Threading.Tasks;
using System.Threading;
using System;
using DG.Tweening;

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

    [SerializeField] private GameObject invenIsFullImage;

    public int selectInvenIndex = 0;

    //UniTask 관련
    private CancellationTokenSource tokenSource;

    private void Start()
    {
        foreach (var obj in inventoryUIList)
        {
            obj.IsSetObject = false;
        }

        if (tokenSource != null)
        {
            tokenSource.Dispose();
        }
        tokenSource = new CancellationTokenSource();
        invenIsFullImage.SetActive(false);
    }


    private void InventoryCheck()
    {
        int minPriority = 987654321;

        for(int i =0; i < inventoryUIList.Count; i++)
        {
            if (!inventoryUIList[i].IsSetObject)
            {
                if (inventoryUIList[i].priority < minPriority)
                {
                    minPriority = inventoryUIList[i].priority;
                    emptyInventory = inventoryUIList[i];
                }
            }
        }
    }

    public void PutInInventory(GameObject obj, Sprite sprite, Color color)
    {
        InventoryCheck();
        if (emptyInventory != null)
        {
            emptyInventory.SetObject(obj, sprite, color);
            emptyInventory = null;
            obj.SetActive(false);
        }
        else
        {
            InventoryIsPullUI();
        }
    }
    private void InventoryIsPullUI()
    {
        if (tokenSource != null)
        {
            tokenSource.Cancel();
        }
        invenIsFullImage.SetActive(true);
        WaningInventoryIsPull().Forget();
    }

    private async UniTaskVoid WaningInventoryIsPull()
    {
        invenIsFullImage.transform.DOShakePosition(1.0f, 0.3f);
        await UniTask.Delay(TimeSpan.FromSeconds(1.0f), cancellationToken: tokenSource.Token);
        invenIsFullImage.SetActive(false);
        tokenSource.Cancel();
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

        if (TileManager.GetInstance != null)
        {
            //TilePatternManager가 있으면 테마1의 주사위 데이터 로드
            TileManager.GetInstance.SetInventorySync();
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
    /// 테마1에서 inven의 object를 Tile에 최종적으로 배치 완료할 때 호출하는 함수
    /// </summary>
    public void InvenObjectPutOnTile()
    {
        emptyInventory.GetObject();
        emptyInventory = null;
    }
}
