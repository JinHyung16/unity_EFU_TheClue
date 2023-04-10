using System.Collections;
using System.Collections.Generic;
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

    [SerializeField] private List<Transform> selectInvenTransforms = new List<Transform>();
    [SerializeField] private GameObject selectMarkerObj;

    [Header("InventoryUI를 갖는 UI List")]
    [SerializeField] private List<InventoryUI> inventoryUIList = new List<InventoryUI>();
    [SerializeField] private GameObject invenIsFullImage;

    private bool isFullInven = false;

    private InventoryUI emptyInventory;
    private GameObject invenObj;

    public int selectInvenIndex = 0;

    //UniTask 관련
    private CancellationTokenSource tokenSource;

    private void Start()
    {
        if (tokenSource != null)
        {
            tokenSource.Dispose();
        }
        tokenSource = new CancellationTokenSource();
        invenIsFullImage.SetActive(false);
    }


    private void InventoryCheck()
    {
        int minPriority = 98765;

        for (int i = 0; i < inventoryUIList.Count; i++)
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
        int minPriority = 98765;

        for (int i = 0; i < inventoryUIList.Count; i++)
        {
            if (!inventoryUIList[i].IsSetObject)
            {
                if (inventoryUIList[i].priority < minPriority)
                {
                    minPriority = inventoryUIList[i].priority;
                    inventoryUIList[i].SetObject(obj, sprite, color);
                    emptyInventory = inventoryUIList[i];
                    obj.SetActive(false);
                    break;
                }
            }
            else
            {
                emptyInventory = null;
            }
        }

        if (emptyInventory != null)
        {
            return;
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
            tokenSource.Dispose();
        }
        tokenSource = new CancellationTokenSource();

        WaningInventoryIsPull().Forget();
    }

    private async UniTaskVoid WaningInventoryIsPull()
    {
        invenIsFullImage.SetActive(true);
        invenIsFullImage.transform.DOShakePosition(1.8f, 3.5f);
        await UniTask.Delay(TimeSpan.FromSeconds(1.0f), cancellationToken: tokenSource.Token);
        invenIsFullImage.SetActive(false);
        await UniTask.Delay(TimeSpan.FromSeconds(0.5f), cancellationToken: tokenSource.Token);
    }

    /// <summary>
    /// 숫자 1,2,3 버튼을 눌러 Inventory를 선택해 담겨있는 아이템 정보를 가져온다.
    /// </summary>
    /// <param invenObjName="selectIdx"> 선택한 inventory 순서 </param>
    public void SelectInventory(int selectIdx)
    {
        selectInvenIndex = selectIdx;
        emptyInventory = inventoryUIList[selectInvenIndex];
        selectMarkerObj.transform.position = selectInvenTransforms[selectIdx].transform.position;
        if (emptyInventory.InventoryObject != null)
        {
            if (TileManager.GetInstance != null)
            {
                //TilePatternManager가 있으면 테마1의 주사위 데이터 로드
                TileManager.GetInstance.SetInventorySync();
            }
            if (ThemeSecondPresenter.GetInstance != null)
            {
                if (ThemeSecondPresenter.GetInstance.IsInteractiveNum == 1)
                {
                    invenObj.SetActive(false);
                    ThemeSecondPresenter.GetInstance.ObjectSyncToDoorKeyHole();
                }
                if (emptyInventory.InventoryObject.GetComponent<Note>() != null
                    && ThemeSecondPresenter.GetInstance.IsInteractiveNum == 0)
                {
                    ThemeSecondPresenter.GetInstance.NoteSelectInInven(emptyInventory.InventoryObject, true);
                }
            }
        }
    }

    /// <summary>
    /// Invetory의 아이템을 버릴때 호출할 함수
    /// </summary>
    /// <param name="selectIdx"> 버릴 인벤토리 번호 </param>
    public void ThrowOutInventoryObject(int selectIdx)
    {
        selectInvenIndex = selectIdx;
        emptyInventory = inventoryUIList[selectInvenIndex];
        selectMarkerObj.transform.position = selectInvenTransforms[selectIdx].transform.position;
        if (emptyInventory.InventoryObject != null && !emptyInventory.InventoryObject.CompareTag("Note"))
        {
            emptyInventory.InventoryObject.SetActive(true);
            emptyInventory.GetObject();
            emptyInventory = null;
        }
    }

    public GameObject GetInvenObject()
    {
        if (emptyInventory != null)
        {
            emptyInventory = inventoryUIList[selectInvenIndex];
            if (emptyInventory.IsSetObject)
            {
                invenObj = emptyInventory.InventoryObject;
            }
            return invenObj;
        }
        return null;
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
