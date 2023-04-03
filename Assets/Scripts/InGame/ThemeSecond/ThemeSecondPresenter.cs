using DG.Tweening;
using HughGenerics;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThemeSecondPresenter : PresenterSingleton<ThemeSecondPresenter>
{
    [SerializeField] private ThemeSecondViewer themeSecondViewer;

    [SerializeField] private Camera cameraMain;

    [Header("Camera와 Interactive Transfomr들 관련")]
    [SerializeField] private Camera cameraInteractive;
    [SerializeField] private Transform noteTransform;
    [SerializeField] private Transform keyHoleTransform;
    [SerializeField] private Transform showcaseTransform;

    [Header("Switch SpotLight")]
    [SerializeField] private Light switchSpotLight;


    //0==상호작용 X, 1==문과 상호작용, 2==showcase와 상호작용
    public int IsInteractiveNum { get; private set; } = 0;

    private string themeName = "ThemeSecond";
    protected override void OnAwake()
    {
        switchSpotLight.color = Color.white;
        switchSpotLight.enabled = true;
    }

    private void Start()
    {
        themeName = "ThemeSecond";
        SceneController.GetInstance.CurSceneName = themeName;

        GameManager.GetInstance.SpawnPlayer();
        GameManager.GetInstance.CameraTheme = this.cameraMain;
        GameManager.GetInstance.CameraInteractive = this.cameraInteractive;

        this.cameraMain.cullingMask = 0;
        this.cameraInteractive.cullingMask = 0;

        GameManager.GetInstance.IsUIOpen = false;
        GameManager.GetInstance.IsInputStop = false;

        TimerManager.GetInstance.ThemeTime = 900.0f;
    }

    private void OnDisable()
    {
        
    }

    private void CmaInteractiveSet(Transform transform, bool isActive)
    {
        cameraInteractive.transform.position = transform.position;
        cameraInteractive.transform.rotation = transform.rotation;

        if (isActive)
        {
            this.cameraInteractive.cullingMask = -1;
            cameraInteractive.depth = 1;
            GameManager.GetInstance.PlayerCameraControl(false);
        }
        else
        {
            this.cameraInteractive.cullingMask = 0;
            cameraInteractive.depth = 0;
            GameManager.GetInstance.PlayerCameraControl(true);
        }
    }

    /// <summary>
    /// Switch On or Off를 통해 불빛 켜고 끄기
    /// </summary>
    public void SwitchOnOff()
    {
        if (switchSpotLight.enabled)
        {
            switchSpotLight.enabled = false;
        }
        else
        {
            switchSpotLight.enabled = true;
        }
    }

    public void DoorKeyHoleInteractive(bool active)
    {
        CmaInteractiveSet(keyHoleTransform, true);
        GameObject obj = InventoryManager.GetInstance.GetInvenObject();
        if (active)
        {
            GameManager.GetInstance.Player.transform.position += new Vector3(0, 0, -2.0f);
            IsInteractiveNum = 1;
            if (obj != null)
            {
                obj.transform.position = cameraInteractive.transform.position + new Vector3(0, 0, 1.0f);
                obj.transform.LookAt(cameraInteractive.transform);
                obj.SetActive(true);
            }
            themeSecondViewer.DoorCanvasOpen();
        }
        else
        {
            IsInteractiveNum = 0;
            if (obj != null)
            {
                obj.SetActive(false);
            }
            CmaInteractiveSet(keyHoleTransform, false);
            themeSecondViewer.CloseCanvas();
        }
    }

    public void ObjectSyncToDoorKeyHole()
    {
        GameObject obj = InventoryManager.GetInstance.GetInvenObject();
        if (obj != null)
        {
            obj.transform.position = cameraInteractive.transform.position + new Vector3(0, 0, 1.0f);
            obj.transform.LookAt(cameraInteractive.transform);
            obj.SetActive(true);
        }
    }

    public void NoteInteractive(bool active)
    {
    }

    public void ShowCaseInteractive(bool active)
    {
        if (active)
        {
            GameManager.GetInstance.Player.transform.position += new Vector3(-1.0f, 0, 0);
            GameManager.GetInstance.IsUIOpen = true;
            IsInteractiveNum = 2;
            CmaInteractiveSet(showcaseTransform, true);
        }
        else
        {
            GameManager.GetInstance.IsUIOpen = false;
            IsInteractiveNum = 0;
            CmaInteractiveSet(showcaseTransform, false);
        }
    }

    #region Inventory에 들어갈 object 함수
    public void DoorKeyInventory(GameObject obj)
    {
        var doorKey = obj.GetComponent<DoorKey>();
        InventoryManager.GetInstance.PutInInventory(obj, doorKey.GetDoorKeyUISprite, UnityEngine.Color.white); ;
    }

    public void NoteInventory(GameObject obj)
    {
        var note = obj.GetComponent<Note>();
        InventoryManager.GetInstance.PutInInventory(obj, note.GetNoteUISprite, UnityEngine.Color.white); ;
    }
    #endregion
}
